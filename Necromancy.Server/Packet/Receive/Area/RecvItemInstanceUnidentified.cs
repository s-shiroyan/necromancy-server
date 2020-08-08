using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemInstanceUnidentified : PacketResponse
    {
        private readonly InventoryItem _inventoryItem;

        public RecvItemInstanceUnidentified(InventoryItem inventoryItem)
            : base((ushort) AreaPacketId.recv_item_instance_unidentified, ServerType.Area)
        {
            _inventoryItem = inventoryItem;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64((ulong) _inventoryItem.Id);
            res.WriteCString(_inventoryItem.Item.Name);
            res.WriteInt32((int) _inventoryItem.Item.ItemType);
            res.WriteInt32((int) _inventoryItem.CurrentEquipmentSlotType);
            res.WriteByte(_inventoryItem.Quantity);
            res.WriteInt32(0); //Item status 0 = identified  
            res.WriteInt32(_inventoryItem.Item.Id); //Item icon 50100301 = camp
            res.WriteByte(5);
            res.WriteByte(4);
            res.WriteByte(7);
            res.WriteInt32(8);
            res.WriteByte(1);
            res.WriteByte(2);
            res.WriteByte(9);
            res.WriteByte(4);
            res.WriteByte(5);
            res.WriteByte(0); // bool
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0); // 0 = adventure bag. 1 = character equipment
            res.WriteByte(0); // 0~2
            res.WriteInt16(_inventoryItem.BagSlotIndex);
            res.WriteInt32(0); //bit mask. This indicates where to put items.   e.g. 01 head 010 arm 0100 feet etc (0 for not equipped)
            res.WriteInt64(69);
            res.WriteInt32(59);
            return res;
        }
    }
}
