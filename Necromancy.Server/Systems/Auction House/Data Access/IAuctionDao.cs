using Necromancy.Server.Model;
using Necromancy.Server.Systems.Auction_House.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Auction_House.Data_Access
{
    public interface IAuctionDao
    {
        public bool InsertAuctionItem(AuctionItem auctionItem);

        public bool UpdateAuctionBid(AuctionItem auctionItem);

        public AuctionItem SelectAuctionItem(long auctionItemId);

        public AuctionItem[] SelectAuctionBids(Character character);

        public AuctionItem[] SelectAuctionLots(Character character);

        public int SelectGold(Character character);

        public void SubtractGold(Character character, int amount);
    }
}
