using System;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model
{
    public class GGateSpawn : IInstance
    {
        public uint InstanceId { get; set; }
        public int Id { get; set; }
        public int SerialId { get; set; }
        public byte Interaction { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int MapId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public byte Heading { get; set; }
        public int ModelId { get; set; }
        public short Size { get; set; }
        public int Active { get; set; }
        public int Glow { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public GGateSpawn()
        {
            Created = DateTime.Now;
            Updated = DateTime.Now;
            Interaction = 0;
            Size = 100;
            Glow = 0b0001;
            ModelId = 1900001;
            Active = 0;
            SerialId = 1900001;

        }
    }
}
