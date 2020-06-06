using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvPartyNotifyEstablish : PacketResponse
    {
        public RecvPartyNotifyEstablish()
            : base((ushort) MsgPacketId.recv_party_notify_establish, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            for (int i = 0; i < 4; i++)
            {
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString("", 0x31); //size is 0x31
                res.WriteFixedString("", 0x5B); //size is 0x5B
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);//bool
                res.WriteByte(0);
                res.WriteByte(0);
            }
            res.WriteByte(0);
            return res;
        }
    }
}
