using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;
using System;
using System.Text;

namespace Necromancy.Server.Systems.Auction_House
{
    public class send_auction_search : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_auction_search));

        public send_auction_search(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_auction_search;

        public override void Handle(NecClient client, NecPacket packet)
        {

            int NUMBER_OF_ITEMS_DEBUG = 8;

            for (int i = 0; i < NUMBER_OF_ITEMS_DEBUG; i++)
            {
                IBuffer r0 = BufferProvider.Provide();
                r0.WriteInt64(i + 300); //spawned item id
                r0.WriteCString("Soldier Cuirass " + i.ToString()); // name
                r0.WriteInt32((int)ItemType.ARMOR_TOPS); // type
                r0.WriteInt32((int)ItemEquipSlot.Torso); // equip slot display on icon
                r0.WriteByte(1); //quantity?
                r0.WriteInt32((int)ItemStatuses.Normal); //statuses
                r0.WriteInt32(200901); //Item icon 50100301 = camp | base item id | leadher guard 100101 | 50100502 bag medium | 200901 soldier cuirass

                r0.WriteByte(5); //unknown
                r0.WriteByte(5); //unknown
                r0.WriteByte(5); //unknown

                r0.WriteInt32(0); // base item id?
                r0.WriteByte(1); //unknown 
                r0.WriteByte(1); //unknown
                r0.WriteByte(1); //unknown

                r0.WriteByte(0); // Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
                r0.WriteByte(0); //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
                r0.WriteByte(1); // bool
                r0.WriteByte(1); //unknown
                r0.WriteByte(1); //unknown
                r0.WriteByte(1); //unknown
                r0.WriteByte(4); //texture related

                r0.WriteByte(1); //unknown

                r0.WriteByte((byte)i); // 0 = adventure bag. 1 = character equipment 2 = Royal bag. _inventoryItem.StorageType
                r0.WriteByte((byte) 0); // 0~2 bag slot?, crashes if no bag equipped in slot
                r0.WriteInt16((short)1); // VERIFIED slot in bag
                r0.WriteInt32((int)ItemEquipSlot.Belt); // equips item to this slot ItemEquipSlot items not in zone adventure bag, character equipment, and royal bag (maybe more) cannot be equipped.
                r0.WriteInt64(long.MaxValue); //unknown
                r0.WriteInt32(1); //unknown

                Router.Send(client.Map, (ushort)AreaPacketId.recv_item_instance_unidentified, r0, ServerType.Area);

                //IBuffer r1 = BufferProvider.Provide();
                //r1.WriteInt64(i + 300); //spawned item iD
                //r1.WriteInt32(0); //equip slot flags? (int)_inventoryItem.CurrentEquipmentSlotType
                //r1.WriteByte((byte)1); //quantity
                //r1.WriteInt32(1050102); //base item id not? 1050102
                //r1.WriteFixedString($"ITEM NAME", 0x10);
                //r1.WriteByte((byte)1); // storage type 0 = adventure bag. 1 = character equipment 2 = Royal bag.
                //r1.WriteByte((byte)0); // bag id 0~2
                //r1.WriteInt16((short)5); //bag slot
                //r1.WriteInt32(64); //bit mask. This indicates where to put items. _inventoryItem.State
                //r1.WriteInt32(2); //Spirit_eq_mask?? _inventoryItem.State
                //r1.WriteByte((byte)2); // unknown
                //r1.WriteByte((byte)2); //unknown
                //r1.WriteCString("ITEM NAME 2"); // _inventoryItem.Item.Name find max size 
                //r1.WriteInt16((short)0); //unknown
                //r1.WriteInt16((short)0); //unknown
                //r1.WriteInt32(0); //unknown
                //r1.WriteByte((byte)0); //unknown
                //r1.WriteInt32((int)2); //channel? _client.Character.InstanceId

                //const int MAX_WHATEVER_SLOTS = 2;
                //const int numEntries = 2;
                //r1.WriteInt32(numEntries); // less than or equal to 2
                //for (int j = 0; j < numEntries; j++)
                //{
                //    r1.WriteInt32(1); //Shop related? "can not equip items listed in your shop" Util.GetRandomNumber(1, 10)
                //}

                //const int MAX_GEM_SLOTS = 3;
                //const int numGemSlots = 3;
                //r1.WriteInt32(numGemSlots); //VERIFIED Count of Gem Slots. less than or equal to 3
                //for (int j = 0; j < numGemSlots; j++)
                //{
                //    r1.WriteByte(1); //VERIFIED GEM SLOT ACTIVE
                //    r1.WriteInt32(4); //slot type 1 round, 2 triangle, 3 diamond
                //    r1.WriteInt32(-1);// theory GEM ID 1
                //    r1.WriteInt32(-1);// theory gem id 2.  Diamonds were two Gems combined to one
                //}

                //r1.WriteInt32(353453); //unknown
                //r1.WriteInt32(34534534); //unknown
                //r1.WriteInt16((short)i); //unknown
                //r1.WriteInt32(i); //1 here lables the item "Gaurd".   no effect from higher numbers
                //r1.WriteInt16((short)i); // unknown

                //Router.Send(client.Map, (ushort)AreaPacketId.recv_item_instance, r1, ServerType.Area);


            }

            Logger.Info("YEFASF");
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // error

            res.WriteInt32(NUMBER_OF_ITEMS_DEBUG); // number of loops

            for (int i = 0; i < 1; i++)
            {
                string hellothere = i.ToString() + " " + Convert.ToString(i, 2).PadLeft(8, '0'); ;
                res.WriteInt32(i); //row identifier 
                res.WriteInt64(i + 300); //spawned item id
                res.WriteInt32(17); // Lowest
                res.WriteInt32(500); // Buy Now
                res.WriteFixedString(hellothere, 49); // Soul Name Of Sellers
                res.WriteByte(8); // 0 = nothing.    Other = Logo appear. maybe it's effect or rank, or somethiung else ?
                res.WriteFixedString("0 000000000 1111111111 2222222222 3333333333 44444444444 5555555555 6666666666 7777777777 88888888888 9999999999 1 000000000 1111111111 2222222222 3333333333 44444444444 5555555555 6666666666 7777777777 88888888888 9999999999 2 000000000 1111111111 2222222222 3333333333 44444444444 5555555555 6666666666 7777777777 88888888888 9999999999 3 000000000 1111111111 2222222222", 385); // Item Comment
                res.WriteInt16(0); // Bid
                res.WriteInt32(1000); // Item remaining time
            }

            Router.Send(client.Map, (ushort) AreaPacketId.recv_auction_search_r, res, ServerType.Area);
        }

        [Flags]
        private enum ItemStatuses
        {
            IsUnidentified  = 1 << 0, //unidentified, cursed and blessed first bit is identified or not
            Normal          = 1 << 1, //does nothing use if you want a normal item no other flags
            Broken          = 1 << 2, 
            Cursed          = 1 << 3,
            Blessed         = 1 << 4            
        }

        [Flags]
        private enum ItemEquipSlot
        {
            None            = 0,
            RightHand       = 1 << 0, 
            LeftHand        = 1 << 1, 
            Quiver          = 1 << 2, 
            Head            = 1 << 3, 
            Torso           = 1 << 4,
            Legs            = 1 << 5,
            Arms            = 1 << 6,
            Feet            = 1 << 7,
            Cape            = 1 << 8,
            Ring            = 1 << 9, 
            Earring         = 1 << 10,
            Necklace        = 1 << 11, 
            Belt            = 1 << 12, 
            Talkring        = 1 << 13,
            AvatarHead      = 1 << 14,
            AvatarTorso     = 1 << 15,
            AvatarLegs      = 1 << 16,
            AvatarArms      = 1 << 17,
            AvatarFeet      = 1 << 18,
            TwoHanded       = RightHand | LeftHand
        }

        [Flags]
        private enum ItemZone {
            AdventureBag    = 0,
            Equipment       = 1,
            RoyalBag        = 2,
            Warehouse       = 3,
            WarehouseSp     = 10,
            AvatarInventory = 12
        }
    }
}
