using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_item_drop : ClientHandler
    {
        public send_item_drop(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_item_drop;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int itemSlot = packet.Data.ReadInt32();
            byte itemCount = packet.Data.ReadByte();

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            Router.Send(client, (ushort) AreaPacketId.recv_item_drop_r, res, ServerType.Area);
        }
    }
}
