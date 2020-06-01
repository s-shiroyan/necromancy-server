using System.Collections.Generic;
using System.Text;
using Arrowgene.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Setting;

namespace Necromancy.Server.Common.Instance
{
    /// <summary>
    /// Provides Unique Ids for instancing.
    /// </summary>
    public class InstanceGenerator
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(InstanceGenerator));

        public const uint UnassignedInstanceId = 0;

        private readonly Dictionary<uint, IInstance> _instances;
        private readonly InstanceIdPool _dynamicPool;
        private readonly DatabaseInstanceIdPool _characterPool;
        private readonly DatabaseInstanceIdPool _npcPool;
        private readonly DatabaseInstanceIdPool _monsterPool;
        private readonly List<IInstanceIdPool> _pools;

        public InstanceGenerator(NecSetting setting)
        {
            _pools = new List<IInstanceIdPool>();
            _instances = new Dictionary<uint, IInstance>();
            _dynamicPool = new InstanceIdPool("Dynamic", setting.PoolDynamicIdLowerBound, setting.PoolDynamicIdSize);
            _pools.Add(_dynamicPool);
            _characterPool = new DatabaseInstanceIdPool("Character", setting.PoolCharacterIdLowerBound,
                setting.PoolCharacterIdSize);
            _pools.Add(_characterPool);
            _npcPool = new DatabaseInstanceIdPool("Npc", setting.PoolNpcLowerBound, setting.PoolNpcIdSize);
            _pools.Add(_npcPool);
            _monsterPool =
                new DatabaseInstanceIdPool("Monster", setting.PoolMonsterIdLowerBound, setting.PoolMonsterIdSize);
            _pools.Add(_monsterPool);

            foreach (IInstanceIdPool pool in _pools)
            {
                foreach (IInstanceIdPool otherPool in _pools)
                {
                    if (pool.LowerBound < otherPool.UpperBound && otherPool.LowerBound < pool.UpperBound)
                    {
                        Logger.Error(
                            $"Pool: {pool.Name}({pool.LowerBound}-{pool.UpperBound}) overlaps with Pool {otherPool.Name}({otherPool.LowerBound}-{otherPool.UpperBound})");
                    }
                }
            }
        }

        public void AssignInstance(IInstance instance)
        {
            uint instanceId;
            if (instance is Character character)
            {
                instanceId = _characterPool.Assign((uint) character.Id);
            }
            else if (instance is NpcSpawn npc)
            {
                instanceId = _npcPool.Assign((uint) npc.Id);
            }
            else if (instance is MonsterSpawn monster)
            {
                instanceId = _monsterPool.Assign((uint) monster.Id);
            }
            else if (_dynamicPool.TryPop(out instanceId))
            {
                Logger.Info($"Registered dynamic instanceId");
                return;
            }
            else
            {
                Logger.Error($"Failed to retrieve id, object not added");
                return;
            }

            if (instanceId == UnassignedInstanceId)
            {
                return;
            }

            instance.InstanceId = instanceId;
            _instances.Add(instanceId, instance);
        }

        public void FreeInstance(IInstance instance)
        {
            uint instanceId = instance.InstanceId;
            if (_instances.ContainsKey(instanceId))
            {
                _instances.Remove(instanceId);
            }

            if (instance is Character character)
            {
                _characterPool.Free(instanceId);
            }
            else if (instance is NpcSpawn npc)
            {
                _npcPool.Free(instanceId);
            }
            else if (instance is MonsterSpawn monster)
            {
                _monsterPool.Free(instanceId);
            }
            else
            {
                _dynamicPool.Push(instanceId);
            }
        }

        public IInstance GetInstance(uint id)
        {
            if (!_instances.ContainsKey(id))
            {
                return null;
            }

            return _instances[id];
        }

        public void LogStatus()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("--- IdPool Status ---");
            foreach (IInstanceIdPool pool in _pools)
            {
                sb.AppendLine(
                    $"{pool.Name}: ${pool.Used}/{pool.Size} ({pool.LowerBound}-{pool.UpperBound})");
            }

            sb.AppendLine("---");
            Logger.Info(sb.ToString());
        }
    }
}
