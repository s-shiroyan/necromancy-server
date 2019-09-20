using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_loot_access_object : Handler
    {
        public send_loot_access_object(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_loot_access_object;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int objectID = packet.Data.ReadInt32();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(objectID);

            Router.Send(client, (ushort) AreaPacketId.recv_loot_access_object_r, res);
        }
    }
}