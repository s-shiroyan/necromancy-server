using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Systems.Auction_House
{
    public class send_auction_bid : ClientHandler
    {
        public send_auction_bid(NecServer server) : base(server)
        {
            //TODO find out why this is here and if its needed.
        }

        public override ushort Id => (ushort) AreaPacketId.send_auction_bid;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_auction_bid_r, res, ServerType.Area);
        }
    }
}
