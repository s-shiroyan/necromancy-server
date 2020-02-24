using System;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model
{
    public class Gimmick : IInstance
    {
        public uint InstanceId { get; set; }
        public int Id { get; set; }
        public int MapId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public byte Heading { get; set; }
        public int ModelId { get; set; }
        public int State { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public Gimmick()
        {
            Created = DateTime.Now;
            Updated = DateTime.Now;
        }
    }
}
