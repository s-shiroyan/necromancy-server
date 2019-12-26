using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.Skills;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Tasks;
using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;

namespace Necromancy.Server.Packet.Area
{
    public class send_skill_exec : ClientHandler
    {
        public send_skill_exec(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_skill_exec;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int targetId = packet.Data.ReadInt32();
            int skillId = client.Character.skillStartCast;
            float X = packet.Data.ReadFloat();
            float Y = packet.Data.ReadFloat();
            float Z = packet.Data.ReadFloat();
         
            int errcode = packet.Data.ReadInt32();

            Logger.Debug($"myTargetID : {targetId}");
            Logger.Debug($"Target location : X-{X}Y-{Y}Z-{Z}");
            Logger.Debug($"ErrorCode : {errcode}");
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(errcode);//see sys_msg.csv
            /*
                -1      Unable to use skill
                -1322   Incorrect target
                -1325   Insufficient usage count for Power Level
                1       Not enough distance
                GENERIC Unable to use skill: < errcode >
            */
            res.WriteFloat(2);//Cool time      ./Skill_base.csv   Column J 
            res.WriteFloat(1);//Rigidity time  ./Skill_base.csv   Column L  
            Router.Send(client.Map, (ushort)AreaPacketId.recv_skill_exec_r, res, ServerType.Area);

            int skillLookup = skillId / 1000;
            var eventSwitchPerObjectID = new Dictionary<Func<int, bool>, Action>
                        {
                         { x => x == 114301, () => PoisonTrap(client, skillId) },
                         { x => x == 114302, () => SpearTrap(client, skillId) },
                         { x => x == 114607, () => Stealth(client, skillId) },
                         { x => x == 113101, () => FlameArrow(client, skillId, targetId) }
                        };

            eventSwitchPerObjectID.First(sw => sw.Key(skillLookup)).Value();
            client.Character.castingSkill = false;



            ////////////////////Battle testing below this line.

            //Delete all this. it was just for fun. and an example for how we impact targets with other more proper recvs.
            //IBuffer res3 = BufferProvider.Provide();
            //res3.WriteInt32(instance.InstanceId);
            //Router.Send(client, (ushort)AreaPacketId.recv_object_disappear_notify, res3, ServerType.Area);


            //IBuffer res4 = BufferProvider.Provide();
            //res4.WriteInt32(instance.InstanceId);
            //res4.WriteByte((byte)(Util.GetRandomNumber(0,70))); // % hp remaining of target.  need to store current NPC HP and OD as variables to "attack" them
            //Router.Send(client, (ushort)AreaPacketId.recv_object_hp_per_update_notify, res4, ServerType.Area);





        }

        private void PoisonTrap(NecClient client, int skillId)
        {
            PoisonTrap poisonTrap = (PoisonTrap)Server.Instances.GetInstance((uint)client.Character.activeSkillInstance);
            Logger.Debug($"poisonTrap.InstanceId [{poisonTrap.InstanceId}]  skillId [{skillId}]");
            poisonTrap.SkillExec();
        }
        private void SpearTrap(NecClient client, int skillId)
        {
            SpearTrap spearTrap = (SpearTrap)Server.Instances.GetInstance((uint)client.Character.activeSkillInstance);
            Logger.Debug($"spearTrap.InstanceId [{spearTrap.InstanceId}]  skillId [{skillId}]");
            spearTrap.SkillExec();
            Vector3 trapPos = new Vector3(client.Character.X, client.Character.Y, client.Character.Z);
            TrapTask trapTask = new TrapTask(Server, client.Map, trapPos, (int)client.Character.InstanceId, 30000);
            trapTask.AddTrap(0,spearTrap);
            trapTask.Start();
        }

        private void Stealth(NecClient client, int skillId)
        {
            Stealth stealth = (Stealth)Server.Instances.GetInstance((uint)client.Character.activeSkillInstance);
            stealth.SkillExec();
        }

        private void FlameArrow(NecClient client, int skillId, int targetId)
        {
            FlameArrow flameArrow = (FlameArrow)Server.Instances.GetInstance((uint)client.Character.activeSkillInstance);
            flameArrow.SkillExec();
        }
    }
}
