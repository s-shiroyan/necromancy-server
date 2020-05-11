using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Model
{
    public class NpcModelUpdate : Event
    {
        public uint ID;
        public int newModelId { get; set; }
        public NpcSpawn npcSpawn { get; set; }

        public NpcModelUpdate()
        {
            EventType = (ushort) AreaPacketId.recv_event_request_int;
        }

        public void Update(NecServer server, NecClient client)
        {
        }
    }
}
