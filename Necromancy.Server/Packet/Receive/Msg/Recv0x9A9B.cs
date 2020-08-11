using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class Recv0x9A9B : PacketResponse
    {
        public Recv0x9A9B()
            : base((ushort) MsgPacketId.recv_0x9A9B, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString("");//max size 0x364
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteFixedString("", 0x31); //size is 0x31
            res.WriteFixedString("", 0x25); //size is 0x25
            res.WriteFixedString("", 0x301); //size is 0x301
            return res;
        }
    }
}
