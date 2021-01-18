using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item
{
    abstract class ItemZone
    {
        public abstract ItemZoneType Type { get; }
        public abstract ItemInstance[][] Bags { get; }
        public bool IsSorted { get; set; }
        public abstract int MaxBagSlots { get; }
        public abstract int MaxBagSize { get; }
        public abstract bool IsValidHolder(int characterId, ItemInstance itemInstance);
        public bool PutItem(ItemLocation itemLocation, ItemInstance itemInstance)
        {
            throw new NotImplementedException();
        }
        public bool AddBag(int bagIndex, int size)
        {
            if (IsInvalidSlotIndex(bagIndex) || IsInvalidSize(size)) return false; //todo maybe make into exceptions
            Bags[bagIndex] = new ItemInstance[size];
            return true;
        }

        public bool RemoveBag(int bagIndex)
        {
            if (IsInvalidSlotIndex(bagIndex)) return false;
            for (int i = 0; i < Bags[bagIndex].Length; i++)
            {
                if (!(Bags[bagIndex][i] is null)) return false;
            }
            Bags[bagIndex] = null;
            return true;
        }

        public ItemInstance GetItem(int bagIndex, int slot)
        {
            return Bags[bagIndex][slot];
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
            List<ItemInstance> allItems = new List<ItemInstance>(MaxBagSize * MaxBagSlots);
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
                    if (listIndex < allItems.Count)
                    {
                        bag[j] = allItems[listIndex];
                        listIndex++;
                    }
                }
            }
            IsSorted = true;
        }

        public bool IsFull()
        {
            if (GetNextOpenSlot().Zone == ItemZoneType.InvalidZone) return true;
            else return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bagIndex"></param>
        /// <param name="slot"></param>
        /// <returns><b>True:</b> There is a valid location for an item.</returns>
        public bool IsValidLocation(int bagIndex, int slot)
        {
            if (bagIndex < 0 || bagIndex > MaxBagSlots)     return false; //Outside range for containers
            if (Bags[bagIndex] is null)                     return false; //No container at that index
            if (slot < 0 || slot >= Bags[bagIndex].Length)  return false; //Outside container's range
            return true; 
        }

        public ItemLocation GetNextOpenSlot()
        {
            for (int i = 0; i < Bags.Length; i++)
            {
                ItemInstance[] bag = Bags[i];
                for (int j = 0; j < bag.Length; j++)
                {
                    if (bag[j] is null) return new ItemLocation(Type, (byte)i, (short)j);
                }
            }
            return new ItemLocation(ItemZoneType.InvalidZone, 0, 0);
        }

        public bool IsLocationEmpty(ItemLocation itemLocation)
        {
            if (Bags[itemLocation.Bag][itemLocation.Slot] is null) return true;
            else return false;
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
