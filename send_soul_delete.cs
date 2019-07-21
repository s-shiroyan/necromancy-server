using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_soul_delete : Handler
    {
        public send_soul_delete(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_soul_delete;

        

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
           

            Router.Send(client, (ushort) MsgPacketId.recv_soul_delete_r, res);
        }
    }
}