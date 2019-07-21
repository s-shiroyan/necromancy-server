using Arrowgene.Services.Logging;
using Necromancy.Server.Logging;

namespace Necromancy.Server.Model
{
    public class Map
    {
        private readonly NecLogger _logger;

        public int Id { get; set; }
        public ClientLookup ClientLookup { get; }

        public Map()
        {
            _logger = LogProvider.Logger<NecLogger>(this);
            ClientLookup = new ClientLookup();
        }

        public void Enter(NecClient client)
        {
            _logger.Info($"Entering Map: {Id}", client);
            ClientLookup.Add(client);
            client.Map = this;
        }

        public void Leave(NecClient client)
        {
            _logger.Info($"Leaving Map: {Id}", client);
            ClientLookup.Remove(client);
            client.Map = null;
        }
    }
}