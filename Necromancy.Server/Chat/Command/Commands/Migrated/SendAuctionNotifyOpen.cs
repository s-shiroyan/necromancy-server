using System.Collections.Generic;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Auction_House;

namespace Necromancy.Server.Chat.Command.Commands
{
    //opens auction house
    public class SendAuctionNotifyOpen : ServerChatCommand
    {
        public SendAuctionNotifyOpen(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            AuctionHouse auctionHouse = new AuctionHouse(client);
            BidsAndLots bnl = auctionHouse.GetBidsAndLots();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(bnl.Lots.Length); // must be <= 5 | Why?            
            
            for (int i = 0; i < bnl.Lots.Length; i++)
            {
                res.WriteByte((byte) i); // row number?
                res.WriteInt32(i); // row number ??
                res.WriteInt64(bnl.Lots[i].ItemID);
                res.WriteInt32(bnl.Lots[i].MinimumBid); // Lowest
                res.WriteInt32(bnl.Lots[i].BuyoutPrice); // Buy Now
                res.WriteFixedString(bnl.Lots[i].ConsignerName, 49);
                res.WriteByte(1); // 1 permit to show item in the search section ?? flags?
                res.WriteFixedString(bnl.Lots[i].Comment, 385); // Comment in the item information
                res.WriteInt16((short) bnl.Lots[i].CurrentBid); // Bid why convert to short?
                res.WriteInt32(bnl.Lots[i].ExpiryTime);

                res.WriteInt32(0); // unknown
                res.WriteInt32(0); // unknown
            }

            res.WriteInt32(bnl.Bids.Length); // must be< = 8 | why?

            for (int i = 0; i < bnl.Bids.Length; i++)
            {
                res.WriteByte((byte)i); // row number?
                res.WriteInt32(i); // row number ??
                res.WriteInt64(bnl.Bids[i].ItemID);
                res.WriteInt32(bnl.Bids[i].MinimumBid); // Lowest
                res.WriteInt32(bnl.Bids[i].BuyoutPrice); // Buy Now
                res.WriteFixedString(bnl.Bids[i].ConsignerName, 49);
                res.WriteByte(1); // 1 permit to show item in the search section ?? flags?
                res.WriteFixedString(bnl.Bids[i].Comment, 385); // Comment in the item information
                res.WriteInt16((short)bnl.Bids[i].CurrentBid); // Bid why convert to short?
                res.WriteInt32(bnl.Bids[i].ExpiryTime);

                res.WriteInt32(0); // unknown
                res.WriteInt32(0); // unknown
            }

            res.WriteByte(0); // bool  | what?
            Router.Send(client, (ushort) AreaPacketId.recv_auction_notify_open, res, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "auct";
    }
}
