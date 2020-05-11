using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_charabody_salvage_abort : ClientHandler
    {
        public send_charabody_salvage_abort(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_charabody_salvage_abort;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int targetId = packet.Data.ReadInt32();
            IBuffer res = BufferProvider.Provide();

	        res.WriteInt32(targetId);
            res.WriteInt32(0);

            Router.Send(client.Map, (ushort) AreaPacketId.recv_charabody_salvage_end, res, ServerType.Area);
        }
    }
}
