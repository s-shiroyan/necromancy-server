using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvNormalSystemMessage : PacketResponse
    {
        public RecvNormalSystemMessage()
            : base((ushort) AreaPacketId.recv_normal_system_message, ServerType.Area)
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
