using System.Collections.Concurrent;

namespace Necromancy.Server.Common.Instance
{
    public class InstanceIdPool : IInstanceIdPool
    {
        private readonly ConcurrentStack<uint> _idPool;
        private readonly uint _lowerBound;
        private readonly uint _size;
        private readonly string _name;

        public InstanceIdPool(string name, uint lowerBound, uint size)
        {
            _name = name;
            _idPool = new ConcurrentStack<uint>();
            _lowerBound = lowerBound;
            _size = size;
            for (uint i = _lowerBound; i < _size; i++)
            {
                _idPool.Push(i);
            }
        }

        public uint Used => _size - (uint) _idPool.Count;
        public uint LowerBound => _lowerBound;
        public uint UpperBound => _lowerBound + _size;
        public uint Size => _size;
        public string Name => _name;

        public void Push(uint id)
        {
            _idPool.Push(id);
        }

        public bool TryPop(out uint id)
        {
            return _idPool.TryPop(out id);
        }
    }
}
