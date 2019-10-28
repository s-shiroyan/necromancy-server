using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Model;

namespace Necromancy.Server.Packet
{
    public abstract class PacketResponse
    {
        private readonly List<NecClient> _clients;

        public PacketResponse(ushort id, ServerType serverType)
        {
            _clients = new List<NecClient>();
            Id = id;
            ServerType = serverType;
        }

        public List<NecClient> Clients => new List<NecClient>(_clients);
        public ServerType ServerType { get; }
        public ushort Id { get; }

        public abstract IBuffer ToBuffer();

        public void AddClients(params NecClient[] clients)
        {
            _clients.AddRange(clients);
        }
    }
}
