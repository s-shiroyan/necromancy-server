using Arrowgene.Services.Buffers;
using Necromancy.Server.Model;

namespace Necromancy.Server.Packet
{
    public class NecPacket
    {
        public NecPacket(ushort id, IBuffer buffer, ServerType serverType)
        {
            Data = buffer;
            Id = id;
            ServerType = serverType;
        }

        public IBuffer Data { get; }
        public ushort Id { get; }
        public byte[] Header { get; set; }
        public ServerType ServerType { get; }
    }
}
