using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodySelfSalvageNotify : PacketResponse
    {
        public RecvCharaBodySelfSalvageNotify()
            : base((ushort) AreaPacketId.recv_charabody_self_salvage_notify, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString("");//Length is 31-02=2F=DEC47
            res.WriteCString("");//Length is 5b-01=5A=Dec132
            return res;
        }
    }
}
