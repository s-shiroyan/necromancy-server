using Necromancy.Server.Model;
using Necromancy.Server.Systems;
using Necromancy.Server.Systems.Auction_House;
using Necromancy.Server.Systems.Auction_House.Data_Access;
using Necromancy.Server.Systems.Auction_House.Logic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Necromancy.Server.Auction.Database
{
    public class AuctionDao : DatabaseAccessObject, IAuctionDao
    {
        private const string SqlCreateItemsUpForAuctionView = @"
            DROP VIEW IF EXISTS [items_up_for_auction];
            CREATE VIEW items_up_for_auction
	            (
		            id, 
                    consigner_id, 
		            consigner_name, 
		            spawn_id, 
		            quantity, 
		            expiry_datetime, 
		            min_bid, 
		            buyout_price, 
		            current_bid, 
		            bidder_id, 
		            comment
	            )
            AS
            SELECT 			
                nec_auction_item.id,
                consigner.id,
                consigner.name,
                nec_auction_item.item_spawn_id, 
                nec_auction_item.quantity,
                nec_auction_item.expiry_datetime, 
                nec_auction_item.min_bid,
                nec_auction_item.buyout_price, 
                nec_auction_item.current_bid, 
                nec_auction_item.bidder_id,
                nec_auction_item.comment
            FROM 
                nec_auction_item
			INNER JOIN
				nec_item_spawn spawn
			ON
				nec_auction_item.item_spawn_id = spawn.id
            INNER JOIN 
                nec_character consigner
            ON 
                spawn.character_id = consigner.id";

        private const string SqlInsertItem = @"
            INSERT INTO 
                nec_auction_item 
                ( 
                    item_spawn_id, 
                    quantity, 
                    expiry_datetime, 
                    min_bid, 
                    buyout_price, 
                    comment
                ) 
            VALUES 
                (
                    @item_spawn_id, 
                    @quantity, 
                    @expiry_datetime, 
                    @min_bid, 
                    @buyout_price, 
                    @comment
                )";

        private const string SqlUpdateBid = @"
            UPDATE 
                nec_auction_item 
            SET 
                bidder_id = @bidder_id, 
                current_bid = @current_bid, 
                is_cancellable = (bidder_id IS NULL) 
            WHERE 
                id = @id";

        private const string SqlSelectBids = @"
            SELECT 			
                *
            FROM
                items_up_for_auction
            WHERE
                bidder_id = @character_id";

        private const string SqlSelectLots = @"
            SELECT 			
                *
            FROM 
                items_up_for_auction			
            WHERE 
                consigner_id = @character_id";

        private const string SqlSelectItem = @"
            SELECT 			
                *
            FROM
                items_up_for_auction
            WHERE 
                id = @id";

        private const string SqlSelectItemsByCriteria = @"";


        public AuctionDao()
        {
            CreateView();
        }

        private void CreateView()
        {
            ExecuteNonQuery(SqlCreateItemsUpForAuctionView, command => { });
        }

        public bool InsertItem(AuctionItem auctionItem)
        {           
              int rowsAffected = ExecuteNonQuery(SqlInsertItem, command =>
                {
                    AddParameter(command, "@character_id", auctionItem.ConsignerID);
                    AddParameter(command, "@spawn_id", auctionItem.SpawnedItemID);
                    AddParameter(command, "@quantity", auctionItem.Quantity);
                    AddParameter(command, "@expiry_datetime", CalcExpiryTime(auctionItem.SecondsUntilExpiryTime));
                    AddParameter(command, "@min_bid", auctionItem.MinimumBid);
                    AddParameter(command, "@buyout_price", auctionItem.BuyoutPrice);
                    AddParameter(command, "@comment", auctionItem.Comment);
                });
            return rowsAffected > 0;
        }

        public AuctionItem SelectItem(int auctionItemId)
        {
            AuctionItem auctionItem = new AuctionItem();
            ExecuteReader(SqlSelectItem,
                command =>
                {
                    AddParameter(command, "@id", auctionItemId);
                }, reader =>
                {
                    MakeAuctionItem(reader);
                });
            return auctionItem;
        }

        public bool UpdateBid(AuctionItem auctionItem)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateBid, command =>
            {
                AddParameter(command, "@bidder_id", auctionItem.BidderID);
                AddParameter(command, "@current_bid", auctionItem.CurrentBid);
            });
            return rowsAffected > 0;
        }

        public AuctionItem[] SelectBids(Character character)
        {
            AuctionItem[] bids = new AuctionItem[AuctionService.MAX_BIDS];
            int i = 0;
            ExecuteReader(SqlSelectBids,
                command =>
                {
                    AddParameter(command, "@character_id", character.Id);
                }, reader =>
                {
                    while (reader.Read())
                    {
                        if (i >= AuctionService.MAX_BIDS) break;
                        AuctionItem bid = MakeAuctionItem(reader);
                        bids[i] = bid;
                        i++;
                    }
                });
            AuctionItem[] truncatedBids = new AuctionItem[i];
            Array.Copy(bids, truncatedBids, i);
            return truncatedBids;
        }

        public AuctionItem[] SelectLots(Character character)
        {
            AuctionItem[] lots = new AuctionItem[AuctionService.MAX_LOTS];
            int i = 0;
            ExecuteReader(SqlSelectLots,
                command =>
                {
                    AddParameter(command, "@character_id", character.Id);
                }, reader =>
                {                    
                    while (reader.Read())
                    {
                        if (i >= AuctionService.MAX_LOTS) break;
                        AuctionItem lot = MakeAuctionItem(reader);
                        lots[i] = lot;
                        i++;
                    }
                });
            AuctionItem[] truncatedLots = new AuctionItem[i];
            Array.Copy(lots, truncatedLots, i);
            return truncatedLots;
        }

        private AuctionItem MakeAuctionItem(DbDataReader reader)
        {
            AuctionItem auctionItem = new AuctionItem();
            auctionItem.Id = reader.GetInt32("id");
            auctionItem.ConsignerID = reader.GetInt32("consigner_id");
            auctionItem.ConsignerName = reader.GetString("consigner_name");
            auctionItem.SpawnedItemID = reader.GetInt64("spawn_id");
            auctionItem.Quantity = reader.GetInt32("quantity");
            auctionItem.SecondsUntilExpiryTime = CalcSecondsToExpiry(reader.GetInt64("expiry_datetime"));
            auctionItem.MinimumBid = reader.GetInt32("min_bid");
            auctionItem.BuyoutPrice = reader.GetInt32("buyout_price");
            auctionItem.CurrentBid = reader.GetInt32("current_bid");
            auctionItem.BidderID = reader.GetInt32("bidder_id");
            auctionItem.Comment = reader.GetString("comment");
            return auctionItem;
        }

        private int CalcSecondsToExpiry(long unixTimeSecondsExpiry)
        {
            DateTime dNow = DateTime.Now;
            DateTimeOffset dOffsetNow = new DateTimeOffset(dNow);
            return ((int)(unixTimeSecondsExpiry - dOffsetNow.ToUnixTimeSeconds()));
        }

        private long CalcExpiryTime(int secondsToExpiry)
        {
            DateTime dNow = DateTime.Now;
            DateTimeOffset dOffsetNow = new DateTimeOffset(dNow);
            return dOffsetNow.ToUnixTimeSeconds() + secondsToExpiry;
        }

        public int SelectGold(Character character)
        {
            throw new NotImplementedException();
        }

        public void AddGold(Character character, int amount)
        {
            throw new NotImplementedException();
        }

        public void SubtractGold(Character character, int amount)
        {
            throw new NotImplementedException();
        }

        public AuctionItem[] SelectItemsByCriteria(SearchCriteria searchCriteria)
        {
            AuctionItem[] results = new AuctionItem[1];
            ExecuteReader(SqlSelectItemsByCriteria,
                command =>
                {
                    
                }, reader =>
                {
                    while (reader.Read()) { 
                    //TODO do something
                    }
                });
            throw new NotImplementedException();
        }
    }
}
