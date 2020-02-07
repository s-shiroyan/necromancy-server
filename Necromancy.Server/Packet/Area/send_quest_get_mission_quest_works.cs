using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_quest_get_mission_quest_works : ClientHandler
    {
        public send_quest_get_mission_quest_works(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_quest_get_mission_quest_works;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);
            res.WriteInt32(0);

            Router.Send(client, (ushort) AreaPacketId.recv_quest_get_mission_quest_works_r, res, ServerType.Area);
        }
    }
}
