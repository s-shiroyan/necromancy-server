using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_charabody_salvage_request : ClientHandler
    {
        public send_charabody_salvage_request(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_charabody_salvage_request;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint targetId = packet.Data.ReadUInt32();
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            Router.Send(client, (ushort) AreaPacketId.recv_charabody_salvage_request_r, res, ServerType.Area);

            IBuffer res2 = BufferProvider.Provide();
	        res2.WriteUInt32(targetId);
            res2.WriteCString("soul"); // find max size
            res2.WriteCString("char"); // find max size
            Router.Send(client.Map, (ushort)AreaPacketId.recv_charabody_salvage_notify_body, res2, ServerType.Area);
        }
    }
}
