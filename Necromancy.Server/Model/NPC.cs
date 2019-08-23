namespace Necromancy.Server.Model
{
    public class npc
    {
        public int npcID { get; set; }
        public int serialID { get; set; }
        public byte status { get; set; }
        public string name { get; set;}
        public string title { get; set;}
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public byte viewOffset { get; set; }
        //public int UNKNOWN1 { get; set; } //first int32 for num of items
        //skipping the item slot/items/anim data 
        public int npcModel { get; set; }
        public short npcModelSize { get; set; }
        public byte UNKNOWN2 { get; set; } //first byte after slot/item/anim data
        public byte UNKNOWN3 { get; set; } //second byte after slot/item/anim data
        public byte UNKNOWN4 { get; set; } //third byte after slot/item/anim data
        public int UNKNOWN5 { get; set; } //int32 before visibility
        public int npcVisibility { get; set; }
        public int UNKNOWN6 { get; set; } //int32 after visibility
        public float UNKNOWN7 { get; set; } //first float
        public float UNKNOWN8 { get; set; } //second float
        public float UNKNOWN9 { get; set; } //third float

    }
}