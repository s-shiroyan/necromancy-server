using Arrowgene.Services.Buffers;
using Arrowgene.Services.Networking.Tcp;

namespace Necromancy.Server
{
    /// <summary>
    /// Necromancy Message Server
    /// 
    /// OP Codes: (Authentication Server Switch: 0x004E4210)
    /// 
    /// 0x831C
    /// 0x4090 proto_msg_implement_client::recv_party_notify_update_map 00 13 90 40 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00
    /// 0x1AF3 proto_msg_implement_client::recv_party_notify_update_map 00 0A F3 1A 00 00 00 00 00 00 00 00
    /// 0x87D
    /// 0x482
    /// 0x482 - (0x6FF) = 0x6FF
    /// 0x482 - (0x6FF + 0x64) = 
    /// 0x482 - (0x6FF + 0x64 + 0x370) = 
    ///
    /// 0x482 - (0x19C) = 0x19C
    /// 0x482 - (0x19C + 0x2A) = 0x1C6
    /// 0x482 - (0x19C + 0x2A + 0x3) = 0x1C9
    ///
    /// 0xBA73 proto_msg_implement_client::recv_cpf_authentication 00 06 73 BA 00 00 00 00
    /// 0x9AD1 proto_msg_implement_client::recv_chara_delete_r 00 06 D1 9A 00 00 00 00
    /// 0x8E0A proto_msg_implement_client::recv_party_notify_kick (no structure)
    /// 0x8D24
    /// 0x8D24 - (0x853F) = 0x853F
    /// 0x8D24 - (0x853F + 0x637) = 0x8B76
    /// 0x8D24 - (0x853F + 0x637 + 0x3B) = 0x8BB1
    ///
    /// 0xD6AD
    /// 0xC680 = proto_msg_implement_client::recv_chara_create_r 00 0A 80 C6 00 00 00 00 00 00 00 00
    /// 0xC003
    /// 0xBA8B
    /// 0xBD6B
    /// 0xBE62
    ///
    /// 0xEFDD
    /// 0xE2BE
    /// 0xD6ED
    /// 0xD6ED - (0x2EA) = 0x2EA
    /// 0xD6ED - (0x2EA + 0x48) = 0x332
    ///
    /// 0xF8B4 = network::proto_msg_implement_client::recv_system_notify_announce  00 04 B4 F8 00 00
    /// 0xF2C3 = proto_msg_implement_client::recv_party_notify_update_soulrank 00 07 C3 F2 00 00 00 00 00
    /// 0xF5BF = proto_msg_implement_client::recv_union_request_member_priv_r 00 06 BF F5 00 00 00 00
    /// 0xF7F0 = proto_msg_implement_client::recv_party_notify_get_item 00 08 F0 F7 00 00 00 00 00 00
    ///
    /// 0xF94C
    /// 0xFC36 = proto_msg_implement_client::recv_party_notify_update_dragon 00 07 36 FC 00 00 00 00 00
    ///
    /// 0xE4DE = proto_msg_implement_client::recv_union_request_expel_member_r 00 06 DE E4 00 00 00 00
    /// 0xEB6E = proto_msg_implement_client::recv_system_register_error_report_r 00 06 6E EB 00 00 00 00
    /// 0xEEB6 = proto_msg_implement_client::recv_party_notify_update_hp 00 0A B6 EE 00 00 00 00 00 00 00 00
    /// 
    /// 0xD0AC
    /// 0xD0AC - (0xC8D0) = 0xC8D0
    /// 0xD0AC - (0xC8D0 + 0x2) = 0xC8D2
    /// 0xD0AC - (0xC8D0 + 0x2 + 0xBF) = 0xC991
    ///
    /// 0xD1A8 = proto_msg_implement_client::recv_party_notify_change_mode 00 06 A8 D1 00 00 00 00
    /// 0xD2D6
    ///
    /// 0xC1E6 = proto_msg_implement_client::recv_easy_friend_notify_delete_member 00 06 E6 C1 00 00 00 00
    /// 0xC561 = proto_msg_implement_client::recv_soul_select_r 00 07 61 C5 00 00 00 00 00
    /// 0xC5F1
    ///
    /// 0xA535
    /// 0xAAB5
    /// 0xAF33
    /// 0xB2B7
    /// 0xA57C
    /// 0xA68E
    /// 0xA878
    /// 0x9D6A
    /// 0x9D6A - (0x9AD4) = 0x9AD4
    /// 0x9D6A - (0x9AD4 + 0x12A) = 0x9BFE
    /// 0x9D6A - (0x9AD4 + 0x12A + 0x7F) = 0x9C7D
    ///
    /// 0x9D6A - (0x9DE2) = 0x9DE2
    /// 0x9D6A - (0x9DE2 + 0x67) = 
    /// 0x9D6A - (0x9DE2 + 0x67 + 0x29F) = 
    ///
    /// 0x95E6
    /// 0x8EEE
    /// 0x8FB2
    /// 0x91EE
    /// 
    /// 0x95FE
    /// 0x96DB
    /// 0x9A9B
    ///
    /// 0x8D24 - (0x8D3A) = 0x8D3A
    /// 0x8D24 - (0x8D3A + 0x18) = 
    /// 0x8D24 - (0x8D3A + 0x18 + 0x22) = 
    ///
    /// 0x64B5
    /// 0x55FF
    /// 0x5315
    /// 0x4426
    /// 0x4CD5
    /// 0x51EB
    ///
    /// 0x7566
    /// 0x6C94
    /// 0x689B
    /// 0x6AC0
    ///
    /// 0x7DB1
    /// 0x7DB1 - (0x7A60) = 0x7A60
    /// 0x7DB1 - (0x7A60 + 0x69) = 
    /// 0x7DB1 - (0x7A60 + 0x69 + 0x177) =
    ///
    /// 0x7E3A
    /// 0x7F64
    ///
    /// 0x7205
    /// 0x73D7
    /// 0x755C
    ///
    /// 0x5B94
    /// 0x5625
    /// 0x58F8
    /// 0x5B32
    ///
    /// 0x5D93
    /// 0x5F0A
    /// 0x5F9A
    ///
    /// 0x5386
    /// 0x542B
    /// 0x557F
    ///
    /// 0x2E98
    /// 0x1EC8
    /// 0x1D09
    /// 0x1DC4
    /// 0x1E68
    ///
    /// 0x3784
    /// 0x3784 - (0x31B4) = 0x31B4
    /// 0x3784 - (0x31B4 + 0x146) = 
    /// 0x3784 - (0x31B4 + 0x146 + 0x16) = 
    ///
    /// 0x392F
    /// 0x3B93
    ///
    /// 0x213A
    /// 0x2C1C
    /// 0x2D92
    ///
    /// 0x1688
    /// 0x1688 (0x1817) = 0x1817
    /// 0x1688 (0x1817 + 0x14) = 
    /// 0x1688 (0x1817 + 0x14 + 0x10A) = 
    /// 0xC87
    /// 0xE51
    /// 0x1421
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
                    //buffer.Position = 7;
                    //string macAddress = buffer.ReadFixedString(17);
                    IBuffer res = new StreamBuffer();
                    res.WriteInt32(0);
                    
                    Send(socket, 0x9BFE, res);
                    //TODO find network::proto_msg_implement_client::recv_base_login_r
                    break;
                }
                default:
                {
                    _logger.Error($"OPCode: {opCode} not handled");
                    break;
                }
            }
        }
    }
}