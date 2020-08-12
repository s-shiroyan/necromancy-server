using Necromancy.Server.Auction.Database;
using Necromancy.Server.Database;
using Necromancy.Server.Model;
using Necromancy.Server.Systems.Auction_House.Data_Access;
using Necromancy.Server.Systems.Auction_House.Logic;
using Necromancy.Server.Systems.Auction_House.Messages.Receive;
using Necromancy.Server.Systems.Inventory.Logic;
using System;
using System.Collections.Generic;
using System.Text;
using static Necromancy.Server.Systems.Auction_House.Logic.AuctionException;

namespace Necromancy.Server.Systems.Auction_House
{
    public class AuctionService
    {
        /*
        recv_auction_bid_r = 0x3F38,
        recv_auction_cancel_bid_r = 0xA0FC,
        recv_auction_cancel_exhibit_r = 0xFC28,
        recv_auction_close_r = 0xD1CB,
        recv_auction_exhibit_r = 0xB38D,
        recv_auction_notify_close = 0x7D2B,
        recv_auction_notify_open = 0xBA71,
        recv_auction_re_exhibit_r = 0xA549,
        recv_auction_receive_gold_r = 0x22C,
        recv_auction_receive_item_r = 0xB1CA,
        recv_auction_search_r = 0xF7E7,
        recv_auction_update_bid_item_state = 0x2E17,
        recv_auction_update_bid_num = 0x9BC6,
        recv_auction_update_exhibit_item_state = 0x5307,
        recv_auction_update_highest_bid = 0xBD99 

        send_auction_bid = 0x63BC, 
        send_auction_cancel_bid = 0xBC65,
        send_auction_cancel_exhibit = 0x375B,
        send_auction_close = 0xE732,
        send_auction_exhibit = 0xED52, 
        send_auction_re_exhibit = 0x61FA, 
        send_auction_receive_gold = 0x4657, 
        send_auction_receive_item = 0x7E18, 
        send_auction_search = 0x8865 
        */

        public const int MAX_BIDS = 8;
        public const int MAX_LOTS = 5;
        private const double LISTING_FEE_PERCENT = .05;

        private readonly NecClient _client;
        private readonly IAuctionDao _auctionDao;

        public AuctionService(NecClient nClient, IAuctionDao auctionDao)
        {
            _client = nClient;
            _auctionDao = auctionDao;
        }
        
        public AuctionService(NecClient nClient)
        {
            _client = nClient;
            _auctionDao = new AuctionDao();
        }

        

        public void Bid(AuctionItem auctionItem, int bid)
        {
            auctionItem.BidderID = _client.Character.Id;
            auctionItem.CurrentBid = bid;

            AuctionItem currentAuctionItem = _auctionDao.SelectItem(auctionItem.Id);
            if (auctionItem.CurrentBid < currentAuctionItem.CurrentBid) throw new AuctionException(AuctionExceptionType.NewBidLowerThanPrev);
            
            AuctionItem[] bids = _auctionDao.SelectBids(_client.Character);
            if(bids.Length >= MAX_BIDS) throw new AuctionException(AuctionExceptionType.BidSlotsFull);

            //TODO ADD CHECK FOR DIMENTO MEDAL / ROYAL after 5 bids
            if (false) throw new AuctionException(AuctionExceptionType.BidDimentoMedalExpired);

            _auctionDao.UpdateBid(auctionItem);
        }

        public void CancelBid(AuctionItem auctionItem)
        {
            bool isCancellable = _auctionDao.SelectIsBidCancellable(auctionItem.Id);
            if (isCancellable)
            {
                
            } else
            {
                throw new AuctionException(AuctionExceptionType.AnotherCharacterAlreadyBid);
            }
        }

        public void CancelExhibit(AuctionItem auctionItem)
        {
            throw new NotImplementedException();
        }

        public void Close(AuctionItem auctionItem)
        {
            throw new NotImplementedException();
        }

        public void ReExhibit(AuctionItem auctionItem)
        {
            throw new NotImplementedException();
        }

        public AuctionItem Exhibit(AuctionItem auctionItem, int bagNumber, int bagSlot)
        {            
            int currentNumLots = GetLots().Length;
            if (currentNumLots >= MAX_LOTS)
                throw new AuctionException(AuctionExceptionType.LotSlotsFull);

            //TODO CHECK IF EQUIPPED
            if (false)
                throw new AuctionException(AuctionExceptionType.EquipListing);

            //TODO CHECK IF INVALID ITEM
            if (false)
                throw new AuctionException(AuctionExceptionType.InvalidListing);

            //TODO CHECK DIMETO MEDAL ROYAL ACCOUNT STATUS
            if (false)
                throw new AuctionException(AuctionExceptionType.LotDimentoMedalExpired);

            //TODO CHECK ITEM ALREADY_LISTED items must have a unique serial in item spawn!
            if (false)
                throw new AuctionException(AuctionExceptionType.ItemAlreadyListed);

            int gold = _auctionDao.SelectGold(_client.Character);

            InventoryService iManager = new InventoryService(_client); //remove this 
            iManager.SubtractGold((int) Math.Ceiling(auctionItem.BuyoutPrice * LISTING_FEE_PERCENT)); 

            _auctionDao.InsertItem(auctionItem);

            return auctionItem;
        }

        public AuctionItem[] Search(SearchCriteria searchCritera)
        {
            throw new NotImplementedException();
        }

        public AuctionItem[]  GetBids() {
            return _auctionDao.SelectBids(_client.Character);
        }

        public AuctionItem[] GetLots()
        {
            return _auctionDao.SelectLots(_client.Character);
        }        

    }
}
