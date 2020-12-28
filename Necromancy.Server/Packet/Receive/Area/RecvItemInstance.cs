using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemInstance : PacketResponse
    {
        private readonly InventoryItem _inventoryItem;
        private readonly NecClient _client;

        public RecvItemInstance(InventoryItem inventoryItem, NecClient client)
            : base((ushort)AreaPacketId.recv_item_instance, ServerType.Area)
        {
            _inventoryItem = inventoryItem;
            _client = client;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64((ulong)_inventoryItem.Id);      //SPAWN ID
            res.WriteInt32(_inventoryItem.Item.Id);         //BASE ID
            res.WriteByte(_inventoryItem.Quantity);         //QUANTITY
            res.WriteUInt32(_client.Character.Alignmentid); //STATUSES
            res.WriteFixedString($"{_inventoryItem.Item.ItemType}", 0x10); //UNKNOWN - ITEM TYPE?
            res.WriteByte(_inventoryItem.StorageType);      // STORAGE ZONE
            res.WriteByte(_inventoryItem.BagId);            //BAG
            res.WriteInt16(_inventoryItem.BagSlotIndex);    //SLOT
            res.WriteInt32(_inventoryItem.State);           //bit mask. This indicates where to put items. PREV EQUIP SLOT
            res.WriteInt32(99); //spirit eq mask??? PREV DURABILITY
            res.WriteInt32(0);  //new
            res.WriteByte(0);   //ENHANCEMENT LEVEL?
            res.WriteByte((byte)_client.Character.Alignmentid); //SPECIAL FORGE LEVEL?
            res.WriteCString($"{_inventoryItem.Item.Id}"); // unknown
            res.WriteInt16(0); //PHYSICAL
            res.WriteInt16(0); //MAGICAL
            res.WriteInt32(100); //MAX DURABILITY
            res.WriteByte(5); //HARDNESS
            res.WriteInt32(0); //UNKNOWN

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

            res.WriteInt32((0b1 >> _inventoryItem.BagSlotIndex));
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
