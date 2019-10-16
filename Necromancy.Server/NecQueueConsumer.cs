using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Arrowgene.Services;
using Arrowgene.Services.Logging;
using Arrowgene.Services.Networking.Tcp;
using Arrowgene.Services.Networking.Tcp.Consumer;
using Arrowgene.Services.Networking.Tcp.Consumer.BlockingQueueConsumption;
using Arrowgene.Services.Networking.Tcp.Server.AsyncEvent;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Setting;

namespace Necromancy.Server
{
    public class NecQueueConsumer : IConsumer
    {
        public const int NoExpectedSize = -1;

        private readonly BlockingCollection<ClientEvent>[] _queues;
        private readonly Thread[] _threads;
        private readonly Dictionary<int, IHandler> _handlers;
        private readonly Dictionary<ITcpSocket, NecClient> _clients;
        private readonly NecLogger _logger;
        private readonly object _lock;
        private readonly int _maxUnitOfOrder;
        private ServerType _serverType;
        private NecSetting _setting;
        private volatile bool _isRunning;

        private CancellationTokenSource _cancellationTokenSource;

        public int HandlersCount => _handlers.Count;

        public Action<NecClient> ClientDisconnected;
        public Action<NecClient> ClientConnected;
        public Action Started;
        public Action Stopped;

        public NecQueueConsumer(ServerType serverType, NecSetting setting, AsyncEventSettings socketSetting)
        {
            _setting = setting;
            _logger = LogProvider.Logger<NecLogger>(this);
            _maxUnitOfOrder = socketSetting.MaxUnitOfOrder;
            _queues = new BlockingCollection<ClientEvent>[_maxUnitOfOrder];
            _threads = new Thread[_maxUnitOfOrder];
            _lock = new object();
            _handlers = new Dictionary<int, IHandler>();
            _clients = new Dictionary<ITcpSocket, NecClient>();
            _serverType = serverType;
        }

        public void Clear()
        {
            _handlers.Clear();
        }

        public void AddHandler(IHandler handler, bool overwrite = false)
        {
            if (overwrite)
            {
                if (_handlers.ContainsKey(handler.Id))
                {
                    _handlers[handler.Id] = handler;
                }
                else
                {
                    _handlers.Add(handler.Id, handler);
                }

                return;
            }

            if (_handlers.ContainsKey(handler.Id))
            {
                _logger.Error($"[{_serverType}] HandlerId: {handler.Id} already exists");
            }
            else
            {
                _handlers.Add(handler.Id, handler);
            }
        }

        private void HandleReceived(ITcpSocket socket, byte[] data)
        {
            if (!socket.IsAlive)
            {
                return;
            }

            NecClient client;
            lock (_lock)
            {
                if (!_clients.ContainsKey(socket))
                {
                    _logger.Error(socket, $"[{_serverType}] Client does not exist in lookup");
                    return;
                }

                client = _clients[socket];
            }

            List<NecPacket> packets = client.Receive(data);
            foreach (NecPacket packet in packets)
            {
                if (_handlers.ContainsKey(packet.Id))
                {
                    IHandler handler = _handlers[packet.Id];
                    if (handler.ExpectedSize != NoExpectedSize && packet.Data.Size < handler.ExpectedSize)
                    {
                        _logger.Error(client,
                            $"[{_serverType}] Ignoring Packed (Id:{packet.Id}) is smaller ({packet.Data.Size}) than expected ({handler.ExpectedSize})");
                        continue;
                    }

                    _logger.LogIncomingPacket(client, packet, _serverType);
                    packet.Data.SetPositionStart();
                    try
                    {
                        handler.Handle(client, packet);
                    }
                    catch (Exception ex)
                    {
                        _logger.Exception(client, ex);
                    }
                }
                else
                {
                    _logger.LogUnknownIncomingPacket(client, packet, _serverType);
                }
            }
        }

