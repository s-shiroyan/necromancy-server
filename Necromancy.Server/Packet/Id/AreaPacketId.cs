// ReSharper disable InconsistentNaming

namespace Necromancy.Server.Packet.Id
{
    /// <summary>
    /// Necromancy Area Server OP Codes, starts at 0x00495B88
    ///
    /// 0x1A6 - proto_area_implement_client::recv_cash_shop_get_url_common_r
    /// 0x1AB (0x1A6 + 0x5) proto_area_implement_client::recv_event_block_message_end_no_object
    /// 0x1DA - proto_area_implement_client::recv_sv_conf_option_request_r
    /// 0x22C - proto_area_implement_client::recv_auction_receive_gold_r
    /// 0x2C5 - proto_area_implement_client::recv_item_update_hardness
    /// 0x2FA - proto_area_implement_client::recv_charabody_notify_loot_start_cancel
    /// 0x397 - proto_area_implement_client::recv_stall_close_r
    /// 0x411  (0x397 + 0x7A)  proto_area_implement_client::recv_skill_custom_close_r
    /// 0x494 - proto_area_implement_client::recv_event_tresurebox_select_r
    /// 0x54E - proto_area_implement_client::recv_charabody_loot_complete2_r
    /// 0x5F8  (0x54E + 0xAA)  proto_area_implement_client::recv_item_update_num
    /// 0x604 - proto_area_implement_client::recv_battle_report_action_skill_exec
    /// 0x65C - proto_area_implement_client::recv_union_mantle_close_r
    /// 0x661 (0x65C + 0x5) proto_area_implement_client::recv_chara_update_ap_cost_per
    /// 0x723 - proto_area_implement_client::recv_cloak_notify_close_r
    /// 0x780 - proto_area_implement_client::recv_chara_update_condition_resist
    /// 0x794 - proto_area_implement_client::recv_wanted_jail_update_money
    /// 0x8CD - proto_area_implement_client::recv_self_action_cost
    /// 0x99D  (0x8CD + 0xD0)  proto_area_implement_client::recv_event_end
    /// 0x9F5 - proto_area_implement_client::recv_gem_synthesis_info
    /// 0xB2F - proto_area_implement_client::recv_charabody_loot_start2_r
    /// 0xBFE - proto_area_implement_client::recv_item_drop_r
    /// 0xC5F - proto_area_implement_client::recv_dbg_option_change_r
    /// 0xCE7 - proto_area_implement_client::recv_mail_open_r
    /// 0xCFF - proto_area_implement_client::recv_party_accept_to_apply_r
    /// 0xD0D - proto_area_implement_client::recv_cash_shop_fitting_equip_r
    /// 0xE0B - proto_area_implement_client::recv_record_close_r
    /// 0xE45 - proto_area_implement_client::recv_job_change_notify_close
    /// 0xECF - proto_area_implement_client::recv_item_cooltime_notify
    /// 0xEF9 - proto_area_implement_client::recv_chara_update_alignment
    /// 0x102E - proto_area_implement_client::recv_trade_notify_replied
    /// 0x10C5 (0x102E + 0x97) proto_area_implement_client::recv_dropobject_notify_access_priority
    /// 0x10DA   no string (or is it????) --structre: int32(5) and loop of int32() 5 times I believe
    /// 0x1105 - proto_area_implement_client::recv_event_union_storage_open
    /// 0x1198 - proto_area_implement_client::recv_logout_cancel
    /// 0x11FA - proto_area_implement_client::recv_skill_inherit_lost
    /// 0x124C - proto_area_implement_client::recv_situation_end
    /// 0x125E (0x124C + 0x12) proto_area_implement_client::recv_event_show_board_end
    /// 0x129B - proto_area_implement_client::recv_battle_report_noact_notify_freeze
    /// 0x12A4 - proto_area_implement_client::recv_charabody_self_salvage_notify
    /// 0x12E0 (0x12A4 + 0x3C) proto_area_implement_client::recv_charabody_self_salvage_end
    /// 0x1392 - proto_area_implement_client::recv_skill_base_notify
    /// 0x1489   ret two int32s
    /// 0x14DA - proto_area_implement_client::recv_chara_view_landing_notify
    /// 0x14F6 - proto_area_implement_client::recv_buff_shop_notify_open
    /// 0x15B0 - proto_area_implement_client::recv_soul_dispitem_notify_data
    /// 0x15D0 - proto_area_implement_client::recv_shop_message_push
    /// 0x166B   no string
    /// 0x1701 - proto_area_implement_client::recv_union_request_establish_r
    /// 0x170B - proto_area_implement_client::recv_dbg_battle_guard_end_notify
    /// 0x170F - proto_area_implement_client::recv_message_board_notify_open
    /// 0x173C (0x170F + 0x2D) proto_area_implement_client::recv_battle_report_notify_phy_damage_hp
    /// 0x179D - proto_area_implement_client::recv_battle_report_notify_hit_effect
    /// 0x182B - proto_area_implement_client::recv_self_soul_point_notify
    /// 0x1837 - proto_area_implement_client::recv_job_change_select_bonuspoint_r
    /// 0x18CC - proto_area_implement_client::recv_gem_notify_close
    /// 0x19C3 (0x18CC + 0xF7) proto_area_implement_client::recv_battle_report_noact_notify_buff_update_time
    /// 0x1A0F - proto_area_implement_client::recv_charabody_state_update_notify
    /// 0x1AA8 - proto_area_implement_client::recv_situation_start
    /// 0x1B5C - proto_area_implement_client::recv_event_start
    /// 0x1BD6 - proto_area_implement_client::recv_quest_get_mission_quest_works_r
    /// 0x1C0A - proto_area_implement_client::recv_skill_tree_notify
    /// 0x1C15 - proto_area_implement_client::recv_item_update_ac
    /// 0x1C20 - proto_area_implement_client::recv_cash_shop_fitting_unequip_r
    /// 0x1C6C (0x1C20 + 0x4C) proto_area_implement_client::recv_skill_exec_r
    /// 0x1D49 - proto_area_implement_client::recv_cash_shop_get_url_commerce_r
    /// 0x1D68 - proto_area_implement_client::recv_trade_notify_invited
    /// 0x1DBE - proto_area_implement_client::recv_battle_report_end_notify
    /// 0x1E65 -  - proto_area_implement_client::recv_charabody_self_warpdragon_penalty
    /// 0x1F32 (0x1E65 + 0xCD)  proto_area_implement_client::recv_souleater_touch_notify
    /// 0x1F5A - proto_area_implement_client::recv_item_update_durability
    /// 0x1F73 - proto_area_implement_client::recv_shortcut_notify_deregist
    /// 0x201C (0x1F73 + 0xA9)  proto_area_implement_client::recv_data_notify_soulmaterialobject_data
    /// 0x2063 - proto_area_implement_client::recv_item_update_date_end_protect
    /// 0x213C - proto_area_implement_client::recv_item_update_date_end_protect
    /// 0x218A (0x213C + 0x4E) ret
    /// 0x2246 - proto_area_implement_client::recv_union_close_window_r
    /// 0x2257 - proto_area_implement_client::recv_charabody_salvage_request_r
    /// 0x22E7 - proto_area_implement_client::recv_refusallist_add_user_r
    /// 0x2353 - proto_area_implement_client::recv_record_notify_open
    /// 0x239D (0x2353 + 0x4A) proto_area_implement_client::recv_emotion_update_type_r
    /// 0x23D3 - proto_area_implement_client::recv_chara_notify_stateflag
    /// 0x23E5 - proto_area_implement_client::recv_object_sub_target_update_notify
    /// 0x2467 - proto_area_implement_client::recv_shop_buy_r
    /// 0x2470 - proto_area_implement_client::recv_data_notify_npc_data
    /// 0x2478 - proto_area_implement_client::recv_chara_update_maxap
    /// 0x250E (0x2478 + 0x96) proto_area_implement_client::recv_data_notify_ggate_stone_data
    /// 0x2513 - proto_area_implement_client::recv_battle_report_notify_soul_point
    /// 0x2576 - proto_area_implement_client::recv_chara_update_mp
    /// 0x260E - proto_area_implement_client::recv_raisescale_view_close
    /// 0x2631 - proto_area_implement_client::recv_soulmaterial_shop_notify_open
    /// 0x266C - proto_area_implement_client::recv_quest_get_soul_mission_quest_works_r
    /// 0x267B (0x266C + 0xF) proto_area_implement_client::recv_event_quest_order
    /// 0x267D - proto_area_implement_client::recv_logout_cancel_request_r
    /// 0x26B8 - proto_area_implement_client::recv_event_removetrap_release_rate_update
    /// 0x2790 - proto_area_implement_client::recv_union_storage_move_item_r
    /// 0x27D6 - proto_area_implement_client::recv_shortcut_request_regist_r
    /// 0x2849 - proto_area_implement_client::recv_channel_notify
    /// 0x287A   --------------------------------------------------------------not an opcode??
    /// 0x28A0   no string
    /// 0x28E7 - proto_area_implement_client::recv_chara_update_con
    /// 0x29C5 (0x28E7 + 0xDE) ret
    /// 0x2A3F - proto_area_implement_client::recv_skill_tree_lost
    /// 0x2A82 - proto_area_implement_client::recv_battle_report_action_monster_skill_exec
    /// 0x2AB0 (0x2A82 + 0x2E) proto_area_implement_client::recv_chara_update_mag_mp_cost_per
    /// 0x2BA4 - proto_area_implement_client::recv_chara_update_ac
    /// 0x2BAB - proto_area_implement_client::recv_blacklist_unlock_r
    /// 0x2BBF - proto_area_implement_client::recv_item_use_r
    /// 0x2C30 (0x2BBF + 0x71) proto_area_implement_client::recv_premium_service_notify_detach
    /// 0x2CAF - proto_area_implement_client::recv_item_update_level
    /// 0x2CB0 - proto_area_implement_client::recv_chara_update_lv_detail_end
    /// 0x2D6D - proto_area_implement_client::recv_cash_shop_buy_r
    /// 0x2DCE - proto_area_implement_client::recv_stall_set_item_price_r
    /// 0x2E17 - proto_area_implement_client::recv_auction_update_bid_item_state
    /// 0x2F0E - proto_area_implement_client::recv_thread_exit_r
    /// 0x2FFF - proto_area_implement_client::recv_stall_notify_closed
    /// 0x300A - proto_area_implement_client::recv_party_invite_r
    /// 0x30BE - network::proto_area_implement_client::recv_get_send_package
    /// 0x30FB (0x30BE + 0x3D) proto_area_implement_client::recv_blacklist_lock_r
    /// 0x316F - proto_area_implement_client::recv_gem_set_synthesis_r
    /// 0x3223 - proto_area_implement_client::recv_chara_update_weight
    /// 0x322F - proto_area_implement_client::recv_chara_update_job_attr_mp_cost_per
    /// 0x3247 - proto_area_implement_client::recv_item_update_state
    /// 0x32ED (0x3247 + 0xA6)  proto_area_implement_client::recv_event_change_type
    /// 0x32F7 - proto_area_implement_client::recv_event_select_push
    /// 0x32FF - proto_area_implement_client::recv_quest_history
    /// 0x3428 - proto_area_implement_client::recv_party_change_mode_r
    /// 0x3544 - proto_area_implement_client::recv_npc_ggate_state_update_notify
    /// 0x362D - proto_area_implement_client::recv_battle_report_noact_notify_buff_attach
    /// 0x36A6 (0x362D + 0x79) proto_area_implement_client::recv_charabody_notify_spirit
    /// 0x36DC - proto_area_implement_client::recv_gem_rebuild_info
    /// 0x3806 - proto_area_implement_client::recv_base_enter_r
    /// 0x394F - proto_area_implement_client::recv_event_soul_rankup_open
    /// 0x39FD - proto_area_implement_client::recv_battle_report_action_skill_start_cast
    /// 0x3A0E   ret
    /// 0x3B77 - proto_area_implement_client::recv_npc_state_update_notify
    /// 0x3B9F - proto_area_implement_client::recv_self_skill_point_notify
    /// 0x3C41 (0x3B9F + 0xA2) proto_area_implement_client::recv_help_new_data
    /// 0x3C68 - proto_area_implement_client::recv_union_request_growth_result
    /// 0x3C81   no string
    /// 0x3C89 (0x3C81 + 0x8) proto_area_implement_client::recv_data_get_self_chara_data_request_r
    /// 0x3D6C - proto_area_implement_client::recv_dbg_chara_equipped
    /// 0x3D9F - proto_area_implement_client::recv_battle_report_action_fall
    /// 0x3DD0 - proto_area_implement_client::recv_event_soul_storage_open
    /// 0x3E0B - proto_area_implement_client::recv_trade_notify_problem
    /// 0x3E7F (0x3E0B + 0x74) proto_area_implement_client::recv_charabody_self_salvage_result
    /// 0x3F2F - proto_area_implement_client::recv_charabody_self_salvage_result
    /// 0x3F38 - proto_area_implement_client::recv_auction_bid_r
    /// 0x3F5B - proto_area_implement_client::recv_self_inherit_skill_level_notify
    /// 0x42B6 - proto_area_implement_client::recv_shortcut_request_deregist_r
    /// 0x42B8 - proto_area_implement_client::recv_quest_display_r
    /// 0x42DE - aproto_area_implement_client::recv_gem_break_cost_r
    /// 0x4404 - proto_area_implement_client::recv_event_select_exec_winpos
    /// 0x442A - proto_area_implement_client::recv_union_rename_open
    /// 0x443C - proto_area_implement_client::recv_item_update_maxdur
    /// 0x4440 - proto_area_implement_client::recv_union_storage_draw_money_r
    /// 0x4537 - proto_area_implement_client::recv_wanted_jail_payment_r
    /// 0x462E - proto_area_implement_client::recv_event_sync
    /// 0x465C - proto_area_implement_client::recv_quest_target_count_r
    /// 0x488D (0x4883 + 0xA) --------------------------------------------------------------not an opcode??
    /// 0x4898 (0x488E + 0xA) proto_area_implement_client::recv_battle_report_notify_exp2
    /// 0x488E - proto_area_implement_client::recv_get_refusallist_r
    /// 0x4978   no string
    /// 0x4981 - proto_area_implement_client::recv_event_select_channel
    /// 0x4992 (0x4978 + 0x1A) proto_area_implement_client::recv_event_select_channel
    /// 0x4ABB   no string
    /// 0x4B94 - proto_area_implement_client::recv_self_toggle_ability_notify
    /// 0x4C6F - proto_area_implement_client::recv_skill_start_cast_ex_r
    /// 0x4C74 - proto_area_implement_client::recv_map_change_force
    /// 0x4C8B (0x4C74 + 0x17) proto_area_implement_client::recv_logout_start_request_r
    /// 0x4CF3   ret
    /// 0x4D70 - proto_area_implement_client::recv_item_sort_r
    /// 0x4DF8 - proto_area_implement_client::recv_party_establish_r
    /// 0x4E10 - proto_area_implement_client::recv_chara_update_lv_detail_start
    /// 0x4E8D (0x4E17 + 0x76) proto_area_implement_client::recv_shop_sell_check_r
    /// 0x4EF4 - proto_area_implement_client::recv_battle_release_attack_pose_self
    /// 0x4F10 - proto_area_implement_client::recv_battle_charge_start_r
    /// 0x5016 - proto_area_implement_client::recv_event_system_message_timer_end
    /// 0x505E - proto_area_implement_client::recv_quest_hint
    /// 0x50B2 - proto_area_implement_client::recv_union_request_mantle_get_result
    /// 0x50D1   no string
    /// 0x5243 - proto_area_implement_client::recv_object_ac_rank_update_notify
    /// 0x5307 (0x5243 + 0xC4) proto_area_implement_client::recv_auction_update_exhibit_item_state
    /// 0x531B   no string
    /// 0x536A - proto_area_implement_client::recv_trade_notify_reverted
    /// 0x5393 (0x536A + 0x29) proto_area_implement_client::recv_party_notify_cancel_member_recruit
    /// 0x5513   no string
    /// 0x551E - proto_area_implement_client::recv_item_equip_r
    /// 0x5527 (0x551E + 0x9) proto_area_implement_client::recv_monster_state_update_notify
    /// 0x556E - proto_area_implement_client::recv_package_notify_add
    /// 0x570B - proto_area_implement_client::recv_cash_shop_get_url_web_goods_r
    /// 0x5842 - proto_area_implement_client::recv_door_open_r
    /// 0x5899 - proto_area_implement_client::recv_battle_report_action_effect_onhit
    /// 0x58A8 - proto_area_implement_client::recv_chara_notify_party_join
    /// 0x58E7 - proto_area_implement_client::recv_logout_start
    /// 0x5A5F - proto_area_implement_client::recv_create_package_r
    /// 0x5AE9 - proto_area_implement_client::recv_battle_charge_end_r
    /// 0x5C15 - proto_area_implement_client::recv_trade_notify_offerd
    /// 0x5C47 - proto_area_implement_client::recv_monster_hate_on
    /// 0x5CF8 - proto_area_implement_client::recv_event_quest_order_list_begin
    /// 0x5D52 - proto_area_implement_client::recv_forge_sp_check_r
    /// 0x5E48 (0x5D52 + 0xF6) proto_area_implement_client::recv_battle_guard_end_self
    /// 0x5E79 - proto_area_implement_client::recv_stall_buy_item_r
    /// 0x5EDB - proto_area_implement_client::recv_event_show_board_start
    /// 0x5F1A (0x5EDB + 0x3F) proto_area_implement_client::recv_party_apply_r
    /// 0x5F6C - proto_area_implement_client::recv_chara_update_action_prohibit
    /// 0x6001 - proto_area_implement_client::recv_message_board_close_r
    /// 0x6023 (0x6001 + 0x22) proto_area_implement_client::recv_loot_access_object_r
    /// 0x6030 - proto_area_implement_client::recv_raisescale_open_cash_shop_r
    /// 0x60E5 - proto_area_implement_client::recv_union_request_secede_result
    /// 0x6175 - proto_area_implement_client::recv_party_decline_to_invite_r
    /// 0x61C8 - proto_area_implement_client::recv_shop_notify_close
    /// 0x621A - proto_area_implement_client::recv_revive_execute_r
    /// 0x62E2 - proto_area_implement_client::recv_trade_notify_fixed
    /// 0x63B2 - proto_area_implement_client::recv_quest_hint_remove
    /// 0x6510 - proto_area_implement_client::recv_data_notify_debug_object_data
    /// 0x6539 - proto_area_implement_client::recv_storage_deposit_money_r
    /// 0x6547 - proto_area_implement_client::recv_get_thread_record_r
    /// 0x6554 - proto_area_implement_client::recv_cash_shop2_notify_open
    /// 0x65A6 - proto_area_implement_client::recv_chara_update_maxac
    /// 0x65EE (0x65A6 + 0x48) proto_area_implement_client::recv_battle_report_noact_notify_buff_move
    /// 0x662F - proto_area_implement_client::recv_event_message
    /// 0x66EC - proto_area_implement_client::recv_wanted_update_state_notify
    /// 0x679B - proto_area_implement_client::recv_charabody_self_notify_abyss_stead_pos
    /// 0x68AA - proto_area_implement_client::recv_party_notify_failed_draw_all
    /// 0x68E7 - proto_area_implement_client::recv_battle_attack_next_r
    /// 0x6911 (0x68E7 + 0x2A) proto_area_implement_client::recv_skill_aptitude_gain
    /// 0x6912 - proto_area_implement_client::recv_gem_break_r
    /// 0x692A   no string
    /// 0x692B (0x692A + 0x1) proto_area_implement_client::recv_thread_enter_r
    /// 0x6979 - proto_area_implement_client::recv_party_disband_r
    /// 0x6A56 - proto_area_implement_client::recv_event_select_exec
    /// 0x6A7A - proto_area_implement_client::recv_chara_update_ap
    /// 0x6B6A   no string
    /// 0x6B74 (0x6B6A + 0xA) proto_area_implement_client::recv_chara_update_maxmp
    /// 0x6BDC - proto_area_implement_client::recv_battle_report_action_item_enchant
    /// 0x6C79 - proto_area_implement_client::recv_data_notify_npc_ex_dragon
    /// 0x6C7B (0x6C79 + 0x2) proto_area_implement_client::recv_quest_chapter_target_updated
    /// 0x6D46 - proto_area_implement_client::recv_quest_abort_r
    /// 0x6DB4 - proto_area_implement_client::recv_gem_cancel_piece_r
    /// 0x6DF4 (0x6DB4 + 0x40) proto_area_implement_client::recv_battle_report_noact_combo_bonus_damage
    /// 0x6E06 - proto_area_implement_client::recv_battle_report_action_steal
    /// 0x6F32 - proto_area_implement_client::recv_cmd_exec_r
    /// 0x6FB2 - proto_area_implement_client::recv_self_dragon_pos_notify
    /// 0x7004 - proto_area_implement_client::recv_event_quest_report_list_begin
    /// 0x708B - proto_area_implement_client::recv_item_move_r
    /// 0x70B7 - proto_area_implement_client::recv_stall_shopping_abort_r
    /// 0x70F6 - proto_area_implement_client::recv_soul_dispitem_remove_data
    /// 0x715E (0x70F6 + 0x68) proto_area_implement_client::recv_trade_remove_item_r
    /// 0x717D- proto_area_implement_client::recv_object_disappear_notify
    /// 0x7181 - proto_area_implement_client::recv_battle_report_notify_exp_bonus
    /// 0x732D - proto_area_implement_client::recv_cash_shop_notify_open
    /// 0x7341 - proto_area_implement_client::recv_forge_sp_execute_r
    /// 0x735E   no string
    /// 0x73A1 - proto_area_implement_client::recv_quest_get_story_quest_works_r
    /// 0x73D1 - proto_area_implement_client::recv_self_buff_notify
    /// 0x746F (0x73D1 + 0x9E) no string
    /// 0x755C   no string
    /// 0x7576 - proto_area_implement_client::recv_party_notify_remove_draw_item
    /// 0x75A5 (0x7576 + 0x2F) proto_area_implement_client::recv_item_use_notify
    /// 0x7697   no string
    /// 0x772A - proto_area_implement_client::recv_door_close_r
    /// 0x77A7 - proto_area_implement_client::recv_event_access_object_r
    /// 0x789E - proto_area_implement_client::recv_self_lost_notify
    /// 0x793E - proto_area_implement_client::recv_map_enter_r
    /// 0x79A2 - proto_area_implement_client::recv_event_abort_r
    /// 0x79A9 - proto_area_implement_client::recv_storage_drawmoney
    /// 0x7A5C - proto_area_implement_client::recv_chara_update_ap_cost_diff
    /// 0x7A6F - proto_area_implement_client::recv_cash_shop_regist_billing_zip_r
    /// 0x7B5D - proto_area_implement_client::recv_object_point_move_r
    /// 0x7B86 (0x7B5D + 0x29) no string
    /// 0x7BB3 - proto_area_implement_client::recv_party_change_leader_r
    /// 0x7CB2 - proto_area_implement_client::recv_battle_attack_pose_start_notify
    /// 0x7CF0 - proto_area_implement_client::recv_wanted_list_actor
    /// 0x7D1C - proto_area_implement_client::recv_charabody_salvage_notify_body
    /// 0x7D2B (0x7D1C + 0xF)  proto_area_implement_client::recv_auction_notify_close
    /// 0x7D53 - proto_area_implement_client::recv_event_change_cancel_sw
    /// 0x7D75 - proto_area_implement_client::recv_union_open_window
    /// 0x7F09 - proto_area_implement_client::recv_cash_shop_get_url_r
    /// 0x7F34 - proto_area_implement_client::recv_skill_custom_slot_set_r
    /// 0x7F50 - proto_area_implement_client::recv_skill_next_cast_r
    /// 0x7FC5 - proto_area_implement_client::recv_stall_notify_opend ///////////////////could be wrong, crashes mid-way through and had to be nop'd
    /// 0x8066   ret
    /// 0x8091 (0x8066 + 0x2B) proto_area_implement_client::recv_event_script_play
    /// 0x825D   ret
    /// 0x8299 - proto_area_implement_client::recv_premium_service_notify_attach2
    /// 0x8395 (0x8299 + 0xFC) proto_area_implement_client::recv_skill_tree_gain
    /// 0x839A - proto_area_implement_client::recv_party_notify_cancel_party_recruit
    /// 0x840E   ------------------------------------------------------------------------------not an opcode?
    /// 0x8487   no string
    /// 0x8549 (0x8487 + 0xC2) no string
    /// 0x855C - proto_area_implement_client::recv_party_decline_to_apply_r
    /// 0x85C6 - proto_area_implement_client::recv_temple_notify_open
    /// 0x86A5 (0x85C6 + 0xDF) proto_area_implement_client::recv_soulmaterial_shop_buy_r
    /// 0x86EA - proto_area_implement_client::recv_item_instance
    /// 0x8778 - proto_area_implement_client::recv_union_storage_deposit_money_r
    /// 0x877C (0x8778 + 0x4) proto_area_implement_client::recv_battle_report_action_attack_failed
    /// 0x8820 - proto_area_implement_client::recv_battle_report_action_removetrap_ontrap
    /// 0x88A1 - proto_area_implement_client::recv_forge_notify_execute_result
    /// 0x88AE (0x88A1 + 0xD) proto_area_implement_client::recv_job_change_notify_exe
    /// 0x88FB - proto_area_implement_client::recv_trade_abort_r
    /// 0x89FF - proto_area_implement_client::recv_charabody_access_end
    /// 0x8A06 (0x89FF + 0x7) proto_area_implement_client::recv_trade_fix_r
    /// 0x8BB4 - proto_area_implement_client::recv_shortcut_notify_regist
    /// 0x8BD2 - proto_area_implement_client::recv_event_block_message
    /// 0x8C2F - proto_area_implement_client::recv_chara_update_notify_honor
    /// 0x8CC6 - proto_area_implement_client::recv_escape_cancel
    /// 0x8D92 (0x8CC6 + 0xCC) no string
    /// 0x8D9B - proto_area_implement_client::recv_event_block_message_no_object
    /// 0x8DBC - proto_area_implement_client::recv_trade_reply_r
    /// 0x8E92 - proto_area_implement_client::recv_battle_report_action_skill_cancel
    /// 0x8F15 - proto_area_implement_client::recv_check_job_change_r
    /// 0x8F84 - proto_area_implement_client::recv_echo_notify
    /// 0x903A - proto_area_implement_client::recv_skill_request_gain_r
    /// 0x906A - proto_area_implement_client::recv_data_notify_charabody_data structure: 32,64,byte,float,float,32,byte,float,float,32,byte,32,16
    /// 0x90E8 (0x906A + 0x7E) proto_area_implement_client::recv_forge_check_r
    /// 0x919C - proto_area_implement_client::recv_stall_sell_item
    /// 0x9201   no string
    /// 0x9227 (0x9201 + 0x26) proto_area_implement_client::recv_message_board_notify_update
    /// 0x924E - proto_area_implement_client::recv_random_box_close_r
    /// 0x9289 - proto_area_implement_client::recv_stall_regist_item_r
    /// 0x92DE (0x9289 + 0x55) proto_area_implement_client::recv_equip_honor_r
    /// 0x935B - proto_area_implement_client::recv_gem_rebuild_r
    /// 0x951B - proto_area_implement_client::recv_record_notify_param
    /// 0x9578 - proto_area_implement_client::recv_charabody_salvage_request_cancel_r
    /// 0x95E6 - proto_area_implement_client::recv_thread_exit_message
    /// 0x965E (0x95E6 + 0x78) proto_area_implement_client::recv_dropobject_notify_stateflag
    /// 0x9666 - proto_area_implement_client::recv_battle_attack_cancel_r
    /// 0x96EA - proto_area_implement_client::recv_eo_update_second_trapid
    /// 0x9700 - proto_area_implement_client::recv_item_remove
    /// 0x971B - proto_area_implement_client::recv_trade_set_money_r
    /// 0x9761 (0x971B + 0x46) proto_area_implement_client::recv_stall_open_r
    /// 0x97D9   no string
    /// 0x9870 - proto_area_implement_client::recv_door_update_notify
    /// 0x9899   no string
    /// 0x98D3 - proto_area_implement_client::recv_raisescale_move_money_r
    /// 0x998F - proto_area_implement_client::recv_battle_attack_exec_direct_r
    /// 0x9A44 - proto_area_implement_client::recv_refusallist_remove_user_r
    /// 0x9A79 - proto_area_implement_client::recv_item_unequip_r
    /// 0x9AA9 (0x9A79 + 0x30) proto_area_implement_client::recv_map_change_sync_ok
    /// 0x9B08 - proto_area_implement_client::recv_item_update_enchantid
    /// 0x9BC6 - proto_area_implement_client::recv_auction_update_bid_num
    /// 0x9C3A (0x9BC6 + 0x74) proto_area_implement_client::recv_comment_set_r
    /// 0x9CA1   no string
    /// 0x9CCF - proto_area_implement_client::recv_self_soul_material_notify
    /// 0x9D52 - proto_area_implement_client::recv_gem_set_support_item_r
    /// 0x9D5A - proto_area_implement_client::recv_cash_shop_check_billing_zip_r
    /// 0x9D96 (0x9D5A + 0x3C) proto_area_implement_client::recv_get_recv_package
    /// 0x9DE2 - proto_area_implement_client::recv_dbg_message
    /// 0x9E19 - proto_area_implement_client::recv_cash_shop2_buy_r
    /// 0x9E75 (0x9E19 + 0x5C) proto_area_implement_client::recv_self_soul_rank_notify
    /// 0x9F31 - proto_area_implement_client::recv_chara_update_maxhp
    /// 0x9F70 - proto_area_implement_client::recv_chara_notify_party_leave
    /// 0xA005 (0x9F70 + 0x95) proto_area_implement_client::recv_cash_shop_get_current_cash_r
    /// 0xA084 - proto_area_implement_client::recv_shortcut_request_data_r
    /// 0xA0E3 - proto_area_implement_client::recv_chara_notify_union_data
    /// 0xA18D (0xA0E3 + 0xAA) proto_area_implement_client::recv_cash_shop_get_url_common_steam_r
    /// 0xA21E - proto_area_implement_client::recv_job_change_r
    /// 0xA2B7 - proto_area_implement_client::recv_stall_shopping_start_r
    /// 0xA43B - proto_area_implement_client::recv_temple_notify_close
    /// 0xA45C - proto_area_implement_client::recv_party_search_recruited_party_r
    /// 0xA4A5 - proto_area_implement_client::recv_gimmick_access_object_r
    /// 0xA4D3 - proto_area_implement_client::recv_party_cancel_party_recruit_r
    /// 0xA508 - proto_area_implement_client::recv_raisescale_update_success_per
    /// 0xA549 (0xA508 + 0x41) proto_area_implement_client::recv_auction_re_exhibit_r
    /// 0xA54C - proto_area_implement_client::recv_get_honor_notify
    /// 0xA611 - proto_area_implement_client::recv_item_update_physics
    /// 0xA6F8 (0xA611 + 0xE7) proto_area_implement_client::recv_skill_cooltime_notify
    /// 0xA7BF - proto_area_implement_client::recv_party_regist_member_recruit_r
    /// 0xA7E8 - proto_area_implement_client::recv_chara_notify_map_fragment
    /// 0xA8BB (0xA7E8 + 0xD3) ret
    /// 0xA90C   ret
    /// 0xA938 - proto_area_implement_client::recv_battle_report_noact_notify_heal_condition
    /// 0xA9C2 - proto_area_implement_client::recv_chara_update_lv_detail
    /// 0xAA8F - proto_area_implement_client::recv_thread_create_r
    /// 0xAADD - proto_area_implement_client::recv_talkring_rename_request
    /// 0xAB14   no string
    /// 0xAB8A - proto_area_implement_client::recv_battle_guard_end_r
    /// 0xABC5 (0xAB8A + 0x3B) ret
    /// 0xABF2 - proto_area_implement_client::recv_gem_close_r
    /// 0xAD6D - proto_area_implement_client::recv_forge_execute_r
    /// 0xADC8 (0xAD6D + 0x5B) proto_area_implement_client::recv_battle_guard_start_r
    /// 0xAE27 - proto_area_implement_client::recv_event_system_message_timer
    /// 0xAE2B - proto_area_implement_client::recv_normal_system_message
    /// 0xAEE9 - proto_area_implement_client::recv_sv_conf_option_change_r
    /// 0xAF49 - proto_area_implement_client::recv_chat_post_message_r
    /// 0xAF6D - proto_area_implement_client::recv_eo_base_notify_sphere
    /// 0xAF76 - proto_area_implement_client::recv_battle_report_notify_attack_hitstate
    /// 0xAF7F - proto_area_implement_client::recv_chara_update_notify_crime_lv
    /// 0xB037 (0xAF7F + 0xB8) proto_area_implement_client::recv_battle_report_notify_hit_effect_name
    /// 0xB0BF - proto_area_implement_client::recv_dbg_battle_guard_start_notify
    /// 0xB0E5 - proto_area_implement_client::recv_event_removetrap_ident_trap_update
    /// 0xB10F (0xB0E5 + 0x2A) proto_area_implement_client::recv_union_request_disband_result
    /// 0xB195 - proto_area_implement_client::recv_stall_update_feature_item
    /// 0xB1CA - proto_area_implement_client::recv_auction_receive_item_r
    /// 0xB292 (0xB1CA + 0xC8) proto_area_implement_client::recv_event_removetrap_begin
    /// 0xB317   ----------------------------------------------------------------------------not an opcode? is actually 0xB371
    /// 0xB319 - proto_area_implement_client::recv_skill_custom_notify_open
    /// 0xB371 - proto_area_implement_client::recv_item_update_place
    /// 0xB39D (0xB317 + 0x86) --------------------------------------------------------------not an opocde, is actually (0xB371 + 2C)
    /// 0xB3F7 (0xB371 + 0x86) proto_area_implement_client::recv_battle_report_action_attack_exec
    /// 0xB417 - proto_area_implement_client::recv_charabody_self_notify_deadnext_time
    /// 0xB435 (0xB417 + 0x1E) proto_area_implement_client::recv_chara_update_alignment_param
    /// 0xB586   ret
    /// 0xB619 - proto_area_implement_client::recv_trade_notify_aborted
    /// 0xB631 (0xB619 + 0x18) proto_area_implement_client::recv_chara_update_notify_item_forth
    /// 0xB684   ret
    /// 0xB6B1 - proto_area_implement_client::recv_chara_pose_notify
    /// 0xB782 - proto_area_implement_client::recv_event_request_int
    /// 0xB813 - proto_area_implement_client::recv_data_notify_eventlink
    /// 0xBA11 - proto_area_implement_client::recv_sixthsense_trap_notify
    /// 0xBA61   ret
    /// 0xBA71 - proto_area_implement_client::recv_auction_notify_open
    /// 0xBA73 network::proto_area_implement_client::recv_cpf_authenticate
    /// 0xBA89 network::proto_area_implement_client::recv_gamepot_web_notify_open
    /// 0xBB65 (0xBA71 + 0xF4) proto_area_implement_client::recv_event_union_storage_close_r
    /// 0xBBA5 - proto_area_implement_client::recv_talkring_rename_masterring_r
    /// 0xBC0A - proto_area_implement_client::recv_monster_hate_off
    /// 0xBCAB - proto_area_implement_client::recv_quest_check_target_r
    /// 0xBCC2 - proto_area_implement_client::recv_chara_notify_raise_downper
    /// 0xBD72 - proto_area_implement_client::recv_get_honor
    /// 0xBD7E - proto_area_implement_client::recv_event_tresurebox_begin
    /// 0xBD90 - proto_area_implement_client::recv_battle_report_notify_rookie
    /// 0xBD99 (0xBD90 + 0x9) - proto_area_implement_client::recv_battle_report_notify_rookie
    /// 0xBF0D   ret
    /// 0xBF34 - proto_area_implement_client::recv_trade_notify_money
    /// 0xBFE9 - proto_area_implement_client::recv_data_notify_gimmick_data
    /// 0xBFEA - proto_area_implement_client::recv_get_thread_all_r
    /// 0xC003 - proto_area_implement_client::recv_chat_notify_message
    /// 0xC06C (0xC026 + 0x46) --------------------------------------------------------------not an opocde?
    /// 0xC078 (0xC003 + 0x75) ret
    /// 0xC0BB - proto_area_implement_client::recv_skill_aptitude_lost
    /// 0xC0D8 - proto_area_implement_client::recv_help_new_remove_r
    /// 0xC1AF (0xC0D8 + 0xD7) proto_area_implement_client::recv_wanted_list_close_r
    /// 0xC1DC - proto_area_implement_client::recv_chara_update_notify_rookie
    /// 0xC206 - proto_area_implement_client::recv_gem_notify_open
    /// 0xC24E - proto_area_implement_client::recv_chara_update_action_prohibit_camp
    /// 0xC2A1 - proto_area_implement_client::recv_battle_report_noact_notify_knockback
    /// 0xC374 (0xC2A1 + 0xD3) proto_area_implement_client::recv_random_box_notify_open
    /// 0xC3EE - proto_area_implement_client::recv_cash_shop2_notify_item
    /// 0xC444 - proto_area_implement_client::recv_random_box_get_item_r
    /// 0xC47A (0xC444 + 0x36) proto_area_implement_client::recv_self_exp_notify
    /// 0xC542 - proto_area_implement_client::recv_skill_start_cast_self
    /// 0xC543 - proto_area_implement_client::recv_skill_combo_cast_r
    /// 0xC54F - proto_area_implement_client::recv_wanted_jail_draw_point_r
    /// 0xC68B - proto_area_implement_client::recv_wanted_jail_update_state
    /// 0xC6EF (0xC68B + 0x64) proto_area_implement_client::recv_self_dragon_warp_notify
    /// 0xC6F2 - proto_area_implement_client::recv_wanted_list_open
    /// 0xC701 - proto_area_implement_client::recv_battle_report_action_skill_onhit
    /// 0xC7E1 - proto_area_implement_client::recv_item_update_sp_level
    /// 0xC8AD - proto_area_implement_client::recv_storage_deposit_item2_r
    /// 0xC96F - proto_area_implement_client::recv_buff_request_detach_r
    /// 0xC9FF - proto_area_implement_client::recv_battle_report_notify_soul_material
    /// 0xCA35 - proto_area_implement_client::recv_battle_report_noact_notify_heal_life
    /// 0xCAB1 - proto_area_implement_client::recv_talkring_create_masterring_r
    /// 0xCB6D - proto_area_implement_client::recv_party_notify_failed_draw
    /// 0xCB94 - proto_area_implement_client::recv_trade_notify_interface_status
    /// 0xCC37 (0xCB94 + 0xA3) proto_area_implement_client::recv_event_message_no_object
    /// 0xCC54 - proto_area_implement_client::recv_battle_report_action_cover
    /// 0xCCE2 - proto_area_implement_client::recv_chara_move_speed_per structure: byte, int16
    /// 0xCD63 - proto_area_implement_client::recv_dbg_select_raise_r
    /// 0xCDC9 - proto_area_implement_client::recv_battle_report_noact_notify_dead
    /// 0xCE36 - proto_area_implement_client::recv_battle_report_noact_notify_buff_attach_failed
    /// 0xCF24 - proto_area_implement_client::recv_event_select_map_and_channel
    /// 0xCF29   ret
    /// 0xCF52 (0xCF29 + 0x29) proto_area_implement_client::recv_echo_r
    /// 0xCFDC - proto_area_implement_client::recv_data_notify_itemobject_data
    /// 0xD04A   ret
    /// 0xD133 (0xD04A + 0xE9) proto_area_implement_client::recv_chara_update_hp
    /// 0xD170 - proto_area_implement_client::recv_select_package_update_r
    /// 0xD1A9 - proto_area_implement_client::recv_random_box_next_open_r
    /// 0xD1BD (0xD1A9 + 0x14) proto_area_implement_client::recv_data_get_self_chara_data
    /// 0xD1CB - proto_area_implement_client::recv_auction_close_r
    /// 0xD1F6   no string
    /// 0xD2D6 - proto_area_implement_client::recv_base_ping_r
    /// 0xD349 - proto_area_implement_client::recv_map_fragment_flag
    /// 0xD400 - proto_area_implement_client::recv_event_removetrap_skill_r2
    /// 0xD43F (0xD400 + 0x3F) - proto_area_implement_client::recv_event_removetrap_skill_r2
    /// 0xD46E - proto_area_implement_client::recv_quest_started
    /// 0xD493 - proto_area_implement_client::recv_battle_report_action_item_use
    /// 0xD57A (0xD493 + 0xE7) - proto_area_implement_client::recv_battle_report_action_item_use
    /// 0xD597 - proto_area_implement_client::recv_cash_shop_notify_item
    /// 0xD5B5 - proto_area_implement_client::recv_chara_update_lv_detail2
    /// 0xD67D (0xD5B5 + 0xC8) - proto_area_implement_client::recv_chara_update_lv_detail2
    /// 0xD688 - proto_area_implement_client::recv_charabody_salvage_end
    /// 0xD68C - proto_area_implement_client::recv_escape_start
    /// 0xD752 (0xD68C + 0xC6) proto_area_implement_client::recv_battle_attack_start_r
    /// 0xD7D8 - proto_area_implement_client::recv_battle_report_notify_invalid_target
    /// 0xD804 - proto_area_implement_client::recv_gimmick_access_object_notify
    /// 0xD8D5 - proto_area_implement_client::recv_battle_report_noact_notify_heal_mp
    /// 0xD909   ret
    /// 0xD972 - proto_area_implement_client::recv_chara_pose_ladder_r
    /// 0xDA4A   ret
    /// 0xDA5C - proto_area_implement_client::recv_base_exitr
    /// 0xDB53 - proto_area_implement_client::recv_battle_report_action_removetrap_success
    /// 0xDB5E - proto_area_implement_client::recv_battle_report_noact_notify_buff_effect
    /// 0xDB88 - proto_area_implement_client::recv_charabody_self_salvage_request_cancel
    /// 0xDBF1 - proto_area_implement_client::recv_cash_shop_fitting_begin
    /// 0xDC5B - proto_area_implement_client::recv_battle_report_action_steal_unidentified
    /// 0xDCB6 (0xDC5B + 0x5B) proto_area_implement_client::recv_event_removetrap_release_rate_close
    /// 0xDD52 - proto_area_implement_client::recv_wanted_update_state
    /// 0xDDD3 - proto_area_implement_client::recv_wanted_entry_r
    /// 0xDE90 - proto_area_implement_client::recv_shop_notify_sell_surrogate_fee
    /// 0xDEB7 - proto_area_implement_client::recv_cash_shop_fitting_end
    /// 0xDEC2 (0xDEB7 + 0xB) proto_area_implement_client::recv_shop_title_push
    /// 0xDF31 - proto_area_implement_client::recv_blacklist_open_r
    /// 0xE039 - proto_area_implement_client::recv_shop_notify_item_sell_price
    /// 0xE03D (0xE039 + 0x4) proto_area_implement_client::recv_premium_service_update_day
    /// 0xE051 - proto_area_implement_client::recv_data_notify_goldobject_data
    /// 0xE07E - proto_area_implement_client::recv_event_quest_report
    /// 0xE14B (0xE07E + 0xCD) proto_area_implement_client::recv_map_update_bgm
    /// 0xE1F8 - proto_area_implement_client::recv_event_select_ready
    /// 0xE207 - proto_area_implement_client::recv_party_kick_r
    /// 0xE462 - proto_area_implement_client::recv_union_request_rename_r
    /// 0xE4B2 - proto_area_implement_client::recv_skill_start_cast_r
    /// 0xE526 - proto_area_implement_client::recv_battle_report_notify_exp
    /// 0xE5FF - proto_area_implement_client::recv_quest_select_error
    /// 0xE748 - proto_area_implement_client::recv_party_notify_add_draw_item
    /// 0xE7BB - proto_area_implement_client::recv_wanted_jail_open
    /// 0xE7E7 (0xE7BB + 0x2C) proto_area_implement_client::recv_union_rename_close_r
    /// 0xE819 - proto_area_implement_client::recv_item_update_spirit_eqmask
    /// 0xE897 - proto_area_implement_client::recv_battle_report_action_skill_failed
    /// 0xE990 (0xE897 + 0xF9) proto_area_implement_client::recv_battle_release_attack_pose_r
    /// 0xEA1C - proto_area_implement_client::recv_dbg_chara_unequipped
    /// 0xEB47 - proto_area_implement_client::recv_battle_report_notify_exp_bonus2
    /// 0xEB7B (0xEB47 + 0x34) proto_area_implement_client::recv_wanted_list_member (i think)
    /// 0xECBA - proto_area_implement_client::recv_shop_identify_r
    /// 0xED4C - proto_area_implement_client::recv_data_notify_chara_data
    /// 0xEDA6 - proto_area_implement_client::recv_map_get_info_r
    /// 0xEDB3 - proto_area_implement_client::recv_data_notify_eo_data2
    /// 0xEE18 (0xEDB3 + 0x65) ret
    /// 0xEE43 - proto_area_implement_client::recv_skill_start_item_cast_r
    /// 0xEEB7   ret
    /// 0xEF8D (0xEEB7 + 0xD6) proto_area_implement_client::recv_raisescale_remove_item_r
    /// 0xEFA4 - proto_area_implement_client::recv_trade_invite_r
    /// 0xEFDD - proto_area_implement_client::recv_base_check_version_r
    /// 0xF02B (0xEFDD + 0x4E) - proto_area_implement_client::recv_random_box_get_item_all_r
    /// 0xF1A0 - proto_area_implement_client::recv_battle_attack_exec_r
    /// 0xF212 - proto_area_implement_client::recv_battle_report_action_attack_onhit
    /// 0xF330 - proto_area_implement_client::recv_cloak_close_r
    /// 0xF393 - proto_area_implement_client::recv_battle_report_notify_damage_ac
    /// 0xF3A1 - proto_area_implement_client::recv_inherit_start_r
    /// 0xF447 - proto_area_implement_client::recv_shop_notify_item
    /// 0xF495 - proto_area_implement_client::recv_party_search_recruited_member_r
    /// 0xF602 - proto_area_implement_client::recv_raisescale_add_item_r
    /// 0xF633 - proto_area_implement_client::recv_battle_attack_pose_end_notify
    /// 0xF6AF - proto_area_implement_client::recv_npc_affection_rank_update_notify
    /// 0xF71C - proto_area_implement_client::recv_chara_pose_r
    /// 0xF7E7 - proto_area_implement_client::recv_auction_search_r
    /// 0xF7F0 (0xF7E7 + 0x9) proto_area_implement_client::recv_party_notify_get_item
    /// 0xF95B - proto_area_implement_client::recv_emotion_notify_type
    /// 0xF9F9 - proto_area_implement_client::recv_map_entry_r
    /// 0xFA0B   no string
    /// 0xFB79   no string
    /// 0xFC1A - proto_area_implement_client::recv_job_change_close_r
    /// 0xFC28 - proto_area_implement_client::recv_auction_cancel_exhibit_r
    /// 0xFC75 - proto_area_implement_client::recv_battle_report_start_notify
    /// 0xFCAD (0xFC75 + 0x38) proto_area_implement_client::recv_cash_shop_fitting_item_update_eqmask
    /// 0xFCC0 - proto_area_implement_client::recv_return_home_request_exec_r
    /// 0xFCF3 - proto_area_implement_client::recv_wanted_update_reward_point
    /// 0xFD78 (0xFCF3 + 0x85) proto_area_implement_client::recv_chara_update_mag_cast_time_per
    /// 0xFDB2 - proto_area_implement_client::recv_package_all_delete_r
    /// 0xFDB8 - proto_area_implement_client::recv_event_quest_report_list_begin2
    /// 0xFDE9 (0xFDB8 + 0x31) proto_area_implement_client::recv_event_quest_report_list_begin2
    /// 0xFE2F - proto_area_implement_client::recv_party_leave_r
    /// 0xFEB7 - proto_area_implement_client::recv_event_removetrap_select_r
    /// 0xFED8 - proto_area_implement_client::recv_cloak_notify_open
    /// 0xFF00 - proto_area_implement_client::recv_event_quest_report_list_begin2
    /// 0xFFD6 (0xFF00 + 0xD6) proto_area_implement_client::recv_quest_hint_othermap
    /// 
    /// Ranges: the opcodes are presented in order from top to bottom in the sense of the jmp operation before each group.
    /// 0xEFDD + 0x4E - 0x004CEC2C 5 opcodes (1 ret)
    ///                 proto_area_implement_client::recv_base_check_version_r                      00(actually 0xEFDD)
    ///                 proto_area_implement_client::recv_chara_update_job_attr_skill_cooltime_per  01(actually 0xEFDD + 0x6)
    ///                 proto_area_implement_client::recv_charabody_access_start_r                  02(actually 0xEFDD + 0x10)
    ///                 ret                                                                         03(actually 0xEFDD + 0x47)
    ///                 proto_area_implement_client::recv_random_box_get_item_all_r                 04(actually 0xEFDD + 0x4E)
    /// 
    /// 0xFF00 + 0xD6 - 0x004D13C3 4 opcodes
    ///                 proto_area_implement_client::recv_quest_hint_othermap               00(actually 0xFF00)
    ///                 proto_area_implement_client::recv_help_new_add()\n                  01(actually 0xFF00 + 0x3F)
    ///                 proto_area_implement_client::recv_object_hp_per_update_notify()     03(actually 0xFF00 + 0xD6)
    ///                 proto_area_implement_client::recv_charabody_notify_loot_start2      02(actually 0xFF00 + 0xD2)
    ///                 
    /// 0xFCF3 + 0x85 - 0x004D042D 4 opcodes
    ///                 proto_area_implement_client::recv_chara_update_mag_cast_time_per    03(actually 0xFCF3 + 0x85)
    ///                 proto_area_implement_client::recv_wanted_update_reward_point()\n    02(actually 0xFCF3 + 0x61)
    ///                 proto_area_implement_client::recv_premium_service_update_time()     01(actually 0xFCF3 + 0x4E)
    ///                 proto_area_implement_client::recv_battle_report_notify_damage_mp    00(actually 0xFCF3)
    /// 
    /// 0xE897 + 0xF9 - 0x004CC897 5 opcodes (2 ret)
    ///                 proto_area_implement_client::recv_eo_update_end_trapid                      02(actually 0xE897 + 0x28)
    ///                 ret                                                                         01(actually 0xE897 + 0x22)
    ///                 proto_area_implement_client::recv_battle_release_attack_pose_r              04(actually 0xE897 + 0xF9)
    ///                 ret                                                                         03(actually 0xE897 + 0xEC)
    ///                 proto_area_implement_client::recv_battle_report_action_skill_failed         05(actually 0xE897)
    /// 
    /// 0xEDB3 + 0x65 - 0x004CDE73 4 opcodes (2 ret)
    ///                 proto_area_implement_client::recv_data_notify_eo_data2      00(actually 0xEDB3)
    ///                 ret                                                         03(actually 0xEDB3 + 0x65)
    ///                 proto_area_implement_client::recv_trade_revert_r            01(actually 0xEDB3 + 0x6)
    ///                 ret                                                         02(actually 0xEDB3 + 0x34)
    /// 
    /// 0xEEB7 + 0xD6 - 0x004CE472 4 opcodes (1 ret)
    ///                 proto_area_implement_client::recv_object_point_move_notify      01(actually 0xEEB7 + 0x39)
    ///                 ret                                                             00(actually 0xEEB7)
    ///                 proto_area_implement_client::recv_raisescale_remove_item_r      03(actually 0xEEB7 + 0xD6)
    ///                 proto_area_implement_client::recv_revive_init_r                 02(actually 0xEEB7 + 0x97)
    /// 
    /// 0xE7BB + 0x2C - 0x004CC261 4 opcodes (1 ret)
    ///                 proto_area_implement_client::recv_shop_message_init                 01(actually 0xE7BB + 0x5)
    ///                 ret                                                                 02(actually 0xE7BB + 0x14)
    ///                 proto_area_implement_client::recv_wanted_jail_open() draw_money     00(actually 0xE7BB)
    ///                 proto_area_implement_client::recv_union_rename_close_r              03(actually 0xE7BB + 0x2C)
    /// 
    /// 0xE07E + 0xCD - 0x004CB292 4 opcodes
    ///                 proto_area_implement_client::recv_map_update_bgm        03(actually 0xE07E + 0xCD)
    ///                 proto_area_implement_client::recv_minimap_notify        02(actually 0xE07E + 0x57)
    ///                 proto_area_implement_client::recv_event_quest_report    00(actually 0xE07E)
    ///                 proto_area_implement_client::recv_union_mantle_open     01(actually 0xE07E + 0x28)
    /// 
    /// 0xD68C + 0xC6 - 0x004C8EE5 5 opcodes
    ///         0xD690? proto_area_implement_client::recv_battle_attack_start_r             04(actually 0xD68C + 0xC6)
    ///         0xD68E? proto_area_implement_client::recv_item_update_place_change          02(actually 0xD68C + 0x1C)
    ///         0xD68C? proto_area_implement_client::recv_escape_start                      00(actually 0xD68C)
    ///         0xD68D? proto_area_implement_client::recv_gem_set_piece_r                   01(actually 0xD68C + 0x18)
    ///         0xD691? proto_area_implement_client::recv_wanted_jail_update_draw_point     03(actually 0xD68C + 0x64)
    /// 
    /// 0xD493 + 0xE7 - 0x004C7D1F 4 opcodes (1 ret)
    ///                 proto_area_implement_client::recv_skill_cast_cancel                 02(actually 0xD493 + 0xCA)
    ///                 ret                                                                 03(actually 0xD493 + 0xE7)
    ///                 proto_area_implement_client::recv_shop_sell_r                       01(actually 0xD493 + 0x65)
    ///                 proto_area_implement_client::recv_battle_report_action_item_use()\n 00(actually 0xD493)
    /// 
    /// 0xD5B5 + 0xC8 - 0x004C86F2 4 opcodes
    ///                 proto_area_implement_client::recv_charabody_notify_party_join           03(actually 0xD68C + 0xC8)
    ///                 proto_area_implement_client::recv_chara_update_lv_detail2               00(actually 0xD68C)
    ///                 proto_area_implement_client::recv_soul_dispitem_request_data_r          01(actually 0xD68C + 0x48)
    ///                 proto_area_implement_client::recv_battle_report_noact_notify_heal_ac    02(actually 0xD68C + 0x52)
    /// 
    /// 0xD400 + 0x3F - 0x004C7645 4 opcodes
    ///                 proto_area_implement_client::recv_self_soul_toggle_ability_notify
    ///                 proto_area_implement_client::recv_event_removetrap_skill_r2
    ///                 proto_area_implement_client::recv_battle_report_action_eq_break
    ///                 proto_area_implement_client::recv_storage_draw_item2_r
    /// 
    /// 0xCB94 + 0xA3 - 0x004C5085 5 opcodes
    ///                 proto_area_implement_client::recv_charabody_notify_deadstate
    ///                 proto_area_implement_client::recv_chara_update_ability
    ///                 proto_area_implement_client::recv_event_message_no_object
    ///                 proto_area_implement_client::recv_trade_notify_interface_status
    ///                 proto_area_implement_client::recv_message_board_notify_close
    /// 
    /// 0xCF29 + 0x29 - 0x004C5F8E 4 opcodes (1 ret)
    ///                 proto_area_implement_client::recv_echo_r
    ///                 proto_area_implement_client::recv_object_region_disappear_update_notify
    ///                 ret
    ///                 proto_area_implement_client::recv_dbg_battle_charge_start_notify
    /// 
    /// 0xD04A + 0xE9 - 0x004C6663 4 opcodes
    ///                 no string
    ///                 proto_area_implement_client::recv_chara_update_hp
    ///                 proto_area_implement_client::recv_chara_update_atk_magic_attr
    ///                 proto_area_implement_client::recv_escape_exec
    /// 
    /// 0xA7E8 + 0xD3 - 0x004BB352 5 opcodes (1 ret)
    ///                 proto_area_implement_client::recv_chara_notify_map_fragment
    ///                 proto_area_implement_client::recv_chara_update_maxweight
    ///                 proto_area_implement_client::recv_charabody_salvage_notify_salvager
    ///                 proto_area_implement_client::recv_dbg_battle_charge_end_notify
    ///                 ret
    /// 
    /// 0xC0D8 + 0xD7 - 0x004C21A0 4 opcodes
    ///                 proto_area_implement_client::recv_eo_notify_disappear_schedule
    ///                 proto_area_implement_client::recv_quest_chapter_updated
    ///                 proto_area_implement_client::recv_help_new_remove_r
    ///                 proto_area_implement_client::recv_wanted_list_close_r
    /// 
    /// 0xC026 + 0x46 - 0x004C2984 4 opcodes
    ///                 proto_area_implement_client::recv_charabody_self_raisescale_end
    ///                 proto_area_implement_client::recv_shop_sell_surrogate_r
    ///                 proto_area_implement_client::recv_gem_notify_open
    ///                 proto_area_implement_client::recv_battle_report_notify_action_bonus
    /// 
    /// 0xC003 + 0x75 - 0x004C1A1E 4 opcodes
    ///                 ret or other
    ///                 proto_area_implement_client::recv_chara_update_def_magic_attr
    ///                 ret or other
    ///                 proto_area_implement_client::recv_chat_notify_message
    /// 
    /// 0xBA71 + 0xF4 - 0x004C02F0 4 opcodes
    ///                 0xBA71 proto_area_implement_client::recv_auction_notify_open
    ///                 0xBA73 proto_area_implement_client::recv_cpf_authenticate
    ///                 0xBA89 proto_area_implement_client::recv_gamepot_web_notify_open
    ///                 0xBB65 (0xBA71 + 0xF4) proto_area_implement_client::recv_event_union_storage_close_r
    /// 
    /// 0xAF7F + 0xB8 - 0x004BD4CC 5 opcodes
    ///                 proto_area_implement_client::recv_chara_update_notify_crime_lv
    ///                 proto_area_implement_client::recv_quest_ended
    ///                     (proto_area_implement_client::send_quest_get_mission_quest_history,
    ///                     proto_area_implement_client::send_quest_get_story_quest_history,
    ///                     proto_area_implement_client::send_quest_get_soul_mission_quest_history happen after the above recv)
    ///                 proto_area_implement_client::recv_battle_report_notify_hit_effect_name
    ///                 proto_area_implement_client::recv_comment_switch_r 0xB01B
    ///                 proto_area_implement_client::recv_wanted_update_state_actor_notify
    /// 
    /// 0xB1CA + 0xC8 - 0x004BE4A3 4 opcodes
    ///                 proto_area_implement_client::recv_event_removetrap_begin
    ///                 proto_area_implement_client::recv_buff_shop_notify_item
    ///                 proto_area_implement_client::recv_stall_set_name_r
    ///                 proto_area_implement_client::recv_auction_receive_item_r
    /// 
    /// 0xB371 + 0x86 - 0x004BEB5D 4 opcodes (not 0xB317)
    ///                 proto_area_implement_client::recv_charabody_notify_crime_lv
    ///                 proto_area_implement_client::recv_item_update_place
    ///                 proto_area_implement_client::recv_battle_report_action_attack_exec
    ///                 proto_area_implement_client::recv_auction_exhibit_r
    /// 
    /// 0xB0E5 + 0x2A - 0x004BDD95 4 opcodes
    ///                 proto_area_implement_client::recv_chara_update_lv
    ///                 proto_area_implement_client::recv_event_removetrap_ident_trap_update
    ///                 proto_area_implement_client::recv_party_cancel_member_recruit_r
    ///                 proto_area_implement_client::recv_union_request_disband_result
    /// 
    /// 0xA0E3 + 0xAA - 0x004B98BB 5 opcodes
    ///                 proto_area_implement_client::recv_self_money_notify
    ///                 proto_area_implement_client::recv_chara_notify_union_data
    ///                 proto_area_implement_client::recv_raisescale_request_revive_r
    ///                 proto_area_implement_client::recv_cash_shop_get_url_common_steam_r
    ///                 proto_area_implement_client::recv_auction_cancel_bid_r
    /// 
    /// 0xA508 + 0x41 - 0x004BA68E 4 opcodes
    ///                 proto_area_implement_client::recv_skill_request_base_from_item_r
    ///                 proto_area_implement_client::recv_skill_custom_notify_close
    ///                 proto_area_implement_client::recv_raisescale_update_success_per
    ///                 proto_area_implement_client::recv_auction_re_exhibit_r
    /// 
    /// 0xA611 + 0xE7 - 0x004BAC6C 4 opcodes
    ///                 proto_area_implement_client::recv_item_update_physics
    ///                 proto_area_implement_client::recv_event_removetrap_close
    ///                 proto_area_implement_client::recv_skill_cooltime_notify
    ///                 proto_area_implement_client::recv_blacklist_update
    /// 
    /// 0x9F70 + 0x95 - 0x004B9266 4 opcodes
    ///                 proto_area_implement_client::recv_chara_notify_party_leave
    ///                 proto_area_implement_client::recv_battle_attack_pose_self
    ///                 proto_area_implement_client::recv_cash_shop_get_current_cash_r
    ///                 proto_area_implement_client::recv_party_notify_recruit_request
    ///                 (proto_area_implement_client::send_party_regist_party_recruit follows the last one instantly, results in a disconnect)
    /// 
    /// 0x8CC6 + 0xCC - 0x004B483D has 5 opcodes (2 ret?)
    ///                 proto_area_implement_client::recv_charabody_notify_loot_item
    ///                 ret or other
    ///                 ret
    ///                 proto_area_implement_client::recv_escape_cancel
    ///                 proto_area_implement_client::recv_thread_entry_message
    /// 
    /// 0x8066 + 0x2B - 0x004B1728 has 5 opcodes (1 ret?)
    ///                 proto_area_implement_client::recv_data_notify_eo_data
    ///                 no string/ret?
    ///                 proto_area_implement_client::recv_chara_update_notify_comment
    ///                 proto_area_implement_client::recv_event_script_play
    ///                 proto_area_implement_client::recv_soulmaterial_shop_notify_item
    /// 
    /// 0x8487 + 0xC2 - 0x004B2631 has 4 opcodes (2 ret)
    ///                 proto_area_implement_client::recv_item_update_weight
    ///                 ret
    ///                 ret
    ///                 proto_area_implement_client::recv_temple_cure_curse_r
    /// 
    /// 0x85C6 + 0xDF - 0x004B2C99 4 opcodes
    ///                 proto_area_implement_client::recv_soulmaterial_shop_buy_r
    ///                 proto_area_implement_client::recv_stall_shopping_notify_aborted
    ///                 proto_area_implement_client::recv_party_entry_draw_r
    ///                 proto_area_implement_client::recv_temple_notify_open
    /// 
    /// 0x8299 + 0xFC - 0x004B1EE6 4 opcodes (1 ret)
    ///                 proto_area_implement_client::recv_skill_tree_gain                   03(actually 0x8299 + 0xFC)
    ///                 proto_area_implement_client::recv_premium_service_notify_attach2    00(actually 0x8299)
    ///                 proto_area_implement_client::recv_buff_shop_buy_r                   01(actually 0x8299 + 0x8C)
    ///                 ret                                                                 02(actually 0x8299 + 0xCB)
    /// 
    /// 0x7D1C +  0xF - 0x004B09BA 4 opcodes
    ///                 proto_area_implement_client::recv_data_notify_maplink
    ///                 proto_area_implement_client::recv_event_union_storage_update_money
    ///                 proto_area_implement_client::recv_charabody_salvage_notify_body
    ///                 proto_area_implement_client::recv_auction_notify_close
    /// 
    /// 0x73D1 + 0x9E - 0x004AEEA5 4 opcodes
    ///                 ret
    ///                 proto_area_implement_client::recv_self_buff_notify
    ///                 proto_area_implement_client::recv_wanted_jail_close_r
    ///                 proto_area_implement_client::recv_cpf_notify_error
    /// 
    /// 0x65A6 + 0x48 - 0x004AB883 4 opcodes
    ///                 proto_area_implement_client::recv_chara_update_maxac
    ///                 proto_area_implement_client::recv_gem_cancel_synthesis_r
    ///                 proto_area_implement_client::recv_battle_report_notify_raise
    ///                 proto_area_implement_client::recv_battle_report_noact_notify_buff_move
    /// 
    /// 0x5D52 + 0xF6 - 0x004A9C48 4 opcodes
    ///                 proto_area_implement_client::recv_chara_target_move_side_speed_per
    ///                 proto_area_implement_client::recv_battle_attack_pose_r
    ///                 proto_area_implement_client::recv_battle_guard_end_self
    ///                 proto_area_implement_client::recv_forge_sp_check_r
    /// 
    /// 0x5243 + 0xC4 - 0x004A7ED1 4 opocdes
    ///                 proto_area_implement_client::recv_object_ac_rank_update_notify
    ///                 proto_area_implement_client::recv_shop_notify_open
    ///                 proto_area_implement_client::recv_trade_add_item_r
    ///                 proto_area_implement_client::recv_auction_update_exhibit_item_state
    /// 
    /// 0x4E17 + 0x76 - 0x004A732D 4 opcodes
    ///                 proto_area_implement_client::recv_shop_close_r
    ///                 proto_area_implement_client::recv_shop_sell_check_r
    ///                 proto_area_implement_client::recv_gem_synthesis_r
    ///                 proto_area_implement_client::recv_party_regist_party_recruit_r
    /// 
    /// 0x1F73 + 0xA9 - 0x0049CF7A 5 opcodes (1 no result?)
    ///                 proto_area_implement_client::recv_data_notify_soulmaterialobject_data
    ///                 not sure what string this is/no result
    ///                 proto_area_implement_client::recv_gem_cancel_support_item_r
    ///                 proto_area_implement_client::recv_shortcut_notify_deregist
    ///                 proto_area_implement_client::recv_job_change_notify_open
    /// 
    /// 0x3B9F + 0xA2 - 0x004A40E1 4 opcodes (1 ret)
    ///                 proto_area_implement_client::recv_self_skill_point_notify
    ///                 proto_area_implement_client::recv_item_update_eqmask
    ///                 proto_area_implement_client::recv_help_new_data
    ///                 ret
    /// 
    /// 0x3247 + 0xA6 - 0x004A29D7 4 opcodes
    ///                 proto_area_implement_client::recv_item_update_state
    ///                 proto_area_implement_client::recv_event_change_type
    ///                 proto_area_implement_client::recv_gem_set_r
    ///                 proto_area_implement_client::recv_party_accept_to_invite_r
    /// 
    /// 0x28E7 + 0xDE - 0x004A064B 4 opcodes (1 ret)
    ///                 proto_area_implement_client::recv_eo_update_state
    ///                 proto_area_implement_client::recv_chara_update_con
    ///                 proto_area_implement_client::recv_shop_repair_r
    ///                 ret
    /// 
    /// 0x2478 + 0x96 - 0x0049EE79 4 opcodes
    ///                 proto_area_implement_client::recv_data_notify_ggate_stone_data      03(actually 0x2478 + 0x96)
    ///                 proto_area_implement_client::recv_chara_update_maxap                00(actually 0x2478)
    ///                 proto_area_implement_client::recv_chara_update_form                 01(actually 0x2478 + 0x1)
    ///                 proto_area_implement_client::recv_create_send_pacakge_info          02(actually 0x2478 + 0x19)
    /// 
    /// 0x18CC + 0xF7 - 0x0049B302 5 opcodes
    ///                 proto_area_implement_client::recv_stall_deregist_item_r
    ///                 proto_area_implement_client::recv_gem_notify_close
    ///                 proto_area_implement_client::recv_battle_report_action_monster_skill_start_cast
    ///                 proto_area_implement_client::recv_battle_report_noact_notify_buff_detach
    ///                 proto_area_implement_client::recv_battle_report_noact_notify_buff_update_time
    /// 
    /// 0x1E65 + 0xCD - 0x0049C989 4 opcodes
    ///                 proto_area_implement_client::recv_battle_guard_start_self
    ///                 proto_area_implement_client::recv_souleater_touch_notify
    ///                 proto_area_implement_client::recv_charabody_self_warpdragon_penalty
    ///                 proto_area_implement_client::recv_blacklist_clear_r
    /// 
    /// 0x8CD  + 0xD0 - 0x004979DC 5 opcodes (1 ret)
    ///                 proto_area_implement_client::recv_self_action_cost
    ///                 proto_area_implement_client::recv_item_update_magic
    ///                 proto_area_implement_client::recv_event_end
    ///                 ret
    ///                 proto_area_implement_client::recv_battle_report_notify_damage_hp
    /// 
    /// 0x102E + 0x97 - 0x00498F03 4 opcodes
    ///                 proto_area_implement_client::recv_object_region_break_update_notify
    ///                 proto_area_implement_client::recv_dropobject_notify_access_priority
    ///                 proto_area_implement_client::recv_gimmick_state_update
    ///                 proto_area_implement_client::recv_trade_notify_replied
    /// 
    /// 0x54E  + 0xAA - 0x00496B4F 5 opcodes
    ///                 proto_area_implement_client::recv_npc_flageffect_update_notify
    ///                 proto_area_implement_client::recv_charabody_notify_party_leave
    ///                 proto_area_implement_client::recv_item_update_num
    ///                 proto_area_implement_client::recv_event_system_message
    ///                 proto_area_implement_client::recv_charabody_loot_complete2_r
    /// 
    /// 0x397  + 0x7A - 0x004964AA 4 opcodes
    ///                 proto_area_implement_client::recv_skill_custom_close_r
    ///                 proto_area_implement_client::recv_premium_service_notify_attach
    ///                 proto_area_implement_client::recv_stall_close_r
    ///                 proto_area_implement_client::recv_temple_close_r
    /// 
    /// 0xC2A1 + 0xD3 - 0x004C2F8E 5 opcodes 
    ///                 proto_area_implement_client::recv_self_returnhome_interval
    ///                 proto_area_implement_client::recv_quest_check_time_limit_r
    ///                 proto_area_implement_client::recv_raisescale_view_open
    ///                 proto_area_implement_client::recv_battle_report_noact_notify_knockback
    ///                 proto_area_implement_client::recv_random_box_notify_open
    /// </summary>
    public enum AreaPacketId : ushort
    {
        // Recv OP Codes - Switch: 0x495B88 - ordered by op code
        recv_map_change_force = 0x4C74,
        recv_data_get_self_chara_data_request_r = 0x3C89,
        recv_base_enter_r = 0x3806,
        recv_data_get_self_chara_data_r = 0xD1BD,
        revc_data_notify_chara_data = 0xED4C,
        recv_map_get_info_r = 0xEDA6,
        recv_base_check_version_r = 0xEFDD,
        recv_map_entry_r = 0xF9F9,
        recv_shortcut_request_data_r = 0xA084,
        recv_sv_conf_option_request_r = 0x1DA,
        recv_map_enter_r = 0x793E,
        recv_get_refusallist_r = 0x488E,
        recv_quest_get_mission_quest_works_r = 0x1BD6,
        recv_quest_get_story_quest_works_r = 0x73A1,
        recv_quest_get_soul_mission_quest_works_r = 0x266C,
        recv_quest_display_r = 0x42B8,
        recv_sv_conf_option_change = 0xAEE9,
        recv_data_notify_charabody_data = 0x906A,
        recv_map_change_sync_ok = 0x9AA9,
        recv_logout_start = 0x58E7,



