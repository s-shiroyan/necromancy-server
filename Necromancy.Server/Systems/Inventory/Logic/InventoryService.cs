using Necromancy.Server.Inventory.Database;
using Necromancy.Server.Model;
using Necromancy.Server.Systems.Auction_House;
using Necromancy.Server.Systems.Inventory.Data_Access;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Inventory.Logic
{
    public class InventoryService
    {
        private readonly NecClient nClient;
        private readonly IInventoryDao _InventoryDao;

        public InventoryService(NecClient nClient, IInventoryDao inventoryDao)
        {
            this.nClient = nClient;
            _InventoryDao = inventoryDao;
        }

        public InventoryService(NecClient nClient)
        {
            this.nClient = nClient;
            _InventoryDao = new InventoryDao();
        }

        public int GetGold()
        {
            return new InventoryDao().SelectInventoryGold(nClient.Character);
        }

        public void AddGold(int amount)
        {
            new InventoryDao().UpdateInventoryGoldAdd(nClient.Character, amount);
        }

        public void SubtractGold(int amount)
        {
            new InventoryDao().UpdateInventoryGoldSubtract(nClient.Character, amount);
        }

        public void AddItem()
        {
            throw new NotImplementedException();
        }

        public void DestroyItem()
        {
            throw new NotImplementedException();
        }

        public void EquipItem()
        {
            throw new NotImplementedException();
        }

        public void UnEquipItem()
        {
            throw new NotImplementedException();
        }
    }
}
