using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Custom
{
    public class SendUnknown1 : ClientHandler
    {
        public SendUnknown1(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) CustomPacketId.SendUnknown1;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint a = packet.Data.ReadUInt32();
            uint b = packet.Data.ReadUInt32();
            Logger.Info(client, $"Unknown1 A:{a}");
            Logger.Info(client, $"Unknown1 B:{b}");

            IBuffer buffer = BufferProvider.Provide();
            buffer.WriteInt32(a);
            buffer.WriteInt32(b);

            NecPacket response = new NecPacket(
                (ushort) CustomPacketId.RecvUnknown1,
                buffer,
                packet.ServerType,
                PacketType.Unknown1
            );

            Router.Send(client, response);
        }
    }
}
