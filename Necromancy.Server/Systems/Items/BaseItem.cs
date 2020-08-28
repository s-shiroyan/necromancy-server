using Necromancy.Server.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Items
{
    public class BaseItem
    {
        public int BaseId { get; set; }
        public ItemType Type { get; set; }
        public ItemEquipSlot EquipSlot { get; set; }
        public int IconId { get; set; }
    }
}
