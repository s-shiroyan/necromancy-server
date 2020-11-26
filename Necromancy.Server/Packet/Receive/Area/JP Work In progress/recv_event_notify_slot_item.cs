using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_event_notify_slot_item : PacketResponse
    {
        public recv_event_notify_slot_item()
            : base((ushort) AreaPacketId.recv_event_notify_slot_item, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteFixedString("Xeno", 0x10);
            return res;
        }
    }
}
