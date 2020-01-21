using Arrowgene.Services.Buffers;
using Necromancy.Server.Chat;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvItemRemove : PacketResponse
    {
        private readonly ulong _instanceId;
        public RecvItemRemove(ulong instanceId)
            : base((ushort) AreaPacketId.recv_item_remove, ServerType.Area)
        {
            _instanceId = instanceId;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(_instanceId); // 0 = normal 1 = cinematic
            return res;
        }
    }
}
