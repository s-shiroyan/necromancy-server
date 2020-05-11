using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

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
            res.WriteUInt64(_instanceId);
            res.WriteInt32(0); //Equip bitmask

            res.WriteInt32(10200101);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteInt32(0);
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
