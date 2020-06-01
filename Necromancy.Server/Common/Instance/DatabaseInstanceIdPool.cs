using System.Collections.Concurrent;
using Arrowgene.Logging;

namespace Necromancy.Server.Common.Instance
{
    public class DatabaseInstanceIdPool : IInstanceIdPool
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(DatabaseInstanceIdPool));

        private readonly ConcurrentDictionary<uint, uint> _idPool;
        private readonly uint _lowerBound;
        private readonly uint _size;
        private readonly string _name;

        public DatabaseInstanceIdPool(string name, uint lowerBound, uint size)
        {
            _name = name;
            _idPool = new ConcurrentDictionary<uint, uint>();
            _lowerBound = lowerBound;
            _size = size;
        }

        public uint Used => _size - (uint) _idPool.Count;
        public uint LowerBound => _lowerBound;
        public uint UpperBound => _lowerBound + _size;
        public uint Size => _size;
        public string Name => _name;

        public uint Assign(uint dbId)
        {
            if (dbId > _size)
            {
                Logger.Error($"Exhausted pool size of {_size} for dbId: {dbId}");
                return InstanceGenerator.UnassignedInstanceId;
            }

            uint instanceId = dbId + _lowerBound;
            if (!_idPool.TryAdd(instanceId, dbId))
            {
                Logger.Error($"DbId: {dbId} already assigned to instanceId: {instanceId}.");
                return InstanceGenerator.UnassignedInstanceId;
            }

            return instanceId;
        }

        public bool Free(uint instanceId)
        {
            return _idPool.TryRemove(instanceId, out uint dbId);
        }
    }
}
