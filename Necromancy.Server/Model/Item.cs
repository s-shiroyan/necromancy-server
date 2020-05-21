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
        }
        public uint InstanceId { get; set; }
        public uint Id { get; set; }
        public string Name { get; set; }
        public int ItemType { get; set; }
        public int IconType { get; set; }
    }
}
