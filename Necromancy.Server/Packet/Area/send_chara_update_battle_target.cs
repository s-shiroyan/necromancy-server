using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_chara_update_battle_target : ClientHandler
    {
        public send_chara_update_battle_target(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_chara_update_battle_target;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint characterID = packet.Data.ReadUInt32();
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            //Router.Send(client.Map, (ushort) AreaPacketId.recv_auction_bid_r, res, ServerType.Area);
        }
    }
}
