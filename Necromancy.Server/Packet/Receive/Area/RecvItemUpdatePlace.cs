using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemUpdatePlace : PacketResponse
    {
        public RecvItemUpdatePlace()
            : base((ushort) AreaPacketId.recv_item_update_place, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0); // item id

            res.WriteByte(0);	
            res.WriteByte(0);	
            res.WriteInt16(0);
            return res;
        }
    }
}
