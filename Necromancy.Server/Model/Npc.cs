namespace Necromancy.Server.Model
{
    public class Npc
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public int Title { get; set; }
        public byte Level { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public bool Active { get; set; }
        public byte Heading { get; set; }
        public short Size { get; set; }
        public int Visibility { get; set; }

    }
}
