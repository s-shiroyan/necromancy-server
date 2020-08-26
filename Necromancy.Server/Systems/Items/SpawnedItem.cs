using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Items
{
    class SpawnedItem
    {
        /// <summary>
        /// ID Generated when item is spawned.
        /// </summary>
        public long Id { get; set; }
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

        public ItemZone Zone { get; set; }

        public byte BagNumber { get; set; }

        public short BagSlot { get; set; }

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
        /// <summary>
        /// Item's gem slots and their type.
        /// </summary>
        public GemType[] GemTypes { get; set; }

        public int EnchantId { get; set; }

        public int GP { get; set; }
        /// <summary>
        /// Base item.
        /// </summary>
        public BaseItem BaseItem { get; set; }
    }
}
