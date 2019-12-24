using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.Skills;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Necromancy.Server.Packet.Area
{
    public class send_skill_start_cast : ClientHandler
    {
        private NecServer _server;
        public send_skill_start_cast(NecServer server) : base(server)
        {
            _server = server;
        }

        public override ushort Id => (ushort) AreaPacketId.send_skill_start_cast;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int skillID = packet.Data.ReadInt32();
            int skillTarget = packet.Data.ReadInt32();
            client.Character.eventSelectReadyCode = (uint)skillTarget;
            client.Character.skillStartCast = skillID;
            int skillLookup = skillID / 1000;
            Logger.Debug($"skillTarget [{skillTarget}]  skillID [{skillID}] skillLookup [{skillLookup}]");
            var eventSwitchPerObjectID = new Dictionary<Func<int, bool>, Action>
            {
                         { x => x == 114301, () => PoisonTrap(client, skillID) },
                         { x => x == 114302, () => SpearTrap(client, skillID) },
                         { x => x == 114607, () => Stealth(client, skillID) },
                         { x => x == 113101, () => FlameArrow(client, skillID, skillTarget) }
            };

            eventSwitchPerObjectID.First(sw => sw.Key(skillLookup)).Value();

        }
        private void SpearTrap(NecClient client, int skillId)
        {
            SpearTrap spearTrap = new SpearTrap(_server, client, skillId);
            Server.Instances.AssignInstance(spearTrap);
            Logger.Debug($"spearTrap.InstanceId [{spearTrap.InstanceId}] SpearTrap skillID [{skillId}]");
            client.Character.activeSkillInstance = (int)spearTrap.InstanceId;
            client.Character.castingSkill = true;
            spearTrap.StartCast();
        }
        private void PoisonTrap(NecClient client, int skillId)
        {
            PoisonTrap poisonTrap = new PoisonTrap(_server, client, skillId);
            Server.Instances.AssignInstance(poisonTrap);
            Logger.Debug($"poisonTrap.InstanceId [{poisonTrap.InstanceId}] PoisonTrap skillID [{skillId}]");
            client.Character.activeSkillInstance = (int)poisonTrap.InstanceId;
            poisonTrap.StartCast();
        }
        private void Stealth(NecClient client, int skillId)
        {
            Stealth stealth = new Stealth(_server, client, skillId);
            Server.Instances.AssignInstance(stealth);
            client.Character.activeSkillInstance = (int)stealth.InstanceId;
            stealth.StartCast();
        }

        private void FlameArrow(NecClient client, int skillId, int skillTarget)
        {
            Vector3 charCoord = new Vector3(client.Character.X, client.Character.Y, client.Character.Z);
            FlameArrow flameArrow = new FlameArrow(_server, client, skillId, skillTarget, charCoord);
            Server.Instances.AssignInstance(flameArrow);
            client.Character.activeSkillInstance = (int)flameArrow.InstanceId;
            flameArrow.StartCast();
        }

        private void SendBattleReportSkillStartCast(NecClient client, int mySkillID)
        {
            IBuffer res4 = BufferProvider.Provide();
            res4.WriteInt32(mySkillID);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_action_skill_start_cast, res4, ServerType.Area);
        }

        private void SendEoNotifyDisappearSchedule(NecClient client, int mySkillID, float castingTime)
        {
            IBuffer res5 = BufferProvider.Provide();
            res5.WriteInt32(mySkillID);
            res5.WriteFloat(castingTime);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_eo_notify_disappear_schedule, res5, ServerType.Area);

        }
        private void SendBattleReportStartNotify(NecClient client)
        {
            IBuffer res4 = BufferProvider.Provide();
            res4.WriteInt32(client.Character.InstanceId);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_start_notify, res4, ServerType.Area);
        }
        private void SendBattleReportEndNotify(NecClient client)
        {
            IBuffer res4 = BufferProvider.Provide();
            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_end_notify, res4, ServerType.Area);
        }

        private void SendSkillStartCast(NecClient client,int mySkillID,int mySkillTarget, float castingTime)
        {
            Logger.Debug($"Skill Int : {mySkillID}");
            Logger.Debug($"Target Int : {mySkillTarget}");
            Logger.Debug($"my Character ID : {client.Character.Id}");
            Logger.Debug($"my Character instanceId : {client.Character.InstanceId}");


            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);//Error check     | 0 - success  
            /* 
            SKILLCAST_FAILED	-1	You failed to cast <skill name>
            SKILLCAST_FAILED	-236	This skill cannot be used in town
            SKILLCAST_FAILED	-1300	Not enough HP
            SKILLCAST_FAILED	-1301	Not enough MP
            SKILLCAST_FAILED	-1302	Not enough OD
            SKILLCAST_FAILED	-1303	Not enough GP
            SKILLCAST_FAILED	-1304	Action failed since it is not ready
            SKILLCAST_FAILED	-1305	Skill cannot be used because you have not drawn your sword
            SKILLCAST_FAILED	-1306	Skill cannot be used because you are casting
            SKILLCAST_FAILED	-1307	Not enough <skill cost name>
            SKILLCAST_FAILED	-1308	Max traps laid
            SKILLCAST_FAILED	-1309	Skill cannot be used because it is already used in a trap
            SKILLCAST_FAILED	-1310	Skill cannot be used because an enemy is in range of the trap
            SKILLCAST_FAILED	-1311	No target to be added
            SKILLCAST_FAILED	-1312	No more locations can be added for this skill
            SKILLCAST_FAILED	-1320	Second trap has already been set
            SKILLCAST_FAILED	-1321	End trap has already been set
            SKILLCAST_FAILED	-1322	Ineligible target
            SKILLCAST_FAILED	-1325	Insufficient usage count for Power Level
            SKILLCAST_FAILED	-1326	You've used a skill that hasn't been set to a custom slot
            SKILLCAST_FAILED	-1327	Unable to use since character level is low
            SKILLCAST_FAILED	GENERIC	Error: <errcode>
            SKILLCAST_FAILED	5	You can not make any more <skill cost name>

            */

            res.WriteFloat(castingTime);//Casting time (countdown before auto-cast)    ./Skill_base.csv   Column I             
            Router.Send(client, (ushort) AreaPacketId.recv_skill_start_cast_r, res, ServerType.Area); 

        }

        private void SendSkillStartCastSelf(NecClient client, int mySkillID,int mySkillTarget, float castingTime)
        {
            Logger.Debug($"Skill Int : {mySkillID}");
            Logger.Debug($"Target Int : {mySkillTarget}");
            Logger.Debug($"my Character ID : {client.Character.Id}");
            Logger.Debug($"my Character instanceId : {client.Character.InstanceId}");
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(mySkillID); //previously Skill ID
            res.WriteFloat(castingTime);
            Router.Send(client, (ushort) AreaPacketId.recv_skill_start_cast_self, res, ServerType.Area);
        }

        private void SendSkillStartCastExR(NecClient client,int mySkillID,int mySkillTarget, float castingTime)
        {
            Logger.Debug($"Skill Int : {mySkillID}");
            Logger.Debug($"Target Int : {mySkillTarget}");
            Logger.Debug($"my Character ID : {client.Character.Id}");
            Logger.Debug($"my Character instanceId : {client.Character.InstanceId}");
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);//Error check     | 0 - success  See other codes above in SendSkillStartCast
            res.WriteFloat(castingTime);//casting time (countdown before auto-cast)    ./Skill_base.csv   Column L

            res.WriteInt32(100);//Cast Script?     ./Skill_base.csv   Column T
            res.WriteInt32(100);//Effect Script    ./Skill_base.csv   Column V
            res.WriteInt32(100);//Effect ID?   ./Skill_base.csv   Column X 
            res.WriteInt32(0100);//Effect ID 2     ./Skill_base.csv   Column Z 

            res.WriteInt32(mySkillID);//

            res.WriteInt32(10000);//Distance?              ./Skill_base.csv   Column AN 
            res.WriteInt32(10000);//Height?                 ./Skill_base.csv   Column AO 
            res.WriteInt32(500);//??                          ./Skill_base.csv   Column AP 
            res.WriteInt32(client.Character.Heading);//??                       ./Skill_base.csv   Column AQ 

            res.WriteInt32(5);// Effect time?

            Router.Send(client, (ushort) AreaPacketId.recv_skill_start_cast_ex_r, res, ServerType.Area);  
        }

    }
}
