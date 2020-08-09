using Necromancy.Server.Model;
using Necromancy.Server.Systems;
using Necromancy.Server.Systems.Auction_House;
using Necromancy.Server.Systems.Auction_House.Logic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Necromancy.Server.Auction.Database
{
    public class DbAuction : DatabaseAccessor
    {
        private const string SqlInsertAuctionItem = @"
            INSERT INTO 
                nec_auction_items 
                (
                    character_id, 
                    item_id, 
                    quantity, 
                    expiry_time, 
                    min_bid, 
                    buyout_price, 
                    comment
                ) 
            VALUES 
                (
                    @character_id, 
                    @item_id, 
                    @quantity, 
                    @expiry_time, 
                    @min_bid, 
                    @buyout_price, 
                    @comment
                )";

        private const string SqlUpdateAuctionBid = @"
            UPDATE 
                nec_auction_items 
                (
                    bidder_id, 
                    current_bid
                ) 
            VALUES 
                (
                    @bidder_id, 
                    @current_bid
                ) 
            WHERE 
                id = @id";

        private const string SqlSelectBids = @"
            SELECT 
                nec_auction_items.id, 
                nec_auction_items.character_id, 
                nec_character.name AS character_name, 
                nec_auction_items.item_id, 
                nec_auction_items.quantity, 
                nec_auction_items.expiry_time, 
                nec_auction_items.min_bid, 
                nec_auction_items.buyout_price, 
                nec_auction_items.current_bid, 
                nec_auction_items.bidder_id, 
                nec_auction_items.comment 
            FROM 
                nec_auction_items 
            INNER JOIN 
                nec_character 
            ON 
                nec_auction_items.character_id = nec_character.id 
            WHERE 
                bidder_id = @character_id";

        private const string SqlSelectLots = @"
            SELECT 
                nec_auction_items.id, 
                nec_auction_items.character_id, 
                nec_character.name AS character_name, 
                nec_auction_items.item_id, 
                nec_auction_items.quantity, 
                nec_auction_items.expiry_time, 
                nec_auction_items.min_bid, 
                nec_auction_items.buyout_price, 
                nec_auction_items.current_bid, 
                nec_auction_items.bidder_id, 
                nec_auction_items.comment 
            FROM 
                nec_auction_items 
            INNER JOIN 
                nec_character 
            ON 
                nec_auction_items.character_id = nec_character.id 
            WHERE 
                character_id = @character_id";

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

        public bool UpdateAuctionBid(AuctionItem auctionItem)
        {
            int rowsAffected = ExecuteNonQuery(SqlInsertAuctionItem, command =>
            {
                AddParameter(command, "@bidder_id", auctionItem.BidderID);
                AddParameter(command, "@current_bid", auctionItem.CurrentBid);
            });
            return rowsAffected > 0;
        }

        public AuctionItem[] SelectAuctionBids(Character character)
        {
            AuctionItem[] bids = new AuctionItem[AuctionHouse.MAX_BIDS];
            int i = 0;
            ExecuteReader(SqlSelectBids,
                command =>
                {
                    AddParameter(command, "@character_id", character.Id);
                }, reader =>
                {
                    while (reader.Read())
                    {
                        if (i >= AuctionHouse.MAX_BIDS) break;
                        AuctionItem bid = makeAuctionItem(reader);
                        bids[i] = bid;
                        i++;
                    }
                });
            AuctionItem[] truncatedBids = new AuctionItem[i];
            Array.Copy(bids, truncatedBids, i);
            return truncatedBids;
        }

        public AuctionItem[] SelectAuctionLots(Character character)
        {
            AuctionItem[] lots = new AuctionItem[AuctionHouse.MAX_LOTS];
            int i = 0;
            ExecuteReader(SqlSelectLots,
                command =>
                {
                    AddParameter(command, "@character_id", character.Id);
                }, reader =>
                {                    
                    while (reader.Read())
                    {
                        if (i >= AuctionHouse.MAX_LOTS) break;
                        AuctionItem lot = makeAuctionItem(reader);
                        lots[i] = lot;
                        i++;
                    }
                });
            AuctionItem[] truncatedLots = new AuctionItem[i];
            Array.Copy(lots, truncatedLots, i);
            return truncatedLots;
        }

        private AuctionItem makeAuctionItem(DbDataReader reader)
        {
            AuctionItem auctionItem = new AuctionItem();
            auctionItem.LotID = reader.GetInt32("id");
            auctionItem.ConsignerID = reader.GetInt32("character_id");
            auctionItem.ConsignerName = reader.GetString("character_name");
            auctionItem.ItemID = reader.GetInt64("item_id");
            auctionItem.Quantity = reader.GetInt32("quantity");
            auctionItem.ExpiryTime = calcSecondsToExpiry(reader.GetInt64("expiry_time"));
            auctionItem.MinimumBid = reader.GetInt32("min_bid");
            auctionItem.BuyoutPrice = reader.GetInt32("buyout_price");
            auctionItem.CurrentBid = reader.GetInt32("current_bid");
            auctionItem.BidderID = reader.GetInt32("bidder_id");
            auctionItem.Comment = reader.GetString("comment");
            return auctionItem;
        }


        //TODO look into moving these to business logic
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
