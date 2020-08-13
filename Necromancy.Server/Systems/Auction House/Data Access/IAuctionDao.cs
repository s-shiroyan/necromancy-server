using Necromancy.Server.Model;
using Necromancy.Server.Systems.Auction_House.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Auction_House.Data_Access
{
    public interface IAuctionDao
    {
        public bool InsertItem(AuctionItem insertItem);

        public bool UpdateBid(AuctionItem updateBidItem);

        public AuctionItem SelectItem(int itemId);

        public AuctionItem[] SelectBids(Character character);

        public AuctionItem[] SelectLots(Character character);

        public int SelectGold(Character character);

        public void AddGold(Character character, int amount);

        public void SubtractGold(Character character, int amount);
        
    }
}
