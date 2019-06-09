using Arrowgene.Services.Buffers;
using Arrowgene.Services.Networking.Tcp;

namespace Necromancy.Server
{
    public class AuthenticationServer : NecromancyServer
    {
        public override void OnReceivedData(ITcpSocket socket, byte[] data)
        {
            IBuffer buffer = new StreamBuffer(data);
            PacketLogIn(buffer);
            buffer.SetPositionStart();

            ushort size = buffer.ReadUInt16(Endianness.Big);
            ushort opCode = buffer.ReadUInt16(Endianness.Big);

            switch (opCode)
            {
                case 0x0557: //network::proto_auth_implement_client::send_base_check_version
                {
                    uint major = buffer.ReadUInt32();
                    uint minor = buffer.ReadUInt32();
                    _logger.Info($"{major} - {minor}");
                    IBuffer res = new StreamBuffer();
                    res.WriteInt32(0);
                    res.WriteInt32(major);
                    res.WriteInt32(minor);
                    Send(socket, 0xDDEF, res); //network::proto_auth_implement_client::recv_base_check_version_r
                    break;
                }
                case 0x93AD: //network::proto_auth_implement_client::send_base_authenticate
                {
                    string accountName = buffer.ReadCString();
                    string password = buffer.ReadCString();
                    string macAddress = buffer.ReadCString();
                    int unknown = buffer.ReadInt16();
                    _logger.Info($"Account:{accountName} Password:{password} Unknown:{unknown}");

                    // TODO find network::proto_auth_implement_client::recv_base_authenticate_r op code

                    // IBuffer res = new StreamBuffer();
                    // res.WriteInt32(0);

                    // Send(socket, 0x4A17, res);
                    // Send(socket, 0x530A, res);
                    // Send(socket, 0xC715, res);

                    // Send(socket, 0xD6D2, res);
                    // Send(socket, 0x17B7, res);
                    // Send(socket, 0x73BA, res);

                    // Send(socket, 0x0AEC, res);

                    // Send(socket, 0xD773, res);
                    // Send(socket, 0xEA7E, res);


                    // Authentication Server Switch - 0x004DE650
                    // OP Codes:
                    //848c proto_auth_implement_client::recv_base_select_world_r()
                    //174a
                    //0a53
                    //15c7
                    //d2d6
                    //b717
                    //ba73
                    //ec0a
                    //efdd proto_auth_implement_client::recv_base_check_version_r
                    //73d7
                    //7eea

                    // Send Move to world for testing currently. 
                    // Replace if we know correct flow
                    IBuffer res = new StreamBuffer();
                    res.WriteInt32(0);
                    res.WriteCString("127.0.0.1"); // Message Server IP
                    res.WriteInt32(60001); // Message Server Port
                    Send(socket, 0x8C84, res); //"network::proto_auth_implement_client::recv_base_select_world_r()"
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