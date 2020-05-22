using System;

namespace Necromancy.Server.Model.ItemModel
{
    [Flags]
    public enum EquipmentSlotType
    {
        HAND_R = 1,
        HAND_L = 1 << 1, // 2
        QUIVER = 1 << 2,
        HEAD = 1 << 3,
        BODY = 1 << 4,
        LEGS = 1 << 5,
        ARMS = 1 << 6,
        FEET = 1 << 7,
        MANTLE = 1 << 8,
        ACCESSORY_1,
        ACCESSORY_2,
        ACCESSORY_3,
        ACCESSORY_4,
        TALKRING,
        AVATAR_HEAD,
        AVATAR_BODY,
        AVATAR_LEGS,
        AVATAR_ARMS,
        AVATAR_FEET,
    }
}
