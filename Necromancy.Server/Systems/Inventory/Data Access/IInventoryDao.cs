using Necromancy.Server.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Inventory.Data_Access
{
    public interface IInventoryDao
    {
        public int SelectInventoryGold(Character character);

        public void UpdateInventoryGoldAdd(Character character, int amount);

        public void UpdateInventoryGoldSubtract(Character character, int amount);

        public long SelectSpawnIdBySlot(Character character, int bagNumber, int bagSlot);
    }
}
