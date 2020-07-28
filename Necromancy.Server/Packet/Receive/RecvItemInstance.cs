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
        private readonly NecClient _client;

        public RecvItemInstance(InventoryItem inventoryItem, NecClient client)
            : base((ushort) AreaPacketId.recv_item_instance, ServerType.Area)
        {
            _inventoryItem = inventoryItem;
            _client = client;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteUInt64((ulong)_inventoryItem.Id);
            res.WriteInt32((int)_inventoryItem.CurrentEquipmentSlotType);
            res.WriteByte(_inventoryItem.Quantity);
            res.WriteInt32(_inventoryItem.Id); //?????
            res.WriteFixedString($"TEST3", 0x10);
            res.WriteByte(_inventoryItem.StorageType); // 0 = adventure bag. 1 = character equipment 2 = Royal bag.
            res.WriteByte(_inventoryItem.BagId); // 0~2
            res.WriteInt16(_inventoryItem.BagSlotIndex);
            res.WriteInt32(_inventoryItem.State); //bit mask. This indicates where to put items. 
            res.WriteInt32((int)_client.Character.InstanceId);
            res.WriteByte(1);
            res.WriteByte(1);
            res.WriteCString(_inventoryItem.Item.Name); // find max size 
            res.WriteInt16(999);
            res.WriteInt16(2999);
            res.WriteInt32(19);
            res.WriteByte(1);
            res.WriteInt32((int)_client.Character.InstanceId);
            int numEntries = 2;
            res.WriteInt32(numEntries); // less than or equal to 2
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(Util.GetRandomNumber(1,10)); //Shop related? "can not equip items listed in your shop"
            }

            numEntries = 3; //gem SLOTS
            res.WriteInt32(numEntries); // Count of Gem Slots. less than or equal to 3
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteByte((byte)(Util.GetRandomNumber(0,2))); //Gem in slot : yes or no
                res.WriteInt32(i+1); //slot type 1 round, 2 triangle, 3 diamond
                res.WriteInt32(1111);// theory GEM ID 1
                res.WriteInt32(2222);// theory gem id 2.  Diamonds were two Gems combined to one
            }

            res.WriteInt32(20);
            res.WriteInt32(200);
            res.WriteInt16(1000);
            res.WriteInt32(0); //1 here lables the item "Gaurd".   no effect from higher numbers
            res.WriteInt16(2000);



            return res;
        }
    }
}
