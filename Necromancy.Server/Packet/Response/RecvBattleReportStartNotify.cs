using Arrowgene.Services.Buffers;
using Necromancy.Server.Chat;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Response
{
    public class RecvBattleReportStartNotify : PacketResponse
    {
        private readonly int _instanceId;
        public RecvBattleReportStartNotify(int instanceId)
            : base((ushort) AreaPacketId.recv_battle_report_start_notify, ServerType.Area)
        {
            _instanceId = instanceId;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_instanceId);
            return res;
        }
    }
}
