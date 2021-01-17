using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item
{
    public readonly struct ItemLocation : IEquatable<ItemLocation>
    {
        public ItemLocation(ItemZone zone, byte bag, short slot)
        {
            Zone = zone;
            Bag = bag;
            Slot = slot;
        }

        public ItemZone Zone { get; }
        public byte Bag { get; }
        public short Slot { get; }

        public bool Equals(ItemLocation other)
        {
            if(other.Zone != Zone) return false;
            if(other.Bag != Bag) return false;
            if(other.Slot != Slot) return false;
            return true;
        }
    }
}
