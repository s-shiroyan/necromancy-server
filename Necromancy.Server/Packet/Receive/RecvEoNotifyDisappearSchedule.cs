using Arrowgene.Services.Buffers;
using Necromancy.Server.Chat;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvEoNotifyDisappearSchedule : PacketResponse
    {
        private readonly int _instanceId;
        private readonly float _disappearTime;
        public RecvEoNotifyDisappearSchedule(int instanceId, float disappearTime)
            : base((ushort) AreaPacketId.recv_eo_notify_disappear_schedule, ServerType.Area)
        {
            _instanceId = instanceId;
            _disappearTime = disappearTime;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_instanceId);
            res.WriteFloat(_disappearTime);
            return res;
        }
    }
}
