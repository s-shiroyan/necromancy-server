using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Auction_House.Logic;
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
            SearchCriteria searchCriteria = new SearchCriteria();
            searchCriteria.SoulRankMin = packet.Data.ReadByte();
            searchCriteria.SoulRankMax = packet.Data.ReadByte();
            searchCriteria.ForgePriceMin = packet.Data.ReadByte();
            searchCriteria.ForgePriceMax = packet.Data.ReadByte();
            searchCriteria.Quality = (SearchCriteria.Qualities) packet.Data.ReadInt16();
            searchCriteria.Class = (SearchCriteria.Classes) packet.Data.ReadInt16();



            Logger.Info("YEFAS2F");
            int NUMBER_OF_ITEMS_DEBUG = 1;

            for (int i = 0; i < NUMBER_OF_ITEMS_DEBUG; i++)
            {

                IBuffer r1 = BufferProvider.Provide();
                r1.WriteInt64(i + 300); //spawned item iD
                r1.WriteInt32((int)1); //equip slot flags? (int)_inventoryItem.CurrentEquipmentSlotType ?? no clue
                r1.WriteByte((byte)i); //quantity
                r1.WriteInt32((int)ItemStatuses.IsUnidentified); //Item status
                r1.WriteFixedString("TEST THIS", 16);
                r1.WriteByte((byte)ItemZone.AdventureBag); // storage type 0 = adventure bag. 1 = character equipment 2 = Royal bag.
                r1.WriteByte((byte)( i  / 50)); // bag id 0~2
                r1.WriteInt16((short) ( i % 50)); //bag slot
                r1.WriteInt32((int)ItemEquipSlot.None); // equips item to this slot ItemEquipSlot items not in zone adventure bag, character equipment, and royal bag, avatar bag (maybe more) cannot be equipped.
                r1.WriteInt32((int)ItemType.ARMOR_TOPS); //Spirit_eq_mask?? _inventoryItem.State // no clue
                r1.WriteByte((byte) i); // enhancement level +1, +2 etc
                r1.WriteByte(0); //unknown
                r1.WriteCString("FOUR SCORE"); // _inventoryItem.Item.Name find max size 
                r1.WriteInt16((short)i); //unknown
                r1.WriteInt16((short)i); //unknown
                r1.WriteInt32((int)i); //unknown
                r1.WriteByte((byte)i); //unknown
                r1.WriteInt32((int)i); //channel? _client.Character.InstanceId

                const int MAX_WHATEVER_SLOTS = 2;
                const int numEntries = 2;
                r1.WriteInt32(numEntries); // less than or equal to 2
                for (int j = 0; j < numEntries; j++)
                {
                    r1.WriteInt32((byte)i); //Shop related? "can not equip items listed in your shop" Util.GetRandomNumber(1, 10)
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

                r1.WriteInt32(i); //unknown
                r1.WriteInt32(i); //unknown
                r1.WriteInt16((short)i); //unknown
                r1.WriteInt32(i); //enchant id 
                r1.WriteInt16((short)i); // unknown

                IBuffer r0 = BufferProvider.Provide();
                r0.WriteInt64(i + 300); //V | SPAWN ID
                r0.WriteCString("? Shoes"); // V | DISPLAY NAME
                r0.WriteInt32((int)ItemType.ARMOR_TOPS); // V | ITEM TYPE
                r0.WriteInt32((int)ItemEquipSlot.Torso); // equip slot display on icon AND WHERE CLIENT SAYS YOU CAN EQUIP
                r0.WriteByte((byte) 1); //V | QUANTITY
                r0.WriteInt32((int) ItemStatuses.IsUnidentified); //V | STATUS, CURSED / BESSED / ETC
                r0.WriteInt32(11300101); //Item icon 50100301 = camp | base item id | leadher guard 100101 | 50100502 bag medium | 200901 soldier cuirass | 90012001 bag?  always 8

                r0.WriteByte((byte)i); //unknown
                r0.WriteByte((byte)i); //unknown
                r0.WriteByte((byte)i); //unknown

                r0.WriteInt32(i); // base item id? tested a bit
                r0.WriteByte((byte)i); //unknown 
                r0.WriteByte((byte)i); //unknown
                r0.WriteByte((byte)i); //unknown

                r0.WriteByte(0); // Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
                r0.WriteByte(0); //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
                r0.WriteByte(1); // bool
                r0.WriteByte((byte)i); //unknown
                r0.WriteByte((byte)i); //unknown
                r0.WriteByte((byte)i); //unknown
                r0.WriteByte((byte)i); //texture related

                r0.WriteByte((byte)i); //unknown
                    
                r0.WriteByte((byte) ItemZone.AdventureBag); // 0 = adventure bag. 1 = character equipment 2 = Royal bag. _inventoryItem.StorageType
                r0.WriteByte((byte) 0); // 0~2 bag slot?, crashes if no bag equipped in slot
                r0.WriteInt16((short) i); // VERIFIED slot in bag
                r0.WriteInt32((int) ItemEquipSlot.None); // equips item to this slot ItemEquipSlot items not in zone adventure bag, character equipment, and royal bag, avatar bag (maybe more) cannot be equipped.
                r0.WriteInt64(50); //unknown tested a bit maybe sale price?
                r0.WriteInt32(i); //unknown tested a bit something to do with gems?

                Router.Send(client.Map, (ushort)AreaPacketId.recv_item_instance, r1, ServerType.Area);
                //Router.Send(client.Map, (ushort)AreaPacketId.recv_item_instance_unidentified, r0, ServerType.Area);

                IBuffer r8 = BufferProvider.Provide();
                r8.WriteInt64(i + 300); // id?
                r8.WriteInt64(i + 100); // price
                r8.WriteInt64(i + 200); // identify
                r8.WriteInt64(i + 300); // curse?
                r8.WriteInt64(i + 400); // repair?
                Router.Send(client.Map, (ushort)AreaPacketId.recv_shop_notify_item_sell_price, r8, ServerType.Area);

                IBuffer r4 = BufferProvider.Provide();
                r4 = BufferProvider.Provide();
                r4.WriteInt64(i + 300);
                r4.WriteInt32(i); // guard
                //Router.Send(client, (ushort)AreaPacketId.recv_item_update_enchantid, r4, ServerType.Area);

                IBuffer r5 = BufferProvider.Provide();
                r5 = BufferProvider.Provide();
                r5.WriteInt64(i + 300);
                r5.WriteInt16((short) i); // Shwo GP on certain items guard points
                //Router.Send(client, (ushort)AreaPacketId.recv_item_update_ac, r5, ServerType.Area);

                IBuffer r6 = BufferProvider.Provide();
                r6 = BufferProvider.Provide();
                r6.WriteInt64(i + 300); //client.Character.EquipId[x]  put stuff unidentified and get the status equipped  , 0 put stuff identified
                r6.WriteInt32((int)ItemStatuses.Normal);
                //Router.Send(client, (ushort)AreaPacketId.recv_item_update_state, r6, ServerType.Area);

                IBuffer r7 = BufferProvider.Provide();
                r7.WriteInt64(i + 300);
                r7.WriteInt32(i); // for the moment i don't know what it change
                //Router.Send(client, (ushort)AreaPacketId.recv_item_update_date_end_protect, r7, ServerType.Area);

                IBuffer r3 = BufferProvider.Provide();
                r3.WriteInt64(i + 300);
                r3.WriteByte((byte)ItemZone.AdventureBag);
                r3.WriteByte(0);
                r3.WriteInt16((short) i);

                //Router.Send(client.Map, (ushort)AreaPacketId.recv_item_update_place, r3, ServerType.Area);

            }            

            Logger.Info("YEFASF");
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // error

            res.WriteInt32(NUMBER_OF_ITEMS_DEBUG); // number of loops
            
            for (int i = 0; i < NUMBER_OF_ITEMS_DEBUG; i++)
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
            Equipment       = 1, //invisible?
            RoyalBag        = 2,
            Warehouse       = 3,
            UNKNOWN         = 4,
            //probably warehouse expansions?
            UNKNOWN2MAYBEOTHERPLAYERS        = 9, //shows item 
            WarehouseSp     = 10,
            AvatarInventory = 12 //seems to be max
        }

        [Flags]
        private enum ItemRarity
        {
            Poor        = 1 << 1,
            Normal      = 1 << 2,
            Good        = 1 << 3,
            Master      = 1 << 4,
            Legend      = 1 << 5,
            Artifact    = 1 << 6
        }
    }
}
