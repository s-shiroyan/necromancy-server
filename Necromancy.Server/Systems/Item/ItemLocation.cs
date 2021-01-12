using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item
{
    public class ItemLocation
    {
        public ItemLocation(ItemZone zone, byte bag, short slot)
        {
            Zone = zone;
            Bag = bag;
            Slot = slot;
        }

        public ItemZone Zone { get; private set; }
        public byte Bag { get; private set; }
        public short Slot { get; private set; }
    }
}
