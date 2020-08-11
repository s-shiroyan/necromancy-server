using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvTradeNotifyMoney : PacketResponse
    {
        public RecvTradeNotifyMoney()
            : base((ushort) AreaPacketId.recv_trade_notify_money, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0);
            return res;
        }
    }
}
