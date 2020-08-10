using Necromancy.Server.Inventory.Database;
using Necromancy.Server.Model;
using Necromancy.Server.Systems.Auction_House;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Inventory.Logic
{
    public class InventoryManager
    {
        private readonly NecClient nClient;

        public InventoryManager(NecClient nClient)
        {
            this.nClient = nClient;
        }

        public int GetGold()
        {
            return new DbInventory().SelectInventoryGold(nClient.Character);
        }

        public void AddGold(int amount)
        {
            new DbInventory().UpdateInventoryGoldAdd(nClient.Character, amount);
        }

        public void SubtractGold(int amount)
        {
            new DbInventory().UpdateInventoryGoldSubtract(nClient.Character, amount);
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
