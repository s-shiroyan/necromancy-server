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
        private const int MAX_CONTAINERS_ADV_BAG = 8;
        private const int MAX_CONTAINER_SIZE_ADV_BAG = 24;

        private const int MAX_CONTAINERS_AVATAR = 9;
        private const int MAX_CONTAINER_SIZE_AVATAR = 50;

        private const int MAX_CONTAINERS_BAG_SLOT = 1;
        private const int MAX_CONTAINER_SIZE_BAG_SLOT = 7;

        private const int MAX_CONTAINERS_ROYAL_BAG = 1;
        private const int MAX_CONTAINER_SIZE_ROYAL_BAG = 24;

        private const int MAX_CONTAINERS_TREASURE_BOX = 1;
        private const int MAX_CONTAINER_SIZE_TREASURE_BOX = 10;

        private Dictionary<ItemZoneType, ItemZone> ZoneMap = new Dictionary<ItemZoneType, ItemZone>();        

        public ItemLocationManager()
        {
            ZoneMap.Add(ItemZoneType.AdventureBag,      new ItemZone(MAX_CONTAINERS_ADV_BAG, MAX_CONTAINER_SIZE_ADV_BAG));
            ZoneMap.Add(ItemZoneType.RoyalBag,          new ItemZone(MAX_CONTAINERS_ROYAL_BAG, MAX_CONTAINER_SIZE_ROYAL_BAG));
            ZoneMap.Add(ItemZoneType.BagSlot,           new ItemZone(MAX_CONTAINERS_BAG_SLOT, MAX_CONTAINER_SIZE_BAG_SLOT));
            ZoneMap.Add(ItemZoneType.AvatarInventory,   new ItemZone(MAX_CONTAINERS_AVATAR, MAX_CONTAINER_SIZE_AVATAR));
            ZoneMap.Add(ItemZoneType.TreasureBox,       new ItemZone(MAX_CONTAINERS_TREASURE_BOX, MAX_CONTAINER_SIZE_TREASURE_BOX));
        }

        public ItemInstance GetItem(ItemLocation itemLocation)
        {
            return ZoneMap[itemLocation.ZoneType].GetContainer(itemLocation.Container).GetItem(itemLocation.Slot);
        }

        public void PutItem(ItemLocation loc, ItemInstance item)
        {
            switch (loc.ZoneType)
            {
                case ItemZoneType.BagSlot:
                    {
                        ZoneMap[ItemZoneType.AdventureBag].PutContainer(loc.Slot + 1, item.BagSize);
                        ZoneMap[loc.ZoneType].GetContainer(loc.Container).PutItem(loc.Slot, item);
                        break;
                    }
                default:
                    {
                        ZoneMap[loc.ZoneType].GetContainer(loc.Container).PutItem(loc.Slot, item);
                        break;
                    }

            }
        }
        
        public void RemoveItem(ItemLocation loc)
        {
            switch (loc.ZoneType)
            {
                case ItemZoneType.BagSlot:
                    {
                        ZoneMap[ItemZoneType.AdventureBag].RemoveContainer(loc.Slot + 1);
                        ZoneMap[loc.ZoneType].GetContainer(loc.Container).RemoveItem(loc.Slot);
                        break;
                    }
                default:
                    {
                        ZoneMap[loc.ZoneType].GetContainer(loc.Container).RemoveItem(loc.Slot);
                        break;
                    }
            }
        }

        public ItemLocation NextOpenSlot(ItemZoneType itemZoneType)
        {
            int nextContainerWithSpace = ZoneMap[itemZoneType].NextContainerWithSpace;
            if (nextContainerWithSpace != ItemZone.NO_CONTAINERS_WITH_SPACE)
            {
                int nextOpenSlot = ZoneMap[itemZoneType].GetContainer(nextContainerWithSpace).NextOpenSlot;
                if(nextOpenSlot != Container.NO_OPEN_SLOTS)
                {
                    ItemLocation itemLocation = new ItemLocation(itemZoneType, (byte)nextContainerWithSpace, (short) nextOpenSlot);
                    return itemLocation;
                }
            }
            return new ItemLocation(ItemZoneType.InvalidZone, 0, 0);
        }

        public ItemLocation[] NextOpenSlots(ItemZoneType itemZoneType, int amount)
        {
            if (amount > ZoneMap[itemZoneType].TotalFreeSpace) throw new ArgumentOutOfRangeException("Not enough open slots");
            return ZoneMap[itemZoneType].GetNextXFreeSpaces(itemZoneType, amount);
        }

        public int GetTotalFreeSpace(ItemZoneType itemZoneType)
        {
            return ZoneMap[itemZoneType].TotalFreeSpace;
        }
    }
}
