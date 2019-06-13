using Arrowgene.Services.Buffers;
using Arrowgene.Services.Networking.Tcp;

namespace Necromancy.Server
{
    /// <summary>
    /// Necromancy Authentication Server
    /// 
    /// OP Codes: (Authentication Server Switch: 0x004DE650)
    /// 848c proto_auth_implement_client::recv_base_select_world_r()
    /// 174a proto_auth_implement_client::recv_base_authenticate_soe_r()
    /// 0a53 proto_auth_implement_client::recv_base_authenticate_hangame_r
    /// 15c7 proto_auth_implement_client::recv_base_authenticate_r
    /// d2d6 proto_auth_implement_client::recv_base_ping_r
    /// b717 proto_auth_implement_client::recv_base_get_worldlist_r
    /// ba73 proto_auth_implement_client::recv_cpf_authenticate
    /// ec0a proto_auth_implement_client::recv_base_check_version2_r
    /// efdd proto_auth_implement_client::recv_base_check_version_r
    /// 73d7 proto_auth_implement_client::recv_cpf_notify_error
    /// 7eea proto_auth_implement_client::recv_base_authenticate_soe_sessionid
    /// </summary>
    public class AuthenticationServer : NecromancyServer
    {
        public override void OnReceivedData(ITcpSocket socket, byte[] data)
        {
            IBuffer buffer = new StreamBuffer(data);
            PacketLogIn(buffer);
            buffer.SetPositionStart();

            ushort size = buffer.ReadUInt16(Endianness.Big);
            ushort opCode = buffer.ReadUInt16();

            switch (opCode)
            {
                case 0x5705: //network::proto_auth_implement_client::send_base_check_version
                {
                    uint major = buffer.ReadUInt32();
                    uint minor = buffer.ReadUInt32();
                    _logger.Info($"{major} - {minor}");
                    IBuffer res = new StreamBuffer();
                    res.WriteInt32(0);
                    res.WriteInt32(major);
                    res.WriteInt32(minor);
                    Send(socket, 0xEFDD, res); //network::proto_auth_implement_client::recv_base_check_version_r
                    break;
                }
                case 0xAD93: //network::proto_auth_implement_client::send_base_authenticate
                {
                    string accountName = buffer.ReadCString();
                    string password = buffer.ReadCString();
                    string macAddress = buffer.ReadCString();
                    int unknown = buffer.ReadInt16();
                    _logger.Info($"[Login]Account:{accountName} Password:{password} Unknown:{unknown}");
                    IBuffer res = new StreamBuffer();
                    res.WriteInt32(0);
                    res.WriteInt32(1);
                    Send(socket, 0x15C7, res); //proto_auth_implement_client::recv_base_authenticate_r
                    break;
                }
                case 0x53CF: //network::proto_auth_implement_client::send_base_get_worldlist
                {
                    int numEntries = 4;
                    IBuffer res = new StreamBuffer();
                    res.WriteInt32(numEntries);
                    for (int i = 1; i <= numEntries; i++)
                    {
                        res.WriteInt32(i);
                        res.WriteFixedString($"World {i}", 37);
                        res.WriteInt32(0);
                        res.WriteInt16(0); //Max Player
                        res.WriteInt16(0); //Current Player
                    }

                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    Send(socket, 0xB717, res); //proto_auth_implement_client::recv_base_get_worldlist_r
                    break;
                }
                case 0x203F: //network::proto_auth_implement_client::send_base_select_world
                {
                    IBuffer res = new StreamBuffer();
                    res.WriteInt32(0);
                    res.WriteCString("127.0.0.1"); //Message Server IP
                    res.WriteInt32(60001); //Message Server Port
                    Send(socket, 0x848C, res); //network::proto_auth_implement_client::recv_base_select_world_r
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