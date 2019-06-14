using Arrowgene.Services.Buffers;

namespace Necromancy.Server.Packet
{
    public class NecPacket
    {
        public NecPacket(ushort id, IBuffer buffer)
        {
            Data = buffer;
            Id = id;
        }

        public IBuffer Data { get; }
        public ushort Id { get; }
        public byte[] Header { get; set; }
    }
}