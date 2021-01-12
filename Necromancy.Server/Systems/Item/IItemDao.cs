using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item
{
    public interface IItemDao
    {
        public ItemInstance InsertSpawnedItem(int baseId);
        //public ItemInstance SelectSpawnedItem(int spawnId);
        //public ItemInstance SelectSpawnedItem(int characterId, ItemLocation);
        
    }
}
