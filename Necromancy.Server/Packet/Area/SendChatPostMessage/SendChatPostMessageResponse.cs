using Arrowgene.Services.Buffers;
using Necromancy.Server.Model;

namespace Necromancy.Server.Packet.Area.SendChatPostMessage
{
    public class SendChatPostMessageResponse : PacketResponse
    {
        public SendChatPostMessageResponse(ushort id, ServerType serverType) : base(id, serverType)
        {
        }

        public override IBuffer ToBuffer()
        {
            throw new System.NotImplementedException();
        }
    }
}
