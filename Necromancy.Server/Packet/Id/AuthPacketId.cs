namespace Necromancy.Server.Packet.Id
{
    public enum AuthPacketId : ushort
    {
        recv_base_select_world_r = 0x848C,
        recv_base_authenticate_soe_r = 0x174A,
        recv_base_authenticate_hangame_r = 0x0A53,
        recv_base_authenticate_r = 0x15C7,
        recv_base_ping_r = 0xD2D6,
        recv_base_get_worldlist_r = 0xB717,
        recv_cpf_authenticate = 0xBA73,
        recv_base_check_version2_r = 0xEC0A,
        recv_base_check_version_r = 0xEFDD,
        recv_cpf_notify_error = 0x73D7,
        recv_base_authenticate_soe_sessionid = 0x7EEA,
        send_base_check_version = 0x5705,
        send_base_authenticate = 0xAD93,
        send_base_get_worldlist = 0x53CF,
        send_base_select_world = 0x203F
    }
}