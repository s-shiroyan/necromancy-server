using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item
{
    public class ItemSpawnParams
    {
        public ItemStatuses ItemStatuses { get; set; }
        public byte Quantity { get; set; } = 1;
        public GemSlot[] GemSlots { get; set; } = new GemSlot[0];
    }
}
