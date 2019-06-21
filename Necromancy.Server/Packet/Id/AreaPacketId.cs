namespace Necromancy.Server.Packet.Id
{
    /// <summary>
    /// Necromancy Message Server
    /// 
    /// Recv OP Codes: (Area Server Switch: 0x00495B83)
    ///
    /// 0x86EA
    /// 0x3DD0 - proto_area_implement_client::recv_event_soul_storage_open
    /// 0x1F5A - proto_area_implement_client::recv_item_update_durability
    /// 0x10DA
    /// 0x794 - proto_area_implement_client::recv_wanted_jail_update_money
    /// 0x494 - proto_area_implement_client::recv_event_tresurebox_select_r
    /// 0x2FA - proto_area_implement_client::recv_charabody_notify_loot_start_cancel
    /// 0x1DA - proto_area_implement_client::recv_sv_conf_option_request_r
    /// 0x1A6 - proto_area_implement_client::recv_cash_shop_get_url_common_r
    /// 0x1A6 + 0x5
    /// 0xC24E - proto_area_implement_client::recv_chara_update_action_prohibit_camp
    /// 0xA7BF - proto_area_implement_client::recv_party_regist_member_recruit_r
    /// 0x9899
    /// 0x903A - proto_area_implement_client::recv_skill_request_gain_r
    /// 0x8C2F - proto_area_implement_client::recv_chara_update_notify_honor
    /// 0x88FB - proto_area_implement_client::recv_trade_abort_r
    /// 0x8820 - proto_area_implement_client::recv_battle_report_action_removetrap_ontrap
    /// 0x8778 - proto_area_implement_client::recv_union_storage_deposit_money_r
    /// 0x8778 + 0x4
    /// 0xDE90 - proto_area_implement_client::recv_shop_notify_sell_surrogate_fee
    /// 0xD170 - proto_area_implement_client::recv_select_package_update_r
    /// 0xCB6D - proto_area_implement_client::recv_party_notify_failed_draw
    /// 0xC54F - proto_area_implement_client::recv_wanted_jail_draw_point_r
    /// 0xC3EE - proto_area_implement_client::recv_cash_shop2_notify_item
    /// 0xC2A1 (investigate 004C2F8E)
    /// 0xC2A1 + 0xD3 (investigate 004C2F8E)
    /// 0xEFA4 - proto_area_implement_client::recv_trade_invite_r
    /// 0xE819 - proto_area_implement_client::recv_item_update_spirit_eqmask
    /// 0xE1F8 - proto_area_implement_client::recv_event_select_ready
    /// 0xE051 - proto_area_implement_client::recv_data_notify_goldobject_data
    /// 0xDF31 - proto_area_implement_client::recv_blacklist_open_r
    /// 0xDEB7 - proto_area_implement_client::recv_cash_shop_fitting_end
    /// 0xDEB7 + 0xB
    /// 0xFA0B
    /// 0xF447 - proto_area_implement_client::recv_shop_notify_item
    /// 0xF1A0 - proto_area_implement_client::recv_battle_attack_exec_r
    /// 0xEFDD (investigate 004CEC2C)
    /// 0xEFDD + 0x4E (investigate 004CEC2C)
    /// 0xFDB2 - proto_area_implement_client::recv_package_all_delete_r
    /// 0xFCC0 - proto_area_implement_client::recv_return_home_request_exec_r
    /// 0xFC28 - proto_area_implement_client::recv_auction_cancel_exhibit_r
    /// 0xFB79
    /// 0xFC1A - proto_area_implement_client::recv_job_change_close_r
    /// 0xFED8 - proto_area_implement_client::recv_cloak_notify_open
    /// 0xFE2F - proto_area_implement_client::recv_party_leave_r
    /// 0xFDB8
    /// 0xFDB8 + 0x31
    /// 0xFF00 (investigate 004D13C3)
    /// 0xFF00 + 0xD6 (investigate 004D13C3)
    /// 0xFEB7 - proto_area_implement_client::recv_event_removetrap_select_r
    /// 0xFCF3 (investigate 004D042D)
    /// 0xFCF3 + 0x85 (investigate 004D042D)
    /// 0xFC75 - proto_area_implement_client::recv_battle_report_start_notify
    /// 0xFC75 + 0x38
    /// 0xF71C - proto_area_implement_client::recv_chara_pose_r
    /// 0xF633 - proto_area_implement_client::recv_battle_attack_pose_end_notify
    /// 0xF495 - proto_area_implement_client::recv_party_search_recruited_member_r
    /// 0xF602 - proto_area_implement_client::recv_raisescale_add_item_r
    /// 0xF95B - proto_area_implement_client::recv_emotion_notify_type
    /// 0xF7E7
    /// 0xF7E7 + 0x9
    /// 0xF9F9 - proto_area_implement_client::recv_map_entry_r
    /// 0xF6AF - proto_area_implement_client::recv_npc_affection_rank_update_notify
    /// 0xF393 - proto_area_implement_client::recv_battle_report_notify_damage_ac
    /// 0xF212 - proto_area_implement_client::recv_battle_report_action_attack_onhit
    /// 0xF330 - proto_area_implement_client::recv_cloak_close_r
    /// 0xF3A1 - proto_area_implement_client::recv_inherit_start_r
    /// 0xEDA6 - proto_area_implement_client::recv_map_get_info_r
    /// 0xEA1C - proto_area_implement_client::recv_dbg_chara_unequipped
    /// 0xE897 (investigate 004CC897)
    /// 0xE897 + 0xF9 (investigate 004CC897)
    /// 0xEE43 - proto_area_implement_client::recv_skill_start_item_cast_r
    /// 0xEDB3 (investigate 004CDE73)
    /// 0xEDB3 + 0x65 (investigate 004CDE73)
    /// 0xEEB7 (investigate 004CE472)
    /// 0xEEB7 + 0xD6 (investigate 004CE472)
    /// 0xECBA - proto_area_implement_client::recv_shop_identify_r
    /// 0xEB47 - proto_area_implement_client::recv_battle_report_notify_exp_bonus2
    /// 0xEB47 + 0x34
    /// 0xED4C
    /// 0xE748 - proto_area_implement_client::recv_party_notify_add_draw_item
    /// 0xE4B2 - proto_area_implement_client::recv_skill_start_cast_r
    /// 0xE207 - proto_area_implement_client::recv_party_kick_r
    /// 0xE462 - proto_area_implement_client::recv_union_request_rename_r
    /// 0xE7BB (investigate 004CC261)
    /// 0xE7BB + 0x2C (investigate 004CC261)
    /// 0xE526 - proto_area_implement_client::recv_battle_report_notify_exp
    /// 0xE5FF - proto_area_implement_client::recv_quest_select_error
    /// 0xE07E (investigate 004CB292)
    /// 0xE07E + 0xCD (investigate 004CB292)
    /// 0xE039 - proto_area_implement_client::recv_shop_notify_item_sell_price
    /// 0xE039 + 0x4
    /// 0xD688 - proto_area_implement_client::recv_charabody_salvage_end
    /// 0xD46E - proto_area_implement_client::recv_quest_started
    /// 0xD349 - proto_area_implement_client::recv_map_fragment_flag
    /// 0xD1CB - proto_area_implement_client::recv_auction_close_r
    /// 0xD1A9 - proto_area_implement_client::recv_random_box_next_open_r
    /// 0xD1A9 + 0x14
    /// 0xDA4A
    /// 0xD7D8 - proto_area_implement_client::recv_battle_report_notify_invalid_target
    /// 0xD68C (investigate 004C8EE5)
    /// 0xD68C + 0xC6 (investigate 004C8EE5)
    /// 0xDBF1 - proto_area_implement_client::recv_cash_shop_fitting_begin
    /// 0xDB5E - proto_area_implement_client::recv_battle_report_noact_notify_buff_effect
    /// 0xDA5C - proto_area_implement_client::recv_base_exitr
    /// 0xDB53 - proto_area_implement_client::recv_battle_report_action_removetrap_success
    /// 0xDD52 - proto_area_implement_client::recv_wanted_update_state
    /// 0xDC5B - proto_area_implement_client::recv_battle_report_action_steal_unidentified
    /// 0xDC5B + 0x5B
    /// 0xDDD3 - proto_area_implement_client::recv_wanted_entry_r
    /// 0xDB88 - proto_area_implement_client::recv_charabody_self_salvage_request_cancel
    /// 0xD909
    /// 0xD804 - proto_area_implement_client::recv_gimmick_access_object_notify
    /// 0xD8D5 - proto_area_implement_client::recv_battle_report_noact_notify_heal_mp
    /// 0xD972 - proto_area_implement_client::recv_chara_pose_ladderr
    /// 0xD597 - proto_area_implement_client::recv_cash_shop_notify_item
    /// 0xD493 (investigate 004C7D1F)
    /// 0xD493 + 0xE7 (investigate 004C7D1F)
    /// 0xD5B5 (investigate 004C86F2)
    /// 0xD5B5 + 0xC8 (investigate 004C86F2)
    /// 0xD400 (investigate 004C7645)
    /// 0xD400 + 0x3F (investigate 004C7645)
    /// 0xD1F6
    /// 0xD2D6 - proto_area_implement_client::recv_base_ping_r
    /// 0xCF24 - proto_area_implement_client::recv_event_select_map_and_channel
    /// 0xCC54
    /// 0xCB94 (investigate 004C5085)
    /// 0xCB94 + 0xA3 (investigate 004C5085)
    /// 0xCFDC
    /// 0xCF29 (investigate 004C5F8E)
    /// 0xCF29 + 0x29 (investigate 004C5F8E)
    /// 0xD04A (investigate 004C6663)
    /// 0xD04A + 0xE9 (investigate 004C6663)
    /// 0xCDC9
    /// 0xCCE2
    /// 0xCD63
    /// 0xCE36
    /// 0xC8AD
    /// 0xC6F2
    /// 0xC8AD
    /// 0xC6F2
    /// 0xC68B
    /// 0xC68B + 0x64
    /// 0xCA35
    /// 0xC96F - proto_area_implement_client::recv_buff_request_detach_r
    /// 0xC9FF
    /// 0xCAB1 - proto_area_implement_client::recv_talkring_create_masterring_r
    /// 0xC701 - proto_area_implement_client::recv_battle_report_action_skill_onhit
    /// 0xC7E1
    /// 0xC542
    /// 0xC444
    /// 0xC444 + 0x36
    /// 0xC543
    /// 0x840E
    /// 0xAF76
    /// 0xAB14
    /// 0xA90C
    /// 0xA7E8  (investigate 004BB352)
    /// 0xA7E8 + 0xD3 (investigate 004BB352)
    /// 0xBD7E
    /// 0xBA61
    /// 0xB684
    /// 0xB586
    /// 0xB417
    /// 0xB417 + 0x1E
    /// 0xC0BB
    /// 0xBFEA
    /// 0xBF0D
    /// 0xBD90
    /// 0xBD90 + 0x9
    /// 0xC1DC
    /// 0xC0D8 (investigate 004C21A0)
    /// 0xC0D8 + 0xD7 (investigate 004C21A0)
    /// 0xC206 (investigate 004C2984)
    /// 0xC026 + 0x46 (investigate 004C2984)
    /// 0xC003 (investigate 004C1A1E)
    /// 0xC003 + 0x75 (investigate 004C1A1E)
    /// 0xBF34
    /// 0xBFE9
    /// 0xBBA5
    /// 0xBA71 (investigate 004C02F0)
    /// 0xBA71 + 0xF4 (investigate 004C02F0)
    /// 0xBCC2
    /// 0xBC0A
    /// 0xBCAB
    /// 0xBD72
    /// 0xB813
    /// 0xB6B1
    /// 0xB782
    /// 0xBA11
    /// 0xB619
    /// 0xB619 + 0x18
    /// 0xB195
    /// 0xB0BF
    /// 0xAF7F (investigate 004BD4CC)
    /// 0xAF7F + 0xB8 (investigate 004BD4CC)
    /// 0xB319
    /// 0xB1CA (investigate 004BE4A3)
    /// 0xB1CA + 0xC8 (investigate 004BE4A3)
    /// 0xB317 (investigate 004BEB5D)
    /// 0xB317 + 0x86 (investigate 004BEB5D) 
    /// 0xB0E5 (investigate 004BDD95) 
    /// 0xB0E5 + 0x2A (investigate 004BDD95) 
    /// 0xAE27
    /// 0xABF2
    /// 0xAB8A
    /// 0xAB8A + 0x3B
    /// 0xAF49
    /// 0xAE2B
    /// 0xAEE9
    /// 0xAF6D
    /// 0xAD6D
    /// 0xAD6D + 0x5B
    /// 0xAA8F
    /// 0xA938
    /// 0xA9C2
    /// 0xAADD
    /// 0xA084
    /// 0x9D52
    /// 0x9B08
    /// 0x9A44
    /// 0x98D3
    /// 0x998F
    /// 0xA4D3
    /// 0xA21E
    /// 0xA0E3 (investigate 004B98BB) 
    /// 0xA0E3 + 0xAA (investigate 004B98BB) 
    /// 0xA54C
    /// 0xA508 (investigate 004BA68E) 
    /// 0xA508 + 0x41 (investigate 004BA68E) 
    /// 0xA611 (investigate 004BAC6C) 
    /// 0xA611 + 0xE7 (investigate 004BAC6C) 
    /// 0xA45C
    /// 0xA2B7
    /// 0xA43B
    /// 0xA4A5
    /// 0x9F31
    /// 0x9DE2
    /// 0x9D5A
    /// 0x9D5A + 0x3C
    /// 0x9F70 (investigate 004B9266) 
    /// 0x9F70 + 0x95 (investigate 004B9266) 
    /// 0x9E19
    /// 0x9E19 + 0x5C
    /// 0x9CA1
    /// 0x9BC6
    /// 0x9BC6 + 0x74
    /// 0x9CCF
    /// 0x9A79
    /// 0x9A79 + 0x30
    /// 0x9578
    /// 0x924E
    /// 0x919C
    /// 0x906A
    /// 0x906A + 0x7E
    /// 0x9700
    /// 0x9666
    /// 0x95E6
    /// 0x95E6 + 0x78
    /// 0x97D9
    /// 0x971B
    /// 0x971B + 0x46
    /// 0x9870
    /// 0x96EA
    /// 0x935B
    /// 0x9289
    /// 0x9289 + 0x55
    /// 0x951B
    /// 0x9201
    /// 0x9201 + 0x26
    /// 0x8D9B
    /// 0x8CC6 (investigate 004B483D) 
    /// 0x8CC6 + 0xCC (investigate 004B483D) 
    /// 0x8F15
    /// 0x8DBC
    /// 0x8E92
    /// 0x8F84
    /// 0x8BB4
    /// 0x89FF
    /// 0x89FF + 0x7
    /// 0x8BD2
    /// 0x88A1
    /// 0x88A1 + 0xD
    /// -----
    /// 0x662F
    /// 0x531B
    /// 0x4C6F
    /// 0x4440
    /// 0x42B8
    /// 0x3F38
    /// 0x3E0B
    /// 0x3E0B + 0x74
    /// 0x77A7
    /// 0x6FB2
    /// 0x6A7A
    /// 0x6912
    /// 0x68AA
    /// 0x66EC
    /// 0x679B
    /// 0x7FC5
    /// 0x7CF0
    /// 0x7A6F
    /// 0x79A2
    /// 0x789E
    /// 0x793E
    /// 0x839A
    /// 0x825D
    /// 0x8066 (investigate 004B1728) 
    /// 0x8066 + 0x2B (investigate 004B1728) 
    /// 0x855C
    /// 0x8487 (investigate 004B2631) 
    /// 0x8487 + 0xC2 (investigate 004B2631)
    /// 0x85C6 (investigate 004B2C99)
    /// 0x85C6 + 0xDF (investigate 004B2C99)
    /// 0x8299 (investigate 004B1EE6)
    /// 0x8299 + 0xFC (investigate 004B1EE6)
    /// 0x7D53
    /// 0x7D1C (investigate 004B09BA)
    /// 0x7D1C + 0xF (investigate 004B09BA)
    /// 0x7F34
    /// 0x7D75
    /// 0x7F09
    /// 0x7F50
    /// 0x7BB3
    /// 0x7B5D
    /// 0x7B5D + 0x29
    /// 0x7CB2
    /// 0x79A9
    /// 0x7A5C
    /// 
    /// </summary>
    
    public enum AreaPacketId : ushort
    {
        send_base_check_version = 0x5705,
        recv_base_check_version_r = 0x9699,
    }
}
