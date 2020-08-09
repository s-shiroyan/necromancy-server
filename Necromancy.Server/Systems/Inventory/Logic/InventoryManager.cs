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

        //DO NOT MAKE GOLD A PROPERTY, HELPS USE DATABASE CALLS SPARINGLY
        public int GetGold()
        {
            return new DbInventory().SelectInventoryGold(nClient.Character);
        }

        public void AddGold(int amount)
        {
            int gold = GetGold();
            gold = gold + amount;
            SetGold(gold);
        }

        public void SubtractGold(int amount)
        {
            int gold = GetGold();
            gold = gold - amount;
            SetGold(gold);
        }

        private void SetGold(int value)
        {
            new DbInventory().UpdateInventoryGold(nClient.Character, value);
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