        private void HandleDisconnected(ITcpSocket socket)
        {
            NecClient client;
            lock (_lock)
            {
                if (!_clients.ContainsKey(socket))
                {
                    _logger.Error(socket, $"[{_serverType}] Disconnected client does not exist in lookup");
                    return;
                }

                client = _clients[socket];
                _clients.Remove(socket);
                _logger.Debug($"[{_serverType}] Clients Count: {_clients.Count}");
            }

            Action<NecClient> onClientDisconnected = ClientDisconnected;
            if (onClientDisconnected != null)
            {
                try
                {
                    onClientDisconnected.Invoke(client);
                }
                catch (Exception ex)
                {
                    _logger.Exception(client, ex);
                }
            }

            _logger.Info(client, $"[{_serverType}] Client disconnected");
        }

        private void HandleConnected(ITcpSocket socket)
        {
            NecClient client = new NecClient(socket, new PacketFactory(_setting), _serverType);
            lock (_lock)
            {
                _clients.Add(socket, client);
                _logger.Debug($"[{_serverType}] Clients Count: {_clients.Count}");
            }

            Action<NecClient> onClientConnected = ClientConnected;
            if (onClientConnected != null)
            {
                try
                {
                    onClientConnected.Invoke(client);
                }
                catch (Exception ex)
                {
                    _logger.Exception(client, ex);
                }
            }

            _logger.Info(client, $"[{_serverType}] Client connected");
        }

        private void Consume(int unitOfOrder)
        {
            while (_isRunning)
            {
                ClientEvent clientEvent;
                try
                {
                    clientEvent = _queues[unitOfOrder].Take(_cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    return;
                }

                switch (clientEvent.ClientEventType)
                {
                    case ClientEventType.ReceivedData:
                        HandleReceived(clientEvent.Socket, clientEvent.Data);
                        break;
                    case ClientEventType.Connected:
                        HandleConnected(clientEvent.Socket);
                        break;
                    case ClientEventType.Disconnected:
                        HandleDisconnected(clientEvent.Socket);
                        break;
                }
            }
        }

        void IConsumer.OnStart()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _isRunning = true;
            for (int i = 0; i < _maxUnitOfOrder; i++)
            {
                int uuo = i;
                _queues[i] = new BlockingCollection<ClientEvent>();
                _threads[i] = new Thread(() => Consume(uuo));
                _threads[i].Name = $"[{_serverType}] Consumer: {i}";
                _logger.Info($"[{_serverType}] Starting Consumer: {i}");
                _threads[i].Start();
            }
        }

        public void OnStarted()
        {
            Action started = Started;
            if (started != null)
            {
                started.Invoke();
            }
        }

        void IConsumer.OnReceivedData(ITcpSocket socket, byte[] data)
        {
            _queues[socket.UnitOfOrder].Add(new ClientEvent(socket, ClientEventType.ReceivedData, data));
        }

        void IConsumer.OnClientDisconnected(ITcpSocket socket)
        {
            _queues[socket.UnitOfOrder].Add(new ClientEvent(socket, ClientEventType.Disconnected));
        }

        void IConsumer.OnClientConnected(ITcpSocket socket)
        {
            _queues[socket.UnitOfOrder].Add(new ClientEvent(socket, ClientEventType.Connected));
        }

        void IConsumer.OnStop()
        {
            _isRunning = false;
            _cancellationTokenSource.Cancel();
            for (int i = 0; i < _maxUnitOfOrder; i++)
            {
                Thread consumerThread = _threads[i];
                _logger.Info($"[{_serverType}] Shutting Consumer: {i} down...");
                Service.JoinThread(consumerThread, 10000, _logger);
                _logger.Info($"[{_serverType}] Consumer: {i} ended.");
                _threads[i] = null;
            }
        }

        public void OnStopped()
        {
            Action stopped = Stopped;
            if (stopped != null)
            {
                stopped.Invoke();
            }
        }
    }
}