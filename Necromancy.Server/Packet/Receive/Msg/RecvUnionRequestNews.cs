using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvUnionRequestNews : PacketResponse
    {
        public RecvUnionRequestNews()
            : base((ushort) MsgPacketId.recv_union_request_news_r, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0x3E8); //less than or equal to 0x3E8
            for (int i = 0; i < 0x3E8; i++) //limit is the int32 above
            {
                res.WriteInt32(0);
                res.WriteFixedString("", 0x31); //soul name?
                res.WriteFixedString("", 0x5B); //character name?
                res.WriteInt32(0);
                res.WriteFixedString("", 0x49);
                res.WriteFixedString("", 0x49);
                res.WriteInt32(0);
            }
            return res;
        }
    }
}
