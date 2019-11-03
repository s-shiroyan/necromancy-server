namespace Necromancy.Server.Packet.Id
{
    /// <summary>
    /// Necromancy Custom OP Codes
    /// </summary>
    public enum CustomPacketId : ushort
    {
        // Recv OP Codes - ordered by op code
        RecvHeartbeat = 0xFFFF,

        // Send OP Codes - ordered by op code
        SendHeartbeat = 0xFFFF,
    }
}
