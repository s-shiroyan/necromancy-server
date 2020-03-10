using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Model
{
    public enum ITEM_TYPE : int
    {
        HAND = 0,
        TRUMP,
        DAGGER,
        SWORD_1H,
        RAPIER,
        SWORD_2H,
        KATANA_1H,
        KATANA_2H,
        AXE_1H,
        AXE_2H,
        CLUB_1H,
        CLUB_2H,
        WAND_1H,
        WAND_2H,
        SPEAR,
        BOW,
        CROSSBOW,
        GUN,
        INSTRUMENT,
        SHIELD_SMALL,
        SHIELD_MEDIUM,
        SHIELD_LARGE,
        ARROW,
        BOLT,
        BULLET,
        PROTECTOR,
        ACCESSORY,
        HELMET,
        HAT,
        HOOD,
        ARMOR_TOPS,
        ARMOR_BOTTOMS,
        CLOTHES_TOPS,
        CLOTHES_BOTTOMS,
        COAT,
        GLOVES,
        GAUNTLETS,
        BRACLET,
        SABATONS,
        SHOES,
        MANTLE,
        RING,
        EARRING,
        NECKLACE,
        BELT,
        DRUG,
        SKILLCHIP,
        BAG,
        LOSTSOUL,
        EVENT,
        FORGESTONE,
        TALKRING_MAKEKIT,
        MASTER_TALKRING,
        TALKRING,
        FORGEGUARDSTONE,
        GEM,
        GEM_PIECE,
        GEM_SYNTHESIS,
        MAP_FRAGMENT,
        OTHERS,
        AVATAR,
        WHOLECHAT,
        BUFF,
        LEATHER,
        FORGETICKET
    }

    public enum EQUIP_BIT : int
    { 
        HAND_R = 1,
        HAND_L = 1 << 1,
        QUIVER = 1 << 2,
        HEAD = 1 << 3,
        BODY = 1 << 4,
        LEGS = 1 << 5,
        ARMS = 1 << 6,
        FEET = 1 << 7,
        MANTLE = 1 << 8
    }

}
