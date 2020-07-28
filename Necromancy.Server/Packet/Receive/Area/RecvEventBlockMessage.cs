using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventBlockMessage : PacketResponse
    {
        public RecvEventBlockMessage()
            : base((ushort) AreaPacketId.recv_event_block_message, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteCString("ToBeFound"); // find max size
            return res;
        }
    }
}
