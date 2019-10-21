namespace Necromancy.Server.Model
{
    public class MapSetting : ISettingRepositoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Orientation { get; set; }
    }
}
