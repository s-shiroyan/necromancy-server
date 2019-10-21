using Necromancy.Server.Model;

namespace Necromancy.Server.Data.Settings
{
    public class ItemInfoCsvReader : CsvReader<Item>
    {
        protected override Item CreateInstance(string[] properties)
        {
            Item item = new Item();
            item.Id = int.Parse(properties[0]);
            item.Name = properties[1];
            return item;
        }
    }
}
