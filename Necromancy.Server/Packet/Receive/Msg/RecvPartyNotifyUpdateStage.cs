using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvPartyNotifyUpdateStage : PacketResponse
    {
        public RecvPartyNotifyUpdateStage()
            : base((ushort) MsgPacketId.recv_party_notify_update_stage, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
