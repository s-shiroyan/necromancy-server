using System.Collections.Generic;

namespace Necromancy.Server.Common.Instance
{
    /// <summary>
    /// Provides Unique Ids for instancing.
    /// </summary>
    public class InstanceGenerator64
    {
        private readonly object _lock;
        private ulong _currentId;

        private readonly Dictionary<ulong, IInstance64> _instances;

        public InstanceGenerator64()
        {
            _lock = new object();
            _currentId = 1;
            _instances = new Dictionary<ulong, IInstance64>();
        }

        public void AssignInstance(IInstance64 instance)
        {
            ulong id;
            lock (_lock)
            {
                id = _currentId;
                _currentId++;
            }

            _instances.Add(id, instance);
            instance.InstanceId = id;
        }

        public T CreateInstance<T>() where T : IInstance64, new()
        {
            ulong id;
            lock (_lock)
            {
                id = _currentId;
                _currentId++;
            }

            T instance = new T();
            _instances.Add(id, instance);
            instance.InstanceId = id;
            return instance;
        }

        public IInstance64 GetInstance(ulong id)
        {
            if (!_instances.ContainsKey(id))
            {
                return null;
            }

            return _instances[id];
        }
    }
}
