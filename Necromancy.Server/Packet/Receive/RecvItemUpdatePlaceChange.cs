using Arrowgene.Services.Buffers;
using Necromancy.Server.Chat;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvItemUpdatePlaceChange : PacketResponse
    {
        private readonly int _instanceId;
        private readonly MoveItem _moveItem;
        public RecvItemUpdatePlaceChange(int instanceId, MoveItem moveItem)
            : base((ushort) AreaPacketId.recv_item_update_place_change, ServerType.Area)
        {
            _instanceId = instanceId;
            _moveItem = moveItem;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(_moveItem.InstanceId);
            res.WriteByte(_moveItem.fromStoreType);
            res.WriteByte(_moveItem.fromBagId);
            res.WriteInt16(_moveItem.fromSlot);
            res.WriteInt64(_moveItem.InstanceId);
            res.WriteByte(_moveItem.toStoreType);
            res.WriteByte(_moveItem.toBagId);
            res.WriteInt16(_moveItem.toSlot);

            return res;
        }
    }
}
