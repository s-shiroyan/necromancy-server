using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item
{
    public class ItemInstance : ItemBase
    {
        public const int MAX_GEM_SLOTS = 3;

        /// <summary>
        /// An instance of a base item. Holds mostly changable values.
        /// </summary>
        /// <param name="instanceId">Item's generated ID from the database.</param>
        /// <param name="location">The location of the item.</param>
        public ItemInstance(ulong instanceId, ItemLocation location)
        {
            InstanceID = instanceId;
            Location = location;
        }
        /// <summary>
        /// ID Generated when item is created from a base item template.
        /// </summary>
        public ulong InstanceID { get; private set; }
        /// <summary>
        /// Owner's character ID.
        /// </summary>
        public uint OwnerID { get; set; }
        /// <summary>
        /// Item's displayed name when unidentified. Always "? <c>ItemType</c>".
        /// </summary>
        public string UnidentifiedName { 
            get {
                return "? " + Type.ToString();
            }
        }

        public byte Quantity { get; set; }

        public ItemStatuses Statuses { get; set; }

        public ItemLocation Location { get; private set; }

        public ItemEquipSlots CurrentEquipSlot { get; set; }
        /// <summary>
        /// Current durability remaining of the item.
        /// </summary>
        public int CurrentDurability { get; set; }

        public byte EnhancementLevel { get; set; }

        public byte SpecialForgeLevel { get; set; }

        public string TalkRingName { get; set; }

        public short Physical { get; set; }

        public short Magical { get; set; }

        public int MaximumDurability { get; set; }

        public byte Hardness { get; set; }

        /// <summary>
        /// Weight in thousandths.
        /// </summary>
        public int Weight { get; set; }
        
        public GemSlot[] GemSlots { get; set; }

        public int EnchantId { get; set; }

        public short GP { get; set; }
        /// <summary>
        /// Item is provided with 'Protect' status until this date in seconds. 
        /// Maximum year is 2038 because it is an integer.
        /// </summary>
        public int ProtectUntil { get; set; }

        public short PlusPhysical { get; set; }
        public short PlusMagical { get; set; }
        public short PlusWeight { get; set; }
        public short PlusDurability { get; set; }
        public short PlusGP { get; set; }
        public short PlusRangedEff { get; set; }
        public short PlusReserviorEff { get; set; }


        /// <summary>
        /// Helper function to check if the item is identified or not.
        /// </summary>
        public bool IsIdentified { 
            get
            {
                return ((ItemStatuses.Identified & Statuses) != 0) && ((ItemStatuses.Unidentified & Statuses) == 0);
            } 
        }
    }
}
