using System;
using System.Collections.Generic;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;
using Arrowgene.Networking.Tcp.Consumer.BlockingQueueConsumption;
using Arrowgene.Networking.Tcp.Server.AsyncEvent;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Setting;

namespace Necromancy.Server
{
    public class NecQueueConsumer : ThreadedBlockingQueueConsumer
    {
        public const int NoExpectedSize = -1;

        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(NecQueueConsumer));

        private readonly Dictionary<int, IClientHandler> _clientHandlers;
        private readonly Dictionary<int, IConnectionHandler> _connectionHandlers;
        private readonly Dictionary<ITcpSocket, NecConnection> _connections;
        private readonly object _lock;

        private ServerType _serverType;
        private NecSetting _setting;

        public NecQueueConsumer(ServerType serverType, NecSetting setting, AsyncEventSettings socketSetting) : base(
            socketSetting, serverType.ToString())
        {
            _serverType = serverType;
            _setting = setting;
            _lock = new object();
            _clientHandlers = new Dictionary<int, IClientHandler>();
            _connectionHandlers = new Dictionary<int, IConnectionHandler>();
            _connections = new Dictionary<ITcpSocket, NecConnection>();
        }

        public Action<NecConnection> ClientDisconnected;
        public Action<NecConnection> ClientConnected;

        public void Clear()
        {
            _clientHandlers.Clear();
            _connectionHandlers.Clear();
        }

        public void AddHandler(IClientHandler clientHandler, bool overwrite = false)
        {
            if (overwrite)
            {
                if (_clientHandlers.ContainsKey(clientHandler.Id))
                {
                    _clientHandlers[clientHandler.Id] = clientHandler;
                }
                else
                {
                    _clientHandlers.Add(clientHandler.Id, clientHandler);
                }

                return;
            }

            if (_clientHandlers.ContainsKey(clientHandler.Id))
            {
                Logger.Error($"[{_serverType}] ClientHandlerId: {clientHandler.Id} already exists");
            }
            else
            {
                _clientHandlers.Add(clientHandler.Id, clientHandler);
            }
        }

        public void AddHandler(IConnectionHandler connectionHandler, bool overwrite = false)
        {
            if (overwrite)
            {
                if (_connectionHandlers.ContainsKey(connectionHandler.Id))
                {
                    _connectionHandlers[connectionHandler.Id] = connectionHandler;
                }
                else
                {
                    _connectionHandlers.Add(connectionHandler.Id, connectionHandler);
                }

                return;
            }

            if (_connectionHandlers.ContainsKey(connectionHandler.Id))
            {
                Logger.Error($"[{_serverType}] ConnectionHandlerId: {connectionHandler.Id} already exists");
            }
            else
            {
                _connectionHandlers.Add(connectionHandler.Id, connectionHandler);
            }
        }

        protected override void HandleReceived(ITcpSocket socket, byte[] data)
        {
            if (!socket.IsAlive)
            {
                return;
            }

            NecConnection connection;
            lock (_lock)
            {
                if (!_connections.ContainsKey(socket))
                {
                    Logger.Error(socket, $"[{_serverType}] Client does not exist in lookup");
                    return;
                }

                connection = _connections[socket];
            }

            List<NecPacket> packets = connection.Receive(data);
            foreach (NecPacket packet in packets)
            {
                NecClient client = connection.Client;
                if (client != null)
                {
                    HandleReceived_Client(client, packet);
                }
                else
                {
                    HandleReceived_Connection(connection, packet);
                }
            }
        }

        private void HandleReceived_Connection(NecConnection connection, NecPacket packet)
        {
            if (!_connectionHandlers.ContainsKey(packet.Id))
            {
                Logger.LogUnknownIncomingPacket(connection, packet, _serverType);
                return;
            }

            IConnectionHandler connectionHandler = _connectionHandlers[packet.Id];
            if (connectionHandler.ExpectedSize != NoExpectedSize && packet.Data.Size < connectionHandler.ExpectedSize)
            {
                Logger.Error(connection,
                    $"[{_serverType}] Ignoring Packed (Id:{packet.Id}) is smaller ({packet.Data.Size}) than expected ({connectionHandler.ExpectedSize})");
                return;
            }

            Logger.LogIncomingPacket(connection, packet, _serverType);
            packet.Data.SetPositionStart();
            try
            {
                connectionHandler.Handle(connection, packet);
            }
            catch (Exception ex)
            {
                Logger.Exception(connection, ex);
            }
        }

        private void HandleReceived_Client(NecClient client, NecPacket packet)
        {
            if (!_clientHandlers.ContainsKey(packet.Id))
            {
                Logger.LogUnknownIncomingPacket(client, packet, _serverType);
                return;
            }

            IClientHandler clientHandler = _clientHandlers[packet.Id];
            if (clientHandler.ExpectedSize != NoExpectedSize && packet.Data.Size < clientHandler.ExpectedSize)
            {
                Logger.Error(client,
                    $"[{_serverType}] Ignoring Packed (Id:{packet.Id}) is smaller ({packet.Data.Size}) than expected ({clientHandler.ExpectedSize})");
                return;
            }

            Logger.LogIncomingPacket(client, packet, _serverType);
            packet.Data.SetPositionStart();
            try
            {
                clientHandler.Handle(client, packet);
            }
            catch (Exception ex)
            {
                Logger.Exception(client, ex);
            }
        }
        
        protected override void HandleDisconnected(ITcpSocket socket)
        {
            NecConnection connection;
            lock (_lock)
            {
                if (!_connections.ContainsKey(socket))
                {
                    Logger.Error(socket, $"[{_serverType}] Disconnected client does not exist in lookup");
                    return;
                }

                connection = _connections[socket];
                _connections.Remove(socket);
                Logger.Debug($"[{_serverType}] Clients Count: {_connections.Count}");
            }

            Action<NecConnection> onClientDisconnected = ClientDisconnected;
            if (onClientDisconnected != null)
            {
                try
                {
                    onClientDisconnected.Invoke(connection);
                }
                catch (Exception ex)
                {
                    Logger.Exception(connection, ex);
                }
            }

            Logger.Info(connection, $"[{_serverType}] Client disconnected");
        }

        protected override void HandleConnected(ITcpSocket socket)
        {
            NecConnection connection = new NecConnection(socket, new PacketFactory(_setting), _serverType);
            lock (_lock)
            {
                _connections.Add(socket, connection);
                Logger.Debug($"[{_serverType}] Clients Count: {_connections.Count}");
            }

            Action<NecConnection> onClientConnected = ClientConnected;
            if (onClientConnected != null)
            {
                try
                {
                    onClientConnected.Invoke(connection);
                }
                catch (Exception ex)
                {
                    Logger.Exception(connection, ex);
                }
            }

            Logger.Info(connection, $"[{_serverType}] Client connected");
        }
    }
}
