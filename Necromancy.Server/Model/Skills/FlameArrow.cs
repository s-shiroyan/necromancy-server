using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Arrowgene.Services.Logging;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Logging;
using Necromancy.Server.Packet.Receive;
using Necromancy.Server.Packet.Response;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;
using System.Collections.Generic;
using System.Numerics;

namespace Necromancy.Server.Model.Skills
{
   class FlameArrow : IInstance
    {
        public uint InstanceId { get; set; }

        private NecClient _client;
        private readonly NecLogger _logger;
        private readonly NecServer _server;
        private int _skillId;
        private int _targetInstanceId;
        private Vector3 _srcCoord;

        public FlameArrow(NecServer server, NecClient client, int skillId, int targetInstanceId, Vector3 srcCoord)
        {
            _server = server;
            _client = client;
            _skillId = skillId;
            _targetInstanceId = targetInstanceId;
            _logger = LogProvider.Logger<NecLogger>(this);
            _srcCoord = srcCoord;
        }

        public void StartCast()
        {
            IInstance target = _server.Instances.GetInstance((uint)_targetInstanceId);
            switch (target)         // ToDO     Do a hositilty check to make sure this is allowed
            {
                case NpcSpawn npcSpawn:
                    _logger.Debug($"Start casting Skill [{_skillId}] on NPCId: {npcSpawn.InstanceId}");
                    break;
                case MonsterSpawn monsterSpawn:
                    _logger.Debug($"Start casting Skill [{_skillId}] on MonsterId: {monsterSpawn.InstanceId}");
                    break;
                case Character character:
                    _logger.Debug($"Start casting Skill [{_skillId}] on CharacterId: {character.InstanceId}");
                    break;
                default:
                    _logger.Error($"Instance with InstanceId: {target.InstanceId} does not exist.  the ground is gettin blasted");
                    break;
            }
            float castTime = 2.0F;
            RecvSkillStartCastExR flameArrow = new RecvSkillStartCastExR((int)InstanceId, _skillId, castTime);
            _server.Router.Send(_client.Map, flameArrow);
            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new Packet.Receive.RecvBattleReportStartNotify(_client.Character.InstanceId);
            RecvBattleReportEndNotify brEnd = new Packet.Receive.RecvBattleReportEndNotify();
            RecvBattleReportActionSkillStartCast brStartCast = new Packet.Receive.RecvBattleReportActionSkillStartCast(_skillId);

            brList.Add(brStart);
            brList.Add(brStartCast);
            brList.Add(brEnd);
            _server.Router.Send(_client.Map, brList);
 
        }

        public void SkillExec()
        {
            Vector3 trgCoord = new Vector3();
            NpcSpawn npcSpawn = null;
            MonsterSpawn monsterSpawn = null;
            Character character = null;
            IInstance target = _server.Instances.GetInstance((uint)_targetInstanceId);
            switch (target)
            {
                case NpcSpawn npc:
                    npcSpawn = npc;
                    _logger.Debug($"NPCId: {npcSpawn.InstanceId} is gettin blasted by Skill Effect {_client.Character.skillStartCast}");
                    trgCoord.X = npcSpawn.X;
                    trgCoord.Y = npcSpawn.Y;
                    trgCoord.Z = npcSpawn.Z;
                    break;
                case MonsterSpawn monster:
                    monsterSpawn = monster;
                    _logger.Debug($"MonsterId: {monsterSpawn.InstanceId} is gettin blasted by Skill Effect {_client.Character.skillStartCast}");
                    trgCoord.X = monsterSpawn.X;
                    trgCoord.Y = monsterSpawn.Y;
                    trgCoord.Z = monsterSpawn.Z;

                    break;
                case Character chara:
                    character = chara;
                    _logger.Debug($"CharacterId: {character.InstanceId} is gettin blasted by Skill Effect {_client.Character.skillStartCast}");
                    trgCoord.X = character.X;
                    trgCoord.Y = character.Y;
                    trgCoord.Z = character.Z;
                    break;
                default:
                    _logger.Error($"Instance with InstanceId: {target.InstanceId} does not exist.  the ground is gettin blasted");
                    break;
            }

            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(_client.Character.InstanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionSkillExec brExec = new RecvBattleReportActionSkillExec(_client.Character.skillStartCast);
            brList.Add(brStart);
            brList.Add(brExec);
            brList.Add(brEnd);
            _server.Router.Send(_client.Map, brList);
            if (!int.TryParse($"{_skillId}".Substring(1, 6)+1, out int effectId))
            {
                _logger.Error($"Creating effectId from skillid [{_skillId}]");
            }
            trgCoord.Z += 10;
            RecvDataNotifyEoData eoData = new RecvDataNotifyEoData((int)InstanceId, _targetInstanceId, 301411, trgCoord, 2, 2);
            _server.Router.Send(_client.Map, eoData);
            RecvEoNotifyDisappearSchedule eoDisappear = new RecvEoNotifyDisappearSchedule((int)InstanceId, 2.0F);
            _server.Router.Send(_client.Map, eoDisappear);
            
            Vector3 _srcCoord  = new Vector3(_client.Character.X, _client.Character.Y, _client.Character.Z);
            Recv8D92 effectMove = new Recv8D92(_srcCoord, trgCoord, (int)InstanceId, _client.Character.skillStartCast, 3000, 2, 2);
            _server.Router.Send(_client.Map, effectMove);

            int damage = Util.GetRandomNumber(70, 90);
            RecvDataNotifyEoData eoTriggerData = new RecvDataNotifyEoData((int)_client.Character.InstanceId, (int)monsterSpawn.InstanceId, 1430212, _srcCoord, 2, 2);
            _server.Router.Send(_client.Map, eoTriggerData);
            int monsterHP = monsterSpawn.GetHP();
            float perHp = monsterHP > 0 ? (((float)monsterHP / (float)monsterSpawn.MaxHp) * 100) : 0;
            RecvBattleReportDamageHp brHp = new RecvBattleReportDamageHp((int)monsterSpawn.InstanceId, damage);
            RecvObjectHpPerUpdateNotify oHpUpdate = new RecvObjectHpPerUpdateNotify((int)monsterSpawn.InstanceId, perHp);
            RecvBattleReportNotifyHitEffect brHit = new RecvBattleReportNotifyHitEffect((int)monsterSpawn.InstanceId);

            brList.Add(brStart);
            brList.Add(brHp);
            brList.Add(oHpUpdate);
            brList.Add(brHit);
            brList.Add(brEnd);
            brList.Add(oHpUpdate);
            _server.Router.Send(_client.Map, brList);
            if (monsterSpawn.GetAgroCharacter((int)_client.Character.InstanceId))
            {
                monsterSpawn.UpdateHP(-damage);
            }
            else
            {
                monsterSpawn.UpdateHP(-damage, _server, true, (int)_client.Character.InstanceId);
            }
            _logger.Debug($"{monsterSpawn.Name} has {monsterSpawn.GetHP()} HP left.");
        }

    }
}
