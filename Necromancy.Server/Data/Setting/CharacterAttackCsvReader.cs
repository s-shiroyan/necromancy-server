using Arrowgene.Services.Logging;
using System.Collections.Generic;

namespace Necromancy.Server.Data.Setting
{
    public class CharacterAttackCsvReader : CsvReader<CharacterAttackSetting>
    {
        private readonly ILogger _logger;

        public CharacterAttackCsvReader()
        {
            _logger = LogProvider.Logger(this);
        }

        protected override int NumExpectedItems => 40;

        protected override CharacterAttackSetting CreateInstance(string[] properties)
        {
            if (!int.TryParse(properties[0], out int id))
            {
                _logger.Debug($"First entry empty!!");
                return null;
            }

            int.TryParse(properties[1], out int motionId);
            bool.TryParse(properties[3], out bool firstShot);
            int.TryParse(properties[4], out int nextAttackId);
            int.TryParse(properties[5], out int channel);
            int.TryParse(properties[6], out int moveStart);
            int.TryParse(properties[7], out int moveEnd);
            int.TryParse(properties[8], out int moveAmount);
            int.TryParse(properties[9], out int swordShadowStart);
            int.TryParse(properties[10], out int swordShadowEnd);
            int.TryParse(properties[11], out int socket1Type);
            int.TryParse(properties[12], out int fx1Id);
            int.TryParse(properties[13], out int socket2Type);
            int.TryParse(properties[14], out int fx2Id);
            int.TryParse(properties[15], out int interruptStart);
            int.TryParse(properties[16], out int interruptEnd);
            int.TryParse(properties[17], out int rigidTime);
            int.TryParse(properties[18], out int inputReception);
            int.TryParse(properties[19], out int hit);
            int.TryParse(properties[20], out int guardCanel);
            int.TryParse(properties[21], out int attackAtariStart);
            int.TryParse(properties[22], out int attackAtariEnd);
            float.TryParse(properties[23], out float consecutiveAttackStart);
            float.TryParse(properties[24], out float continuousAttackEnd);
            float.TryParse(properties[25], out float delay);
            float.TryParse(properties[26], out float rigidity);
            bool.TryParse(properties[27], out bool reuse);

            return new CharacterAttackSetting
            {
                Id = id,
                MotionId = motionId,
                Weapon = properties[2],
                FirstShot = firstShot,
                NextAttackId = nextAttackId,
                Channel = channel,
                MoveStart = moveStart,
                MoveEnd = moveEnd,
                MoveAmount = moveAmount,
                SwordShadowStart = swordShadowStart,
                SwordShadowEnd = swordShadowEnd,
                Socket1Type = socket1Type,
                Fx1Id = fx1Id,
                Socket2Type = socket2Type,
                Fx2Id = fx2Id,
                InterruptStart = interruptStart,
                InterruptEnd = interruptEnd,
                RigidTime = rigidTime,
                InputReception = inputReception,
                Hit = hit,
                GuardCanel = guardCanel,
                AttackAtariStart = attackAtariStart,
                AttackAtariEnd = attackAtariEnd,
                ConsecutiveAttackStart = consecutiveAttackStart,
                ContinuousAttackEnd = continuousAttackEnd,
                Delay = delay,
                Rigidity = rigidity,
                Reuse = reuse
            };
        }
    }
}
