using Necromancy.Server.Systems.Auction_House.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Auction_House
{
    public class BidsAndLots
    {
        public AuctionItem[] Bids { get; private set; }
        public AuctionItem[] Lots { get; private set; }
        public BidsAndLots(AuctionItem[] bids, AuctionItem[] lots)
        {
            //TODO implement bounds checking
            Bids = bids;
            Lots = lots;
        }
    }
}
