using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharabodySelfSalvageRequestCancel : PacketResponse
    {
        public RecvCharabodySelfSalvageRequestCancel()
            : base((ushort) AreaPacketId.recv_charabody_self_salvage_request_cancel, ServerType.Area)
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
