namespace Necromancy.Server.Common
{
    /// <summary>
    /// Provides Unique Ids for instancing.
    /// </summary>
    public class InstanceIdGenerator
    {
        private readonly object _lock;
        private int _currentId;

        public InstanceIdGenerator()
        {
            _lock = new object();
            _currentId = 0;
        }

        public int GetId()
        {
            int id;
            lock (_lock)
            {
                id = _currentId;
                _currentId++;
            }

            return id;
        }
    }
}
