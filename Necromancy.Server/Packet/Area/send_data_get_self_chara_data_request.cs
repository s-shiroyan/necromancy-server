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
            for (int i = 0; i < 28; i++)
                res.WriteByte(0);
            //hit with 0 bytes sent for some reason but also crashed..?

            Router.Send(client, (ushort) AreaPacketId.recv_data_get_self_chara_data_request_r, res);
        }
    }
}