using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_logout_start_request : Handler
    {
        public send_logout_start_request(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_logout_start_request;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(2);//0 = nothing happens, 1 = logout failed:1
            Router.Send(client, (ushort) AreaPacketId.recv_logout_start_request_r, res);            
        }
    }
}