using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item
{
    public readonly struct ItemLocation
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
    }
}
