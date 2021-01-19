using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item.LocationManager
{
    class Container
    {
        private const int NO_OPEN_SLOTS = -1;
        private readonly ItemInstance[] _slots;
        public int Size { get; }
        public int Count { get; private set; }
        public bool IsSorted { get; private set; }
        public Container(int size)
        {
            _slots = new ItemInstance[size];
            Size = size;
        }
        public bool IsFull
        {
            get
            {
                if (NextOpenSlot == NO_OPEN_SLOTS) return true;
                else return false;
            }
        }
        public bool IsEmpty
        {
            get
            {
                foreach (ItemInstance item in _slots)
                {
                    if (item != null) return false;
                }
                return true;
            }
        }
        public int NextOpenSlot
        {
            get
            {
                for (int i = 0; i < Size; i++)
                {
                    if (_slots[i] is null) return i;
                }
                return NO_OPEN_SLOTS;
            }
        }
        public void PutItem(int slot, ItemInstance item)
        {
            _slots[slot] = item;
            Count++;
            IsSorted = false;
        }
        public ItemInstance GetItem(int slot)
        {
            return _slots[slot];
        }
        public void RemoveItem(int slot)
        {
            _slots[slot] = null;
            Count--;
            IsSorted = false;
        }        
        public bool HasItem(int slot)
        {
            return _slots[slot] != null;
        }
        public void Sort()
        {
            if (IsSorted) return;
            Array.Sort(_slots, ItemComparer.Instance);
            IsSorted = true;
        }        
    }
}
