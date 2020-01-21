using Arrowgene.Services.Buffers;
using Necromancy.Server.Chat;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvItemUpdateLevel : PacketResponse
    {
        private readonly ulong _instanceId;
        private readonly byte _level;
        public RecvItemUpdateLevel(ulong instanceId, byte level)
            : base((ushort) AreaPacketId.recv_item_update_level, ServerType.Area)
        {
            _instanceId = instanceId;
            _level = level;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(_instanceId);
            res.WriteByte(_level);

            return res;
        }
    }
}
