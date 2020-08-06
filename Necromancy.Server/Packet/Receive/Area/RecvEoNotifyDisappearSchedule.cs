using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEoNotifyDisappearSchedule : PacketResponse
    {
        public RecvEoNotifyDisappearSchedule()
            : base((ushort) AreaPacketId.recv_eo_notify_disappear_schedule, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteFloat(0);
            return res;
        }
    }
}
