using Arrowgene.Services.Buffers;
using Necromancy.Server.Chat;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvItemUpdatePlace : PacketResponse
    {
        private readonly ulong _instanceId;
        private readonly byte _toType;
        private readonly byte _toBagId;
        private readonly short _toSlot;
        public RecvItemUpdatePlace(ulong instanceId, byte toType, byte toBagId, short toSlot)
            : base((ushort) AreaPacketId.recv_item_update_place, ServerType.Area)
        {
            _instanceId = instanceId;
            _toType = toType;
            _toBagId = toBagId;
            _toSlot = toSlot;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(_instanceId);
            res.WriteByte(_toType);
            res.WriteByte(_toBagId);
            res.WriteInt16(_toSlot);

            return res;
        }
    }
}
