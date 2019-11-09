using System;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model
{
    public class MonsterSpawn : IInstance
    {
        public uint InstanceId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public MonsterSpawn()
        {
            Created = DateTime.Now;
            Updated = DateTime.Now;
        }
    }
}
