using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_soul_authenticate_passwd : Handler
    {
        public send_soul_authenticate_passwd(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_soul_authenticate_passwd;

        public override void Handle(NecClient client, NecPacket packet)
        {
            string souldPassword = packet.Data.ReadCString();
            Logger.Info($"Entered Soul Password: {souldPassword}");

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteByte(0); // 0 = OK | 1 = need to change soul name
            Router.Send(client, (ushort) MsgPacketId.recv_soul_authenticate_passwd_r, res);
        }
    }
}