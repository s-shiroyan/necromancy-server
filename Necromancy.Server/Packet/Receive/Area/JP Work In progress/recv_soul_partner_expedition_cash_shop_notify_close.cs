using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_soul_partner_expedition_cash_shop_notify_close : PacketResponse
    {
        public recv_soul_partner_expedition_cash_shop_notify_close()
            : base((ushort) AreaPacketId.recv_soul_partner_expedition_cash_shop_notify_close, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //No Structure

            return res;
        }
    }
}
