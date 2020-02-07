namespace Necromancy.Server.Data.Setting
{
    public class EoBaseSetting : ISettingRepositoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LogId { get; set; }
        public string Faction { get; set; }
        public bool OnlyOwner { get; set; }
        public bool ShowActivationTime { get; set; }
        public bool ShowName { get; set; }
        public string damageShape { get; set; }
        public int EffectRadius { get; set; }
    }
}
