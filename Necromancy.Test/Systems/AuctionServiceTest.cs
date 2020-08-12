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

        private class MockAuctionDao : IAuctionDao
        {

            public int Gold { get; set; }
            public List<AuctionItem> AuctionedItems { get; set; }

            public MockAuctionDao()
            {
                AuctionedItems = new List<AuctionItem>();
            }

            public bool InsertAuctionItem(AuctionItem auctionItem)
            {
                AuctionedItems.Add(auctionItem);
                return true;
            }

            public AuctionItem[] SelectAuctionBids(Character character)
            {
                List<AuctionItem> bids = new List<AuctionItem>();
                foreach(AuctionItem auctionItem in AuctionedItems)
                {
                    if (auctionItem.BidderID == character.Id)
                    {
                        bids.Add(auctionItem);
                    }
                }
                return bids.ToArray();
            }

            public AuctionItem SelectAuctionItem(long auctionItemId)
            {
                AuctionItem auctionItem = new AuctionItem();
                foreach (AuctionItem a in AuctionedItems)
                {
                    if (a.Id == auctionItemId)
                    {
                        auctionItem = a;
                    }
                }
                return auctionItem;
            }

            public AuctionItem[] SelectAuctionLots(Character character)
            {
                throw new NotImplementedException();
            }

            public int SelectGold(Character character)
            {
                return Gold;
            }

            public void SubtractGold(Character character, int amount)
            {
                Gold -= amount;
            }

            public bool UpdateAuctionBid(AuctionItem auctionItem)
            {
                foreach (AuctionItem a in AuctionedItems)
                {
                    if (a.Id == auctionItem.Id)
                    {
                        a.CurrentBid = auctionItem.CurrentBid;
                        a.BidderID = auctionItem.BidderID;
                        return true;
                    }
                }

                return false;
            }
        }

        private class MockAuctionItem : AuctionItem
        {
            public MockAuctionItem()
            {
                Id = 38;
                ConsignerID = 5;
                ConsignerName = "Test Consigner";
                ItemID = 200901;  // 50100301 = camp | base item id | leather guard 100101 | 50100502 bag medium | 200901 soldier cuirass
                SpawnedItemID = 3840;
                Quantity = 1;
                SecondsUntilExpiryTime = 564;
                MinimumBid = 49;
                BuyoutPrice = 2000;
                BidderID = 0;
                BidderName = "Default Bidder Name";
                CurrentBid = 0;
                Comment ="Default Comment";
            }
        }

        private class MockNecClient : NecClient
        {
            public MockNecClient() : base()
            {
                Character = new Character();
                Character.Id = 999;
            }
        }


        [TestMethod]
        public void TestBid()
        {
            //VARIABLES
            const int MOCK_AUCTION_ITEM_ID = 100;
            const int MOCK_CHARACTER_ID = 777;
            const int MOCK_BID = 5000;

            //SETUP
            MockAuctionItem mockAuctionItem = new MockAuctionItem();
            mockAuctionItem.Id = MOCK_AUCTION_ITEM_ID;
            mockAuctionItem.CurrentBid = 0;
            mockAuctionItem.BidderID = 0;

            MockAuctionDao mockAuctionDao = new MockAuctionDao();
            mockAuctionDao.AuctionedItems.Add(mockAuctionItem);

            MockNecClient mockNecClient = new MockNecClient();
            mockNecClient.Character.Id = MOCK_CHARACTER_ID;

            AuctionService auctionService = new AuctionService(mockNecClient, mockAuctionDao);

            //TEST            
            AuctionItem auctionItem = new AuctionItem();
            auctionItem.Id = MOCK_AUCTION_ITEM_ID;
            auctionService.Bid(auctionItem, MOCK_BID);

            //RESOLVE
            foreach(AuctionItem a in mockAuctionDao.AuctionedItems)
            {
                if(a.Id == MOCK_AUCTION_ITEM_ID)
                {
                    Assert.AreEqual(MOCK_CHARACTER_ID, a.BidderID);
                    Assert.AreEqual(MOCK_BID, a.CurrentBid);
                }
            }
        }

        [TestMethod]
        public void TestBidNewBidLowerThanPrev()
        {
            //VARIABLES
            const int MOCK_AUCTION_ITEM_ID = 100;
            const int MOCK_CHARACTER_ID = 777;
            const int MOCK_BID = 5000;

            //SETUP
            MockAuctionItem mockAuctionItem = new MockAuctionItem();
            mockAuctionItem.Id = MOCK_AUCTION_ITEM_ID;
            mockAuctionItem.CurrentBid = MOCK_BID + 500;
            mockAuctionItem.BidderID = MOCK_CHARACTER_ID;

            MockAuctionDao mockAuctionDao = new MockAuctionDao();
            mockAuctionDao.AuctionedItems.Add(mockAuctionItem);

            MockNecClient mockNecClient = new MockNecClient();
            mockNecClient.Character.Id = MOCK_CHARACTER_ID;

            AuctionService auctionService = new AuctionService(mockNecClient, mockAuctionDao);

            //TEST            
            AuctionItem auctionItem = new AuctionItem();
            auctionItem.Id = MOCK_AUCTION_ITEM_ID;         
            
            //TEST / RESOLVE
            AuctionException e = Assert.ThrowsException<AuctionException>(() => auctionService.Bid(auctionItem, MOCK_BID));
            Assert.AreEqual(AuctionException.AuctionExceptionType.NewBidLowerThanPrev, e.Type);
        }

        [TestMethod]
        public void TestBidBidSlotsFull()
        {
            //VARIABLES
            const int MOCK_AUCTION_ITEM_ID = 100;
            const int MOCK_CHARACTER_ID = 777;
            const int MOCK_BID = 5000;

            //SETUP
            MockAuctionDao mockAuctionDao = new MockAuctionDao();
            for (int i = 0; i < AuctionService.MAX_BIDS; i++)
            {
                MockAuctionItem mockAuctionItem = new MockAuctionItem();
                mockAuctionItem.Id = i;
                mockAuctionItem.BidderID = MOCK_CHARACTER_ID;
                mockAuctionDao.AuctionedItems.Add(mockAuctionItem);
            }

            MockNecClient mockNecClient = new MockNecClient();
            mockNecClient.Character.Id = MOCK_CHARACTER_ID;

            AuctionService auctionService = new AuctionService(mockNecClient, mockAuctionDao);

            //TEST            
            AuctionItem auctionItem = new AuctionItem();
            auctionItem.Id = MOCK_AUCTION_ITEM_ID;            

            //TEST / RESOLVE
            AuctionException e = Assert.ThrowsException<AuctionException>(() => auctionService.Bid(auctionItem, MOCK_BID));
            Assert.AreEqual(AuctionException.AuctionExceptionType.BidSlotsFull, e.Type);
        }

        [Ignore] //TODO Stop ignoring me baka....
        [TestMethod]
        public void TestBidDimentoMedalExpired()
        {
            //VARIABLES
            const int MOCK_AUCTION_ITEM_ID = 100;
            const int MOCK_CHARACTER_ID = 777;
            const int MOCK_BID = 5000;

            //SETUP
            MockAuctionItem mockAuctionItem = new MockAuctionItem();
            mockAuctionItem.Id = MOCK_AUCTION_ITEM_ID;
            mockAuctionItem.CurrentBid = 0;
            mockAuctionItem.BidderID = 0;

            MockAuctionDao mockAuctionDao = new MockAuctionDao();
            mockAuctionDao.AuctionedItems.Add(mockAuctionItem);

            MockNecClient mockNecClient = new MockNecClient();
            mockNecClient.Character.Id = MOCK_CHARACTER_ID;
            //mockNecClient.Character.IsRoyal = true; //TODO DIMETO MEDAL

            AuctionService auctionService = new AuctionService(mockNecClient, mockAuctionDao);

            //TEST            
            AuctionItem auctionItem = new AuctionItem();
            auctionItem.Id = MOCK_AUCTION_ITEM_ID;
            auctionService.Bid(auctionItem, MOCK_BID);

            //RESOLVE
            AuctionException e = Assert.ThrowsException<AuctionException>(() => auctionService.Bid(auctionItem, MOCK_BID));
            Assert.AreEqual(AuctionException.AuctionExceptionType.BidSlotsFull, e.Type);
        }

        [Ignore] //TODO Stop ignoring me baka....
        [TestMethod]
        public void TestCancelBid()
        {
           //ignore for now
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

        }
    }
}
