using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemInstance : PacketResponse
    {
        private readonly ItemInstance _itemInstance;
        private readonly NecClient _client;

        public RecvItemInstance(NecClient client, ItemInstance itemInstance)
            : base((ushort)AreaPacketId.recv_item_instance, ServerType.Area)
        {
            _itemInstance = itemInstance;
            _client = client;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64(_itemInstance.InstanceID);              //INSTANCE ID
            res.WriteInt32(_itemInstance.BaseID);                   //BASE ID
            res.WriteByte(_itemInstance.Quantity);                  //QUANTITY
            res.WriteUInt32(_client.Character.AlignmentId);         //STATUSES
            res.WriteFixedString("", 0x10);                         //UNKNOWN - ITEM TYPE?
            res.WriteByte((byte)_itemInstance.Location.Zone);       //STORAGE ZONE
            res.WriteByte(_itemInstance.Location.Bag);              //BAG
            res.WriteInt16(_itemInstance.Location.Slot);            //SLOT
            res.WriteInt32(0);                                      //UNKNOWN
            res.WriteInt32((int)_itemInstance.CurrentEquipSlot);    //EQUIP SLOT
            res.WriteInt32(_itemInstance.CurrentDurability);        //CURRENT DURABILITY
            res.WriteByte(_itemInstance.EnhancementLevel);          //ENHANCEMENT LEVEL?
            res.WriteByte(_itemInstance.SpecialForgeLevel);         //?SPECIAL FORGE LEVEL?
            res.WriteCString(_itemInstance.TalkRingName);           //TALK RING NAME
            res.WriteInt16(_itemInstance.Physical);                 //PHYSICAL
            res.WriteInt16(_itemInstance.Magical);                  //MAGICAL
            res.WriteInt32(_itemInstance.MaximumDurability);        //MAX DURABILITY
            res.WriteByte(_itemInstance.Hardness);                  //HARDNESS
            res.WriteInt32(_itemInstance.Weight);                   //WEIGHT IN THOUSANDTHS

            const int MAX_WHATEVER_SLOTS = 2;
            int numEntries = 2;
            res.WriteInt32(numEntries);                  //less than or equal to 2?
            for (int j = 0; j < numEntries; j++)
            {
                res.WriteInt32((byte)0);                 //unknown
            }

            int numOfGemSlots = 1;
            res.WriteInt32(numOfGemSlots); //number of gem slots
            for (int j = 0; j < numOfGemSlots; j++)
            {
                res.WriteByte(0); //IS FILLED
                res.WriteInt32(0); //GEM TYPE
                res.WriteInt32(0); // GEM BASE ID
                res.WriteInt32(0);                       //maybe gem item 2 id for diamon 2 gem combine 
            }

            //res.WriteInt32((0b1 >> _inventoryItem.BagSlotIndex));
            //res.WriteInt32((0b1 >> _inventoryItem.BagSlotIndex));
            res.WriteInt64(0);//new
            res.WriteInt16(0xff); //0 = green (in shop for sale)  0xFF = normal /*item.ShopStatus*/
            res.WriteInt32(1); //ENCHANT ID
            res.WriteInt16(1); //GP

            numEntries = 5; //new
            res.WriteInt32(numEntries); // less than or equal to 5
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(0);//new
                res.WriteByte(0);//new
                res.WriteByte(0);//new
                res.WriteInt16(0);//new
                res.WriteInt16(0);//new
            }

            res.WriteInt64(0);//new
            res.WriteInt16(0);//new
            res.WriteInt16(0);//new
            res.WriteInt16(0);//new
            res.WriteInt16(0);//new
            res.WriteInt16(0);//new
            res.WriteInt16(0);//new
            res.WriteInt16(0);//new
            res.WriteInt32(0);//new
            res.WriteInt32(0);//new
            res.WriteInt32(0);//new
            res.WriteInt32(0);//new
            res.WriteInt32(0);//new
            res.WriteInt32(0);//new
            res.WriteInt32(0);//new
            res.WriteInt32(0);//new
            res.WriteInt32(0);//new
            res.WriteInt32(0);//new

            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(0);//new
                res.WriteByte(0);//new
                res.WriteByte(0);//new
                res.WriteInt16(0);//new
                res.WriteInt16(0);//new
            }

            res.WriteInt16(-2);//new
            res.WriteInt16(-1);//new
            res.WriteByte(1);//new
            res.WriteByte(2);//new

            res.WriteInt64(-1);//new
            res.WriteInt16(0);//new
            res.WriteInt32(0);//new
            res.WriteInt32(0);//new
            res.WriteInt16(0);//new
            res.WriteInt16(0);//new

            res.WriteInt32(0);//new
            res.WriteInt32(0);//new
            res.WriteInt16(0);//new
            res.WriteInt16(0);//new

            numEntries = 5;
            for (int i = 0; i < 5; i++)
            {
                res.WriteInt16(0);//new
                res.WriteInt32(0);//new
                res.WriteInt32(0);//new
                res.WriteInt16(0);//new
                res.WriteInt16(0);//new
            }

            res.WriteInt16(69);//new

            return res;            
        }
    }
}
