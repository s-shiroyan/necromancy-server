using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodySelfNotifyDeadNextTime : PacketResponse
    {
        public RecvCharaBodySelfNotifyDeadNextTime()
            : base((ushort) AreaPacketId.recv_charabody_self_notify_deadnext_time, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // time dead length
            return res;
        }
    }
}
