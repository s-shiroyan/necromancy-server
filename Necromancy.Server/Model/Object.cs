using System.Numerics;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model
{
    public class Object : IInstance
    {
        public uint InstanceId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public Vector3 ObjectCoord { get; set; }
        public Vector3 TriggerCoord { get; set; }
        public int Bitmap1 { get; set; }
        public int Bitmap2 { get; set; }
        public byte Heading { get; set; }
        public int Unknown1 { get; set; }
        public int Unknown2 { get; set; }
        public int Unknown3 { get; set; }
        public Object()
        {
        }

    }
}
