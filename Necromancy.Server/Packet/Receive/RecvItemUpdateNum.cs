using Arrowgene.Services.Buffers;
using Necromancy.Server.Chat;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvItemUpdateNum : PacketResponse
    {
        private readonly ulong _instanceId;
        private readonly byte _count;
        public RecvItemUpdateNum(ulong instanceId, byte count)
            : base((ushort) AreaPacketId.recv_item_update_num, ServerType.Area)
        {
            _instanceId = instanceId;
            _count = count;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(_instanceId);
            res.WriteByte(_count);

            return res;
        }
    }
}
