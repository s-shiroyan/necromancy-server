using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodySalvageNotifyBody : PacketResponse
    {
        public RecvCharaBodySalvageNotifyBody()
            : base((ushort) AreaPacketId.recv_charabody_salvage_notify_body, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteCString("ToBeFound"); // find max size
            res.WriteCString("ToBeFound"); // find max size
            return res;
        }
    }
}
