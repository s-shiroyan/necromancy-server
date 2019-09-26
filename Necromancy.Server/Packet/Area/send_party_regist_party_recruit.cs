using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_regist_party_recruit : Handler
    {
        public send_party_regist_party_recruit(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_party_regist_party_recruit;

       public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);
            res.WriteInt32(77777777);
            res.WriteInt32(88888888);
            res.WriteInt32(99999999);




            //Router.Send(client, (ushort)AreaPacketId.recv_party_notify_recruit_request, res);
        }
    }
}