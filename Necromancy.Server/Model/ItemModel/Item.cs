using System;

namespace Necromancy.Server.Model.ItemModel
{
    public class Item
    {
        /// <summary>
        /// Retrieve EquipmentSlotType by itemId.
        /// TODO: remove this method and use `EquipmentSlotTypeByItemType` instead when Item -> ItemType mapping is complete.
        /// </summary>
        public static EquipmentSlotType EquipmentSlotTypeByItemId(int itemId)
        {
            int value = itemId / 100000;
            switch (value)
            {
                case 0: return EquipmentSlotType.NONE;
                case 1: return EquipmentSlotType.HEAD;
                case 2: return EquipmentSlotType.BODY;
                case 3: return EquipmentSlotType.LEGS;
                case 4: return EquipmentSlotType.ARMS;
                case 5: return EquipmentSlotType.FEET;
                case 6: return EquipmentSlotType.MANTLE;
                case 7: return EquipmentSlotType.BODY;
                case 101: return EquipmentSlotType.HAND_R;
                case 102: return EquipmentSlotType.HAND_R;
                case 103: return EquipmentSlotType.HAND_R;
                case 104: return EquipmentSlotType.HAND_R;
                case 105: return EquipmentSlotType.HAND_R | EquipmentSlotType.HAND_L;
                case 108: return EquipmentSlotType.HAND_R;
                case 109: return EquipmentSlotType.HAND_R | EquipmentSlotType.HAND_L;
                case 110: return EquipmentSlotType.HAND_R;
                case 111: return EquipmentSlotType.HAND_R | EquipmentSlotType.HAND_L;
                case 112: return EquipmentSlotType.HAND_R;
                case 113: return EquipmentSlotType.HAND_R | EquipmentSlotType.HAND_L;
                case 114: return EquipmentSlotType.HAND_R;
                case 115: return EquipmentSlotType.HAND_R | EquipmentSlotType.HAND_L;
                case 116: return EquipmentSlotType.HAND_R | EquipmentSlotType.HAND_L;
                case 117: return EquipmentSlotType.HAND_R | EquipmentSlotType.HAND_L;
                case 150: return EquipmentSlotType.HAND_L;
                case 151: return EquipmentSlotType.HAND_L;
                case 152: return EquipmentSlotType.HAND_L;
                case 200: return EquipmentSlotType.QUIVER;
                case 300: return EquipmentSlotType.TALKRING;
                case 301: return EquipmentSlotType.ACCESSORY_1;
                case 302: return EquipmentSlotType.ACCESSORY_2;
                case 303: return EquipmentSlotType.ACCESSORY_3;
                case 304: return EquipmentSlotType.ACCESSORY_4;
                case 501: return EquipmentSlotType.NONE;
                case 503: return EquipmentSlotType.NONE;
                case 504: return EquipmentSlotType.NONE;
                case 505: return EquipmentSlotType.NONE;
                case 506: return EquipmentSlotType.NONE;
                case 510: return EquipmentSlotType.NONE;
                case 511: return EquipmentSlotType.NONE;
                case 601: return EquipmentSlotType.NONE;
                case 602: return EquipmentSlotType.NONE;
                case 603: return EquipmentSlotType.NONE;
                case 621: return EquipmentSlotType.NONE;
                case 700: return EquipmentSlotType.NONE;
                case 800: return EquipmentSlotType.NONE;
                case 900: return EquipmentSlotType.NONE;
                case 998: return EquipmentSlotType.NONE;
                case 999: return EquipmentSlotType.NONE;
                default: return EquipmentSlotType.NONE;
            }
        }

