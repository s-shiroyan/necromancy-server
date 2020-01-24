using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_change_mode : ClientHandler
    {
        public send_party_change_mode(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_party_change_mode;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int partyMode = packet.Data.ReadInt32();
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(partyMode);

            Router.Send(client, (ushort) AreaPacketId.recv_party_change_mode_r, res, ServerType.Area);
        }
    }
}
