using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item.LocationManager
{
    class IZWarehouseSP : ItemZone
    {
        public override ItemZoneType Type => ItemZoneType.WarehouseSp;
        public override int MaxBagSlots => throw new NotImplementedException();
        public override int MaxBagSize => throw new NotImplementedException();
        public override ItemInstance[][] Bags => _bags;

        private readonly ItemInstance[][] _bags;
        public IZWarehouseSP()
        {
            _bags = new ItemInstance[MaxBagSlots][];
        }
        public override bool IsValidHolder(int characterId, ItemInstance itemInstance)
        {
            if (itemInstance.OwnerID == 0 || itemInstance.OwnerID != characterId) return false;
            if (!itemInstance.IsStorable) return false;
            return true;
        }
    }
}
