using Arrowgene.Services.Logging;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Logging;
using Necromancy.Server.Tasks;
using System.Numerics;

namespace Necromancy.Server.Model
{
   class MapTransition : IInstance
    {
        public uint InstanceId { get; set; }

        private readonly NecLogger _logger;
        private readonly NecServer _server;
        private readonly Vector3 _leftPos;
        private readonly Vector3 _rightPos;
        private readonly MapTransitionTask _transitionTask;
        private readonly Map _map;
        private readonly MapPosition _mapPosition;
        private readonly int _transitionMapId;
        private readonly bool _invertedTransition;
        public MapTransition(NecServer server, Map map, int transitionMapId, Vector3 leftPos, Vector3 rightPos, bool invertedTransition, MapPosition mapPosition = null)
        {
            _server = server;
            _logger = LogProvider.Logger<NecLogger>(this);
            _leftPos = leftPos;
            _rightPos = rightPos;
            _map = map;
            _mapPosition = mapPosition;
            _transitionMapId = transitionMapId;
            _invertedTransition = invertedTransition;
            _transitionTask = new MapTransitionTask(_server, _map, _transitionMapId, _leftPos, _rightPos, (int)InstanceId, _invertedTransition, _mapPosition);
            _transitionTask.Start();

        }

    }
}
