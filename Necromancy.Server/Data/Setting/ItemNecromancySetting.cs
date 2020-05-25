namespace Necromancy.Server.Data.Setting
{
    /// <summary>
    /// Additional Item information, that was not provided or could not be extracted from the client.
    /// </summary>
    public class ItemNecromancySetting : ISettingRepositoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public int Physical { get; set; }
        public int Magical { get; set; }
        public int Durability { get; set; }
        public int Hardness { get; set; }
        public float Weight { get; set; }
    }
}
