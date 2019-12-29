using System.Collections.Generic;

namespace Necromancy.Server.Data.Setting
{
    public class SkillBaseSetting : ISettingRepositoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int LogId { get; set; }
        public bool LogBlockEnemy { get; set; }
        public int CastLogId { get; set; }
        public int HitLogId { get; set; }
        public string EffectType { get; set; }
        public int OccupationEffectType { get; set; }
        public float CastingTime { get; set; }
        public float CastingCooldown { get; set; }
        public int ChangeByMapId { get; set; }
        public int RigidityTime { get; set; }
        public int NoSword { get; set; }
        public int NecessaryLevel { get; set; }
        public int HpUsed { get; set; }
        public int MpUsed { get; set; }
        public int ApUsed { get; set; }
        public int AcUsed { get; set; }
        public int DurabilityUsed { get; set; }
        public int Item1Id { get; set; }
        public int Item1Count { get; set; }
        public int Item2Id { get; set; }
        public int Item2Count { get; set; }
        public int Item3Id { get; set; }
        public int Item3Count { get; set; }
        public int Item4Id { get; set; }
        public int Item4Count { get; set; }
        public int CastScriptId { get; set; }
        public int ActivatedScriptId { get; set; }
        public int ActivatedEffect1Id { get; set; }
        public int ActivatedEffect2Id { get; set; }
        public int EquipmentScriptChange { get; set; }
        public bool EffectOnSelf { get; set; }
        public string ObjectFaction { get; set; }
        public int AutomaticCombo { get; set; }
        public int HitEffect2 { get; set; }
        public int ScriptParameter1 { get; set; }
        public int ScriptParameter2 { get; set; }
        public string ScanType { get; set; }
        public int Unknown1 { get; set; }
        public int Unknown2 { get; set; }
        public int Unknown3 { get; set; }
        public int Unknown4 { get; set; }
        public string DisplayName { get; set; }
        public int EffectTime { get; set; }
    }
}
