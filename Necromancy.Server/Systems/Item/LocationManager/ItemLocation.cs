using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item
{
    public readonly struct ItemLocation : IEquatable<ItemLocation>
    {
        public ItemLocation(ItemZoneType zone, byte bag, short slot)
        {
            Zone = zone;
            Bag = bag;
            Slot = slot;
            _hashcode = ((int) zone << 24) + (bag << 16) + slot;
        }

        public ItemZoneType Zone { get; }
        public byte Bag { get; }
        public short Slot { get; }

        private readonly int _hashcode;

        public bool Equals(ItemLocation other)
        {
            if(other.Zone != Zone)  return false;
            if(other.Bag  != Bag)   return false;
            if(other.Slot != Slot)  return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ItemLocation && Equals((ItemLocation) obj);
        }

        public override int GetHashCode() { return _hashcode; }
    }
}