        recv_battle_attack_pose = 0x0, //todo
        recv_battle_release_attack_pose = 0x0, //todo
        recv_battle_attack_start = 0xD752, //1 other possible inside 0xD68C + 0xC6 - 0x004C8EE5
        recv_battle_attack_exec = 0x0, // 0x998F - recv_battle_attack_exec_direct_r and then 0xF1A0 - recv_battle_attack_exec_r?
        recv_skill_request_gain_r = 0x903A,
        recv_quest_get_mission_quest_history = 0x0,//missing recv?
        recv_quest_get_story_quest_history = 0x0,//missing recv?
        recv_quest_get_soul_mission_quest_history = 0x0,//missing recv?
        recv_cash_shop_open_by_menu = 0x0,//missing recv
        recv_stall_deregist_item_r= 0x0,//todo inside 0x18CC + 0xF7 - 0x0049B302 
        recv_stall_set_name_r = 0x0,//todo inside 0xB1CA + 0xC8 - 0x004BE4A3
        recv_logout_start_request_r = 0x4C8B,
        recv_logout_cancel_request_r = 0x267D,
        recv_data_notify_npc_data = 0x2470,
        recv_npc_state_update_notify = 0x3B77,
        recv_event_access_object_r = 0x77A7,
        recv_event_message = 0x662F,
        recv_event_block_message = 0x8BD2,
        recv_chat_post_message_r = 0xAF49,
        recv_chat_notify_message = 0xC003,
        recv_chara_pose_r = 0xF71C,
        recv_chara_pose_ladder_r = 0xD972,
        recv_comment_set_r = 0x9C3A,
        recv_comment_switch_r = 0xB025,
        recv_refusallist_add_user_r = 0x22E7,
        recv_blacklist_open_r = 0xDF31,
        recv_emotion_update_type_r = 0x239D,
        recv_blacklist_clear_r = 0x1E6B,
        recv_blacklist_lock_r = 0x30FB,
        recv_blacklist_unlock_r = 0x2BAB,
        recv_wanted_entry_r = 0xDDD3,
        recv_cmd_exec_r = 0x6F32,
        recv_charabody_access_start_r = 0xEFED,
        recv_skill_tree_gain = 0x8395,
        recv_shortcut_notify_regist = 0x8BB4,
        recv_battle_report_action_skill_exec = 0x604,
        recv_skill_exec_r = 0x1C6C,


