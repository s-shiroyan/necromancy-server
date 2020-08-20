using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodySalvageNotifySalvager : PacketResponse
    {
        public RecvCharaBodySalvageNotifySalvager()
            : base((ushort) AreaPacketId.recv_charabody_salvage_notify_salvager, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteCString(""); // Length 0x31
            res.WriteCString(""); // Length 0x5B
            return res;
        }
    }
}
