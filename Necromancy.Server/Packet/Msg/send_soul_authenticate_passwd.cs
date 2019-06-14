using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Packet.Msg
{
    public class send_soul_authenticate_passwd : Handler
    {
        public send_soul_authenticate_passwd(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) 0;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            // res.WriteByte(1);
            // Router.Send(client, (ushort) MsgPacketId.recv_soul_select_r, res);
        }
    }
}