        // Send OP Codes - ordered by op code
        send_base_check_version = 0x5705,
        send_base_enter = 0xAE43,
        send_data_get_self_chara_data_request = 0x74DD,
        send_skill_request_info = 0x4EB5,
        send_sv_conf_option_request = 0x615E,
        send_get_refusallist = 0x6C17,
        send_party_request_draw_item_list = 0x86FD,
        send_shortcut_request_data = 0x6FC6,
        send_quest_get_mission_quest_works = 0x7C9A,
        send_quest_get_story_quest_works = 0x2A95,
        send_quest_get_soul_mission_quest_works = 0xB090,
        send_map_entry = 0x2DE3,
        send_map_get_info = 0x25D7,
        send_map_enter = 0x70FD,
        send_soul_dispitem_request_data = 0xEC5A,
        send_sv_conf_option_change = 0x1B99,
        send_map_change_force_r = 0x4CB0,
        send_battle_attack_pose = 0xC137,
        send_battle_release_attack_pose = 0x26BE,
        send_battle_attack_start = 0x6A72,
        send_battle_attack_exec = 0xC38D,
        send_skill_request_gain = 0x6507,
        send_quest_get_mission_quest_history = 0xA3E6,
        send_quest_get_story_quest_history = 0xCB91,
        send_quest_get_soul_mission_quest_history = 0x9E5C,
        send_cash_shop_open_by_menu = 0x9945,
        send_stall_deregist_item = 0xFC7D,
        send_stall_set_name = 0xB93,
        send_logout_start_request = 0x38FC,
        send_logout_cancel_request = 0xB224,
        send_event_access_object = 0x718D,
        send_chat_post_message = 0x1132,
        send_chara_pose = 0x7DE7,
        send_chara_pose_ladder = 0xE75C,
        send_comment_set = 0x9A13,
        send_comment_switch = 0xECF5,
        send_refusallist_add_user = 0xF1D5,
        send_blacklist_open = 0xC923 ,
        send_emotion_update_type = 0x7672,
        send_blacklist_close = 0xC780,
        send_blacklist_clear = 0xB742,
        send_blacklist_lock = 0xD189,
        send_blacklist_unlock = 0x23AD,
        send_wanted_entry = 0xA86F,
        send_cmd_exec = 0x2E64,
        send_party_search_recruited_party = 0x90BD,
        send_charabody_access_start = 0x6D2F,
        send_shortcut_request_regist = 0x8090,
        send_skill_start_cast = 0xF7B9,


    }
}