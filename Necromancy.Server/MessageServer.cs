using Arrowgene.Services.Buffers;
using Arrowgene.Services.Networking.Tcp;

namespace Necromancy.Server
{
    /// <summary>
    /// Necromancy Message Server
    /// 
    /// Recv OP Codes: (Authentication Server Switch: 0x004E4210)
    ///
    /// 0x019C
    /// 0x01C6 (0x19C + 0x2A)
    /// 0x01C9 (0x19C + 0x2A + 0x3)
    /// 0x02EA
    /// 0x0332 (0x2EA + 0x48)
    /// 0x0482
    /// 0x06FF
    /// 0x0763 (0x6FF + 0x64)
    /// 0x087D
    /// 0x0AD3 (0x6FF + 0x64 + 0x370)
    /// 0x0C87
    /// 0x0E51
    /// 
    /// 0x1421
    /// 0x1688
    /// 0x1817 (0x1817)
    /// 0x182B (0x1817 + 0x14)
    /// 0x1935 network::proto_msg_implement_client::recv_union_request_news_r (0x1817 + 0x14 + 0x10A)
    /// 0x1AF3 proto_msg_implement_client::recv_party_notify_update_map 00 0A F3 1A 00 00 00 00 00 00 00 00
    /// 0x1D09
    /// 0x1DC4
    /// 0x1E68
    /// 0x1EC8
    /// 
    /// 0x213A proto_msg_implement_client::recv_friend_reply_to_link_r
    /// 0x2C1C
    /// 0x2D92 proto_msg_implement_client::recv_party_notify_update_maxhp
    /// 0x2E98
    /// 
    /// 0x31B4
    /// 0x32FA (0x31B4 + 0x146)
    /// 0x3310 (0x31B4 + 0x146 + 0x16)
    /// 0x3784
    /// 0x392F
    /// 0x3B93
    ///
    /// 0x4090 proto_msg_implement_client::recv_party_notify_update_map 00 13 90 40 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00
    /// 0x4426
    /// 0x4CD5 proto_msg_implement_client::recv_party_notify_accept_to_apply
    ///
    /// 0x51EB proto_msg_implement_client::recv_union_request_detail_r
    /// 0x5315
    /// 0x5386 proto_msg_implement_client::recv_refusallist_notify_remove_user_souldelete
    /// 0x542B proto_msg_implement_client::recv_party_notify_change_leader
    /// 0x557F proto_msg_implement_client::recv_union_request_set_mantle_r
    /// 0x55FF
    /// 0x5625
    /// 0x58F8
    /// 0x5B32 proto_msg_implement_client::recv_soul_delete_r
    /// 0x5B94
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
    /// 0x73D7
    /// 0x7566
    /// 0x755C
    /// 0x7A60
    /// 0x7AC9 (0x7A60 + 0x69)
    /// 0x7C40 (0x7A60 + 0x69 + 0x177)
    /// 0x7DB1
    /// 0x7E3A proto_msg_implement_client::recv_friend_notify_delete_member_souldelete
    /// 0x7F64 proto_msg_implement_client::recv_union_notify_expelled_member
    /// 
    /// 0x831C
    /// 0x853F
    /// 0x8B76 - (0x853F + 0x637)
    /// 0x8BB1 - (0x853F + 0x637 + 0x3B)
    /// 0x8D24
    /// 0x8D52 (0x8D3A + 0x18)
    /// 0x8D74 (0x8D3A + 0x18 + 0x22)
    /// 0x8D3A
    /// 0x8E0A proto_msg_implement_client::recv_party_notify_kick (no structure)
    /// 0x8EEE proto_msg_implement_client::recv_party_notify_decline_to_apply
    /// 0x8FB2 proto_msg_implement_client::recv_friend_notify_cancel_link
    ///
    /// 0x91EE
    /// 0x95E6
    /// 0x95FE proto_msg_implement_client::recv_party_notify_remove_member
    /// 0x96DB proto_msg_implement_client::recv_party_notify_update_maxap
    /// 0x9A9B
    /// 0x9AD1 proto_msg_implement_client::recv_chara_delete_r 00 06 D1 9A 00 00 00 00
    /// 0x9AD4
    /// 0x9BFE (0x9AD4 + 0x12A)
    /// 0x9C7D (0x9AD4 + 0x12A + 0x7F)
    /// 0x9D6A
    /// 0x9DE2
    /// 0x9E49 (0x9DE2 + 0x67)
    /// 
    /// 0xA0E8 (0x9DE2 + 0x67 + 0x29F)
    /// 0xA535
    /// 0xA57C
    /// 0xA68E
    /// 0xA878
    /// 0xAAB5
    /// 0xAF33
    ///
    /// 0xB2B7 proto_msg_implement_client::recv_soul_set_passwd_r
    /// 0xBA73 proto_msg_implement_client::recv_cpf_authentication 00 06 73 BA 00 00 00 00
    /// 0xBA8B
    /// 0xBD6B
    /// 0xBE62
    ///
    /// 0xC003
    /// 0xC1E6 proto_msg_implement_client::recv_easy_friend_notify_delete_member 00 06 E6 C1 00 00 00 00
    /// 0xC561 proto_msg_implement_client::recv_soul_select_r 00 07 61 C5 00 00 00 00 00
    /// 0xC5F1
    /// 0xC680 proto_msg_implement_client::recv_chara_create_r 00 0A 80 C6 00 00 00 00 00 00 00 00
    /// 0xC8D0
    /// 0xC8D2 - (0xC8D0 + 0x2)
    /// 0xC991 - (0xC8D0 + 0x2 + 0xBF)
    ///
    /// 0xD0AC
    /// 0xD1A8 proto_msg_implement_client::recv_party_notify_change_mode 00 06 A8 D1 00 00 00 00
    /// 0xD2D6
    /// 0xD6AD
    /// 0xD6ED proto_msg_implement_client::recv_party_notify_cancel_invitation 00 02 ED D6
    /// 
    /// 0xE2BE proto_msg_implement_client::recv_cash_update_r 00 0A BE E2 00 00 00 00 00 00 00 00
    /// 0xE4DE proto_msg_implement_client::recv_union_request_expel_member_r 00 06 DE E4 00 00 00 00
    /// 0xEB6E proto_msg_implement_client::recv_system_register_error_report_r 00 06 6E EB 00 00 00 00
    /// 0xEEB6 proto_msg_implement_client::recv_party_notify_update_hp 00 0A B6 EE 00 00 00 00 00 00 00 00
    /// 0xEFDD
    /// 
    /// 0xF2C3 proto_msg_implement_client::recv_party_notify_update_soulrank 00 07 C3 F2 00 00 00 00 00
    /// 0xF5BF proto_msg_implement_client::recv_union_request_member_priv_r 00 06 BF F5 00 00 00 00
    /// 0xF7F0 proto_msg_implement_client::recv_party_notify_get_item 00 08 F0 F7 00 00 00 00 00 00
    /// 0xF8B4 network::proto_msg_implement_client::recv_system_notify_announce  00 04 B4 F8 00 00
    /// 0xF94C proto_msg_implement_client::recv_union_result_reply_invitat2
    /// 0xFC36 proto_msg_implement_client::recv_party_notify_update_dragon 00 07 36 FC 00 00 00 00 00
    ///
    /// Send OP:
    /// 0xF56C proto_msg_implement_client::send_chara_get_list
    /// 
    /// </summary>
    public class MessageServer : NecromancyServer
    {
        public override void OnReceivedData(ITcpSocket socket, byte[] data)
        {
            IBuffer buffer = new StreamBuffer(data);
            PacketLogIn(buffer);
            buffer.SetPositionStart();

            int size = buffer.ReadInt16(Endianness.Big);
            int opCode = buffer.ReadUInt16();

            switch (opCode)
            {
                case 0x5705: //network::proto_msg_implement_client::send_base_check_version
                {
                    uint unknown = buffer.ReadUInt32();
                    uint major = buffer.ReadUInt32();
                    uint minor = buffer.ReadUInt32();
                    _logger.Info($"{major} - {minor}");
                    IBuffer res = new StreamBuffer();

                    res.WriteInt32(0);
                    res.WriteInt32(unknown);
                    res.WriteInt32(major);
                    res.WriteInt32(minor);

                    Send(socket, 0xEFDD, res); //network::proto_msg_implement_client::recv_base_check_version_r
                    break;
                }
                case 0xA53D: //network::proto_msg_implement_client::send_base_login
                {
                    IBuffer res = new StreamBuffer();
                    res.WriteInt32(0);
                    res.WriteInt32(0);
                    Send(socket, 0x1935, res);
                    //TODO find network::proto_msg_implement_client::recv_base_login_r
                    break;
                }
                case 0xCE74: //network::proto_msg_implement_client::send_soul_create
                {
                    byte unknown = buffer.ReadByte();
                    string soulName = buffer.ReadCString();
                    _logger.Error($"Created SoulName: {soulName}");
                    break;
                }
                default:
                {
                    _logger.Error($"OPCode: {opCode} not handled");
                    break;
                }
            }
        }

        private void send__recv_chara_create_r(ITcpSocket socket)
        {
            IBuffer res = new StreamBuffer();
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);

            Send(socket, 0xC680, res); //proto_msg_implement_client::recv_chara_create_r
        }
    }
}