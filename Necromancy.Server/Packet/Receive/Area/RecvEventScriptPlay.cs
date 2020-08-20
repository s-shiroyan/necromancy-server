using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventScriptPlay : PacketResponse
    {
        public RecvEventScriptPlay()
            : base((ushort) AreaPacketId.recv_event_script_play, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString("ToBeFound"); // find max size
            return res;
        }
    }
}
