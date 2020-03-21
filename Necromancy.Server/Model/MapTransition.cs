using Arrowgene.Services.Logging;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Logging;
using Necromancy.Server.Tasks;
using System;
using System.Numerics;

namespace Necromancy.Server.Model
{
    public class MapTransition : IInstance
    {
        public uint InstanceId { get; set; }
        public int Id { get; set; }

        private NecServer _server;
        private MapTransitionTask _transitionTask;
        public Vector3 ReferencePos;
        public int RefDistance;
        public Vector3 LeftPos;
        public Vector3 RightPos;
        public byte MaplinkHeading;
        public int MaplinkColor;
        public int MaplinkOffset;
        public int MaplinkWidth;
        public int MapId;
        public MapPosition ToPos;
        public int TransitionMapId;
        public bool State { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool InvertedTransition;
        public MapTransition()
        {
            ToPos = new MapPosition();
        }

        public void Start(NecServer server, Map map)
        {
            _server = server;
            _transitionTask = new MapTransitionTask(_server, map, TransitionMapId, ReferencePos, RefDistance, LeftPos, RightPos, InstanceId, InvertedTransition, ToPos);
            _transitionTask.Start();
        }

        public void Stop()
        {
            _transitionTask.Stop();
        }
    }
}