using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvItemUpdateEqMask : PacketResponse
    {
        private readonly InventoryItem _inventoryItem;

        public RecvItemUpdateEqMask(InventoryItem inventoryItem)
            : base((ushort) AreaPacketId.recv_item_update_eqmask, ServerType.Area)
        {
            _inventoryItem = inventoryItem;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64((ulong) _inventoryItem.Id);
            res.WriteInt32((int) _inventoryItem.CurrentEquipmentSlotType);

            res.WriteInt32(_inventoryItem.Item.Id);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteInt32(_inventoryItem.Item.Id);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0); //bool
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            return res;
        }
    }
}
