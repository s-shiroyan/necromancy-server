using System.Collections.Generic;
using System.Numerics;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Receive;
using Necromancy.Server.Packet.Response;

namespace Necromancy.Server.Model.Skills
{
    public class ThiefSkill : IInstance
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(ThiefSkill));

        public uint InstanceId { get; set; }

        private NecClient _client;
        private readonly NecServer _server;
        private int _skillId;
        private uint _targetInstanceId;

        public ThiefSkill(NecServer server, NecClient client, int skillId, uint targetInstanceId)
        {
            _server = server;
            _client = client;
            _skillId = skillId;
            _targetInstanceId = targetInstanceId;
        }

        public void StartCast()
        {
            IInstance target = _server.Instances.GetInstance(_targetInstanceId);
            switch (target) // ToDO     Do a hositilty check to make sure this is allowed
            {
                case NpcSpawn npcSpawn:
                    Logger.Debug(
                        $"Start casting Skill [{_skillId}] on NPCId: {npcSpawn.InstanceId} SerialId: {npcSpawn.NpcId}");
                    break;
                case MonsterSpawn monsterSpawn:
                    Logger.Debug($"Start casting Skill [{_skillId}] on MonsterId: {monsterSpawn.InstanceId}");
                    break;
                case Character character:
                    Logger.Debug($"Start casting Skill [{_skillId}] on CharacterId: {character.InstanceId}");
                    break;
                default:
                    Logger.Error(
                        $"Instance with InstanceId: {_targetInstanceId} does not exist.  the ground is gettin blasted");
                    break;
            }

            if (!_server.SettingRepository.SkillBase.TryGetValue(_skillId, out SkillBaseSetting skillBaseSetting))
            {
                Logger.Error($"Could not get SkillBaseSetting for skillId : {_skillId}");
                return;
            }

            float castTime = skillBaseSetting.CastingTime;
            Logger.Debug($"Start casting Skill [{_skillId}] cast time is [{castTime}]");
            RecvSkillStartCastR thiefSkill = new RecvSkillStartCastR(0, castTime);
            _server.Router.Send(thiefSkill, _client);
            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(_client.Character.InstanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionSkillStartCast brStartCast = new RecvBattleReportActionSkillStartCast(_skillId);
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
            IInstance target = _server.Instances.GetInstance(_targetInstanceId);
            switch (target)
            {
                case NpcSpawn npc:
                    npcSpawn = npc;
                    Logger.Debug(
                        $"NPCId: {npcSpawn.InstanceId} is gettin blasted by Skill Effect {_client.Character.skillStartCast}");
                    trgCoord.X = npcSpawn.X;
                    trgCoord.Y = npcSpawn.Y;
                    trgCoord.Z = npcSpawn.Z;
                    break;
                case MonsterSpawn monster:
                    monsterSpawn = monster;
                    Logger.Debug(
                        $"MonsterId: {monsterSpawn.InstanceId} is gettin blasted by Skill Effect {_client.Character.skillStartCast}");
                    trgCoord.X = monsterSpawn.X;
                    trgCoord.Y = monsterSpawn.Y;
                    trgCoord.Z = monsterSpawn.Z;

                    break;
                case Character chara:
                    character = chara;
                    Logger.Debug(
                        $"CharacterId: {character.InstanceId} is gettin blasted by Skill Effect {_client.Character.skillStartCast}");
                    trgCoord.X = character.X;
                    trgCoord.Y = character.Y;
                    trgCoord.Z = character.Z;
                    break;
                default:
                    Logger.Error(
                        $"Instance with InstanceId: {_targetInstanceId} does not exist.  the ground is gettin blasted");
                    break;
            }

            if (!_server.SettingRepository.SkillBase.TryGetValue(_skillId, out SkillBaseSetting skillBaseSetting))
            {
                Logger.Error($"Could not get SkillBaseSetting for skillId : {_skillId}");
                return;
            }

            if (!int.TryParse($"{_skillId}".Substring(1, 6) + 1, out int effectId))
            {
                Logger.Error($"Creating effectId from skillid [{_skillId}]");
            }

            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(_client.Character.InstanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionSkillExec brExec =
                new RecvBattleReportActionSkillExec(_client.Character.skillStartCast);
            RecvBattleReportActionEffectOnHit brEof = new RecvBattleReportActionEffectOnHit(600021);
            brList.Add(brStart);
            brList.Add(brExec);
            brList.Add(brEof);
            brList.Add(brEnd);
            _server.Router.Send(_client.Map, brList);
            brList.Clear();
            trgCoord.Z += 10;
            Logger.Debug($"skillid [{_skillId}] effectId [{effectId}]");

            RecvDataNotifyEoData eoData =
                new RecvDataNotifyEoData(InstanceId, _targetInstanceId, effectId, trgCoord, 2, 2);
            //_server.Router.Send(_client.Map, eoData);
            RecvEoNotifyDisappearSchedule eoDisappear = new RecvEoNotifyDisappearSchedule(InstanceId, 2.0F);
            //_server.Router.Send(_client.Map, eoDisappear);

            //Vector3 _srcCoord  = new Vector3(_client.Character.X, _client.Character.Y, _client.Character.Z);
            //Recv8D92 effectMove = new Recv8D92(_srcCoord, trgCoord, InstanceId, _client.Character.skillStartCast, 3000, 2, 2);  // ToDo need real velocities
            //_server.Router.Send(_client.Map, effectMove);

            int damage = Util.GetRandomNumber(70, 90);
            //RecvDataNotifyEoData eoTriggerData = new RecvDataNotifyEoData(_client.Character.InstanceId, monsterSpawn.InstanceId, effectId, _srcCoord, 2, 2);
            //_server.Router.Send(_client.Map, eoTriggerData);
            int monsterHP = monsterSpawn.Hp.current;
            List<PacketResponse> brList2 = new List<PacketResponse>();
            float perHp = monsterHP > 0 ? (((float) monsterHP / (float) monsterSpawn.Hp.max) * 100) : 0;
            RecvBattleReportStartNotify brStart1 = new RecvBattleReportStartNotify(_client.Character.InstanceId);
            RecvBattleReportEndNotify brEnd1 = new RecvBattleReportEndNotify();
            //RecvBattleReportDamageHp brHp = new RecvBattleReportDamageHp(monsterSpawn.InstanceId, damage);
            RecvBattleReportPhyDamageHp brPhyHp = new RecvBattleReportPhyDamageHp(monsterSpawn.InstanceId, damage);
            RecvObjectHpPerUpdateNotify oHpUpdate = new RecvObjectHpPerUpdateNotify(monsterSpawn.InstanceId, perHp);
            RecvBattleReportNotifyHitEffect brHit = new RecvBattleReportNotifyHitEffect(monsterSpawn.InstanceId);

            brList2.Add(brStart1);
            //brList2.Add(brHp);
            brList2.Add(brPhyHp);
            brList2.Add(oHpUpdate);
            brList2.Add(brHit);
            brList2.Add(brEnd1);
            //brList.Add(oHpUpdate);
            _server.Router.Send(_client.Map, brList2);
            //if (monsterSpawn.GetAgroCharacter(_client.Character.InstanceId))
            //{
            // monsterSpawn.UpdateHP(-damage);
            //}
            //else
            //{
            //monsterSpawn.UpdateHP(-damage, _server, true, _client.Character.InstanceId);
            //}
            Logger.Debug($"{monsterSpawn.Name} has {monsterSpawn.Hp.current} HP left.");
        }
    }
}
