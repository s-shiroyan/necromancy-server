using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item
{
    public class ItemInstance : ItemBase
    {
        public const int MAX_GEM_SLOTS = 3;

        public ItemInstance(long spawnId, ItemLocation location)
        {
            SpawnId = spawnId;
            Location = location;
        }

        /// <summary>
        /// ID Generated when item is spawned.
        /// </summary>
        public long SpawnId { get; private set; }
        /// <summary>
        /// Owner's character's ID.
        /// </summary>
        public int OwnerID { get; set; }
        /// <summary>
        /// Item's displayed name when unidentified. Typically "? <c>ItemType</c>".
        /// </summary>
        public string UnidentifiedName { get; set; }

        public byte Quantity { get; set; }

        public ItemStatuses Statuses { get; set; }

        public ItemLocation Location { get; private set; }

        public ItemEquipSlot CurrentEquipSlot { get; set; }
        /// <summary>
        /// Current durability remaining of the item.
        /// </summary>
        public int CurrentDurability { get; set; }

        public byte EnhancementLevel { get; set; }

        public byte SpecialForgeLevel { get; set; }

        public short Physical { get; set; }

        public short Magical { get; set; }

        public int MaximumDurability { get; set; }

        public byte Hardness { get; set; }
        
        public GemSlot[] GemSlots { get; set; }

        public int EnchantId { get; set; }

        public short GP { get; set; }

        public bool IsIdentified { 
            get
            {
                return ((ItemStatuses.Identified & Statuses) != 0) && ((ItemStatuses.Unidentified & Statuses) == 0);
            } 
        }
    }
}
