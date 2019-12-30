using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_disband : ClientHandler
    {
        public send_party_disband(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_party_disband;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            Router.Send(client, (ushort) AreaPacketId.recv_party_disband_r, res, ServerType.Area);

            IBuffer res2 = BufferProvider.Provide();

            Router.Send(client, (ushort)MsgPacketId.recv_party_notify_disband, res2, ServerType.Msg);
        }
    }
}
