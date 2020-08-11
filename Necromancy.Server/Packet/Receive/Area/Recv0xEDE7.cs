using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class Recv0xEDE7 : PacketResponse
    {
        public Recv0xEDE7()
            : base((ushort) AreaPacketId.recv_0xEDE7, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            
            return res;
        }
    }
}
