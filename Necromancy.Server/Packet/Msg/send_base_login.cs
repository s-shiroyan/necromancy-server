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

        public const int SoulCount = 2;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //  Error
            for (int i = 0; i < SoulCount; i++)
            {
                res.WriteByte(1);
                res.WriteFixedString($"Talin", 49);
                res.WriteByte(1); // Soul Level
                res.WriteByte(0); // bool   (important bool, if use value 1 - can't join in msg server character list)
            }

            res.WriteByte(0); //bool
            res.WriteByte(0);

            Router.Send(client, (ushort) MsgPacketId.recv_base_login_r, res);
        }
    }
}