using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_base_login : Handler
    {
        public send_base_login(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_base_login;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            res.WriteByte(1);
            res.WriteFixedString("Soul 1", 49);
            res.WriteByte(1); // Soul Level
            res.WriteByte(0);

            res.WriteByte(2);
            res.WriteFixedString("Soul 2", 49);
            res.WriteByte(2); // Soul Level
            res.WriteByte(0);

            res.WriteByte(1); // cmp to 1
            res.WriteByte(0);

            Router.Send(client, (ushort) MsgPacketId.recv_base_login_r, res);
        }
    }
}