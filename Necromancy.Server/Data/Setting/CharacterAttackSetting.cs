using System.Collections.Generic;

namespace Necromancy.Server.Data.Setting
{
    public class CharacterAttackSetting : ISettingRepositoryItem
    {
        public int Id { get; set; }
        public int MotionId { get; set; }
        public string Weapon { get; set; }
        public bool FirstShot { get; set; }
        public int NextAttackId { get; set; }
        public int Channel { get; set; }
        public int MoveStart { get; set; }
        public int MoveEnd { get; set; }
        public int MoveAmount { get; set; }
        public int SwordShadowStart { get; set; }
        public int SwordShadowEnd { get; set; }
        public int Socket1Type { get; set; }
        public int Fx1Id { get; set; }
        public int Socket2Type { get; set; }
        public int Fx2Id { get; set; }
        public int InterruptStart { get; set; }
        public int InterruptEnd { get; set; }
        public int RigidTime { get; set; }
        public int InputReception { get; set; }
        public int Hit { get; set; }
        public int GuardCanel { get; set; }
        public int AttackAtariStart { get; set; }
        public int AttackAtariEnd { get; set; }
        public float ConsecutiveAttackStart { get; set; }
        public float ContinuousAttackEnd { get; set; }
        public float Delay { get; set; }
        public float Rigidity { get; set; }
        public bool Reuse { get; set; }

    }
}
