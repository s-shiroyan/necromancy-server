using Necromancy.Server.Data.Setting;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model
{
    public class Item : IInstance
    {
        public Item()
        {
        }

        public void AddItemSetting(ItemSetting setting)
        {
            Id = (uint)setting.Id;
            Name = setting.Name;
            ItemType = setting.ItemType;
            IconType = setting.IconType;
            name = setting.Name;
        }
        public uint InstanceId { get; set; }
        public uint Id { get; set; }
        public string Name { get; set; }
        public int ItemType { get; set; }
        public int IconType { get; set; }
        public long rowId { get; set; }
	    public string name { get; set; }
	    public int type { get; set; }
	    public int bitmask { get; set; }
	    public byte count { get; set; }
        public int state { get; set; }
        public int icon { get; set; }
        public int hairOverride { get; set; }
        public int faceOverride { get; set; }
        public int durability { get; set; }
        public int maxDurability { get; set; }
        public int weight { get; set; }
        public short physics { get; set; }
        public short magic { get; set; }
        public int enchatId { get; set; }
        public short ac { get; set; }
        public int dateEndProtect { get; set; }
	    public byte hardness { get; set; }
        public byte level { get; set; }
    }
}
