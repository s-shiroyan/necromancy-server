using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvPartyNotifyUpdateMap : PacketResponse
    {
        public RecvPartyNotifyUpdateMap()
            : base((ushort) MsgPacketId.recv_party_notify_update_map, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteCString("");// max of 0x61 (character name i think)
            return res;
        }
    }
}
