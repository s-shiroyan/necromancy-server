using Necromancy.Server.Systems.Item;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Character
{
    /// <summary>
    /// Holds item cache in memory.<br/> <br/>
    /// Stores information about published items, current equipment, and equipped stats <br/>
    /// so that some calls to the database are not always required. 
    /// </summary>
    public class ItemManager
    {
        /// <summary>
        /// A Dictionary of all items that have been published to the client with the item's instance ID as the key.<br/>
        /// </summary>
        private readonly Dictionary<ulong, ItemInstance> PublishedItems = new Dictionary<ulong, ItemInstance>();

        //TODO find max of each
        private Zone AdventureBag = new Zone(5);
        private Zone RoyalBag = new Zone(0);
        private Zone Warehouse = new Zone(10);
        private Zone BagSlot = new Zone(5);
        private Zone WarehouseSp = new Zone(5);
        private Zone AvatarInventory = new Zone(5);
        private Zone TreasureBox = new Zone(0);

        private ItemLocation _nextOpenInventoryLocation = new ItemLocation(ItemZone.AdventureBag, 0, 0);
        private List<ItemInstance> _ListNormalEquipment = new List<ItemInstance>(14);
        
        public void AddPublishedItem(ItemInstance itemInstance)
        {
            PublishedItems.Add(itemInstance.InstanceID, itemInstance);          
        }

        public void RemovePublishedItem(ItemInstance itemInstance)
        {
            PublishedItems.Remove(itemInstance.InstanceID);
        }

        private class Zone
        {
            private ItemInstance[][] Bags { get; }
            public bool IsSorted { get; private set; }
            public int MaxBagSlots { get; }
            public int MaxBagSize { get; }

            private ArgumentOutOfRangeException _argumentOutOfRangeException = new ArgumentOutOfRangeException("Slot index out of range.");
            public Zone(int maxBagSlots, int maxBagSize)
            {
                Bags = new ItemInstance[maxBagSlots][];
                MaxBagSlots = maxBagSlots;
                MaxBagSize = maxBagSize;
            }

            public bool AddBag(int slot, int size)
            {
                if (IsInvalidSlotIndex(slot) || IsInvalidSize(size)) return false; //todo maybe make into exceptions
                Bags[slot] = new ItemInstance[size];
                return true;
            }

            public bool RemoveBag(int slot)
            {
                if (IsInvalidSlotIndex(slot)) return false;
                for (int i = 0; i < Bags[slot].Length; i++)
                {
                    if (!(Bags[slot][i] is null)) return false;
                }
                Bags[slot] = null;
                return true;
            }

            private bool IsInvalidSlotIndex(int slot)
            {
                return slot >= MaxBagSlots || slot < 0;
            }

            private bool IsInvalidSize(int size)
            {
                return size > MaxBagSize || size <= 0;
            }

            public void AutoSort()
            {
                if (IsSorted) return;
                List<ItemInstance> allItems = new List<ItemInstance>();
                for (int i = 0; i < Bags.Length; i++)
                {
                    ItemInstance[] bag = Bags[i];
                    if (bag is null) continue;
                    for (int j = 0; j < bag.Length; j++)
                    {
                        ItemInstance itemInstance = bag[j];
                        if (itemInstance is null) continue;
                        allItems.Add(itemInstance);
                    }                    
                }

                allItems.Sort(new ItemComparer());

                int listIndex = 0;
                for (int i = 0; i < Bags.Length; i++)
                {
                    ItemInstance[] bag = Bags[i];
                    if (bag is null) continue;
                    bag = new ItemInstance[bag.Length]; //empty the bag
                    Bags[i] = bag; //add it back to the bag slot

                    for (int j = 0; j < bag.Length; j++)
                    {      
                        if (listIndex < allItems.Count) {
                            bag[j] = allItems[listIndex];
                            listIndex++;
                        }
                    }
                }
                IsSorted = true;
            }
        }

        private class ItemComparer : IComparer<ItemInstance>
        {
            public int Compare(ItemInstance x, ItemInstance y)
            {
                //put unidentified items last
                if (x.IsIdentified && !y.IsIdentified) return -1;
                else if (!x.IsIdentified && y.IsIdentified) return 1;                
                else
                {
                    //then sort by type
                    if (x.Type < y.Type) return -1;
                    else if (x.Type > y.Type) return 1;
                    else
                    {
                        //then sort by base id, can't sort alphabetically as names are stored client side
                        if (x.BaseID > y.BaseID) return -1;
                        else if (x.BaseID < y.BaseID) return 1;
                        else return 0;
                    }                    
                }
            }
        }
    }
}
