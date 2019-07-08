using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_soul_select_C44F : Handler
    {

        public send_soul_select_C44F(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)MsgPacketId.send_soul_select_C44F;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // 0 = OK | 1 = Failed to Select Soul
            res.WriteByte(1); // 1 = OK | 0, 2 = set password
            Router.Send(client, (ushort)MsgPacketId.recv_soul_select_r, res);
        }
    }
}