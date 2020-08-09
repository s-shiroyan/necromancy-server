using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.World.Logic
{
    class SpawnedItem
    {
        /// <summary>
        /// ID Generated when item is spawned.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Base item ID.
        /// </summary>
        public int ItemID { get; set; }
        /// <summary>
        /// Owner's character's ID.
        /// </summary>
        public int OwnerID { get; set; }
        /// <summary>
        /// Item's displayed name.
        /// </summary>
        public int Name { get; set; }
        /// <summary>
        /// Durability remaining of the item.
        /// </summary>
        public int Durability { get; set; }
        /// <summary>
        /// Item's gem slots and their type.
        /// </summary>
        public GemType[] GemTypes { get; set; }
    }
}
