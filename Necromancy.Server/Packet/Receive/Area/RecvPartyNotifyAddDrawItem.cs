using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvPartyNotifyAddDrawItem : PacketResponse
    {
        public RecvPartyNotifyAddDrawItem()
            : base((ushort) AreaPacketId.recv_party_notify_add_draw_item, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0);

            res.WriteFloat(0);

            res.WriteInt32(0);
            return res;
        }
    }
}
