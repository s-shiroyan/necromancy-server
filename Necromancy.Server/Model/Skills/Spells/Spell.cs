using System.Collections.Generic;
using System.Numerics;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Model.Skills
{
    public class Spell : IInstance
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(Spell));

        public uint InstanceId { get; set; }

        private readonly NecClient _client;
        private readonly NecServer _server;
        private readonly int _skillId;
        private uint _targetInstanceId;
        private Vector3 _srcCoord;

        public Spell(NecServer server, NecClient client, int skillId, uint targetInstanceId, Vector3 srcCoord)
        {
            _server = server;
            _client = client;
            _skillId = skillId;
            _targetInstanceId = targetInstanceId;
            _srcCoord = srcCoord;
        }

        public void StartCast()
        {
            if (_targetInstanceId == 0) _targetInstanceId = _client.Character.InstanceId;
            IInstance target = _server.Instances.GetInstance((uint) _targetInstanceId);
            switch (target) // ToDO     Do a hositilty check to make sure this is allowed
            {
                case NpcSpawn npcSpawn:
                    Logger.Debug($"Start casting Skill [{_skillId}] on NPCId: {npcSpawn.InstanceId}");
                    break;
                case MonsterSpawn monsterSpawn:
                    Logger.Debug($"Start casting Skill [{_skillId}] on MonsterId: {monsterSpawn.InstanceId}");
                    break;
                case Character character:
                    Logger.Debug($"Start casting Skill [{_skillId}] on CharacterId: {character.InstanceId}");
                    break;
                default:
                    Logger.Error(
                        $"Instance with InstanceId: {target.InstanceId} does not exist.  the ground is gettin blasted");
                    break;
            }

            if (!_server.SettingRepository.SkillBase.TryGetValue(_skillId, out SkillBaseSetting skillBaseSetting))
            {
                Logger.Error($"Could not get SkillBaseSetting for skillId : {_skillId}");
                return;
            }

            float castTime = skillBaseSetting.CastingTime;
            Logger.Debug($"Start casting Skill [{_skillId}] cast time is [{castTime}]");
            RecvSkillStartCastR spell = new RecvSkillStartCastR(0, castTime);
            _server.Router.Send(spell, _client);
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
            float perHp = 0;
            int damage = Util.GetRandomNumber(70, 90);
            IInstance target = _server.Instances.GetInstance((uint) _targetInstanceId);
            switch (target)
            {
                case NpcSpawn npc:
                    npcSpawn = npc;
                    Logger.Debug(
                        $"NPCId: {npcSpawn.InstanceId} SerialId: {npcSpawn.Id} is gettin blasted by Skill Effect {_client.Character.skillStartCast}");
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
                    int monsterHP = monsterSpawn.Hp.current;
                    perHp = monsterHP > 0 ? (((float) monsterHP / (float) monsterSpawn.Hp.max) * 100) : 0;
                    if (monsterSpawn.GetAgroCharacter(_client.Character.InstanceId))
                    {
                        monsterSpawn.UpdateHP(-damage);
                    }
                    else
                    {
                        monsterSpawn.UpdateHP(-damage, _server, true, _client.Character.InstanceId);
                    }

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
                        $"Instance with InstanceId: {target.InstanceId} does not exist.  the ground is gettin blasted");
                    break;
            }

            if (!_server.SettingRepository.SkillBase.TryGetValue(_skillId, out SkillBaseSetting skillBaseSetting))
            {
                Logger.Error($"Could not get SkillBaseSetting for skillId : {_skillId}");
                return;
            }

            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(_client.Character.InstanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionSkillExec brExec =
                new RecvBattleReportActionSkillExec(_client.Character.skillStartCast);
            brList.Add(brStart);
            brList.Add(brExec);
            brList.Add(brEnd);
            _server.Router.Send(_client.Map, brList);
            if (!int.TryParse($"{_skillId}".Substring(1, 6) + 1, out int effectId))
            {
                Logger.Error($"Creating effectId from skillid [{_skillId}]");
            }

            trgCoord.Z += 10;
            Logger.Debug($"skillid [{_skillId}] effectId [{effectId}]");
            RecvDataNotifyEoData eoData =
                new RecvDataNotifyEoData(InstanceId, _targetInstanceId, effectId, trgCoord, 2, 2);
            _server.Router.Send(_client.Map, eoData);
            RecvEoNotifyDisappearSchedule eoDisappear = new RecvEoNotifyDisappearSchedule(InstanceId, 2.0F);
            _server.Router.Send(_client.Map, eoDisappear);

            Vector3 _srcCoord = new Vector3(_client.Character.X, _client.Character.Y, _client.Character.Z);
            Recv8D92 effectMove = new Recv8D92(_srcCoord, trgCoord, InstanceId, _client.Character.skillStartCast, 3000,
                2, 2); // ToDo need real velocities
            _server.Router.Send(_client.Map, effectMove);

            RecvDataNotifyEoData eoTriggerData = new RecvDataNotifyEoData(_client.Character.InstanceId,
                _targetInstanceId, effectId, _srcCoord, 2, 2);
            _server.Router.Send(_client.Map, eoTriggerData);

            RecvBattleReportDamageHp brHp = new RecvBattleReportDamageHp(_targetInstanceId, damage);
            RecvObjectHpPerUpdateNotify oHpUpdate = new RecvObjectHpPerUpdateNotify(_targetInstanceId, perHp);
            RecvBattleReportNotifyHitEffect brHit = new RecvBattleReportNotifyHitEffect(_targetInstanceId);

            brList.Add(brStart);
            brList.Add(brHp);
            brList.Add(oHpUpdate);
            brList.Add(brHit);
            brList.Add(brEnd);
            brList.Add(oHpUpdate);
            _server.Router.Send(_client.Map, brList);
        }
    }
}
