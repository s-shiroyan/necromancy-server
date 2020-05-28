namespace Necromancy.Server.Data.Setting
{
    public class ItemNecromancyCsvReader : CsvReader<ItemNecromancySetting>
    {
        protected override int NumExpectedItems => 13;

        protected override ItemNecromancySetting CreateInstance(string[] properties)
        {
            if (!int.TryParse(properties[0], out int id))
            {
                return null;
            }
            
            string nameEn = properties[3];
            
            if (!int.TryParse(properties[6], out int physical))
            {
                physical = 0;
                // TODO fail on empty string
                // return;
            }

            if (!int.TryParse(properties[7], out int magical))
            {
                magical = 0;
                // TODO fail on empty string
                // return;
            }

            if (!int.TryParse(properties[10], out int durability))
            {
                durability = 0;
                // TODO fail on empty string
                // return;
            }

            if (!int.TryParse(properties[11], out int hardness))
            {
                hardness = 0;
                // TODO fail on empty string
                // return;
            }

            if (!float.TryParse(properties[12], out float weight))
            {
                weight = 0;
                // TODO fail on empty string
                // return;
            }

            return new ItemNecromancySetting
            {
                Id = id,
                Name = nameEn,
                Physical = physical,
                Magical = magical,
                Durability = durability,
                Hardness = hardness,
                Weight = weight
            };
        }
    }
}
