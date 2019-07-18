using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_map_get_info : Handler
    {
        public send_map_get_info(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_map_get_info;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(1001001);
            

            Router.Send(client, (ushort) AreaPacketId.recv_map_get_info_r, res);            
        }
    }
}