using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvChatNotifyMessage : PacketResponse
    {
        public RecvChatNotifyMessage()
            : base((ushort) AreaPacketId.recv_chat_notify_message, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteFixedString("", 0x31);
            res.WriteFixedString("", 0x25);
            res.WriteFixedString("", 0x301);
            return res;
        }
    }
}
