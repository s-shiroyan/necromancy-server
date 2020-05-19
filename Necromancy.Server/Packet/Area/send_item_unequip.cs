using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_item_unequip : ClientHandler
    {
        public send_item_unequip(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_item_unequip;
        public override void Handle(NecClient client, NecPacket packet)
        {
            int slotNum = packet.Data.ReadInt32();

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0); //error check. 0 to work

            /*
            ERR_UNEQUIP 1
            ERR_UNEQUIP - 203
            ERR_UNEQUIP - 201
            ERR_UNEQUIP - 208
            ERR_UNEQUIP GENERIC
            */

            Router.Send(client, (ushort) AreaPacketId.recv_item_unequip_r, res, ServerType.Area);
            EQMask(client, i);
        }

        void EQMask(NecClient client, int instanceId)
        {
            IBuffer res13 = BufferProvider.Provide();
            res13.WriteInt64(instanceId);
            res13.WriteInt32(0); // Bitmask for location (0 to unequip)

            res13.WriteInt32(0); // List of items that gonna be equip on the chara
            res13.WriteByte(0); // ?? when you change this the armor dissapear, apparently
            res13.WriteByte(0);
            res13.WriteByte(0); //need to find the right number, permit to get the armor on the chara

            res13.WriteInt32(0); //previously 1
            res13.WriteByte(0);
            res13.WriteByte(0);
            res13.WriteByte(0);

            res13.WriteByte(0);
            res13.WriteByte(0);
            res13.WriteByte(0); //bool
            res13.WriteByte(0);
            res13.WriteByte(0);
            res13.WriteByte(0);
            res13.WriteByte(0); // 1 = body pink texture
            res13.WriteByte(0);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_item_update_eqmask, res13, ServerType.Area);
        }

    }
}
