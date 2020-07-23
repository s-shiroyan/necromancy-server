using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvItemInstance : PacketResponse
    {
        private readonly InventoryItem _inventoryItem;

        public RecvItemInstance(InventoryItem inventoryItem)
            : base((ushort) AreaPacketId.recv_item_instance, ServerType.Area)
        {
            _inventoryItem = inventoryItem;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteUInt64((ulong)_inventoryItem.Id);
            res.WriteInt32((int)_inventoryItem.CurrentEquipmentSlotType);
            res.WriteByte(_inventoryItem.Quantity);
            res.WriteInt32(0b1111111110000000); //Item status 0 = identified  //bitmask . cursed, blessed, etc 0001 Shows correct icon in bag, 0010 unknown, 0100 shows broken, 1000 shows cursed, 1 0000 shows blessed
            res.WriteFixedString($"TEST3", 0x10);
            res.WriteByte(_inventoryItem.StorageType); // 0 = adventure bag. 1 = character equipment 2 = Royal bag.
            res.WriteByte(_inventoryItem.BagId); // 0~2
            res.WriteInt16(_inventoryItem.BagSlotIndex);
            res.WriteInt32(_inventoryItem.State); //bit mask. This indicates where to put items. 
            res.WriteInt32(12341234);
            res.WriteByte(10);
            res.WriteByte(10);
            res.WriteCString("ThisIsThis"); // find max size 
            res.WriteInt16(55);
            res.WriteInt16(66);
            res.WriteInt32((int)_inventoryItem.CurrentEquipmentSlotType);
            res.WriteByte(_inventoryItem.Quantity);
            res.WriteInt32(int.MaxValue);
            int numEntries = 2;
            res.WriteInt32(numEntries); // less than or equal to 2
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(Util.GetRandomNumber(1,255));
            }

            numEntries = 3; //gem SLOTS
            res.WriteInt32(numEntries); // Count of Gem Slots. less than or equal to 3
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteByte(1); //Gem in slot : yes or no
                res.WriteInt32(i+1); //slot type 1 round, 2 triangle, 3 diamond
                res.WriteInt32(0b11111111);// theory GEM ID 1
                res.WriteInt32(1);// theory gem id 2.  Diamonds were two Gems combined to one
            }

            res.WriteInt32(1);
            res.WriteInt32(2);
            res.WriteInt16(44);
            res.WriteInt32(0b11111111); //1 here lables the item "Gaurd".   no effect from higher numbers
            res.WriteInt16(22);



            return res;
        }
    }
}
