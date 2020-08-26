using System;

namespace Necromancy.Server.Systems.Items
{
    [Flags]
    public enum ItemZone
    {
        AdventureBag    = 0,
        UNKNOWN1        = 1, //invisible?
        RoyalBag        = 2,
        Warehouse       = 3,
        UNKNOWN4        = 4,
        BagSlot         = 8,
        //probably warehouse expansions?
        UNKNOWN9        = 9, //shows item 
        WarehouseSp     = 10,
        AvatarInventory = 12 //seems to be max
    }
}
