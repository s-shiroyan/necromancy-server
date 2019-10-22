using Arrowgene.Services.Logging;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;

namespace Necromancy.Server.Model
{
    public class Map
    {
        private readonly NecLogger _logger;

        public Map(MapSetting setting)
        {
            _logger = LogProvider.Logger<NecLogger>(this);
            ClientLookup = new ClientLookup();
            Id = setting.Id;
            X = setting.X;
            Y = setting.Y;
            Z = setting.Z;
            Orientation = setting.Orientation;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Orientation { get; set; }
        public ClientLookup ClientLookup { get; }


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
