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
        public int battlePose { get; set; }
        public byte charaPose { get; set; }
        public byte movementAnim { get; set; }
        public byte animJumpFall { get; set; }
        public byte xAnim { get; set; }


        public byte b1 { get; set; }
        public byte e1 { get; set; }

        public byte H { get; set; }

        public byte a { get; set; }
        public byte b { get; set; }
        
        public byte c { get; set; }
        public byte d { get; set; }
        public byte e { get; set; }
        
        
        public byte g1 { get; set; }
        public byte g2 { get; set; }
        public byte g3 { get; set; }
        public byte g4 { get; set; }
        public byte h { get; set; }
        public byte i { get; set; }
        public byte j { get; set; }
        public byte k { get; set; }
        public byte k1 { get; set; }
        public byte k2 { get; set; }
        public byte k3 { get; set; }
        public byte l { get; set; }
        
        
        



        /*client.Character.d = packet.Data.ReadByte();
        client.Character.e = packet.Data.ReadInt16();
        client.Character.f = packet.Data.ReadByte();
        client.Character.g = packet.Data.ReadInt32();
        client.Character.h = packet.Data.ReadByte();
        client.Character.i = packet.Data.ReadByte();
        client.Character.j = packet.Data.ReadByte();
        client.Character.k = packet.Data.ReadInt32();
        client.Character.l = packet.Data.ReadByte();
        client.Character.movementAnim = packet.Data.ReadByte();
        client.Character.m = packet.Data.ReadByte();*/
    }

}