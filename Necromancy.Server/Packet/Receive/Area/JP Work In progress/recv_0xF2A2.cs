using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_0xF2A2 : PacketResponse
    {
        public recv_0xF2A2()
            : base((ushort) AreaPacketId.recv_0xF2A2, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //Unknown no-string shop interface.
            res.WriteInt16(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt16(0);
            return res;
        }
    }
}
