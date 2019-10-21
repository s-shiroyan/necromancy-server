using Necromancy.Server.Model;

namespace Necromancy.Server.Data.Setting
{
    public class ItemInfoCsvReader : CsvReader<ItemSetting>
    {
        protected override ItemSetting CreateInstance(string[] properties)
        {
            ItemSetting item = new ItemSetting();
            item.Id = int.Parse(properties[0]);
            item.Name = properties[1];
            return item;
        }
    }
}
