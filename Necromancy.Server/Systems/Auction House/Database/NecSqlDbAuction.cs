using Necromancy.Server.Model;
using Necromancy.Server.Systems.Auction_House;
using Necromancy.Server.Systems.Auction_House.Logic;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
namespace Necromancy.Server.Database
{    public partial interface IDatabase
    {
        bool InsertAuctionItem(AuctionItem auctionItem);
        AuctionItem[] SelectAuctionBids(Character character);
        AuctionItem[] SelectAuctionLots(Character character);
    }
}

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlInsertAuctionItem =
            "INSERT INTO nec_auction_items " +
            "(" +
                "character_id, " +
                "item_id, " +
                "quantity, " +
                "expiry_time, " +
                "min_bid, " +
                "buyout_price, " +
                "comment" +
            ") " +
            "VALUES " +
            "(" +
                "@character_id, " +
                "@item_id, " +
                "@quantity, " +
                "@expiry_time, " +
                "@min_bid, " +
                "@buyout_price, " +
                "@comment" +
            ")";

        private const string SqlUpdateAuctionBid =
            "UPDATE nec_auction_items " +
            "(" +
                "bidder_id, " +
                "current_bid" +
            ") " +
            "VALUES " +
            "(" +
                "@bidder_id, " +
                "@current_bid" +
            ") " +
            "WHERE id = @id";

        private const string SqlSelectBids =
            "SELECT " +
                "nec_auction_items.id, " +
                "nec_auction_items.character_id, " +
                "nec_character.name AS character_name, " +                
                "nec_auction_items.item_id, " +
                "nec_auction_items.quantity, " +
                "nec_auction_items.expiry_time, " +                
                "nec_auction_items.min_bid, " +
                "nec_auction_items.buyout_price, " +
                "nec_auction_items.current_bid, " +
                "nec_auction_items.bidder_id, " +
                "nec_auction_items.comment, " +
            "FROM nec_auction_items " +
            "INNER JOIN nec_character " +
            "ON nec_auction_items.character_id = nec_character.id " +
            "WHERE bidder_id = @character_id";

        private const string SqlSelectLots =
            "SELECT " +
                "nec_auction_items.id, " +
                "nec_auction_items.character_id, " +
                "nec_character.name AS character_name, " +
                "nec_auction_items.item_id, " +
                "nec_auction_items.quantity, " +
                "nec_auction_items.expiry_time, " +
                "nec_auction_items.min_bid, " +
                "nec_auction_items.buyout_price, " +
                "nec_auction_items.current_bid, " +
                "nec_auction_items.bidder_id, " +
                "nec_auction_items.comment, " +
            "FROM nec_auction_items " +
            "INNER JOIN nec_character " +
            "ON nec_auction_items.character_id = nec_character.id " +
            "WHERE character_id = @character_id";

        public bool InsertAuctionItem(AuctionItem auctionItem)
        {           
              int rowsAffected = ExecuteNonQuery(SqlInsertAuctionItem, command =>
                {
                    AddParameter(command, "@character_id", auctionItem.ConsignerID);
                    AddParameter(command, "@item_id", auctionItem.ItemID);
                    AddParameter(command, "@quantity", auctionItem.Quantity);
                    AddParameter(command, "@expiry_time", calcExpiryTime(auctionItem.ExpiryTime));
                    AddParameter(command, "@min_bid", auctionItem.MinimumBid);
                    AddParameter(command, "@buyout_price", auctionItem.BuyoutPrice);
                    AddParameter(command, "@comment", auctionItem.Comment);
                });
            return rowsAffected > 0;
        }

        public AuctionItem[] SelectAuctionBids(Character character)
        {
            AuctionItem[] bids = new AuctionItem[AuctionHouse.MAX_BIDS];
            ExecuteReader(SqlSelectBids,
                command =>
                {
                    AddParameter(command, "@character_id", character.Id);
                }, reader =>
                {
                    int i = 0;
                    while (reader.Read())
                    {
                        if (i >= AuctionHouse.MAX_BIDS) break;
                        AuctionItem bid = makeAuctionItem(reader);
                        bids[i] = bid;
                        i++;
                    }
                });
            return bids;
        }

        public AuctionItem[] SelectAuctionLots(Character character)
        {
            AuctionItem[] lots = new AuctionItem[AuctionHouse.MAX_LOTS];
            ExecuteReader(SqlSelectBids,
                command =>
                {
                    AddParameter(command, "@character_id", character.Id);
                }, reader =>
                {
                    int i = 0;
                    while (reader.Read())
                    {
                        if (i >= AuctionHouse.MAX_LOTS) break;
                        AuctionItem lot = makeAuctionItem(reader);
                        lots[i] = lot;
                        i++;
                    }
                });
            return lots;
        }

        private AuctionItem makeAuctionItem(DbDataReader reader)
        {
            AuctionItem auctionItem = new AuctionItem();
            auctionItem.LotID = GetInt64(reader, "id");
            auctionItem.ConsignerID = GetInt32(reader, "character_id");
            auctionItem.ConsignerName = GetString(reader, "character_name");
            auctionItem.ItemID = GetInt64(reader, "item_id");
            auctionItem.Quantity = GetInt32(reader, "quantity");
            auctionItem.ExpiryTime = calcSecondsToExpiry(GetInt64(reader, "expiry_time"));
            auctionItem.MinimumBid = GetInt32(reader, "min_bid");
            auctionItem.BuyoutPrice = GetInt32(reader, "buyout_price");
            auctionItem.CurrentBid = GetInt32(reader, "current_bid");
            auctionItem.BidderID = GetInt32(reader, "bidder_id");
            auctionItem.Comment = GetString(reader, "comment");
            return auctionItem;
        }

        private int calcSecondsToExpiry(long unixTimeSecondsExpiry)
        {
            DateTime dNow = DateTime.Now;
            DateTimeOffset dOffsetNow = new DateTimeOffset(dNow);
            return ((int)(unixTimeSecondsExpiry - dOffsetNow.ToUnixTimeSeconds()));
        }

        private long calcExpiryTime(int secondsToExpiry)
        {
            DateTime dNow = DateTime.Now;
            DateTimeOffset dOffsetNow = new DateTimeOffset(dNow);
            return dOffsetNow.ToUnixTimeSeconds() + secondsToExpiry;
        }
    }
}
