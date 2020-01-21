using Necromancy.Server.Data.Setting;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model
{
    public class Event : IInstance
    {
        public uint InstanceId { get; set; }
        public ushort EventType { get; set; }
        public Event()
        {
        }
    }
}
