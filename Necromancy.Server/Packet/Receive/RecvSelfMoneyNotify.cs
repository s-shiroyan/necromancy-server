using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvSelfMoneyNotify : PacketResponse
    {
        private readonly long _gold;

        public RecvSelfMoneyNotify(long gold)
            : base((ushort) AreaPacketId.recv_self_money_notify, ServerType.Area)
        {
            _gold = gold;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(_gold);

            return res;
        }
    }
}
