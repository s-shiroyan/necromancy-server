namespace Necromancy.Server.Data.Setting
{
    public class HonorSetting : ISettingRepositoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Condition { get; set; }
//        public int EffectId { get; set; }
        public int HiddenTitle { get; set; }
        public int AlwaysDisplayTitle { get; set; }
        public int Prerequesit { get; set; }
    }
}
