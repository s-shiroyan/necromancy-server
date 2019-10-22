namespace Necromancy.Server.Model
{
    public class NPC
    {
        public int objectID { get; set; }
        public int npcID { get; set; }
        public int serialID { get; set; }
        public byte status { get; set; }
        public string name { get; set;}
        public string title { get; set;}
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public byte viewOffset { get; set; }
        public int UNKNOWN1 { get; set; }
        //skipping the item slot/items/anim data 
        public int npcModel { get; set; }
        public short npcModelSize { get; set; }
        public byte UNKNOWN2 { get; set; }
        public byte UNKNOWN3 { get; set; }
        public byte UNKNOWN4 { get; set; }
        public int UNKNOWN5 { get; set; }
        public int npcVisibility { get; set; }
        public int UNKNOWN6 { get; set; }
        public float UNKNOWN7 { get; set; }
        public float UNKNOWN8 { get; set; }
        public float UNKNOWN9 { get; set; }

    }
}