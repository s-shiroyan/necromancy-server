using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventSystemMessageTimer : PacketResponse
    {
        public RecvEventSystemMessageTimer()
            : base((ushort) AreaPacketId.recv_event_system_message_timer, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString("ToBeFound");
            return res;
        }
    }
}
