using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System.Threading;
using System;
using System.Threading.Tasks;

namespace Necromancy.Server.Packet.Area
{
    public class send_skill_start_cast : ClientHandler
    {
        public send_skill_start_cast(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_skill_start_cast;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int mySkillID = packet.Data.ReadInt32();
            int mySkillTarget = packet.Data.ReadInt32();
            client.Character.eventSelectReadyCode = (uint)mySkillTarget;
#pragma warning disable CS0219 // Variable is assigned but its value is never used
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            int CastingTime = 3;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning restore CS0219 // Variable is assigned but its value is never used

            if (mySkillTarget > 0 && mySkillTarget < 991024) // the range is for all monsters. but there's no reason to have a cast specific to monsters.   Logic TBD maybe something with Skill_sort.CSV
            {   SendSkillStartCast(client,mySkillID,mySkillTarget);    }

            if (mySkillTarget == 0) // self cast skills 0 out your our target ID, even if you have something targeted.
            {   SendSkillStartCastSelf(client,mySkillID,mySkillTarget);    }

            if (mySkillTarget > 9910024) // All NPCs have Serial ID's of 10,000,000 or greater.  all Monsters are 9910024 or less. most are only 6 digits. 9 digit monster ID's are for testing.
            { SendSkillStartCastExR(client, mySkillID, mySkillTarget); }

            //To Do.  Identify SkillID or Target ID numbering convention that specifies 1.)NPC 2.)Monster 3.)self 4.)party 5.)item.   this logic will determine which recv to direct send_skill_start_cast to above.


            //recv_skill_start_item_cast_r // To-Do .  after Items exist,  start casting based on item i.e. camp.

            //Task.Delay(TimeSpan.FromMilliseconds((int)(CastingTime * 1000))).ContinueWith (t1 => { SendSkillStartCastExR(client, mySkillID, mySkillTarget); });
            //Task.Delay(TimeSpan.FromMilliseconds((int)(CastingTime * 1000 * 2))).ContinueWith(t1 => { SendSkillExecR(client, mySkillID, mySkillTarget); });

        }

        private void SendSkillStartCast(NecClient client,int mySkillID,int mySkillTarget)
        {
            Console.WriteLine($"Skill Int : {mySkillID}");
            Console.WriteLine($"Target Int : {mySkillTarget}");
            Console.WriteLine($"my Character ID : {client.Character.Id}");
            float CastingTime = 2;

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

            res.WriteFloat(CastingTime);//Casting time (countdown before auto-cast)    ./Skill_base.csv   Column I             
            Router.Send(client, (ushort) AreaPacketId.recv_skill_start_cast_r, res, ServerType.Area); 

        }

        private void SendSkillStartCastSelf(NecClient client, int mySkillID,int mySkillTarget)
        {
            Console.WriteLine($"Skill Int : {mySkillID}");
            Console.WriteLine($"Target Int : {mySkillTarget}");
            Console.WriteLine($"my Character ID : {client.Character.Id}");
            float CastingTime = 2;
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(mySkillID); //previously Skill ID
            res.WriteFloat(CastingTime);
            Router.Send(client, (ushort) AreaPacketId.recv_skill_start_cast_self, res, ServerType.Area);
        }

        private void SendSkillStartCastExR(NecClient client,int mySkillID,int mySkillTarget)
        {
            Console.WriteLine($"Skill Int : {mySkillID}");
            Console.WriteLine($"Target Int : {mySkillTarget}");
            Console.WriteLine($"my Character ID : {client.Character.Id}");
            float CastingTime = 2;
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);//Error check     | 0 - success  See other codes above in SendSkillStartCast
            res.WriteFloat(CastingTime);//casting time (countdown before auto-cast)    ./Skill_base.csv   Column L

            res.WriteInt32(4);//Cast Script?     ./Skill_base.csv   Column T
            res.WriteInt32(7);//Effect Script    ./Skill_base.csv   Column V
            res.WriteInt32(1);//Effect ID?   ./Skill_base.csv   Column X 
            res.WriteInt32(0);//Effect ID 2     ./Skill_base.csv   Column Z 

            res.WriteInt32(mySkillID);//

            res.WriteInt32(1000);//Distance?              ./Skill_base.csv   Column AN 
            res.WriteInt32(200);//Height?                 ./Skill_base.csv   Column AO 
            res.WriteInt32(0);//??                          ./Skill_base.csv   Column AP 
            res.WriteInt32(0);//??                       ./Skill_base.csv   Column AQ 

            res.WriteInt32(15);// Effect time?

            Router.Send(client, (ushort) AreaPacketId.recv_skill_start_cast_ex_r, res, ServerType.Area);  
        }

    }
}
