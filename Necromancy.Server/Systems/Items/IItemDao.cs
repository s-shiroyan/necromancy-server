using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Items
{
    public interface IItemDao
    {
        public SpawnedItem InsertSpawnedItem(int baseId);
        public SpawnedItem SelectSpawnedItem(int spawnId);
        public SpawnedItem SelectSpawnedItem(int characterId, ItemLocation);
        
    }
}
