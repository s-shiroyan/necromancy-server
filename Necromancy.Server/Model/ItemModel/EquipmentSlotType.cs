using System;

namespace Necromancy.Server.Model.ItemModel
{
    [Flags]
    public enum EquipmentSlotType
    {
        NONE = 0,
        HAND_R = 1 << 0, // 1
        HAND_L = 1 << 1, // 2
        QUIVER = 1 << 2, // 4
        HEAD = 1 << 3, // 8
        BODY = 1 << 4,
        LEGS = 1 << 5,
        ARMS = 1 << 6,
        FEET = 1 << 7,
        MANTLE = 1 << 8,
        ACCESSORY_1 = 1 << 9,
        ACCESSORY_2 = 1 << 10,
        ACCESSORY_3 = 1 << 11,
        ACCESSORY_4 = 1 << 12,
        TALKRING = 1 << 13,
        AVATAR_HEAD = 1 << 14,
        AVATAR_BODY = 1 << 15,
        AVATAR_LEGS = 1 << 16,
        AVATAR_ARMS = 1 << 17,
        AVATAR_FEET = 1 << 18,
    }
}
