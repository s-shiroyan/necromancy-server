using Necromancy.Server.Model;
using System;
using System.Data;
using System.Data.Common;

namespace Necromancy.Server.Systems.Item
{
    public class ItemDao : DatabaseAccessObject, IItemDao
    {
        private const string SqlSpawnedView = @"
            DROP VIEW IF EXISTS item_instance;
            CREATE VIEW IF NOT EXISTS 
                item_instance 
                    (
                        id,
                        character_id,
                        zone,
                        bag,
                        slot,
                        base_id,
                        quantity,
                        is_identified,
                        is_cursed,
                        is_blessed,
                        current_equip_slot,
                        current_durability,
                        maximum_durability,
                        enhancement_level,
                        special_forge_level,
                        physical,
                        magical,
                        hardness,
                        gem_slot_1_type,
                        gem_slot_2_type,
                        gem_slot_3_type,
                        gem_id_slot_1,
                        gem_id_slot_2,
                        gem_id_slot_3,
                        enchant_id,
                        gp,
                        item_type,
                        quality,
                        '1',
                        max_stack_size,
                        '3',
                        es_hand_r,
                        es_hand_l,
                        es_quiver,
                        es_head,
                        es_body,
                        es_legs,
                        es_arms,
                        es_feet,
                        es_mantle,
                        es_ring,
.                       es_earring,
                        es_necklace,
                        es_belt,
                        es_talkring,
                        es_avatar_head,
                        es_avatar_body,
                        es_avatar_legs,
                        es_avatar_arms,
                        es_avatar_feet,
                        req_hum_m,
                        req_hum_f,
                        req_elf_m,
                        req_elf_f,
                        req_dwf_m,
                        req_dwf_f,
                        req_por_m,
                        req_por_f,
                        req_gnm_m,
                        req_gnm_f,
                        req_fighter,
                        req_thief,
                        req_mage,
                        req_priest,
                        req_lawful,
                        req_neutral,
                        req_chaotic,
                        '40',
                        '41',
                        req_str,
                        req_vit,
                        req_dex,
                        req_agi,
                        req_int,
                        req_pie,
                        req_luk,
                        req_soul_rank,
                        req_lvl,
                        '51',
                        '52',
                        phys_slash,
                        phys_strike,
                        phys_pierce,
                        '56',
                        pdef_fire,
                        pdef_water,
                        pdef_wind,
                        pdef_earth,
                        pdef_light,
                        pdef_dark,
                        '63',
                        '64',
                        '65',
                        matk_fire,
                        matk_water,
                        matk_wind,
                        matk_earth,
                        matk_light,
                        matk_dark,
                        '72',
                        '73',
                        '74',
                        '75',
                        '76',
                        seffect_hp,
                        seffect_mp,
                        seffect_str,
                        seffect_vit,
                        seffect_dex,
                        seffect_agi,
                        seffect_int,
                        seffect_pie,
                        seffect_luk,
                        res_poison,
                        res_paralyze,
                        res_petrified,
                        res_faint,
                        res_blind,
                        res_sleep,
                        res_silent,
                        res_charm,
                        res_confusion,
                        res_fear,
                        '96',
                        status_malus,
                        status_percent,
                        '99',
                        object_type,
                        equip_slot,
                        '102',
                        '103',
                        '104',
                        icon_type,
                        no_storage,
                        no_discard,
                        no_sell,
                        no_trade,
                        no_trade_after_used,
                        no_stolen,
                        gold_border,
                        lore,
                        icon,
                        field118,
                        field119,
                        field120,
                        field121,
                        field122,
                        field123,
                        field124,
                        field125,
                        field126,
                        field127,
                        field128,
                        field129,
                        field130,
                        field131,
                        field132,
                        field133,
                        field134,
                        field135,
                        field136,
                        field137,
                        field138,
                        field139,
                        field140,
                        field141,
                        grade,
                        weight
                    ) 
            AS 
                SELECT  id,
                        character_id,
                        zone,
                        bag,
                        slot,
                        base_id,
                        quantity,
                        is_identified,
                        is_cursed,
                        is_blessed,
                        current_equip_slot,
                        current_durability,
                        maximum_durability,
                        enhancement_level,
                        special_forge_level,
                        physical,
                        magical,
                        hardness,
                        gem_slot_1_type,
                        gem_slot_2_type,
                        gem_slot_3_type,
                        gem_id_slot_1,
                        gem_id_slot_2,
                        gem_id_slot_3,
                        enchant_id,
                        gp,
                        item_type,
                        quality,
                        '1',
                        max_stack_size,
                        '3',
                        es_hand_r,
                        es_hand_l,
                        es_quiver,
                        es_head,
                        es_body,
                        es_legs,
                        es_arms,
                        es_feet,
                        es_mantle,
                        es_ring,
.                       es_earring,
                        es_necklace,
                        es_belt,
                        es_talkring,
                        es_avatar_head,
                        es_avatar_body,
                        es_avatar_legs,
                        es_avatar_arms,
                        es_avatar_feet,
                        req_hum_m,
                        req_hum_f,
                        req_elf_m,
                        req_elf_f,
                        req_dwf_m,
                        req_dwf_f,
                        req_por_m,
                        req_por_f,
                        req_gnm_m,
                        req_gnm_f,
                        req_fighter,
                        req_thief,
                        req_mage,
                        req_priest,
                        req_lawful,
                        req_neutral,
                        req_chaotic,
                        '40',
                        '41',
                        req_str,
                        req_vit,
                        req_dex,
                        req_agi,
                        req_int,
                        req_pie,
                        req_luk,
                        req_soul_rank,
                        req_lvl,
                        '51',
                        '52',
                        phys_slash,
                        phys_strike,
                        phys_pierce,
                        '56',
                        pdef_fire,
                        pdef_water,
                        pdef_wind,
                        pdef_earth,
                        pdef_light,
                        pdef_dark,
                        '63',
                        '64',
                        '65',
                        matk_fire,
                        matk_water,
                        matk_wind,
                        matk_earth,
                        matk_light,
                        matk_dark,
                        '72',
                        '73',
                        '74',
                        '75',
                        '76',
                        seffect_hp,
                        seffect_mp,
                        seffect_str,
                        seffect_vit,
                        seffect_dex,
                        seffect_agi,
                        seffect_int,
                        seffect_pie,
                        seffect_luk,
                        res_poison,
                        res_paralyze,
                        res_petrified,
                        res_faint,
                        res_blind,
                        res_sleep,
                        res_silent,
                        res_charm,
                        res_confusion,
                        res_fear,
                        '96',
                        status_malus,
                        status_percent,
                        '99',
                        object_type,
                        equip_slot,
                        '102',
                        '103',
                        '104',
                        icon_type,
                        no_storage,
                        no_discard,
                        no_sell,
                        no_trade,
                        no_trade_after_used,
                        no_stolen,
                        gold_border,
                        lore,
                        icon,
                        field118,
                        field119,
                        field120,
                        field121,
                        field122,
                        field123,
                        field124,
                        field125,
                        field126,
                        field127,
                        field128,
                        field129,
                        field130,
                        field131,
                        field132,
                        field133,
                        field134,
                        field135,
                        field136,
                        field137,
                        field138,
                        field139,
                        field140,
                        field141,
                        grade,
                        weight 
                FROM 
                    nec_item_instance 
                INNER JOIN 
                    nec_item_library 
                ON 
                    nec_item_instance.base_id = nec_item_library.id";

