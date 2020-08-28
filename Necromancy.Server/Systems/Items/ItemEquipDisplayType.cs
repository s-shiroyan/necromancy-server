namespace Necromancy.Server.Model
{
    public enum ItemEquipDisplayType
    {
        //TODO 100,120 str_table weird bullshit?        
        NONE = 0,
        TRUMP = 1,
        DAGGER = 2,
        SWORD_1H = 4,
        RAPIER = 3,
        SWORD_2H = 5,
        KATANA_1H = 6,
        KATANA_2H = 7,
        AXE_1H = 8,
        AXE_2H = 9,
        CLUB_1H = 11,
        CLUB_2H = 12,
        WAND_1H = 13,
        WAND_2H = 14,
        SPEAR = 10,
        BOW = 15,
        CROSSBOW = 16,
        GUN = 17,
        INSTRUMENT = 18,
        SHIELD_SMALL = 19,
        SHIELD_MEDIUM = 20,
        SHIELD_LARGE = 21,
        ARROW = 22,
        BOLT = 23,
        BULLET = 24,        
        ARMOR = 25,
        ACCESSORY = 26,
        OTHER = 27,        
        PROTECTOR = ARMOR, 
        HELMET = ARMOR,
        HAT = ARMOR,
        HOOD = ARMOR,
        ARMOR_TOP = ARMOR, 
        ARMOR_BOTTOM = ARMOR, 
        CLOTHES_TOP = ARMOR, 
        CLOTHES_BOTTOM = ARMOR,
        COAT = ARMOR,
        GLOVES = ARMOR,
        GAUNTLETS = ARMOR,
        BRACLET = ARMOR,
        SABATONS = ARMOR, //S.SHOE
        SHOES = ARMOR, // SHOES
        CLOAK = ARMOR, 
        RING = ACCESSORY,
        EARRING = ACCESSORY,
        NECKLACE = ACCESSORY, //NCKLCE
        BELT = ACCESSORY,
        DRUG = OTHER, //MEDICINE
        SKILLCHIP = OTHER, //SKLCHP
        BAG = OTHER,
        LOSTSOUL = OTHER, //LSTSOL
        EVENT = OTHER,
        FORGESTONE = OTHER, //FG_STN
        TALKRING_MAKEKIT = OTHER, //TOKRNG
        MASTER_TALKRING = OTHER, //TOKRING
        TALKRING = OTHER, //TOKRNG
        FORGEGUARDSTONE = OTHER, //FG_GRD
        GEM = OTHER, //GEM
        GEM_PIECE = OTHER, //GEM
        GEM_SYNTHESIS = OTHER, //GEM
        MAP_FRAGMENT = OTHER, //MAP PC
        OTHERS = OTHER, //OTHER
        AVATAR = ARMOR,
        ALLCHT = OTHER,
        BUFF = OTHER,
        LEATHER = OTHER, //L.SHOES
        FORGETICKET = OTHER, //FG_TKT
        FORGE_SP_SUPPORT = OTHER, //特殊鍛錬万能素材
        MATERIAL = OTHER, //JP ONLY
        WHOLECHAT = ALLCHT,
    }
}
