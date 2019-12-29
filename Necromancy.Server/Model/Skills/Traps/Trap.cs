using Necromancy.Server.Common.Instance;
using Necromancy.Server.Data.Setting;

namespace Necromancy.Server.Model.Skills
{
   public class Trap : IInstance
    {
        public uint InstanceId { get; set; }
        public string _name { get; set; }
        public int _skillId { get; set; }
        public int _skillBaseId { get; set; }
        public int _triggerRadius { get; set; }
        public int _effectRadius { get; set; }
        public int _itemType { get; set; }
        public int _itemCount { get; set; }
        public float _castTime { get; set; }
        public float _trapTime { get; set; }
        public int _skillEffectId { get; set; }
        public int _triggerEffectId { get; set; }
        public float  _coolTime { get; set; }
        public Trap(int skillBase, SkillBaseSetting skillBaseSetting, EoBaseSetting eoBaseSetting, EoBaseSetting eoBaseSettingTriggered)
        {
            _skillBaseId = skillBase;
            _skillId = skillBaseSetting.Id;
            _name = skillBaseSetting.Name;
            _triggerRadius = eoBaseSetting.EffectRadius;
            _effectRadius = eoBaseSettingTriggered.EffectRadius;
            _itemType = skillBaseSetting.Item1Id;
            _itemCount = skillBaseSetting.Item1Count;
            _castTime = skillBaseSetting.CastingTime;
            _trapTime = skillBaseSetting.EffectTime;
            _skillEffectId = eoBaseSetting.Id;
            _triggerEffectId = eoBaseSettingTriggered.Id;
            _coolTime = skillBaseSetting.CastingCooldown;
        }

    }
}
