using Discord;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Necromancy.Server.Model;
using Necromancy.Server.Systems.Auction_House;
using Necromancy.Server.Systems.Auction_House.Data_Access;
using Necromancy.Server.Systems.Auction_House.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Test.Systems
{
    [TestClass]
    public class AuctionServiceTest
    {

        private class AuctionDaoAdapter : IAuctionDao
        {
            private readonly Dictionary<int, AuctionItem> Items = new Dictionary<int, AuctionItem>();

            public void AddGold(Character character, int amount)
            {
                character.AdventureBagGold += amount;
            }

            public bool InsertItem(AuctionItem insertItem)
            {
                Items.Add(insertItem.Id, insertItem);
                return true;
            }

            public AuctionItem[] SelectBids(Character character)
            {
                List<AuctionItem> bids = new List<AuctionItem>();
                
                foreach (AuctionItem a in Items.Values)
                {
                    if(a.BidderID == character.Id) bids.Add(a);                     
                }

                return bids.ToArray();
            }

            public int SelectGold(Character character)
            {
                return character.AdventureBagGold;
            }

            public AuctionItem SelectItem(int itemId)
            {
                if (Items.ContainsKey(itemId))
                {
                    return Items[itemId];
                } else {
                    AuctionItem auctionItem = new AuctionItem();
                    auctionItem.Id = -1;
                    return auctionItem;
                }                
            }

            public AuctionItem[] SelectItemsByCriteria(SearchCriteria searchCriteria)
            {
                throw new NotImplementedException();
            }

            public AuctionItem[] SelectLots(Character character)
            {
                throw new NotImplementedException();
            }

            public void SubtractGold(Character character, int amount)
            {
                character.AdventureBagGold -= amount;
            }

            public bool UpdateBid(AuctionItem updateBidItem)
            {
                Items[updateBidItem.Id].BidderID = updateBidItem.BidderID;
                Items[updateBidItem.Id].CurrentBid = updateBidItem.CurrentBid;
                return true;
            }
        }

        private NecClient _dummyClient;
        private AuctionService _dummyAuctionService;
        private AuctionDaoAdapter _dummyAuctionDao;
        private AuctionItem _dummyRowInDatabaseItem;        

        private const int CHARACTER_ID = 777;
        private const int OTHER_CHARACTER_ID = 888;
        private const int BID = 5000;
        private const int DUMMY_ROW_IN_DATABASE_ID =1704;
        private const int START_GOLD = 10000;
                

        [TestInitialize]
        public void Setup()
        {
            _dummyClient = new NecClient();
            _dummyClient.Character.Id = CHARACTER_ID;
            _dummyClient.Character.AdventureBagGold = START_GOLD;

            _dummyRowInDatabaseItem = new AuctionItem();
            _dummyRowInDatabaseItem.Id = DUMMY_ROW_IN_DATABASE_ID;

            _dummyAuctionDao = new AuctionDaoAdapter();
            _dummyAuctionDao.InsertItem(_dummyRowInDatabaseItem);

            _dummyAuctionService = new AuctionService(_dummyClient, _dummyAuctionDao);
        }

        [TestMethod]
        public void TestBid()
        {            
            //SETUP
            _dummyRowInDatabaseItem.CurrentBid = 0;
            _dummyRowInDatabaseItem.BidderID = 0;

            AuctionItem auctionItem = new AuctionItem();
            auctionItem.Id = DUMMY_ROW_IN_DATABASE_ID;

            //EXERCISE
            _dummyAuctionService.Bid(auctionItem, BID);

            //VERIFY
            Assert.AreEqual(CHARACTER_ID, _dummyRowInDatabaseItem.BidderID);
            Assert.AreEqual(BID, _dummyRowInDatabaseItem.CurrentBid);
            Assert.AreEqual(START_GOLD - BID, _dummyClient.Character.AdventureBagGold);
        }

        [TestMethod]
        public void TestBidNotEnoughGold()
        {
            //SETUP
            _dummyClient.Character.AdventureBagGold = BID - 1;

            AuctionItem auctionItem = new AuctionItem();
            auctionItem.Id = DUMMY_ROW_IN_DATABASE_ID;

            //EXERCISE AND VERIFY
            AuctionException e = Assert.ThrowsException<AuctionException>(() => _dummyAuctionService.Bid(auctionItem, BID));
            Assert.AreEqual(AuctionException.AuctionExceptionType.Generic, e.Type);
        }

        [TestMethod]
        public void TestBidNewBidLowerThanPrev()
        {            
            //SETUP
            _dummyRowInDatabaseItem.CurrentBid = BID + 500;
            _dummyRowInDatabaseItem.BidderID = OTHER_CHARACTER_ID; 

            AuctionItem auctionItem = new AuctionItem();
            auctionItem.Id = DUMMY_ROW_IN_DATABASE_ID;

            //EXERCISE AND VERIFY
            AuctionException e = Assert.ThrowsException<AuctionException>(() => _dummyAuctionService.Bid(auctionItem, BID));
            Assert.AreEqual(AuctionException.AuctionExceptionType.NewBidLowerThanPrev, e.Type);
        }

        [TestMethod]
        public void TestBidBidSlotsFull()
        {
            //SETUP
            for (int i = 0; i < AuctionService.MAX_BIDS; i++) 
            {
                AuctionItem a = new AuctionItem();
                a.Id = i;
                a.BidderID = CHARACTER_ID;
                _dummyAuctionDao.InsertItem(a);
            }

            AuctionItem auctionItem = new AuctionItem();
            auctionItem.Id = DUMMY_ROW_IN_DATABASE_ID;

            //EXERCISE AND VERIFY
            AuctionException e = Assert.ThrowsException<AuctionException>(() => _dummyAuctionService.Bid(auctionItem, BID));
            Assert.AreEqual(AuctionException.AuctionExceptionType.BidSlotsFull, e.Type);
        }

        [Ignore] //TODO Stop ignoring me baka....
        [TestMethod]
        public void TestBidDimentoMedalExpired()
        {
            //SETUP
            for (int i = 0; i < AuctionService.MAX_BIDS_NO_DIMENTO; i++)
            {
                AuctionItem a = new AuctionItem();
                a.Id = i;
                a.BidderID = CHARACTER_ID;
                _dummyAuctionDao.InsertItem(a);
            }
 
            AuctionItem auctionItem = new AuctionItem();
            auctionItem.Id = AuctionService.MAX_BIDS_NO_DIMENTO + 1;

            //EXERCISE AND VERIFY
            AuctionException e = Assert.ThrowsException<AuctionException>(() => _dummyAuctionService.Bid(auctionItem, BID));
            Assert.AreEqual(AuctionException.AuctionExceptionType.BidDimentoMedalExpired, e.Type);
        }

        [TestMethod]
        public void TestCancelBid()
        {
            //SETUP
            _dummyRowInDatabaseItem.BidderID = CHARACTER_ID;
            _dummyRowInDatabaseItem.CurrentBid = BID;
            _dummyRowInDatabaseItem.SecondsUntilExpiryTime = AuctionService.SECONDS_IN_AN_HOUR;
            _dummyRowInDatabaseItem.IsCancellable = true;

            AuctionItem auctionItem = new AuctionItem();
            auctionItem.Id = DUMMY_ROW_IN_DATABASE_ID;

            //EXERCISE
            _dummyAuctionService.CancelBid(auctionItem);

            //VERIFY
            Assert.AreEqual(0, _dummyRowInDatabaseItem.BidderID);
            Assert.AreEqual(0, _dummyRowInDatabaseItem.CurrentBid);
            Assert.AreEqual(true, _dummyRowInDatabaseItem.IsCancellable);
            Assert.AreEqual(START_GOLD + BID, _dummyClient.Character.AdventureBagGold);
        }

        [TestMethod]
        public void TestCancelBidAnotherCharacterAlreadyBid()
        {
            //SETUP
            _dummyRowInDatabaseItem.BidderID = CHARACTER_ID;
            _dummyRowInDatabaseItem.CurrentBid = BID;
            _dummyRowInDatabaseItem.SecondsUntilExpiryTime = AuctionService.SECONDS_IN_AN_HOUR;
            _dummyRowInDatabaseItem.IsCancellable = false;

            AuctionItem auctionItem = new AuctionItem();
            auctionItem.Id = DUMMY_ROW_IN_DATABASE_ID;      

            //EXERCISE AND VERIFY
            AuctionException e = Assert.ThrowsException<AuctionException>(() => _dummyAuctionService.CancelBid(auctionItem));
            Assert.AreEqual(AuctionException.AuctionExceptionType.AnotherCharacterAlreadyBid, e.Type);
        }

        [TestMethod]
        public void TestCancelBidNoCancelExpiry()
        {
            //SETUP
            _dummyRowInDatabaseItem.BidderID = CHARACTER_ID;
            _dummyRowInDatabaseItem.CurrentBid = BID;
            _dummyRowInDatabaseItem.SecondsUntilExpiryTime = AuctionService.SECONDS_IN_AN_HOUR - 1;
            _dummyRowInDatabaseItem.IsCancellable = true;

            AuctionItem auctionItem = new AuctionItem();
            auctionItem.Id = DUMMY_ROW_IN_DATABASE_ID;

            //EXERCISE AND VERIFY
            AuctionException e = Assert.ThrowsException<AuctionException>(() => _dummyAuctionService.CancelBid(auctionItem));
            Assert.AreEqual(AuctionException.AuctionExceptionType.NoCancelExpiry, e.Type);
        }

        [TestMethod]
        public void TestCancelExhibit()
        {

        }

        [TestMethod]
        public void TestClose()
        {

        }

        [TestMethod]
        public void TestExhibit()
        {

        }

        [TestMethod]
        public void TestReExhibit()
        {

        }

        [TestMethod]
        public void TestReceiveGold()
        {

        }

        [TestMethod]
        public void TestReceiveItem()
        {

        }

        [TestMethod]
        public void TestSearch()
        {
            //SETUP
            SearchCriteria searchCriteria = new SearchCriteria();

            //EXERCISE
            AuctionItem[] results = _dummyAuctionService.Search(searchCriteria);

            //VERIFY
            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void TestSearchIllegalSearchQuery()
        {
            //SETUP
            SearchCriteria searchCriteria = new SearchCriteria();
            searchCriteria.Class = SearchCriteria.Classes.Priest - 10;
            throw new NotImplementedException();

            //EXERCISE AND VERIFY
            AuctionException e = Assert.ThrowsException<AuctionException>(() => _dummyAuctionService.Search(searchCriteria));
            Assert.AreEqual(AuctionException.AuctionExceptionType.IllegalSearchQuery, e.Type);
        }
    }
}
