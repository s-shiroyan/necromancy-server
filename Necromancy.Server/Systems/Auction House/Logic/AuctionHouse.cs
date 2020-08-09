using Necromancy.Server.Auction.Database;
using Necromancy.Server.Database;
using Necromancy.Server.Model;
using Necromancy.Server.Systems.Auction_House.Logic;
using Necromancy.Server.Systems.Auction_House.Messages.Receive;
using Necromancy.Server.Systems.Inventory.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Auction_House
{
    public class AuctionHouse
    {
        public const int MAX_BIDS = 8;
        public const int MAX_LOTS = 5;
        private const double LISTING_FEE_PERCENT = .05;

        private readonly NecClient nClient;

        //TODO clean this passing of client variables up maybe turn it into a context?
        public AuctionHouse(NecClient nClient)
        {
            this.nClient = nClient;
        }

        public AuctionItem[] Search(SearchCriteria searchCritera)
        {
            return new AuctionItem[4];
        }

        public void Show()
        {
            recv_auction_notify_open aOpen = new recv_auction_notify_open();
            //aOpen.send(this);
        }

        public void Exhibit(AuctionItem auctionItem)
        {            
            int currentNumLots = GetLots().Length;
            if (currentNumLots >= MAX_LOTS)
                throw new AuctionException(AuctionException.Type.LOT_SLOTS_FULL);

            //TODO CHECK IF EQUIPPED
            if (false)
                throw new AuctionException(AuctionException.Type.EQUIP_LISTING);

            //TODO CHECK IF INVALID ITEM
            if (false)
                throw new AuctionException(AuctionException.Type.INVALID_LISTING);

            //TODO CHECK DIMETO MEDAL ROYAL ACCOUNT STATUS
            if (false)
                throw new AuctionException(AuctionException.Type.LOT_DIMETO_MEDAL_EXPIRED);

            //TODO CHECK ITEM ALREADY_LISTED items must have a unique serial in item spawn!
            if (false)
                throw new AuctionException(AuctionException.Type.LOT_DIMETO_MEDAL_EXPIRED);

            InventoryManager iManager = new InventoryManager(nClient);
            iManager.SubtractGold((int) Math.Ceiling(auctionItem.BuyoutPrice * LISTING_FEE_PERCENT));

            new DbAuction().InsertAuctionItem(auctionItem);
        }

        public AuctionItem[]  GetBids() {
            return new DbAuction().SelectAuctionBids(nClient.Character);
        }

        public AuctionItem[] GetLots()
        {
            return new DbAuction().SelectAuctionLots(nClient.Character);
        }
    
    }
}