        private const string SqlSelectItemInstanceById = @"
            SELECT
                *
            FROM
                nec_item_instance
            WHERE
                id = @id";

        private const string SqlSelectItemInstanceByLocation = @"
            SELECT
                *
            FROM
                nec_item_instance
            WHERE
                character_id = @character_id
            AND
                zone = @zone
            AND
                bag = @bag
            AND
                slot = @slot";

        private const string SqlSelectSpawnedItemByIds = @"
            SELECT
                *
            FROM
                nec_item_instance
            WHERE
                id IN @ids";


        public ItemInstance InsertItemInstance(int baseId)
        {
            throw new NotImplementedException();
        }

        public ItemInstance SelectItemInstance(long instanceId)
        {
            ItemInstance itemInstance = null;
            ExecuteReader(SqlSelectItemInstanceById,
                command =>
                {
                    AddParameter(command, "@id", instanceId);
                }, reader =>
                {
                    itemInstance = MakeItemInstance(reader);
                });
            return itemInstance;
        }

        public ItemInstance[] SelectItemInstances(long[] instanceIds)
        {
            throw new NotImplementedException();
        }

        public ItemInstance SelectItemInstance(int characterId, ItemLocation itemLocation)
        {
            throw new NotImplementedException();
        }

        private ItemInstance MakeItemInstance(DbDataReader reader)
        {
            ItemZoneType zone = (ItemZoneType) reader.GetByte("zone");
            byte bag = reader.GetByte("bag");
            short slot = reader.GetInt16("slot");
            ItemLocation itemLocation = new ItemLocation(zone, bag, slot);

            ulong instanceId = (ulong) reader.GetInt64("id");

            ItemInstance itemInstance = new ItemInstance(instanceId, itemLocation);

            itemInstance.BaseID = reader.GetInt32("base_id");

            itemInstance.Quantity = reader.GetByte("quantity");

            if (reader.GetBoolean("is_identified"))     itemInstance.Statuses |= ItemStatuses.Identified;
            if (reader.GetBoolean("is_cursed"))         itemInstance.Statuses |= ItemStatuses.Cursed;
            if (reader.GetBoolean("is_blessed"))        itemInstance.Statuses |= ItemStatuses.Blessed;

            itemInstance.CurrentEquipSlot = (ItemEquipSlots) reader.GetInt32("current_equip_slot");

            itemInstance.CurrentDurability = reader.GetInt32("current_durability");
            itemInstance.MaximumDurability = reader.GetInt32("maximum_durability");

            itemInstance.EnhancementLevel = reader.GetByte("enhancement_level");

            itemInstance.SpecialForgeLevel = reader.GetByte("special_forge_level");

            itemInstance.Physical = reader.GetInt16("physical");
            itemInstance.Magical = reader.GetInt16("magical");

            itemInstance.Hardness = reader.GetByte("hardness");

            int gemSlotNum = 0;
            int gemSlot1Type = reader.GetByte("gem_slot_1_type");
            if (gemSlot1Type != 0) gemSlotNum++;
            int gemSlot2Type = reader.GetByte("gem_slot_2_type");
            if (gemSlot2Type != 0) gemSlotNum++;
            int gemSlot3Type = reader.GetByte("gem_slot_3_type");
            if (gemSlot3Type != 0) gemSlotNum++;
            GemSlot[] gemSlot = new GemSlot[gemSlotNum];

            itemInstance.EnchantId = reader.GetInt32("enchant_id");
            itemInstance.GP = reader.GetInt16("gp");
            itemInstance.Type = (ItemType) Enum.Parse(typeof(ItemType), reader.GetString("item_type"));
            itemInstance.Quality = (ItemQualities) Enum.Parse(typeof(ItemType), reader.GetString("quality"));
            itemInstance.MaxStackSize = reader.GetByte("max_stack_size");

            if (reader.GetBoolean("es_hand_r"))         itemInstance.EquipAllowedSlots |= ItemEquipSlots.RightHand;
            if (reader.GetBoolean("es_hand_l"))         itemInstance.EquipAllowedSlots |= ItemEquipSlots.LeftHand;
            if (reader.GetBoolean("es_quiver"))         itemInstance.EquipAllowedSlots |= ItemEquipSlots.Quiver;
            if (reader.GetBoolean("es_head"))           itemInstance.EquipAllowedSlots |= ItemEquipSlots.Head;
            if (reader.GetBoolean("es_body"))           itemInstance.EquipAllowedSlots |= ItemEquipSlots.Torso;
            if (reader.GetBoolean("es_legs"))           itemInstance.EquipAllowedSlots |= ItemEquipSlots.Legs;
            if (reader.GetBoolean("es_arms"))           itemInstance.EquipAllowedSlots |= ItemEquipSlots.Arms;
            if (reader.GetBoolean("es_feet"))           itemInstance.EquipAllowedSlots |= ItemEquipSlots.Feet;
            if (reader.GetBoolean("es_mantle"))         itemInstance.EquipAllowedSlots |= ItemEquipSlots.Cape;
            if (reader.GetBoolean("es_ring"))           itemInstance.EquipAllowedSlots |= ItemEquipSlots.Ring;
            if (reader.GetBoolean("es_earring"))        itemInstance.EquipAllowedSlots |= ItemEquipSlots.Earring;
            if (reader.GetBoolean("es_necklace"))       itemInstance.EquipAllowedSlots |= ItemEquipSlots.Necklace;
            if (reader.GetBoolean("es_belt"))           itemInstance.EquipAllowedSlots |= ItemEquipSlots.Belt;
            if (reader.GetBoolean("es_talkring"))       itemInstance.EquipAllowedSlots |= ItemEquipSlots.Talkring;
            if (reader.GetBoolean("es_avatar_head"))    itemInstance.EquipAllowedSlots |= ItemEquipSlots.AvatarHead;
            if (reader.GetBoolean("es_avatar_body"))    itemInstance.EquipAllowedSlots |= ItemEquipSlots.AvatarTorso;
            if (reader.GetBoolean("es_avatar_legs"))    itemInstance.EquipAllowedSlots |= ItemEquipSlots.Legs;
            if (reader.GetBoolean("es_avatar_arms"))    itemInstance.EquipAllowedSlots |= ItemEquipSlots.Arms;
            if (reader.GetBoolean("es_avatar_feet"))    itemInstance.EquipAllowedSlots |= ItemEquipSlots.Feet;

            if (reader.GetBoolean("req_hum_m"))         itemInstance.RequiredRaces |= Races.HumanMale;
            if (reader.GetBoolean("req_hum_f"))         itemInstance.RequiredRaces |= Races.HumanFemale;
            if (reader.GetBoolean("req_elf_m"))         itemInstance.RequiredRaces |= Races.ElfMale;
            if (reader.GetBoolean("req_elf_f"))         itemInstance.RequiredRaces |= Races.ElfFemale;
            if (reader.GetBoolean("req_dwf_m"))         itemInstance.RequiredRaces |= Races.DwarfMale;
            if (reader.GetBoolean("req_por_m"))         itemInstance.RequiredRaces |= Races.PorkulMale;
            if (reader.GetBoolean("req_por_f"))         itemInstance.RequiredRaces |= Races.PorkulFemale;
            if (reader.GetBoolean("req_gnm_f"))         itemInstance.RequiredRaces |= Races.GnomeFemale;

            if (reader.GetBoolean("req_fighter"))       itemInstance.RequiredClasses |= Classes.Fighter;
            if (reader.GetBoolean("req_thief"))         itemInstance.RequiredClasses |= Classes.Thief;
            if (reader.GetBoolean("req_mage"))          itemInstance.RequiredClasses |= Classes.Mage;
            if (reader.GetBoolean("req_priest"))        itemInstance.RequiredClasses |= Classes.Priest;
            if (reader.GetBoolean("req_samurai"))       itemInstance.RequiredClasses |= Classes.Samurai;
            if (reader.GetBoolean("req_bishop"))        itemInstance.RequiredClasses |= Classes.Bishop;
            if (reader.GetBoolean("req_ninja"))         itemInstance.RequiredClasses |= Classes.Ninja;
            if (reader.GetBoolean("req_lord"))          itemInstance.RequiredClasses |= Classes.Lord;
            if (reader.GetBoolean("req_clown"))         itemInstance.RequiredClasses |= Classes.Clown;
            if (reader.GetBoolean("req_alchemist"))     itemInstance.RequiredClasses |= Classes.Alchemist;

            if (reader.GetBoolean("req_lawful"))        itemInstance.RequiredAlignments |= Alignments.Lawful;
            if (reader.GetBoolean("req_neutral"))       itemInstance.RequiredAlignments |= Alignments.Neutral;
            if (reader.GetBoolean("req_chaotic"))       itemInstance.RequiredAlignments |= Alignments.Chaotic;

            itemInstance.RequiredStrength           = reader.GetByte("req_str");
            itemInstance.RequiredVitality           = reader.GetByte("req_vit");
            itemInstance.RequiredDexterity          = reader.GetByte("req_dex");
            itemInstance.RequiredAgility            = reader.GetByte("req_agi");
            itemInstance.RequiredIntelligence       = reader.GetByte("req_int");
            itemInstance.RequiredPiety              = reader.GetByte("req_pie");
            itemInstance.RequiredLuck               = reader.GetByte("req_luk");
            
            itemInstance.RequiredSoulRank   = reader.GetByte("req_soul_rank");
            itemInstance.RequiredLevel      = reader.GetByte("req_lvl");
            
            itemInstance.PhysicalSlash  = reader.GetByte("phys_slash");
            itemInstance.PhysicalStrike = reader.GetByte("phys_strike");
            itemInstance.PhysicalPierce = reader.GetByte("phys_pierce");
            
            itemInstance.PhysicalDefenseFire    = reader.GetByte("pdef_fire");
            itemInstance.PhysicalDefenseWater   = reader.GetByte("pdef_water");
            itemInstance.PhysicalDefenseWind    = reader.GetByte("pdef_wind");
            itemInstance.PhysicalDefenseEarth   = reader.GetByte("pdef_earth");
            itemInstance.PhysicalDefenseLight   = reader.GetByte("pdef_light");
            itemInstance.PhysicalDefenseDark    = reader.GetByte("pdef_dark");

            itemInstance.MagicalAttackFire  = reader.GetByte("matk_fire");
            itemInstance.MagicalAttackWater = reader.GetByte("matk_water");
            itemInstance.MagicalAttackWind  = reader.GetByte("matk_wind");
            itemInstance.MagicalAttackEarth = reader.GetByte("matk_earth");
            itemInstance.MagicalAttackLight = reader.GetByte("matk_light");
            itemInstance.MagicalAttackDark  = reader.GetByte("matk_dark");

            itemInstance.Hp     = reader.GetByte("seffect_hp");
            itemInstance.Mp     = reader.GetByte("seffect_mp");
            itemInstance.Str    = reader.GetByte("seffect_str");
            itemInstance.Vit    = reader.GetByte("seffect_vit");
            itemInstance.Dex    = reader.GetByte("seffect_dex");
            itemInstance.Agi    = reader.GetByte("seffect_agi");
            itemInstance.Int    = reader.GetByte("seffect_int");
            itemInstance.Pie    = reader.GetByte("seffect_pie");
            itemInstance.Luk    = reader.GetByte("seffect_luk");
            
            itemInstance.ResistPoison       = reader.GetByte("res_poison");
            itemInstance.ResistParalyze     = reader.GetByte("res_paralyze");
            itemInstance.ResistPetrified    = reader.GetByte("res_petrified");
            itemInstance.ResistFaint        = reader.GetByte("res_faint");
            itemInstance.ResistBlind        = reader.GetByte("res_blind");
            itemInstance.ResistSleep        = reader.GetByte("res_sleep");
            itemInstance.ResistSilence      = reader.GetByte("res_silent");
            itemInstance.ResistCharm        = reader.GetByte("res_charm");
            itemInstance.ResistConfusion    = reader.GetByte("res_confusion");
            itemInstance.ResistFear         = reader.GetByte("res_fear");
            
            itemInstance.StatusMalus        = (ItemStatusEffect) Enum.Parse(typeof(ItemStatusEffect), reader.GetString("status_malus"));
            itemInstance.StatusMalusPercent = reader.GetInt32("status_percent");

            itemInstance.ObjectType = reader.GetString("object_type"); //not sure what this is for
            itemInstance.EquipSlot2 = reader.GetString("equip_slot"); //not sure what this is for
            itemInstance.IconType   = reader.GetString("icon_type"); //not sure what this is for

            itemInstance.IsStorable         = !reader.GetBoolean("no_storage");
            itemInstance.IsDiscardable      = !reader.GetBoolean("no_discard");
            itemInstance.IsSellable         = !reader.GetBoolean("no_sell");
            itemInstance.IsTradeable        = !reader.GetBoolean("no_trade");
            itemInstance.IsTradableAfterUse = !reader.GetBoolean("no_trade_after_used");
            itemInstance.IsStealable        = !reader.GetBoolean("no_stolen");

            itemInstance.IsGoldBorder = reader.GetBoolean("gold_border");
            
            itemInstance.Lore = reader.GetString("lore");

            itemInstance.IconId = reader.GetInt32("icon");

            itemInstance.BagSize = reader.GetByte("num_of_bag_slots");
            
            //grade,
            //weight


            return itemInstance;
        }
    }
}
