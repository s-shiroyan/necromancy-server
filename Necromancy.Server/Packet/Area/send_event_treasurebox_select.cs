using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_event_treasurebox_select : ClientHandler
    {
        public send_event_treasurebox_select(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_event_treasurebox_select;

        public override void Handle(NecClient client, NecPacket packet)
        {

            byte fromStoreType = packet.Data.ReadByte();
            byte fromBagId = packet.Data.ReadByte();
            short fromSlot = packet.Data.ReadInt16();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(10003);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteInt16(2);
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_place, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_event_treasurebox_select_r, res, ServerType.Area);
        }
    }
}
