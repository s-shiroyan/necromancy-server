using Arrowgene.Services.Buffers;
using Arrowgene.Services.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;
namespace Necromancy.Server.Model
{
    public class NpcModelUpdate : Event
    {
        private readonly NecLogger _logger;
        public uint ID;
        public int newModelId { get; set; }
        public NpcSpawn npcSpawn { get; set; }

        public NpcModelUpdate()
        {
            EventType = (ushort)AreaPacketId.recv_event_request_int;
            _logger = LogProvider.Logger<NecLogger>(this);
        }

        public void Update(NecServer server, NecClient client)
        {
        }
    }
}
