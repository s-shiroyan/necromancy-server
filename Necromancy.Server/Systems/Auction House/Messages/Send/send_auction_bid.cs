using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Auction_House.Logic;
using Newtonsoft.Json.Serialization;

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
            AuctionService auctionService = new AuctionService(client);
            int error = 0;
            try
            {
                //auctionService.Bid(); //TODO find data
            } catch(AuctionException e)
            {
                error = (int) e.Type;
            }
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(error); 
            Router.Send(client.Map, (ushort)AreaPacketId.recv_auction_bid_r, res, ServerType.Area);
        }
    }
}
