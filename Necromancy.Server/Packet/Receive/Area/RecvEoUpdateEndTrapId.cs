using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEoUpdateEndTrapId : PacketResponse
    {
        public RecvEoUpdateEndTrapId()
            : base((ushort) AreaPacketId.recv_eo_update_end_trapid, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
