using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item.LocationManager
{
    /// <summary>
    /// Holds item cache in memory.<br/> <br/>
    /// Stores information about published items, and their locations. <b>Does not validate any actions.</b>
    /// </summary>
    public class ItemLocationManager
    {
        /// <summary>
        /// A Dictionary of all items that have been published to the client with the item's instance ID as the key.<br/>
        /// </summary>
        private readonly Dictionary<ulong, ItemInstance> PublishedItems = new Dictionary<ulong, ItemInstance>();

        //TODO FIX TEMP VALUES
        private const int SLOT_MAX_ADV_BAG      = 5;
        private const int SLOT_MAX_ROYAL_BAG    = 1;
        private const int SLOT_MAX_WAREHOUSE    = 10;
        private const int SLOT_MAX_BAG_SLOT     = 1;
        private const int SLOT_MAX_WARHOUSE_SP  = 5;
        private const int SLOT_MAX_AVAT_INV     = 1;
        private const int SLOT_MAX_TREASURE_BOX = 1;

        private const int BAG_SIZE_MAX_ADV_BAG      = 24;
        private const int BAG_SIZE_MAX_ROYAL_BAG    = 24;
        private const int BAG_SIZE_MAX_WAREHOUSE    = 40;
        private const int BAG_SIZE_MAX_BAG_SLOT     = 5;
        private const int BAG_SIZE_MAX_WARHOUSE_SP  = 40;
        private const int BAG_SIZE_MAX_AVAT_INV     = 24;
        private const int BAG_SIZE_MAX_TREASURE_BOX = 10;

        private Dictionary<ItemZoneType, ItemZone> ZoneMap = new Dictionary<ItemZoneType, ItemZone>();
        private List<ItemInstance> _ListNormalEquipment = new List<ItemInstance>(14);
        
        public void AddPublishedItem(ItemInstance itemInstance)
        {
            PublishedItems.Add(itemInstance.InstanceID, itemInstance);          
        }

        public void RemovePublishedItem(ItemInstance itemInstance)
        {
            PublishedItems.Remove(itemInstance.InstanceID);
        }

        public ItemLocationManager()
        {
            ZoneMap.Add(ItemZoneType.AdventureBag,      new IZAdventureBag());
            ZoneMap.Add(ItemZoneType.RoyalBag,          new IZRoyalBag());
            ZoneMap.Add(ItemZoneType.Warehouse,         new IZWarehouse());
            ZoneMap.Add(ItemZoneType.BagSlot,           new IZBagSlot());
            ZoneMap.Add(ItemZoneType.WarehouseSp,       new IZWarehouseSP());
            ZoneMap.Add(ItemZoneType.AvatarInventory,   new IZAvatarInventory());
            ZoneMap.Add(ItemZoneType.TreasureBox,       new IZTreasureBox());
        }

        public ItemInstance GetItem(ItemLocation itemLocation)
        {
            return ZoneMap[itemLocation.Zone].GetItem(itemLocation.Bag, itemLocation.Slot);
        }


        public void PutItem(ItemLocation itemLocation, ItemInstance itemInstance)
        {

        }

        public ItemInstance GetItem(ulong instanceId)
        {
            return PublishedItems[instanceId];
        }

        public void AutoSort(ItemZoneType itemZoneType)
        {
            ZoneMap[itemZoneType].AutoSort();
        }

        public bool IsValidLocation(ItemLocation itemLocation)
        {
            if (!ZoneMap.ContainsKey(itemLocation.Zone)) return false;
            else
            {
                return ZoneMap[itemLocation.Zone].IsValidLocation(itemLocation.Bag, itemLocation.Slot);
            }
        }


        

        
    }
}
