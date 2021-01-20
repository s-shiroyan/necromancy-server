using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item
{
    public interface IItemDao
    {
        public ItemInstance InsertItemInstance(int baseId);
        public List<ItemInstance> InsertItemInstances(int ownerId, ItemLocation[] locs, int[] baseId, ItemSpawnParams[] spawnParams);
        public ItemInstance SelectItemInstance(long instanceId);
        public ItemInstance SelectItemInstance(int characterId, ItemLocation itemLocation);
        public void UpdateItemLocations(ulong[] instanceIds, ItemLocation[] locs);
        public List<ItemInstance> SelectOwnedInventoryItems(int ownerId);
        
    }
}
