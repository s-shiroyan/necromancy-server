using Arrowgene.Services.Logging;
using Arrowgene.Services.Tasks;
using Necromancy.Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;


namespace Necromancy.Server.Tasks
{
    // Usage: create a monster and spawn it then use the following
    //MonsterThread monsterThread = new MonsterThread(Server, client, monsterSpawn);
    //Thread workerThread2 = new Thread(monsterThread.InstanceMethod);
    //workerThread2.Start();
    class MapTransitionTask : PeriodicTask
    {
        private NecServer _server { get; }
        private Vector3 _transitionPos1;
        private Vector3 _transitionPos2;
        private MapPosition _changePos;

        private Map _map;
        private int _instanceId;
        private bool _taskActive;
        private bool _invertedTransition;
        private int _tickTime;
        private int _transitionMapId;
        private readonly ILogger _logger;
        public MapTransitionTask(NecServer server, Map map, int transitionMapId, Vector3 transitionPos1, Vector3 transitionPos2, int instanceId, bool invertedTransition, MapPosition changePos)
        {
            _server = server;
            _transitionPos1 = transitionPos1;
            _transitionPos2 = transitionPos2;
            _changePos = changePos;
            _instanceId = instanceId;
            _transitionMapId = transitionMapId;
            _taskActive = false;
            _invertedTransition = invertedTransition;
            _tickTime = 500;
            _logger = LogProvider.Logger(server);
            _map = map;

        }

        public override string Name { get; }
        public override TimeSpan TimeSpan { get; }
        protected override bool RunAtStart { get; }
        protected override void Execute()
        {
            _taskActive = true;

            while (_taskActive)
            {
                List<Character> characters =_map.GetCharactersRange(_transitionPos1, 1000);
                foreach (Character character in characters)
                {
                    Vector3 characterPos = new Vector3(character.X, character.Y, character.Z);
                    bool transition = TransitionCheck(characterPos);
                    if (transition)
                    {
                        if (!_server.Maps.TryGet(_transitionMapId, out Map transitionMap))
                        {
                            return;
                        }
                        NecClient client = _server.Clients.GetByCharacterInstanceId(character.InstanceId);
                        transitionMap.EnterForce(client, _changePos);
                    }
                    //_logger.Debug($"{character.Name} in range [transition] [{transition}].");

                }
                Thread.Sleep(_tickTime);
            }
            this.Stop();
        }

        private bool TransitionCheck(Vector3 characterPos)
        {
            bool trasition = false;
            trasition = ((characterPos.X - _transitionPos1.X) * (_transitionPos2.Y - _transitionPos1.Y)) - ((characterPos.Y - _transitionPos1.Y) * (_transitionPos2.X - _transitionPos1.X)) <= 0;
            //= (x−x1)(y2−y1)−(y−y1)(x2−x1)
            return trasition != _invertedTransition;
        }
    }
}
