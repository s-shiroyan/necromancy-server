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
    ///
    /// 0x82 - (0x19C) = 0x19C
    /// 0x82 - (0x19C + 0x2A) = 0x1C6
    /// 0x82 - (0x19C + 0x2A + 0x3) = 0x1C9
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
    /// 0xC680
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
    /// 0xF2C3
    /// 0xF5BF
    /// 0xF7F0
    ///
    /// 0xF94C
    /// 0xFC36
    ///
    /// 0xE4DE
    /// 0xEB6E
    /// 0xEEB6
    /// 
    /// 0xD0AC
    /// 0xD0AC - (0xC8D0) = 0xC8D0
    /// 0xD0AC - (0xC8D0 + 0x2) = 0xC8D2
    /// 0xD0AC - (0xC8D0 + 0x2 + 0xBF) = 0xC991
    ///
    /// 0xD1A8
    /// 0xD2D6
    ///
    /// 0xC1E6
    /// 0xC561
    /// 0xC5F1
    ///
    /// 0xA535
    /// 0x9D6A
    /// 0x9D6A - (9AD4) = 0x9AD4
    /// 0x9D6A - (9AD4 + 12A) = 0x9BFE
    /// 0x9D6A - (9AD4 + 12A + 7F) = 0x9C7D
    /// 
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
            int opCode = buffer.ReadInt16();

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