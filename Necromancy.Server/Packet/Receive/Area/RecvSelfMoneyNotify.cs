using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSelfMoneyNotify : PacketResponse
    {
        private readonly long _currentGold;
        public RecvSelfMoneyNotify(NecClient client, long currentGold)
            : base((ushort) AreaPacketId.recv_self_money_notify, ServerType.Area)
        {
            _currentGold = currentGold;
            Clients.Add(client);
        }
        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(_currentGold);
            return res;
        }
    }
}
