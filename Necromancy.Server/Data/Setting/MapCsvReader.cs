using Necromancy.Server.Model;

namespace Necromancy.Server.Data.Setting
{
    public class MapCsvReader : CsvReader<MapSetting>
    {
        protected override MapSetting CreateInstance(string[] properties)
        {
            MapSetting map = new MapSetting();
            map.Id = int.Parse(properties[0]);
            map.Id = int.Parse(properties[0]);
            map.Id = int.Parse(properties[0]);
            map.Id = int.Parse(properties[0]);
            map.Id = int.Parse(properties[0]);
            return map;
        }
    }
}
