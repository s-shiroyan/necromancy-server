
namespace Necromancy.Server.Model
{
    public class MapPosition
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public byte Heading { get; set; }

        public MapPosition(float Xpos = 0, float Ypos = 0, float Zpos = 0, byte heading = 0)
        {
            X = Xpos;
            Y = Ypos;
            Z = Zpos;
            Heading = heading;
        }
    }
}
