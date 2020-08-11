using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodySelfRaiseScaleEnd : PacketResponse
    {
        public RecvCharaBodySelfRaiseScaleEnd()
            : base((ushort) AreaPacketId.recv_charabody_self_raisescale_end, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            return res;
        }
    }
}