        /// <summary>
        /// Retrieve ItemType by itemId.
        /// TODO: remove this method when Item -> ItemType mapping is complete.
        /// </summary>
        public static ItemType ItemTypeByItemId(int itemId)
        {
            int value = itemId / 100000;
            switch (value)
            {
                case 0: return ItemType.NONE;
                case 1: return ItemType.HELMET;
                case 2: return ItemType.ARMOR_TOPS;
                case 3: return ItemType.ARMOR_BOTTOMS;
                case 4: return ItemType.GLOVES;
                case 5: return ItemType.SABATONS;
                case 6: return ItemType.MANTLE;
                case 7: return ItemType.EVENT;
                case 101: return ItemType.HELMET;
                case 102: return ItemType.DAGGER;
                case 103: return ItemType.SWORD_1H;
                case 104: return ItemType.RAPIER;
                case 105: return ItemType.SWORD_2H;
                case 108: return ItemType.AXE_1H;
                case 109: return ItemType.AXE_2H;
                case 110: return ItemType.CLUB_1H;
                case 111: return ItemType.CLUB_2H;
                case 112: return ItemType.WAND_1H;
                case 113: return ItemType.WAND_2H;
                case 114: return ItemType.SPEAR;
                case 115: return ItemType.BOW;
                case 116: return ItemType.CROSSBOW;
                case 117: return ItemType.GUN;
                case 150: return ItemType.SHIELD_SMALL;
                case 151: return ItemType.SHIELD_MEDIUM;
                case 152: return ItemType.SHIELD_LARGE;
                case 200: return ItemType.ARROW;
                case 300: return ItemType.TALKRING;
                case 301: return ItemType.RING;
                case 302: return ItemType.EARRING;
                case 303: return ItemType.NECKLACE;
                case 304: return ItemType.BELT;
                case 501: return ItemType.OTHERS;
                case 503: return ItemType.OTHERS;
                case 504: return ItemType.OTHERS;
                case 505: return ItemType.OTHERS;
                case 506: return ItemType.OTHERS;
                case 510: return ItemType.OTHERS;
                case 511: return ItemType.OTHERS;
                case 601: return ItemType.OTHERS;
                case 602: return ItemType.OTHERS;
                case 603: return ItemType.OTHERS;
                case 621: return ItemType.OTHERS;
                case 700: return ItemType.MASTER_TALKRING;
                case 800: return ItemType.GEM;
                case 900: return ItemType.BAG;
                case 998: return ItemType.OTHERS;
                case 999: return ItemType.OTHERS;
                default: return ItemType.NONE;
            }
        }

        public static EquipmentSlotType GetEquipmentSlotTypeBySlotNumber(int slotNumber)
        {
            EquipmentSlotType equipmentSlotType = EquipmentSlotType.NONE;
            if (slotNumber > 0)
            {
                int value = 1 << slotNumber;
                if (Enum.IsDefined(typeof(EquipmentSlotType), value))
                {
                    equipmentSlotType = (EquipmentSlotType) value;
                }
            }

            return equipmentSlotType;
        }

