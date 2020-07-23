using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvMapFragmentFlag : PacketResponse
    {
        public RecvMapFragmentFlag()
            : base((ushort) AreaPacketId.recv_map_fragment_flag, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // map id
            res.WriteInt32(0); // fragment flag
            return res;
        }
    }
}
