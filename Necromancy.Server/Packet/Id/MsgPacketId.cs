namespace Necromancy.Server.Packet.Id
{
    /// <summary>
    /// Necromancy Message Server
    /// 
    /// Recv OP Codes: (Authentication Server Switch: 0x004E4210)
    ///
    /// 0x019C proto_msg_implement_client::recv_union_request_invite_target_r
    /// 0x01C6 proto_msg_implement_client::recv_union_request_set_info_r (0x19C + 0x2A)
    /// 0x01C9 proto_msg_implement_client::recv_chara_select_back_r (0x19C + 0x2A + 0x3)
    /// 0x0482 proto_msg_implement_client::recv_chara_select_r
    /// 0x06FF proto_msg_implement_client::recv_cash_buy_premium_r
    /// 0x0763 proto_msg_implement_client::recv_soul_create_r (0x6FF + 0x64)
    /// 0x0AD3 proto_msg_implement_client::recv_party_notify_update_job (0x6FF + 0x64 + 0x370)
    /// 0x0B7D proto_msg_implement_client::recv_friend_request_load_r
    /// 0x0C87 proto_msg_implement_client::recv_party_notify_update_pos
    /// 0x0E51 proto_msg_implement_client::recv_party_notify_update_deadstate
    /// 
    /// 0x1421 proto_msg_implement_client::recv_party_notify_update_ac
    /// 0x1688 proto_msg_implement_client::recv_easy_friend_notify_member_state
    /// 0x1817 proto_msg_implement_client::recv_friend_notify_add_member_r
    /// 0x182B proto_msg_implement_client::recv_party_notify_dead (0x1817 + 0x14) 
    /// 0x1935 proto_msg_implement_client::recv_union_request_news_r (0x1817 + 0x14 + 0x10A)
    /// 0x1AF3 proto_msg_implement_client::recv_party_notify_update_map 00 0A F3 1A 00 00 00 00 00 00 00 00
    /// 0x1D09 proto_msg_implement_client::recv_party_notify_update_body_pos 00 13 09 1D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00
    /// 0x1DC4 proto_msg_implement_client::recv_party_notify_get_money 00 0E C4 1D 00 00 00 00 00 00 00 00 00 00 00 00
    /// 0x1E68 proto_msg_implement_client::recv_union_request_secede_r 00 06 68 1E 00 00 00 00
    /// 0x1EC8 proto_msg_implement_client::recv_chara_get_inheritinfo_r 00 1F C8 1E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00
    /// 
    /// 0x213A proto_msg_implement_client::recv_friend_reply_to_link_r
    /// 0x2C1C proto_msg_implement_client::recv_party_notify_add_member
    /// 0x2D92 proto_msg_implement_client::recv_party_notify_update_maxhp
    /// 0x2E98 proto_msg_implement_client::recv_party_notify_attach_buff
    /// 
    /// 0x31B4 proto_msg_implement_client::recv_party_notify_update_premium_service_notify_flag 00 07 B4 31 00 00 00 00 00
    /// 0x32FA proto_msg_implement_client::recv_party_notify_cancel_application (0x31B4 + 0x146)  (no structure)
    /// 0x3310 proto_msg_implement_client::recv_chara_get_list_r (0x31B4 + 0x146 + 0x16)  00 0A 10 33 00 00 00 00 00 00 00 00
    /// 0x3784 proto_msg_implement_client::recv_chara_notify_data_complete 00 0F 84 37 00 00 00 00 00 00 00 00 00 00 00 00 00
    /// 0x392F proto_msg_implement_client::recv_union_reply_to_invite_r 00 0A 2F 39 00 00 00 00 00 00 00 00
    /// 0x3B93 proto_msg_implement_client::recv_soul_update_premium_flags 00 0A 93 3B 00 00 00 00 00 00 00 00
    ///
    /// 0x4090 proto_msg_implement_client::recv_party_notify_update_map 00 13 90 40 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00
    /// 0x4426 proto_msg_implement_client::recv_cash_get_url_commerce_r
    /// 0x4CD5 proto_msg_implement_client::recv_party_notify_accept_to_apply
    ///
    /// 0x51EB proto_msg_implement_client::recv_union_request_detail_r
    /// 0x5315 proto_msg_implement_client::recv_union_notify_joined_member
    /// 0x5386 proto_msg_implement_client::recv_refusallist_notify_remove_user_souldelete
    /// 0x542B proto_msg_implement_client::recv_party_notify_change_leader
    /// 0x557F proto_msg_implement_client::recv_union_request_set_mantle_r
    /// 0x55FF proto_msg_implement_client::recv_party_notify_update_ap
    /// 0x5652 proto_msg_implement_client::recv_soul_authenticate_passwd_r
    /// 0x58F8
    /// 0x5B32 proto_msg_implement_client::recv_soul_delete_r
    /// 0x5B94 proto_msg_implement_client::recv_union_notify_growth
    /// 0x5D93 proto_msg_implement_client::recv_party_notify_cancel_recruit
    /// 0x5F0A proto_msg_implement_client::recv_union_request_disband_r
    /// 0x5F9A proto_msg_implement_client::recv_party_notify_disband
    ///
    /// 0x64B5
    /// 0x689B proto_msg_implement_client::recv_cash_get_url_web_goods_r
    /// 0x6AC0 proto_msg_implement_client::recv_chara_draw_bonuspoint_r
    /// 0x6C94 proto_msg_implement_client::recv_chara_draw_bonuspoint_r
    ///
    /// 0x7205
    /// 0x73D7 proto_msg_implement_client::recv_cpf_notify_error (no structure)
    /// 0x7566
    /// 0x755C proto_msg_implement_client::recv_union_notify_info 00 07 5C 75 00 00 00 00 00
    /// 0x7A60
    /// 0x7AC9 (0x7A60 + 0x69)
    /// 0x7C40 (0x7A60 + 0x69 + 0x177)
    /// 0x7DB1
    /// 0x7E3A proto_msg_implement_client::recv_friend_notify_delete_member_souldelete
    /// 0x7F64 proto_msg_implement_client::recv_union_notify_expelled_member
    /// 
    /// 0x831C
    /// 0x853F proto_msg_implement_client::recv_union_request_change_role_r
    /// 0x8B76 (0x853F + 0x637)
    /// 0x8BB1 (0x853F + 0x637 + 0x3B)
    /// 0x8D24
    /// 0x8D52 (0x8D3A + 0x18)
    /// 0x8D74 (0x8D3A + 0x18 + 0x22)
    /// 0x8D3A
    /// 0x8E0A proto_msg_implement_client::recv_party_notify_kick (no structure)
    /// 0x8EEE proto_msg_implement_client::recv_party_notify_decline_to_apply
    /// 0x8FB2 proto_msg_implement_client::recv_friend_notify_cancel_link
    ///
    /// 0x91EE proto_msg_implement_client::recv_chara_select_back_soul_select_r 00 06 EE 91 00 00 00 00
    /// 0x95E6
    /// 0x95FE proto_msg_implement_client::recv_party_notify_remove_member
    /// 0x96DB proto_msg_implement_client::recv_party_notify_update_maxap
    /// 0x9A9B
    /// 0x9AD1 proto_msg_implement_client::recv_chara_delete_r 00 06 D1 9A 00 00 00 00
    /// 0x9AD4 proto_msg_implement_client::recv_party_notify_raise 00 06 D4 9A 00 00 00 00
    /// 0x9BFE proto_msg_implement_client::recv_friend_request_delete_friend_r (0x9AD4 + 0x12A) 
    /// 0x9C7D proto_msg_implement_client::recv_party_notify_update_level (0x9AD4 + 0x12A + 0x7F) 
    /// 0x9D6A proto_msg_implement_client::recv_chara_select_back_r
    /// 0x9DE2 proto_msg_implement_client::recv_chara_select_back_r
    /// 0x9E49 (0x9DE2 + 0x67)
    /// 
    /// 0xA0E8 (0x9DE2 + 0x67 + 0x29F)
    /// 0xA535
    /// 0xA57C
    /// 0xA68E proto_msg_implement_client::recv_base_login_r
    /// 0xA878 proto_msg_implement_client::recv_union_notify_mantle
    /// 0xAAB5 proto_msg_implement_client::recv_union_notify_invite
    /// 0xAF33 proto_msg_implement_client::recv_chara_get_createinfo_r
    ///
    /// 0xB2B7 proto_msg_implement_client::recv_soul_set_passwd_r
    /// 0xBA73 proto_msg_implement_client::recv_cpf_authentication 00 06 73 BA 00 00 00 00
    /// 0xBA8B
    /// 0xBD6B proto_msg_implement_client::recv_friend_request_link_target_r 00 0A 6B BD 00 00 00 00 00 00 00 00
    /// 0xBE62 proto_msg_implement_client::recv_cash_get_url_r 00 07 62 BE 00 00 00 00 00
    ///
    /// 0xC003 proto_msg_implement_client::recv_chat_notify_message
    /// 0xC1E6 proto_msg_implement_client::recv_easy_friend_notify_delete_member 00 06 E6 C1 00 00 00 00
    /// 0xC561 proto_msg_implement_client::recv_soul_select_r 00 07 61 C5 00 00 00 00 00
    /// 0xC5F1 proto_msg_implement_client::recv_union_notify_detail
    /// 0xC680 proto_msg_implement_client::recv_chara_create_r 00 0A 80 C6 00 00 00 00 00 00 00 00
    /// 0xC8D0
    /// 0xC8D2 proto_msg_implement_client::recv_chara_select_back_r (0xC8D0 + 0x2)
    /// 0xC991 - (0xC8D0 + 0x2 + 0xBF)
    ///
    /// 0xD0AC
    /// 0xD1A8 proto_msg_implement_client::recv_party_notify_change_mode 00 06 A8 D1 00 00 00 00
    /// 0xD2D6 proto_msg_implement_client::recv_cash_get_url_r
    /// 0xD6AD
    /// 0xD6ED proto_msg_implement_client::recv_party_notify_cancel_invitation 00 02 ED D6
    /// 0xD9D7 (0xD6ED + 0x2EA)
    /// 0xDA1F (0xD6ED + 0x2EA + 0x48)
    /// 
    /// 0xE2BE proto_msg_implement_client::recv_cash_update_r 00 0A BE E2 00 00 00 00 00 00 00 00
    /// 0xE4DE proto_msg_implement_client::recv_union_request_expel_member_r 00 06 DE E4 00 00 00 00
    /// 0xEB6E proto_msg_implement_client::recv_system_register_error_report_r 00 06 6E EB 00 00 00 00
    /// 0xEEB6 proto_msg_implement_client::recv_party_notify_update_hp 00 0A B6 EE 00 00 00 00 00 00 00 00
    /// 0xEFDD proto_msg_implement_client::recv_base_check_version_r
    /// 
    /// 0xF2C3 proto_msg_implement_client::recv_party_notify_update_soulrank 00 07 C3 F2 00 00 00 00 00
    /// 0xF5BF proto_msg_implement_client::recv_union_request_member_priv_r 00 06 BF F5 00 00 00 00
    /// 0xF7F0 proto_msg_implement_client::recv_party_notify_get_item 00 08 F0 F7 00 00 00 00 00 00
    /// 0xF8B4 proto_msg_implement_client::recv_system_notify_announce  00 04 B4 F8 00 00
    /// 0xF94C proto_msg_implement_client::recv_union_result_reply_invitat2
    /// 0xFC36 proto_msg_implement_client::recv_party_notify_update_dragon 00 07 36 FC 00 00 00 00 00
    ///
    /// Send OP:
    /// 0xF56C proto_msg_implement_client::send_chara_get_list
    /// 0x5705 proto_msg_implement_client::send_base_check_version
    /// 0xA53D proto_msg_implement_client::send_base_login
    /// 0xCE74 proto_msg_implement_client::send_soul_create
    /// 0x7E62 proto_msg_implement_client::send_chara_get_createinfo
    /// 0x733E proto_msg_implement_client::send_cash_get_url_common
    /// 0x5208 proto_msg_implement_client::send_soul_delete
    /// 0xB4BB proto_msg_implement_client::send_soul_authenticate_passwd
    /// 0x733E proto_msg_implement_client::send_cash_get_url_common
    /// 0x8C9D proto_msg_implement_client::send_soul_set_passwd
    /// </summary>
    public enum MsgPacketId : ushort
    {
        send_base_check_version = 0x5705,
        recv_base_check_version_r = 0xEFDD,
        send_base_login = 0xA53D,
        recv_base_login_r = 0xA68E,
        recv_soul_select_r = 0xC561,
        send_soul_select = 0xC44F,
        send_soul_create = 0xCE74,
        send_soul_authenticate_passwd = 0xB4BB,
        recv_soul_authenticate_passwd_r = 0x5652,
        send_chara_get_list = 0xF56C,
        recv_chara_get_list_r = 0x3310,
        send_cash_get_url_common = 0x733E,
        send_chara_get_createinfo = 0x7E62,
        recv_chara_get_createinfo_r = 0xAF33,
        send_soul_set_passwd = 0x8C9D,
        recv_soul_set_passwd_r = 0xB2B7,
        recv_soul_create_r = 0x0763,
        recv_chara_select_r = 0x0482
    }
}