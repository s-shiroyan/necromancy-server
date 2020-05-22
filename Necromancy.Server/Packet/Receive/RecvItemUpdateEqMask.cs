using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvItemUpdateEqMask : PacketResponse
    {
        private readonly InventoryItem _invItem;
        private readonly int _bitmask;

        public RecvItemUpdateEqMask(InventoryItem invItem, int bitmask)
            : base((ushort) AreaPacketId.recv_item_update_eqmask, ServerType.Area)
        {
            _invItem = invItem;
            _bitmask = bitmask;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
    //     res.WriteUInt64(_invItem.InstanceId);
    //     res.WriteInt32(_bitmask); //Equip bitmask

    //     res.WriteInt32(_invItem.StorageItem.icon);
    //     res.WriteByte(0);
    //     res.WriteByte(0);
    //     res.WriteByte(0);

    //     res.WriteInt32(_invItem.StorageItem.icon);
    //     res.WriteByte(0);
    //     res.WriteByte(0);
    //     res.WriteByte(0);

    //     res.WriteByte(0);
    //     res.WriteByte(0);
    //     res.WriteByte(0); //bool
    //     res.WriteByte(0);
    //     res.WriteByte(0);
    //     res.WriteByte(0);
    //     res.WriteByte(0);
    //     res.WriteByte(0);

            return res;
        }
    }
}
