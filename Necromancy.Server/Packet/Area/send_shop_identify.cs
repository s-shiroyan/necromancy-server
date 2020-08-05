using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_shop_identify : ClientHandler
    {
        public send_shop_identify(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort)AreaPacketId.send_shop_identify;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int itemId = 10200101;

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_shop_identify_r, res, ServerType.Area);

            SendItemInstanceUnidentified(client, itemId);
        }

        private void SendItemInstanceUnidentified(NecClient client, int itemId)
        {
            //recv_item_instance_unidentified = 0xD57A,

            IBuffer res = BufferProvider.Provide();

            res.WriteInt64(itemId); //Item Object ID 

            res.WriteCString("DAGGER 10200101"); //Name

            res.WriteInt32(2); //Wep type

            res.WriteInt32(1); //Bit mask designation? (Only lets you apply this to certain slots dependant on what you send here) 1 for right hand, 2 for left hand

            res.WriteByte(100); //Number of items

            res.WriteInt32(0); //Item status 0 = identified  (same as item status inside senditeminstance)

            res.WriteInt32(10200101); //Item icon
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0); // bool
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteByte(0); // 0 = adventure bag. 1 = character equipment
            res.WriteByte(0); // 0~2
            res.WriteInt16(3); // bag index

            res.WriteInt32(0); //bit mask. This indicates where to put items.   e.g. 01 head 010 arm 0100 feet etc (0 for not equipped)

            res.WriteInt64(-1);

            res.WriteInt32(1);

            Router.Send(client, (ushort)AreaPacketId.recv_item_instance_unidentified, res, ServerType.Area);

            /*IBuffer res30 = BufferProvider.Provide();
            res30.WriteInt64(itemId);
            res30.WriteInt32(100); // MaxDura points
            Router.Send(client, (ushort) AreaPacketId.recv_item_update_maxdur, res30, ServerType.Area);

            //recv_item_update_durability = 0x1F5A, 
            IBuffer res31 = BufferProvider.Provide();
            res31.WriteInt64(itemId);
            res31.WriteInt32(10);
            Router.Send(client, (ushort) AreaPacketId.recv_item_update_durability, res31, ServerType.Area);

            IBuffer res4 = BufferProvider.Provide();
            res4.WriteInt64(itemId);
            res4.WriteInt32(27); // Weight points
            Router.Send(client, (ushort) AreaPacketId.recv_item_update_weight, res4, ServerType.Area);

            IBuffer res5 = BufferProvider.Provide();
            res5.WriteInt64(itemId);
            res5.WriteInt16(51); // Defense and attack points
            Router.Send(client, (ushort) AreaPacketId.recv_item_update_physics, res5, ServerType.Area);

            IBuffer res6 = BufferProvider.Provide();
            res6.WriteInt64(itemId);
            res6.WriteInt16(69); // Magic def and attack Points
            Router.Send(client, (ushort) AreaPacketId.recv_item_update_magic, res6, ServerType.Area);

            IBuffer res7 = BufferProvider.Provide();
            res7.WriteInt64(itemId);
            res7.WriteInt32(26); // for the moment i don't know what it change
            //Router.Send(client, (ushort) AreaPacketId.recv_item_update_enchantid, res7, ServerType.Area);

            IBuffer res8 = BufferProvider.Provide();
            res8.WriteInt64(itemId);
            res8.WriteInt16(23); // Shwo GP on certain items
            Router.Send(client, (ushort) AreaPacketId.recv_item_update_ac, res8, ServerType.Area);

            IBuffer res9 = BufferProvider.Provide();
            res9.WriteInt64(itemId);
            res9.WriteInt32(30); // for the moment i don't know what it change
            Router.Send(client, (ushort) AreaPacketId.recv_item_update_date_end_protect, res9, ServerType.Area);


            IBuffer res11 = BufferProvider.Provide();
            res11.WriteInt64(itemId);
            res11.WriteByte(50); // Hardness
            Router.Send(client, (ushort) AreaPacketId.recv_item_update_hardness, res11, ServerType.Area);


            IBuffer res1 = BufferProvider.Provide();
            res1.WriteInt64(itemId); //client.Character.EquipId[x]   put stuff unidentified and get the status equipped  , 0 put stuff identified
            res1.WriteInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_item_update_state, res1, ServerType.Area);*/
        }

    }
}
