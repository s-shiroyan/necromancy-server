using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Auction_House.Logic
{
    public class AuctionItem
    {
        public long LotID { get; set; }
        public int ConsignerID { get; set; }
        public string ConsignerName { get; set; }
        public long ItemID { get; set; }
        public int Quantity { get; set; }
        public int ExpiryTime { get; set; }
        public int MinimumBid { get; set; }
        public int BuyoutPrice { get; set; }
        public long BidderID { get; set; }          
        public string BidderName { get; set; }
        public int CurrentBid { get; set; }
        public string Comment { get; set; }
    }
}
