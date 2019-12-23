using Arrowgene.Services.Buffers;
using Necromancy.Server.Chat;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Response
{
    public class RecvBattleReportNoactDead : PacketResponse
    {
        private readonly int _instancedId;
        public RecvBattleReportNoactDead(int instancedId)
            : base((ushort) AreaPacketId.recv_battle_report_noact_notify_dead, ServerType.Area)
        {
            _instancedId = instancedId;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_instancedId);
            res.WriteInt32(2); //Death int, needs to be 2 for Playable Characters
            res.WriteInt32(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
