namespace Necromancy.Server.Data.Setting
{
    public class ItemSetting : ISettingRepositoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ItemType { get; set; }
        public int IconType { get; set; }
    }
}
