using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_battle_report_notify_partner_exp : PacketResponse
    {
        public recv_battle_report_notify_partner_exp()
            : base((ushort) AreaPacketId.recv_battle_report_notify_partner_exp, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0);
            res.WriteCString("What");
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteByte(0);
            return res;
        }
    }
}
