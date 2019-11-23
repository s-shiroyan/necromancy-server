using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System.Threading;
using System;
using Necromancy.Server.Common.Instance;

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

            int myTargetID = packet.Data.ReadInt32();

            float X = packet.Data.ReadFloat();
            float Y = packet.Data.ReadFloat();
            float Z = packet.Data.ReadFloat();
         
            int errcode = packet.Data.ReadInt32();

            Logger.Debug($"myTargetID : {myTargetID}");
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
            res.WriteFloat(4);//Cool time      ./Skill_base.csv   Column J 
            res.WriteFloat(1);//Rigidity time  ./Skill_base.csv   Column L  
            Router.Send(client.Map, (ushort)AreaPacketId.recv_skill_exec_r, res, ServerType.Area);

            IInstance instance = Server.Instances.GetInstance((uint)myTargetID);

            //if (myTargetID != 0)
            {
                

                switch (instance)
                {
                    case NpcSpawn npcSpawn:
                        Logger.Debug($"NPCId: {npcSpawn.Id} is gettin blasted by Skill Effect {client.Character.skillStartCast}");
                        X = npcSpawn.X;
                        Y = npcSpawn.Y;
                        Z = npcSpawn.Z;
                        break;
                    case MonsterSpawn monsterSpawn:
                        Logger.Debug($"MonsterId: {monsterSpawn.Id} is gettin blasted by Skill Effect {client.Character.skillStartCast}");
                        X = monsterSpawn.X;
                        Y = monsterSpawn.Y;
                        Z = monsterSpawn.Z;
                        break;
                    case Character character:
                        Logger.Debug($"CharacterId: {character.Id} is gettin blasted by Skill Effect {client.Character.skillStartCast}");
                        X = character.X;
                        Y = character.Y;
                        Z = character.Z;
                        break;
                    default:
                        Logger.Error($"Instance with InstanceId: {instance.InstanceId} does not exist.  the ground is gettin blasted");
                        break;
                }
            }



            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(Server.Instances.CreateInstance<Skill>().InstanceId); // 0 = nothing, 1 = activate effect. //Somehow this is effect Instance ID....
            res2.WriteFloat(X);//Effect Object X
            res2.WriteFloat(Y);//Effect Object y
            res2.WriteFloat(Z+100);//Effect Object z

            //orientation, and animation speed related?
            res2.WriteFloat(500);//Not X
            res2.WriteFloat(500);//Not Y
            res2.WriteFloat(500);//Not Z

            res2.WriteInt32(client.Character.skillStartCast);// effect id
            res2.WriteInt32(client.Character.InstanceId); //unknown
            res2.WriteInt32(myTargetID);//unknown

            res2.WriteInt32(1);
            Router.Send(client, (ushort)AreaPacketId.recv_data_notify_eo_data, res2, ServerType.Area);

            ////////////////////Battle testing below this line.

            //Delete all this. it was just for fun. and an example for how we impact targets with other more proper recvs.
            IBuffer res3 = BufferProvider.Provide();
            res3.WriteInt32(instance.InstanceId);
            //Router.Send(client, (ushort)AreaPacketId.recv_object_disappear_notify, res3, ServerType.Area);


            IBuffer res4 = BufferProvider.Provide();
            res4.WriteInt32(instance.InstanceId);
            res4.WriteByte((byte)(Util.GetRandomNumber(0,70))); // % hp remaining of target.  need to store current NPC HP and OD as variables to "attack" them
            Router.Send(client, (ushort)AreaPacketId.recv_object_hp_per_update_notify, res4, ServerType.Area);





        }




    }
}
