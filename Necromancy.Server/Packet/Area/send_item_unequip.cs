using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_item_unequip : Handler
    {
        public send_item_unequip(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_item_unequip;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int slotNum = packet.Data.ReadInt32();

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(00); //error check. 0 to work

            /*
            ERR_UNEQUIP 1
            ERR_UNEQUIP - 203
            ERR_UNEQUIP - 201
            ERR_UNEQUIP - 208
            ERR_UNEQUIP GENERIC
            */

            Router.Send(client, (ushort) AreaPacketId.recv_item_unequip_r, res);
        }
    }
}