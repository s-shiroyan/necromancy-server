using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventMessageNoObject : PacketResponse
    {
        public RecvEventMessageNoObject()
            : base((ushort) AreaPacketId.recv_event_message_no_object, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString("");//need to find max size
            res.WriteCString("");//need to find max size
            res.WriteCString("");//need to find max size
            return res;
        }
    }
}
