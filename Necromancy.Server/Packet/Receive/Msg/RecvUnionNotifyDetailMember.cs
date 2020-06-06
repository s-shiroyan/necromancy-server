using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvUnionNotifyDetailMember : PacketResponse
    {
        public RecvUnionNotifyDetailMember()
            : base((ushort) MsgPacketId.recv_union_notify_detail_member, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteFixedString("", 0x31); //size is 0x31
            res.WriteFixedString("", 0x5B); //size is 0x5B
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteFixedString("", 0x61); //size is 0x61
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
