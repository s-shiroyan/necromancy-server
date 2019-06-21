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
    /// 0x73A1
    /// 0x717D
    /// 0x70B7
    /// 0x7004
    /// 0x708B
    /// 0x755C
    /// 0x73D1(investigate 004AEEA5)
    /// 0x73D1 + 0x9E(investigate 004AEEA5)
    /// 0x7697
    /// 0x7576
    /// 0x7576 + 0x2F
    /// 0x772A
    /// 0x7341
    /// 0x7181
    /// 0x732D
    /// 0x735E
    /// 0x70F6
    /// 0x70F6 - 0x68
    /// 0x6D46
    /// 0x6BDC
    /// 0x6B6A
    /// 0x6B6A + 0xA
    /// 0x6E06
    /// 0x6DB4
    /// 0x6DB4 + 0x40
    /// 0x6F32
    /// 0x6C79
    /// 0x6C79 + 0x2
    /// 0x6979
    /// 0x692A
    /// 0x692A + 0x1
    /// 0x6A56
    /// 0x68E7
    /// 0x68E7 + 0x2A
    /// 0x5E79
    /// 0x58E7
    /// 0x556E
    /// 0x5513
    /// 0x536A
    /// 0x536A + 0x29
    /// 0x62E2
    /// 0x6030
    /// 0x5F6C
    /// 0x5EDB
    /// 0x5EDB + 0x3F
    /// 0x6554
    /// 0x6539
    /// 0x63B2
    /// 0x6510
    /// 0x65A6(investigate 004AB883)
    /// 0x65A6 + 0x48(investigate 004AB883)
    /// 0x6547
    /// 0x61C8
    /// 0x60E5
    /// 0x6175
    /// 0x621A
    /// 0x6001
    /// 0x6001 + 0x22
    /// 0x5CF8
    /// 0x5C15
    /// 0x5A5F
    /// 0x5AE9
    /// 0x5D52(investigate 004A9C48)
    /// 0x5D52 + 0xF6(investigate 004A9C48)
    /// 0x5C47
    /// 0x5899
    /// 0x570B
    /// 0x5842
    /// 0x58A8
    /// 0x551E
    /// 0x551E + 0x9
    /// 0x4EF4
    /// 0x4E10
    /// 0x4CF3
    /// 0x4C74
    /// 0x4C74 + 0x17
    /// 0x50D1
    /// 0x505E
    /// 0x4F10
    /// 0x5016
    /// 0x5243(investigate 004A7ED1)
    /// 0x5243 + 0xC4(investigate 004A7ED1)
    /// 0x50B2 (investigate 004A732D)
    /// 0x4E17 + 0x76 (investigate 004A732D)
    /// 0x4D70
    /// 0x4DF8
    /// 0x4981
    /// 0x465C
    /// 0x4537
    /// 0x462E
    /// 0x4ABB
    /// 0x4978
    /// 0x4978 + 0x1A
    /// 0x4B94
    /// 0x488E
    /// 0x4883 + 0xA
    /// 0x442A
    /// 0x42DE
    /// 0x4404
    /// 0x443C
    /// 0x3F5B
    /// 0x42B6
    /// ----
    /// 0x2BAB
    /// 0x2513
    /// 0x22E7
    /// 0x2063
    /// 0x1F73(investigate 0049CF7A)
    /// 0x1F73 + 0xA9(investigate 0049CF7A)
    /// 0x32F7
    /// 0x300A
    /// 0x2DCE
    /// 0x2CAF
    /// 0x2BBF
    /// 0x2BBF + 0x71
    /// 0x3B77
    /// 0x36DC
    /// 0x3544
    /// 0x32FF
    /// 0x3428
    /// 0x3C68
    /// 0x3B9F(investigate 004A40E1)
    /// 0x3B9F + 0xA2(investigate 004A40E1)
    /// 0x3D6C
    /// 0x3C81
    /// 0x3C81 + 0x8
    /// 0x3D9F
    /// 0x39FD
    /// 0x3806
    /// 0x394F
    /// 0x3A0E
    /// 0x362D
    /// 0x362D + 0x79
    /// 0x322F
    /// 0x316F
    /// 0x30BE
    /// 0x30BE + 0x3D
    /// 0x3247(investigate 004A29D7)
    /// 0x3247 + 0xA6(investigate 004A29D7)
    /// 0x3223
    /// 0x3F2F
    /// 0x2E17
    /// 0x2F0E
    /// 0x2FFF
    /// 0x2CB0
    /// 0x2D6D
    /// 0x28A0
    /// 0x267D
    /// 0x2631
    /// 0x2576
    /// 0x260E
    /// 0x2A3F
    /// 0x28E7(investigate 004A064B)
    /// 0x28E7 + 0xDE(investigate 004A064B)
    /// 0x287A
    /// 0x2A82
    /// 0x2A82 + 0x2E
    /// 0x2BA4
    /// 0x27D6
    /// 0x26B8
    /// 0x2790
    /// 0x2849
    /// 0x266C
    /// 0x266C + 0xF
    /// 0x2470
    /// 0x23D3
    /// 0x2353
    /// 0x2353 + 0x4A
    /// 0x2478(investigate 0049EE79)
    /// 0x2478 + 0x96(investigate 0049EE79)
    /// 0x23E5
    /// 0x2467
    /// 0x2246
    /// 0x213C
    /// 0x213C + 0x4E
    /// 0x2257
    /// ---
    /// 0x1837
    /// 0x14DA
    /// 0x129B
    /// 0x11FA
    /// 0x1105
    /// 0x1198
    ///
    ///
    ///
    ///
    ///
    ///
    ///
    /// 
    /// 
    /// 
    /// </summary>
    
    public enum AreaPacketId : ushort
    {
        send_base_check_version = 0x5705,
        recv_base_check_version_r = 0x9699,
    }
}
