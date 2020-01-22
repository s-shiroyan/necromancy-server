using Arrowgene.Services.Buffers;
using Arrowgene.Services.Logging;
using Arrowgene.Services.Tasks;
using Necromancy.Server.Common;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.Skills;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;
using Necromancy.Server.Packet.Response;
using System;
using System.Threading;

namespace Necromancy.Server.Tasks
{
    public class CharacterTask : PeriodicTask
    {
        private readonly NecLogger _logger;
        private NecServer _server;
        private NecClient _client;
        private int tickTime;
        public CharacterTask(NecServer server, NecClient client)
        {
            _logger = LogProvider.Logger<NecLogger>(server);
            _server = server;
            _client = client;
            tickTime = 500;
        }

        public override string Name { get; }
        public override TimeSpan TimeSpan { get; }
        protected override bool RunAtStart { get; }
        protected override void Execute() 
        {
            while (_client.Character.characterActive)
            {
                // ToDo Character task 
                Thread.Sleep(tickTime);
            }
            this.Stop();
        }
    }

}
