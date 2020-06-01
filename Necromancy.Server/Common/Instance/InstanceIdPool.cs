using System.Collections.Concurrent;
using Arrowgene.Logging;

namespace Necromancy.Server.Common.Instance
{
    public class InstanceIdPool : IInstanceIdPool
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(InstanceIdPool));

        private readonly ConcurrentStack<uint> _idPool;
        private readonly uint _lowerBound;

        public InstanceIdPool(string name, uint lowerBound, uint size)
        {
            _idPool = new ConcurrentStack<uint>();
            _lowerBound = lowerBound;
            Name = name;
            Size = size;
            UpperBound = _lowerBound + Size;
            Logger.Debug($"Pool:{Name} Loading:{Size}");
            for (uint i = _lowerBound; i < UpperBound; i++)
            {
                _idPool.Push(i);
            }

            Logger.Debug($"Pool:{Name} - Finished");
        }

        public uint Used => Size - (uint) _idPool.Count;
        public uint LowerBound => _lowerBound;
        public uint UpperBound { get; }
        public uint Size { get; }
        public string Name { get; }

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
