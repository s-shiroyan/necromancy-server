namespace Necromancy.Server.Model
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public byte viewOffset { get; set; }
    }
}