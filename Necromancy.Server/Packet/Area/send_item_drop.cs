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
            byte storageType = packet.Data.ReadByte();
            byte bagId = packet.Data.ReadByte();
            ushort bagSlot = packet.Data.ReadUInt16();
            byte stackSize = packet.Data.ReadByte();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_item_drop_r, res, ServerType.Area);

            res.SetPositionStart();
            res.WriteInt64(10200101); //Item instance Id here
            Router.Send(client, (ushort)AreaPacketId.recv_item_remove, res, ServerType.Area);
        }
    }
}
