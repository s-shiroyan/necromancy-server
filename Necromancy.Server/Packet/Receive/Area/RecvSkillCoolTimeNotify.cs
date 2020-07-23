using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSkillCoolTimeNotify : PacketResponse
    {
        public RecvSkillCoolTimeNotify()
            : base((ushort) AreaPacketId.recv_skill_cooltime_notify, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            res.WriteFloat(0);

            res.WriteFloat(0);
            return res;
        }
    }
}
