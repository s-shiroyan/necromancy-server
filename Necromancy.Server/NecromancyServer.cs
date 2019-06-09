using System.Text;
using Arrowgene.Services.Buffers;
using Arrowgene.Services.Logging;
using Arrowgene.Services.Networking.Tcp;
using Arrowgene.Services.Networking.Tcp.Consumer;

namespace Necromancy.Server
{
    public abstract class NecromancyServer : IConsumer
    {
        protected ILogger _logger;

        public NecromancyServer()
        {
            _logger = LogProvider.Logger(this);
        }

        public void OnStart()
        {
        }

        public void OnStarted()
        {
        }

        public void OnStop()
        {
        }

        public void OnStopped()
        {
        }

        protected void Send(ITcpSocket socket, ushort opCode, IBuffer response)
        {
            byte[] payload = response.GetAllBytes();
            ushort opSize = 2;
            ushort size = (ushort)(payload.Length + opSize);
            IBuffer buffer = new StreamBuffer();
            buffer.SetPositionStart();
            buffer.WriteInt16(size, Endianness.Big);
            buffer.WriteInt16(opCode, Endianness.Big);
            buffer.WriteBytes(payload);
            socket.Send(buffer.GetAllBytes());
            PacketLogOut(buffer);
        }

        public void OnClientDisconnected(ITcpSocket socket)
        {
            _logger.Info("Client Disconnected");
        }

        public void OnClientConnected(ITcpSocket socket)
        {
            _logger.Info("Client Connected");
        }

        protected void PacketLogIn(IBuffer packet)
        {
            PacketLog(packet, "IN");
        }

        protected void PacketLogOut(IBuffer packet)
        {
            PacketLog(packet, "OUT");
        }

        protected void PacketLog(IBuffer packet, string tag)
        {
            packet.SetPositionStart();
            ushort size = packet.ReadUInt16(Endianness.Big);
            ushort opCode = packet.ReadUInt16(Endianness.Big);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Packet Log:");
            sb.AppendLine($"[Type:{tag}][TotalSize:{packet.Size}] Header:[Size:{size}][OPCode:{opCode}]");
            sb.AppendLine("------------------------------------------------------------");
            sb.AppendLine(packet.ToAsciiString(true));
            sb.AppendLine(packet.ToHexString(' '));
            sb.AppendLine("------------------------------------------------------------");
            _logger.Write(LogLevel.Debug, tag, sb.ToString());
        }

        public abstract void OnReceivedData(ITcpSocket socket, byte[] data);
    }
}