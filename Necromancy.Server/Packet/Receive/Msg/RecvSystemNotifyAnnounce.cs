using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvSystemNotifyAnnounce : PacketResponse
    {
        public RecvSystemNotifyAnnounce()
            : base((ushort) MsgPacketId.recv_system_notify_announce, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString("");//max size 0x301
            res.WriteByte(0);
            return res;
        }
    }
}
