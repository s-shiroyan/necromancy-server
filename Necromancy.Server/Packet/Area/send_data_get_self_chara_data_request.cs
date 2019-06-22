using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_data_get_self_chara_data_request : Handler
    {
        public send_data_get_self_chara_data_request(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_data_get_self_chara_data_request;

        public override void Handle(NecClient client, NecPacket packet)
        {
             IBuffer res = BufferProvider.Provide();
             res.WriteInt32(0);
             


           //  Router.Send(client, (ushort) AreaPacketId.recv_base_enter, res);
        }
    }
}