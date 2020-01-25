using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_apply : ClientHandler
    {
        public send_party_apply(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_party_apply;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int partyModeMaybe = packet.Data.ReadInt32();
            int myCharacterInstanceId = packet.Data.ReadInt32();

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(myCharacterInstanceId);

            Router.Send(client, (ushort) AreaPacketId.recv_party_apply_r, res, ServerType.Area);
        }
    }
}
