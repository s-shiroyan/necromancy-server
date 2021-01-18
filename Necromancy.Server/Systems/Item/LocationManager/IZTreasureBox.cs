using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item.LocationManager
{
    class IZTreasureBox : ItemZone
    {
        public override ItemZoneType Type => ItemZoneType.TreasureBox;
        public override int MaxBagSlots => throw new NotImplementedException();
        public override int MaxBagSize => throw new NotImplementedException();
        public override ItemInstance[][] Bags => _bags;

        private readonly ItemInstance[][] _bags;
        public IZTreasureBox()
        {
            _bags = new ItemInstance[MaxBagSlots][];
        }
        public override bool IsValidHolder(int characterId, ItemInstance itemInstance)
        {
            return itemInstance.OwnerID == 0;
        }
    }
}