        /// <summary>
        /// Maps ItemType to EquipmentSlotType
        /// </summary>
        public static EquipmentSlotType EquipmentSlotTypeByItemType(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.TRUMP: return EquipmentSlotType.HAND_R;
                case ItemType.DAGGER: return EquipmentSlotType.HAND_R;
                case ItemType.RAPIER: return EquipmentSlotType.HAND_R;
                case ItemType.SWORD_1H: return EquipmentSlotType.HAND_R;
                case ItemType.SWORD_2H: return EquipmentSlotType.HAND_R | EquipmentSlotType.HAND_L;
                case ItemType.KATANA_1H: return EquipmentSlotType.HAND_R;
                case ItemType.KATANA_2H: return EquipmentSlotType.HAND_R | EquipmentSlotType.HAND_L;
                case ItemType.AXE_1H: return EquipmentSlotType.HAND_R;
                case ItemType.AXE_2H: return EquipmentSlotType.HAND_R | EquipmentSlotType.HAND_L;
                case ItemType.SPEAR: return EquipmentSlotType.HAND_R;
                case ItemType.CLUB_1H: return EquipmentSlotType.HAND_R;
                case ItemType.CLUB_2H: return EquipmentSlotType.NONE;
                case ItemType.WAND_1H: return EquipmentSlotType.HAND_R;
                case ItemType.WAND_2H: return EquipmentSlotType.HAND_R | EquipmentSlotType.HAND_L;
                case ItemType.BOW: return EquipmentSlotType.HAND_R | EquipmentSlotType.HAND_L;
                case ItemType.CROSSBOW: return EquipmentSlotType.HAND_R | EquipmentSlotType.HAND_L;
                case ItemType.GUN: return EquipmentSlotType.HAND_R | EquipmentSlotType.HAND_L;
                case ItemType.INSTRUMENT: return EquipmentSlotType.NONE;
                case ItemType.SHIELD_SMALL: return EquipmentSlotType.HAND_R;
                case ItemType.SHIELD_MEDIUM: return EquipmentSlotType.HAND_R;
                case ItemType.SHIELD_LARGE: return EquipmentSlotType.HAND_R;
                case ItemType.ARROW: return EquipmentSlotType.QUIVER;
                case ItemType.BOLT: return EquipmentSlotType.QUIVER;
                case ItemType.BULLET: return EquipmentSlotType.QUIVER;
                case ItemType.PROTECTOR: return EquipmentSlotType.NONE;
                case ItemType.ACCESSORY: return EquipmentSlotType.NONE;
                case ItemType.HELMET: return EquipmentSlotType.HEAD;
                case ItemType.HAT: return EquipmentSlotType.HEAD;
                case ItemType.HOOD: return EquipmentSlotType.HEAD;
                case ItemType.ARMOR_TOPS: return EquipmentSlotType.NONE;
                case ItemType.ARMOR_BOTTOMS: return EquipmentSlotType.NONE;
                case ItemType.CLOTHES_TOPS: return EquipmentSlotType.NONE;
                case ItemType.CLOTHES_BOTTOMS: return EquipmentSlotType.NONE;
                case ItemType.COAT: return EquipmentSlotType.NONE;
                case ItemType.GLOVES: return EquipmentSlotType.NONE;
                case ItemType.GAUNTLETS: return EquipmentSlotType.NONE;
                case ItemType.BRACLET: return EquipmentSlotType.NONE;
                case ItemType.SABATONS: return EquipmentSlotType.NONE;
                case ItemType.LEATHER: return EquipmentSlotType.NONE;
                case ItemType.SHOES: return EquipmentSlotType.NONE;
                case ItemType.MANTLE: return EquipmentSlotType.NONE;
                case ItemType.RING: return EquipmentSlotType.NONE;
                case ItemType.EARRING: return EquipmentSlotType.NONE;
                case ItemType.NECKLACE: return EquipmentSlotType.NONE;
                case ItemType.BELT: return EquipmentSlotType.NONE;
                case ItemType.DRUG: return EquipmentSlotType.NONE;
                case ItemType.SKILLCHIP: return EquipmentSlotType.NONE;
                case ItemType.BAG: return EquipmentSlotType.NONE;
                case ItemType.LOSTSOUL: return EquipmentSlotType.NONE;
                case ItemType.EVENT: return EquipmentSlotType.NONE;
                case ItemType.SYSTEM: return EquipmentSlotType.NONE;
                case ItemType.FORGESTONE: return EquipmentSlotType.NONE;
                case ItemType.TALKRING_MAKEKIT: return EquipmentSlotType.TALKRING;
                case ItemType.MASTER_TALKRING: return EquipmentSlotType.TALKRING;
                case ItemType.TALKRING: return EquipmentSlotType.TALKRING;
                case ItemType.FORGEGUARDSTONE: return EquipmentSlotType.NONE;
                case ItemType.GEM: return EquipmentSlotType.NONE;
                case ItemType.GEM_PIECE: return EquipmentSlotType.NONE;
                case ItemType.GEM_SYNTHESIS: return EquipmentSlotType.NONE;
                case ItemType.MAP_FRAGMENT: return EquipmentSlotType.NONE;
                case ItemType.OTHERS: return EquipmentSlotType.NONE;
                case ItemType.AVATAR: return EquipmentSlotType.NONE;
                case ItemType.WHOLECHAT: return EquipmentSlotType.NONE;
                case ItemType.ALLCHT: return EquipmentSlotType.NONE;
                case ItemType.BUFF: return EquipmentSlotType.NONE;
                case ItemType.FORGETICKET: return EquipmentSlotType.NONE;
                case ItemType.FORGE_SP_SUPPORT: return EquipmentSlotType.NONE;
                default: return EquipmentSlotType.NONE;
            }
        }

        public Item()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ItemType ItemType { get; set; }
        public EquipmentSlotType EquipmentSlotType { get; set; }
        public int Physical { get; set; }
        public int Magical { get; set; }
        public int Durability { get; set; }
    }
}
