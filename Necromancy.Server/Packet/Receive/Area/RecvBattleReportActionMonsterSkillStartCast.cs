using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleReportActionMonsterSkillStartCast : PacketResponse
    {
        public RecvBattleReportActionMonsterSkillStartCast()
            : base((ushort) AreaPacketId.recv_battle_report_action_monster_skill_start_cast, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteFloat(0);
            return res;
        }
    }
}
