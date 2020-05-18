using Necromancy.Server.Data.Setting;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model
{
    public class Item : IInstance64
    {
        public Item()
        {
        }

        public void AddItemSetting(ItemSetting setting)
        {
            Id = setting.Id;
            Name = setting.Name;
            ItemType = setting.ItemType;
            IconType = setting.IconType;
        }
        public ulong InstanceId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int ItemType { get; set; }
        public int IconType { get; set; }
        public long rowId { get; set; }
	    public string name { get; set; }
	    public int type { get; set; }
	    public byte bitmask { get; set; }
	    public int count { get; set; }
	    public int state { get; set; }
        public int icon1 { get; set; }
        public int icon2 { get; set; }
        public int hairOverride { get; set; }
        public int faceOverride { get; set; }
        public int durability { get; set; }
        public int maxDurability { get; set; }
        public int weight { get; set; }
        public int physics { get; set; }
        public int magic { get; set; }
        public int enchatId { get; set; }
        public int ac { get; set; }
        public int dateEndProtect { get; set; }
	    public int hardness { get; set; }
        public int level { get; set; }
    }
}
