namespace Necromancy.Server.Data.Setting
{
    public class MonsterCsvReader : CsvReader<MonsterSetting>
    {
        protected override int NumExpectedItems => 2;

        protected override MonsterSetting CreateInstance(string[] properties)
        {
            if (!int.TryParse(properties[0], out int id))
            {
                return null;
            }

            return new MonsterSetting
            {
                Id = id,
                Name = properties[1]
            };
        }
    }
}
