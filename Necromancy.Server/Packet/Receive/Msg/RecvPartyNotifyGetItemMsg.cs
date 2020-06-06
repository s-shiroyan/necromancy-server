using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvPartyNotifyGetItemMsg : PacketResponse
    {
        public RecvPartyNotifyGetItemMsg()
            : base((ushort) MsgPacketId.recv_party_notify_get_item, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteCString("");//max size 0x49
            res.WriteByte(0);
            return res;
        }
    }
}
