using System.Collections.Generic;
using Arrowgene.Services.Logging;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Packet.Response;

namespace Necromancy.Server.Model
{
    public class Map
    {
        public const int NewCharacterMapId = 1001902;

        private readonly NecLogger _logger;
        private readonly NecServer _server;

        public Map(MapSetting setting, NecServer server)
        {
            _server = server;
            _logger = LogProvider.Logger<NecLogger>(this);
            ClientLookup = new ClientLookup();
            NpcSpawns = new Dictionary<int, NpcSpawn>();
            Id = setting.Id;
            X = setting.X;
            Y = setting.Y;
            Z = setting.Z;
            Country = setting.Country;
            Area = setting.Area;
            Place = setting.Place;
            Orientation = setting.Orientation;
        }

        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public string Country { get; set; }
        public string Area { get; set; }
        public string Place { get; set; }
        public int Orientation { get; set; }
        public string FullName => $"{Country}/{Area}/{Place}";
        public ClientLookup ClientLookup { get; }
        public Dictionary<int, NpcSpawn> NpcSpawns { get; }

        public void EnterForce(NecClient client)
        {
            Enter(client);
            _server.Router.Send(new RecvMapChangeForce(this), client);
        }

        public void Enter(NecClient client)
        {
            if (client.Map != null)
            {
                client.Map.Leave(client);
            }

            _logger.Info(client, $"Entering Map: {Id}:{FullName}", client);
            ClientLookup.Add(client);
            client.Map = this;
            client.Character.MapId = Id;
            client.Character.X = X;
            client.Character.Y = Y;
            client.Character.Z = Z;
        }

        public void Leave(NecClient client)
        {
            _logger.Info(client, $"Leaving Map: {Id}:{FullName}", client);
            ClientLookup.Remove(client);
            client.Map = null;
        }
    }
}
