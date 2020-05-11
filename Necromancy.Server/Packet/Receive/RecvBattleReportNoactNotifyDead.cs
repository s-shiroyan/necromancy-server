using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvBattleReportNoactDead : PacketResponse
    {
        private readonly uint _instancedId;
        private readonly int _state;

        public RecvBattleReportNoactDead(uint instancedId, int state)
            : base((ushort) AreaPacketId.recv_battle_report_noact_notify_dead, ServerType.Area)
        {
            _instancedId = instancedId;
            _state = state;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_instancedId);
            res.WriteInt32(_state); //Death int
            res.WriteInt32(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
