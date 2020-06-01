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

        public uint GetInstanceId(uint dbId)
        {
            return dbId + LowerBound;
        }

        public int GetDatabaseId(uint instanceId)
        {
            return (int) (instanceId - LowerBound);
        }

        public bool TryAssign(uint dbId, out uint instanceId)
        {
            if (dbId > Size)
            {
                Logger.Error($"Exhausted pool {Name} size of {Size} for dbId: {dbId}");
                instanceId = InstanceGenerator.UnassignedInstanceId;
                return false;
            }

            instanceId = GetInstanceId(dbId);
            if (_idPool.ContainsKey(instanceId))
            {
                // Instance already recorded
                return false;
            }

            if (!_idPool.TryAdd(instanceId, dbId))
            {
                Logger.Error($"DbId: {dbId} already assigned to instanceId: {instanceId} for pool {Name}");
                instanceId = InstanceGenerator.UnassignedInstanceId;
                return false;
            }

            return true;
        }

        public bool Free(uint instanceId)
        {
            return _idPool.TryRemove(instanceId, out uint dbId);
        }
    }
}
