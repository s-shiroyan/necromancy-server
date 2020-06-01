using System.Collections.Concurrent;
using Arrowgene.Logging;

namespace Necromancy.Server.Common.Instance
{
    public class DatabaseInstanceIdPool : IInstanceIdPool
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(DatabaseInstanceIdPool));

        private readonly ConcurrentDictionary<uint, uint> _idPool;

        public DatabaseInstanceIdPool(string name, uint lowerBound, uint size)
        {
            _idPool = new ConcurrentDictionary<uint, uint>();
            Name = name;
            LowerBound = lowerBound;
            Size = size;
            UpperBound = LowerBound + Size;
        }

        public uint Used => (uint) _idPool.Count;
        public uint LowerBound { get; }
        public uint UpperBound { get; }
        public uint Size { get; }
        public string Name { get; }

        public uint Assign(uint dbId)
        {
            if (dbId > Size)
            {
                Logger.Error($"Exhausted pool {Name} size of {Size} for dbId: {dbId}");
                return InstanceGenerator.UnassignedInstanceId;
            }

            uint instanceId = dbId + LowerBound;
            if (!_idPool.TryAdd(instanceId, dbId))
            {
                Logger.Error($"DbId: {dbId} already assigned to instanceId: {instanceId} for pool {Name}");
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
