using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleReportActionStealUnidentified : PacketResponse
    {
        public RecvBattleReportActionStealUnidentified()
            : base((ushort) AreaPacketId.recv_battle_report_action_steal_unidentified, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            res.WriteInt32(0);

            res.WriteCString("ToBeFound"); // most likely unidentified item name
            return res;
        }
    }
}
