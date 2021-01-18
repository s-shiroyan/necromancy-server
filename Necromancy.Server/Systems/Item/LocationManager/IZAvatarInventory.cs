using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item.LocationManager
{
    class IZAvatarInventory : ItemZone
    {
        public override ItemZoneType Type => ItemZoneType.AvatarInventory;
        public override int MaxBagSlots => throw new NotImplementedException();
        public override int MaxBagSize => throw new NotImplementedException();
        public override ItemInstance[][] Bags => _bags;

        private readonly ItemInstance[][] _bags;
        public IZAvatarInventory()
        {
            _bags = new ItemInstance[MaxBagSlots][];
        }
        public override bool IsValidHolder(int characterId, ItemInstance itemInstance)
        {
            if (itemInstance.OwnerID == 0 || itemInstance.OwnerID != characterId) return false;
            return true;
        }
    }
}
