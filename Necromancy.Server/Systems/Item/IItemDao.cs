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
        public void DeleteItemInstance(ulong instanceIds);
        public void UpdateItemLocations(ulong[] instanceIds, ItemLocation[] locs);
        public void UpdateItemQuantities(ulong[] instanceIds, byte[] quantities);
        public void UpdateItemEquipMask(ulong instanceId, ItemEquipSlots equipSlots);
        public List<ItemInstance> SelectOwnedInventoryItems(int ownerId);
        
    }
}
