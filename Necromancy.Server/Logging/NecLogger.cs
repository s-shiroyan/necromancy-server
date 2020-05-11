using System;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Setting;

namespace Necromancy.Server.Logging
{
    public class NecLogger : Logger
    {
        private NecSetting _setting;

        public override void Initialize(string identity, string name, Action<Log> write, object configuration)
        {
            base.Initialize(identity, name, write, configuration);
            _setting = configuration as NecSetting;
            if (_setting == null)
            {
                Error("Couldn't apply NecLogger configuration");
            }
        }

        public void Info(NecClient client, string message)
        {
            Info($"{client.Identity} {message}");
        }

        public void Info(NecConnection connection, string message)
        {
            NecClient client = connection.Client;
            if (client != null)
            {
                Info(client, message);
                return;
            }

            Info($"{connection.Identity} {message}");
        }

        public void Debug(NecClient client, string message)
        {
            Debug($"{client.Identity} {message}");
        }

        public void Error(NecClient client, string message)
        {
            Error($"{client.Identity} {message}");
        }

        public void Error(NecConnection connection, string message)
        {
            NecClient client = connection.Client;
            if (client != null)
            {
                Error(client, message);
                return;
            }

            Error($"{connection.Identity} {message}");
        }

        public void Exception(NecClient client, Exception exception)
        {
            if (exception == null)
            {
                Write(LogLevel.Error, $"{client.Identity} Exception was null.", null);
            }
            else
            {
                Write(LogLevel.Error, $"{client.Identity} {exception}", exception);
            }
        }

        public void Exception(NecConnection connection, Exception exception)
        {
            NecClient client = connection.Client;
            if (client != null)
            {
                Exception(client, exception);
                return;
            }

            if (exception == null)
            {
                Write(LogLevel.Error, $"{connection.Identity} Exception was null.", null);
            }
            else
            {
                Write(LogLevel.Error, $"{connection.Identity} {exception}", exception);
            }
        }

        public void Info(ITcpSocket socket, string message)
        {
            Info($"[{socket.Identity}] {message}");
        }

        public void Debug(ITcpSocket socket, string message)
        {
            Debug($"[{socket.Identity}] {message}");
        }

        public void Error(ITcpSocket socket, string message)
        {
            Error($"[{socket.Identity}] {message}");
        }

        public void Exception(ITcpSocket socket, Exception exception)
        {
            if (exception == null)
            {
                Write(LogLevel.Error, $"{socket.Identity} Exception was null.", null);
            }
            else
            {
                Write(LogLevel.Error, $"{socket.Identity} {exception}", exception);
            }
        }

        public void LogIncomingPacket(NecClient client, NecPacket packet, ServerType serverType)
        {
            if (_setting.LogIncomingPackets)
            {
                NecLogPacket logPacket = new NecLogPacket(client.Identity, packet, NecLogType.PacketIn, serverType);
                WritePacket(logPacket);
            }
        }

        public void LogIncomingPacket(NecConnection connection, NecPacket packet, ServerType serverType)
        {
            NecClient client = connection.Client;
            if (client != null)
            {
                LogIncomingPacket(client, packet, serverType);
                return;
            }

            if (!_setting.LogIncomingPackets)
            {
                return;
            }

            NecLogPacket logPacket = new NecLogPacket(connection.Identity, packet, NecLogType.PacketIn, serverType);
            WritePacket(logPacket);
        }

        public void LogUnknownIncomingPacket(NecClient client, NecPacket packet, ServerType serverType)
        {
            if (_setting.LogUnknownIncomingPackets)
            {
                NecLogPacket logPacket =
                    new NecLogPacket(client.Identity, packet, NecLogType.PacketUnhandled, serverType);
                WritePacket(logPacket);
            }
        }

        public void LogUnknownIncomingPacket(NecConnection connection, NecPacket packet, ServerType serverType)
        {
            NecClient client = connection.Client;
            if (client != null)
            {
                LogUnknownIncomingPacket(client, packet, serverType);
                return;
            }

            if (!_setting.LogIncomingPackets)
            {
                return;
            }

            NecLogPacket logPacket =
                new NecLogPacket(connection.Identity, packet, NecLogType.PacketUnhandled, serverType);
            WritePacket(logPacket);
        }

        public void LogOutgoingPacket(NecClient client, NecPacket packet, ServerType serverType)
        {
            if (_setting.LogOutgoingPackets)
            {
                NecLogPacket logPacket = new NecLogPacket(client.Identity, packet, NecLogType.PacketOut, serverType);
                WritePacket(logPacket);
            }
        }

        public void LogOutgoingPacket(NecConnection connection, NecPacket packet, ServerType serverType)
        {
            NecClient client = connection.Client;
            if (client != null)
            {
                LogOutgoingPacket(client, packet, serverType);
                return;
            }

            if (!_setting.LogIncomingPackets)
            {
                return;
            }

            NecLogPacket logPacket = new NecLogPacket(connection.Identity, packet, NecLogType.PacketOut, serverType);
            WritePacket(logPacket);
        }

        private void WritePacket(NecLogPacket packet)
        {
            Write(LogLevel.Info, packet.ToLogText(), packet);
        }
    }
}
