using Arrowgene.Services.Buffers;
using Necromancy.Server.Chat;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvItemUpdateEqMask : PacketResponse
    {
        private readonly ulong _instanceId;

        public RecvItemUpdateEqMask(ulong instanceId)
            : base((ushort) AreaPacketId.recv_item_update_eqmask, ServerType.Area)
        {
            _instanceId = instanceId;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(_instanceId);
            res.WriteInt32(2);

            res.WriteInt32(10200101);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteInt32(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(1);//bool
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            return res;
        }
    }
}
