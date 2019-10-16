using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_skill_request_info : ClientHandler
    {
        public send_skill_request_info(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_skill_request_info;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            // No Response OP code
            // Router.Send(client, (ushort) 0x0000, res);            
        }
    }
}