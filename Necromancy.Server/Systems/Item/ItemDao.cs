using Necromancy.Server.Model;
using System;
using System.Data;
using System.Data.Common;

namespace Necromancy.Server.Systems.Item
{
    public class ItemDao : DatabaseAccessObject, IItemDao
    {
        private const string SqlSpawnedView = @"
            DROP VIEW IF EXISTS spawned_item;
            CREATE VIEW IF NOT EXISTS 
                spawned_item 
                    (
                        id,
                        character_id,
                        zone,
                        bag,
                        slot,
                        base_id,
                        quantity,
                        unidentified_name,
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
                        max_slots,
                        '3',
                        es_hand_r,
                        es_hand_,
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
                        unidentified_name,
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
                        max_slots,
                        '3',
                        es_hand_r,
                        es_hand_,
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
                    nec_spawned_item 
                INNER JOIN 
                    nec_base_item 
                ON 
                    nec_spawned_item.base_id = nec_base_item.id";

        private const string SqlSelectSpawnedItemById = @"
            SELECT
                *
            FROM
                spawned_item
            WHERE
                id = @id";

        private const string SqlSelectSpawnedItemByLocation = @"
            SELECT
                *
            FROM
                spawned_item
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
                spawned_item
            WHERE
                id IN @ids";


        public ItemInstance InsertSpawnedItem(int baseId)
        {
            throw new NotImplementedException();
        }

        //public ItemInstance SelectSpawnedItem(int spawnId)
        //{
        //    ItemInstance spawnedItem = null;
        //    ExecuteReader(SqlSelectSpawnedItemById,
        //        command =>
        //        {
        //            AddParameter(command, "@id", spawnId);
        //        }, reader =>
        //        {
        //            spawnedItem = MakeSpawnedItem(reader);
        //        });
        //    return spawnedItem;
        //}

        public ItemInstance SelectSpawnedItems(int[] spawnIds)
        {
            throw new NotImplementedException();
        }

        public ItemInstance SelectSpawnedItem(int characterId, ItemLocation itemLocation)
        {
            throw new NotImplementedException();
        }

//        public AuctionItem SelectItem(int auctionItemId)
//        {
//            AuctionItem auctionItem = new AuctionItem();
//            ExecuteReader(SqlSelectItem,
//                command =>
//                {
//                    AddParameter(command, "@id", auctionItemId);
//                }, reader =>
//                {
//                    MakeAuctionItem(reader);
//                });
//            return auctionItem;
//        }

//        public bool UpdateBid(AuctionItem auctionItem)
//        {
//            int rowsAffected = ExecuteNonQuery(SqlUpdateBid, command =>
//            {
//                AddParameter(command, "@bidder_id", auctionItem.BidderID);
//                AddParameter(command, "@current_bid", auctionItem.CurrentBid);
//            });
//            return rowsAffected > 0;
//        }

//        public AuctionItem[] SelectBids(Character character)
//        {
//            AuctionItem[] bids = new AuctionItem[AuctionService.MAX_BIDS];
//            int i = 0;
//            ExecuteReader(SqlSelectBids,
//                command =>
//                {
//                    AddParameter(command, "@character_id", character.Id);
//                }, reader =>
//                {
//                    while (reader.Read())
//                    {
//                        if (i >= AuctionService.MAX_BIDS) break;
//                        AuctionItem bid = MakeAuctionItem(reader);
//                        bids[i] = bid;
//                        i++;
//                    }
//                });
//            AuctionItem[] truncatedBids = new AuctionItem[i];
//            Array.Copy(bids, truncatedBids, i);
//            return truncatedBids;
//        }

//        public AuctionItem[] SelectLots(Character character)
//        {
//            AuctionItem[] lots = new AuctionItem[AuctionService.MAX_LOTS];
//            int i = 0;
//            ExecuteReader(SqlSelectLots,
//                command =>
//                {
//                    AddParameter(command, "@character_id", character.Id);
//                }, reader =>
//                {
//                    while (reader.Read())
//                    {
//                        if (i >= AuctionService.MAX_LOTS) break;
//                        AuctionItem lot = MakeAuctionItem(reader);
//                        lots[i] = lot;
//                        i++;
//                    }
//                });
//            AuctionItem[] truncatedLots = new AuctionItem[i];
//            Array.Copy(lots, truncatedLots, i);
//            return truncatedLots;
//        }

//        private ItemInstance MakeSpawnedItem(DbDataReader reader)
//        {
//            ItemZone zone = (ItemZone) reader.GetByte("zone");
//            byte bag = reader.GetByte("bag");
//            short slot = reader.GetInt16("slot");
//            ItemLocation itemLocation = new ItemLocation(zone, bag, slot);

//            int spawnId = reader.GetInt32("id");

//            ItemInstance spawnedItem = new ItemInstance(spawnId, itemLocation);

//            spawnedItem.Id = reader.GetInt32("base_id");

//            spawnedItem.Quantity = reader.GetByte("quantity");

//            spawnedItem.UnidentifiedName = reader.GetString("unidentified_name");

//            if (reader.GetBoolean("is_identified")) spawnedItem.Statuses |= ItemStatuses.Identified;
//            if (reader.GetBoolean("is_cursed")) spawnedItem.Statuses |= ItemStatuses.Cursed;
//            if (reader.GetBoolean("is_blessed")) spawnedItem.Statuses |= ItemStatuses.Blessed;

//            spawnedItem.CurrentEquipSlot = (ItemEquipSlot) reader.GetInt32("current_equip_slot");

//            spawnedItem.CurrentDurability = reader.GetInt32("current_durability");
//            spawnedItem.MaximumDurability = reader.GetInt32("maximum_durability");

//            spawnedItem.EnhancementLevel = reader.GetByte("enhancement_level");

//            spawnedItem.SpecialForgeLevel = reader.GetByte("special_forge_level");

//            spawnedItem.Physical = reader.GetInt16("physical");
//            spawnedItem.Magical = reader.GetInt16("magical");

//            spawnedItem.Hardness = reader.GetByte("hardness");

//            int gemSlotNum = 0;
//            int gemSlot1Type = reader.GetByte("gem_slot_1_type");
//            if (gemSlot1Type != 0) gemSlotNum++;
//            int gemSlot2Type = reader.GetByte("gem_slot_2_type");
//            if (gemSlot2Type != 0) gemSlotNum++;
//            int gemSlot3Type = reader.GetByte("gem_slot_3_type");
//            if (gemSlot3Type != 0) gemSlotNum++;
//            GemSlot[] gemSlot = new GemSlot[gemSlotNum];
//            //FIX GEMS
//            //gemSlot[0] = new GemSlot() 
//            //            gem_id_slot_1,
//            //            gem_id_slot_2,
//            //           gem_id_slot_3,

//            spawnedItem.EnchantId = reader.GetInt32("enchant_id");
//            spawnedItem.GP = reader.GetInt16("gp");
//            spawnedItem.Type = (ItemType) Enum.Parse(typeof(ItemType), reader.GetString("item_type"));
//            spawnedItem.Quality = (ItemQualities) Enum.Parse(typeof(ItemType), reader.GetString("quality"));
//            spawnedItem.MaxStackSize = reader.GetInt32("max_slots");
//                        es_hand_r,
//                        es_hand_,
//                        es_quiver,
//                        es_head,
//                        es_body,
//                        es_legs,
//                        es_arms,
//                        es_feet,
//                        es_mantle,
//                        es_ring,
//.                       es_earring,
//                        es_necklace,
//                        es_belt,
//                        es_talkring,
//                        es_avatar_head,
//                        es_avatar_body,
//                        es_avatar_legs,
//                        es_avatar_arms,
//                        es_avatar_feet,
//                spawnedItem.RequiresHumanMale = reader.GetBoolean("req_hum_m");
//            spawnedItem.RequiresHumanFemale = reader.GetBoolean("req_hum_f");
//                        req_elf_m,
//                        req_elf_f,
//                        req_dwf_m,
//                        req_dwf_f,
//                        req_por_m,
//                        req_por_f,
//                        req_gnm_m,
//                        req_gnm_f,
//                        req_fighter,
//                        req_thief,
//                        req_mage,
//                        req_priest,
//                        req_lawful,
//                        req_neutral,
//                        req_chaotic,
//                        '40',
//                        '41',
//                        req_str,
//                        req_vit,
//                        req_dex,
//                        req_agi,
//                        req_int,
//                        req_pie,
//                        req_luk,
//                        req_soul_rank,
//                        req_lvl,
//                        '51',
//                        '52',
//                        phys_slash,
//                        phys_strike,
//                        phys_pierce,
//                        '56',
//                        pdef_fire,
//                        pdef_water,
//                        pdef_wind,
//                        pdef_earth,
//                        pdef_light,
//                        pdef_dark,
//                        '63',
//                        '64',
//                        '65',
//                        matk_fire,
//                        matk_water,
//                        matk_wind,
//                        matk_earth,
//                        matk_light,
//                        matk_dark,
//                        '72',
//                        '73',
//                        '74',
//                        '75',
//                        '76',
//                        seffect_hp,
//                        seffect_mp,
//                        seffect_str,
//                        seffect_vit,
//                        seffect_dex,
//                        seffect_agi,
//                        seffect_int,
//                        seffect_pie,
//                        seffect_luk,
//                        res_poison,
//                        res_paralyze,
//                        res_petrified,
//                        res_faint,
//                        res_blind,
//                        res_sleep,
//                        res_silent,
//                        res_charm,
//                        res_confusion,
//                        res_fear,
//                        '96',
//                        status_malus,
//                        status_percent,
//                        '99',
//                        object_type,
//                        equip_slot,
//                        '102',
//                        '103',
//                        '104',
//                        icon_type,
//                        no_storage,
//                        no_discard,
//                        no_sell,
//                        no_trade,
//                        no_trade_after_used,
//                        no_stolen,
//                        gold_border,
//                        lore,
//                        icon,
//                        field118,
//                        field119,
//                        field120,
//                        field121,
//                        field122,
//                        field123,
//                        field124,
//                        field125,
//                        field126,
//                        field127,
//                        field128,
//                        field129,
//                        field130,
//                        field131,
//                        field132,
//                        field133,
//                        field134,
//                        field135,
//                        field136,
//                        field137,
//                        field138,
//                        field139,
//                        field140,
//                        field141,
//                        grade,
//                        weight


//            return spawnedItem;
//        }
    }
}
