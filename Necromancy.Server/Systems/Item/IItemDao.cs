using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item
{
    public interface IItemDao
    {
        public ItemInstance InsertItemInstance(int baseId);
        public List<ItemInstance> InsertItemInstances(int characterId, ItemLocation[] itemLocations, int[] baseId);
        public ItemInstance SelectItemInstance(long instanceId);
        public ItemInstance SelectItemInstance(int characterId, ItemLocation itemLocation);
        public List<ItemInstance> SelectOwnedInventoryItems(int characterId);
        
    }
}
