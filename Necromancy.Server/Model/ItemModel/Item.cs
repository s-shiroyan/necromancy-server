using Necromancy.Server.Data.Setting;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model.ItemModel
{
    public class Item
    {
        public Item()
        {
        }

        public void AddItemSetting(ItemSetting setting)
        {
            Id = (uint)setting.Id;
            Name = setting.Name;
        }
        
        public uint Id { get; set; }
        public string Name { get; set; }
        public ItemType ItemType { get; set; }
        public EquipmentSlotType EquipmentSlotType { get; set; }
        public int Physical { get; set; }
        public int Magical { get; set; }
        public int Durability { get; set; }
    }
}
