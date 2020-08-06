using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventSystemMessage : PacketResponse
    {
        public RecvEventSystemMessage()
            : base((ushort) AreaPacketId.recv_event_system_message, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString(""); // Length 0xC01
            return res;
        }
    }
}
