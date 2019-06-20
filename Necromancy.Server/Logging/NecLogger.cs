using System;
using Arrowgene.Services.Buffers;
using Arrowgene.Services.Logging;
using Arrowgene.Services.Networking.Tcp;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Setting;

namespace Necromancy.Server.Logging
{
    public class NecLogger : Logger
    {
        private NecSetting _setting;

        public NecLogger() : this(null)
        {
        }

        public NecLogger(string identity, string zone = null) : base(identity, zone)
        {
        }

        public override void Initialize(string identity, string zone, object configuration)
        {
            base.Initialize(identity, zone, configuration);
            _setting = configuration as NecSetting;
            if (_setting == null)
            {
                Error("Couldn't apply NecLogger configuration");
            }
        }

        public void Info(NecClient client, string message, params object[] args)
        {
            Write(LogLevel.Info, null, $"{client.Identity} {message}", args);
        }

        public void Debug(NecClient client, string message, params object[] args)
        {
            Write(LogLevel.Debug, null, $"{client.Identity} {message}", args);
        }

        public void Error(NecClient client, string message, params object[] args)
        {
            Write(LogLevel.Error, null, $"{client.Identity} {message}", args);
        }

        public void Exception(NecClient client, Exception exception)
        {
            Write(LogLevel.Error, null, $"{client.Identity} {exception}");
        }

        public void Info(ITcpSocket socket, string message, params object[] args)
        {
            Write(LogLevel.Info, null, $"[{socket.Identity}] {message}", args);
        }

        public void Debug(ITcpSocket socket, string message, params object[] args)
        {
            Write(LogLevel.Debug, null, $"[{socket.Identity}] {message}", args);
        }

        public void Error(ITcpSocket socket, string message, params object[] args)
        {
            Write(LogLevel.Error, null, $"[{socket.Identity}] {message}", args);
        }

        public void Exception(ITcpSocket socket, Exception exception)
        {
            Write(LogLevel.Error, null, $"[{socket.Identity}] {exception}");
        }

        public void LogErrorPacket(NecClient client, byte[] data, string identity)
        {
            IBuffer buffer = BufferProvider.Provide(data);
            String log = $"{client.Identity} Packet Log";
            log += Environment.NewLine;
            log += "----------";
            log += Environment.NewLine;
            if (identity != null)
            {
                log += $"[{identity}]";
            }

            log += $"[Len:{buffer.Size}]";
            log += Environment.NewLine;
            log += "ASCII:";
            log += Environment.NewLine;
            log += buffer.ToHexString('-');
            log += Environment.NewLine;
            log += "HEX:";
            log += Environment.NewLine;
            log += buffer.ToAsciiString(true);
            ;
            log += Environment.NewLine;
            log += "----------";

            Write(LogLevel.Error, NecLogType.PacketError, log);
        }

        public void LogIncomingPacket(NecClient client, NecPacket packet, string identity)
        {
            if (_setting.LogIncomingPackets)
            {
                NecLogPacket logPacket = new NecLogPacket(client, packet, NecLogType.In, identity);
                Packet(logPacket);
            }
        }

        public void LogUnknownIncomingPacket(NecClient client, NecPacket packet, string identity)
        {
            if (_setting.LogUnknownIncomingPackets)
            {
                NecLogPacket logPacket = new NecLogPacket(client, packet, NecLogType.Unhandled, identity);
                Packet(logPacket);
            }
        }

        public void LogOutgoingPacket(NecClient client, NecPacket packet)
        {
            if (_setting.LogOutgoingPackets)
            {
                NecLogPacket logPacket = new NecLogPacket(client, packet, NecLogType.Out);
                Packet(logPacket);
            }
        }

        public void Packet(NecLogPacket packet)
        {
            Write(LogLevel.Info, packet.LogType, packet.ToLogText());
        }
    }
}