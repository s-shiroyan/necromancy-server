using Necromancy.Server.Database;
using Necromancy.Server.Model;
using Necromancy.Server.Systems.Auction_House.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Auction_House
{
    public class AuctionHouse
    {
        public const int MAX_BIDS = 8;
        public const int MAX_LOTS = 5;

        //TODO FIX THIS DATABASE PASSING, probably house all database functions in session data
        // probably move full logic to database folder something like Database.getConnection(), prepared statement error handling.
        public BidsAndLots GetBidsAndLots(IDatabase database, Character character)
        {
            return new BidsAndLots(database.SelectAuctionBids(character), database.SelectAuctionLots(character));
        }

        public AuctionItem[] search(SearchCriteria searchCritera)
        {
            return new AuctionItem[4];
        }
    }
}
