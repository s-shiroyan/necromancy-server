using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_shop_identify : ClientHandler
    {

        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_shop_identify));
        public send_shop_identify(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort)AreaPacketId.send_shop_identify;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte itemZone = packet.Data.ReadByte();
            byte itemBag = packet.Data.ReadByte();
            byte itemSlot = packet.Data.ReadByte();

            //Logger.Debug(client, itemId + " ItemID Identify");
            IBuffer r1 = BufferProvider.Provide();
            r1.WriteInt64(300); //spawned item iD
            r1.WriteInt32((int)11300101); //equip slot flags? (int)_inventoryItem.CurrentEquipmentSlotType ?? no clue
            r1.WriteByte((byte)1); //quantity
            r1.WriteInt32((int)0); //Item status
            r1.WriteFixedString("? Shoes", 16);
            r1.WriteByte((byte)itemZone); // V | ItemZone - storage type 0 = adventure bag. 1 = character equipment 2 = Royal bag.
            r1.WriteByte((byte)itemBag); // V | bag id 0~2
            r1.WriteInt16((short)itemSlot); // V | bag slot
            r1.WriteInt32((int)2); // V | ItemEquipSlot - equips item to this slot ItemEquipSlot items not in zone adventure bag, character equipment, and royal bag, avatar bag (maybe more) cannot be equipped.
            r1.WriteInt32((int)2); //Spirit_eq_mask?? _inventoryItem.State // no clue
            r1.WriteByte((byte)3); // V | enhancement level +1, +2 etc
            r1.WriteByte(0); //unknown
            r1.WriteCString(""); // _inventoryItem.Item.Name find max size 
            r1.WriteInt16((short)001); //unknown
            r1.WriteInt16((short)01); //unknown
            r1.WriteInt32((int)2); //unknown
            r1.WriteByte(1); //unknown
            r1.WriteInt32((int)2); //channel? _client.Character.InstanceId

            const int MAX_WHATEVER_SLOTS = 2;
            const int numEntries = 2;
            r1.WriteInt32(numEntries); // less than or equal to 2
            for (int j = 0; j < numEntries; j++)
            {
                r1.WriteInt32(0); //Shop related? "can not equip items listed in your shop" Util.GetRandomNumber(1, 10)
            }

            const int MAX_GEM_SLOTS = 3;
            const int numGemSlots = 1;
            r1.WriteInt32(numGemSlots); //VERIFIED Count of Gem Slots. less than or equal to 3
            for (int j = 0; j < numGemSlots; j++)
            {
                r1.WriteByte(1); //VERIFIED GEM SLOT ACTIVE
                r1.WriteInt32(3); //slot type 1 round, 2 triangle, 3 diamond
                r1.WriteInt32(0);// theory GEM ID 1
                r1.WriteInt32(0);// theory gem id 2.  Diamonds were two Gems combined to one
            }

            r1.WriteInt32(0); //unknown
            r1.WriteInt32(0); //unknown
            r1.WriteInt16((short)0); //unknown
            r1.WriteInt32(0); //enchant id 
            r1.WriteInt16((short)0); // unknown


            IBuffer r0 = BufferProvider.Provide();
            r0.WriteInt64(300); //V | SPAWN ID
            r0.WriteCString("Durka Durka"); // V | DISPLAY NAME
            r0.WriteInt32((int)ItemType.ARMOR_TOPS); // V | ITEM TYPE
            r0.WriteInt32((int)(1 << 4)); // equip slot display on icon AND WHERE CLIENT SAYS YOU CAN EQUIP
            r0.WriteByte((byte)1); //V | QUANTITY
            r0.WriteInt32((int)1); //V | STATUS, CURSED / BESSED / ETC
            r0.WriteInt32(11300101); //Item icon 50100301 = camp | base item id | leadher guard 100101 | 50100502 bag medium | 200901 soldier cuirass | 90012001 bag?  always 8

            r0.WriteByte((byte)1); //unknown
            r0.WriteByte((byte)1); //unknown
            r0.WriteByte((byte)1); //unknown

            r0.WriteInt32(11300101); // base item id? tested a bit
            r0.WriteByte((byte)1); //unknown 
            r0.WriteByte((byte)1); //unknown
            r0.WriteByte((byte)1); //unknown

            r0.WriteByte(0); // Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
            r0.WriteByte(0); //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
            r0.WriteByte(1); // bool
            r0.WriteByte((byte)1); //unknown
            r0.WriteByte((byte)1); //unknown
            r0.WriteByte((byte)1); //unknown
            r0.WriteByte((byte)1); //texture related

            r0.WriteByte((byte)1); //unknown

            r0.WriteByte((byte)0); // 0 = adventure bag. 1 = character equipment 2 = Royal bag. _inventoryItem.StorageType
            r0.WriteByte((byte)itemBag); // 0~2 bag slot?, crashes if no bag equipped in slot
            r0.WriteInt16((short) itemSlot); // VERIFIED slot in bag
            r0.WriteInt32((int)0); // equips item to this slot ItemEquipSlot items not in zone adventure bag, character equipment, and royal bag, avatar bag (maybe more) cannot be equipped.
            r0.WriteInt64(long.MaxValue); //unknown tested a bit maybe sale price?
            r0.WriteInt32(1); //unknown tested a bit something to do with gems?

            //Router.Send(client.Map, (ushort)AreaPacketId.recv_item_instance, r1, ServerType.Area);
            

           // Router.Send(client.Map, (ushort)AreaPacketId.recv_item_instance, r1, ServerType.Area);

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //error message 1 "You cannot identify items you have already identified
            Router.Send(client.Map, (ushort)AreaPacketId.recv_shop_identify_r, res, ServerType.Area);

            Router.Send(client.Map, (ushort)AreaPacketId.recv_item_instance_unidentified, r0, ServerType.Area);

            //SendItemInstanceUnidentified(client, itemId);

            res = BufferProvider.Provide();
            res.WriteInt64(300);
            res.WriteInt32(0); // State bitmask
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_state, res, ServerType.Area);
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
