using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System.Threading;
using System;
using System.Threading.Tasks;

namespace Necromancy.Server.Packet.Area
{
    public class send_skill_start_cast : Handler
    {
        public send_skill_start_cast(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_skill_start_cast;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int mySkillID = packet.Data.ReadInt32();
            int mySkillTarget = packet.Data.ReadInt32();
            int CastingTime = 3;

            if (mySkillTarget != 0)
            {   SendSkillStartCast(client,mySkillID,mySkillTarget);    }
            else
            {   SendSkillStartCastSelf(client,mySkillID,mySkillTarget);    }

            //To Do.  Identify SkillID or Target ID numbering convention that specifies 1.)NPC 2.)Monster 3.)self 4.)party 5.)item.   this logic will determine which recv to direct send_skill_start_cast to.

            //SendSkillStartCastExR(client,mySkillID,mySkillTarget); // TO-DO  logic for when to use this
            //recv_skill_start_item_cast_r // To-Do .  after Items exist,  start casting based on item i.e. camp.
            //This is only here for testing. The delay gives the above sends/recvs time to process   :To Do- write 'send_skill_exec.cs'
            Task.Delay(TimeSpan.FromMilliseconds((int)(CastingTime * 1000))).ContinueWith (t1 => {SendSkillExecR(client,mySkillID,mySkillTarget);});           
 
        }

        private void SendSkillStartCast(NecClient client,int mySkillID,int mySkillTarget)
        {
            Console.WriteLine($"Skill Int : {mySkillID}");
            Console.WriteLine($"Target Int : {mySkillTarget}");
            Console.WriteLine($"my Character ID : {client.Character.Id}");
            float CastingTime = 2;
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);//Error check     | 0 - success  1 or above "Error : %int32%"
            res.WriteFloat(CastingTime);//Casting time (countdown before auto-cast)    ./Skill_base.csv   Column I 
            //Router.Send(client.Map, (ushort) AreaPacketId.recv_skill_start_cast_r, res, client);                     
            Router.Send(client, (ushort) AreaPacketId.recv_skill_start_cast_r, res); 

        }

        private void SendSkillStartCastSelf(NecClient client, int mySkillID,int mySkillTarget)
        {
            Console.WriteLine($"Skill Int : {mySkillID}");
            Console.WriteLine($"Target Int : {mySkillTarget}");
            Console.WriteLine($"my Character ID : {client.Character.Id}");
            float CastingTime = 2;
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(mySkillID);
            res.WriteFloat(CastingTime);
            Router.Send(client, (ushort) AreaPacketId.recv_skill_start_cast_self, res);
        }

        private void SendSkillStartCastExR(NecClient client,int mySkillID,int mySkillTarget)
        {
            Console.WriteLine($"Skill Int : {mySkillID}");
            Console.WriteLine($"Target Int : {mySkillTarget}");
            Console.WriteLine($"my Character ID : {client.Character.Id}");
            float CastingTime = 2;
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);//Error check     | 0 - success  1 or above "Error : %int32%"
            res.WriteFloat(CastingTime);//Casting time (countdown before auto-cast)    ./Skill_base.csv   Column I 

            res.WriteInt32(50100104);//Consumable item 1??     ./Skill_base.csv   Column T
            res.WriteInt32(50100104);//Consumable item 2??     ./Skill_base.csv   Column V
            res.WriteInt32(50100104);//Consumable item 3??     ./Skill_base.csv   Column X 
            res.WriteInt32(50100104);//Consumable item 4??     ./Skill_base.csv   Column Z 

            res.WriteInt32(mySkillID);//

            res.WriteInt32(50100104);//Number of items1??      ./Skill_base.csv   Column U 
            res.WriteInt32(50100104);//Number of items1??      ./Skill_base.csv   Column W 
            res.WriteInt32(50100104);//Number of items1??      ./Skill_base.csv   Column Y 
            res.WriteInt32(50100104);//Number of items1??      ./Skill_base.csv   Column AA 

            res.WriteInt32(mySkillTarget);//

            Router.Send(client, (ushort) AreaPacketId.recv_skill_start_cast_ex_r, res);  
        }





        ////This should be sent from the client, the game has a send string network::proto_area_implement_client::send_skill_exec
        private void SendSkillExecR(NecClient client,int mySkillID,int mySkillTarget)
        {
            //Console.WriteLine($"Float : {mySkillEffectTime}");
            Console.WriteLine($"Skill Int : {mySkillID}");
            Console.WriteLine($"Target Int : {mySkillTarget}");
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);//Error check     | 1 - not enough distance, 2 or above - unable to use skill: 2 error, 0 - success
            res.WriteFloat(1);//Cool time      ./Skill_base.csv   Column J 
            res.WriteFloat(1);//Rigidity time  ./Skill_base.csv   Column L  
            Router.Send(client, (ushort) AreaPacketId.recv_skill_exec_r, res);
            
        }

    }
}