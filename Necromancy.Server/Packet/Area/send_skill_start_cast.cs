using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System.Threading;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_skill_start_cast : Handler
    {
        bool skillExec = false;
        public send_skill_start_cast(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_skill_start_cast;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int mySkillID = packet.Data.ReadInt32();
            int mySkillTarget = packet.Data.ReadInt32();
            
            float mySkillEffectTime = 15;
            SendSkillStartCast(client,mySkillID,mySkillEffectTime);
            //SendSkillStartCastSelf(client,mySkillID);
            SendSkillStartCastExR(client,mySkillID,mySkillEffectTime);
            if(skillExec)
                SendSkillExecR(client,mySkillID,mySkillEffectTime,mySkillTarget);
                
            
            //SendSkillStartCast(client);
            //SendSkillStartCastSelf(client);
           // SendSkillStartCastExR(client);
            //if(skillExec)
              //  SendSkillExecR(client);

           

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(mySkillID);
            res.WriteFloat(0);

            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);

            res.WriteInt32(0);

            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);

            res.WriteInt32(0);

            Router.Send(client.Map, (ushort)AreaPacketId.recv_skill_start_cast_ex_r, res, client);
            
        }

        private void SendSkillStartCast(NecClient client,int mySkillTarget,float mySkillEffectTime)
        {
            IBuffer res = BufferProvider.Provide();
            IBuffer res2 = BufferProvider.Provide();
            res.WriteInt32(mySkillTarget);
            res.WriteFloat(mySkillEffectTime);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_skill_start_cast_r, res, client);
            /////////////////////////////////////
            res2.WriteInt32(client.Character.Id);
            res2.WriteFloat(10);
            Router.Send(client, (ushort) AreaPacketId.recv_skill_start_cast_self, res2);
        }
        private void SendSkillStartCastSelf(NecClient client, int mySkillTarget)
        {

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(mySkillTarget);
            res.WriteFloat(100);
            Router.Send(client, (ushort) AreaPacketId.recv_skill_start_cast_self, res);
        }
        private void SendSkillStartCastExR(NecClient client,int mySkillTarget,float mySkillEffectTime)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(mySkillTarget);
            res.WriteFloat(mySkillEffectTime);

            res.WriteInt32(0x3910);
            res.WriteInt32(0x3911);
            res.WriteInt32(0x3912);
            res.WriteInt32(0x3913);

            res.WriteInt32(0x30D5);

            res.WriteInt32(0x3914);
            res.WriteInt32(0x3915);
            res.WriteInt32(0x3916);
            res.WriteInt32(0x3917);

            res.WriteInt32(0x30D5);

            //Thread.Sleep(1 );
            skillExec = true;
        }

        private void SendSkillExecR(NecClient client,int mySkillID,float mySkillEffectTime,int mySkillTarget)
        {
            Console.WriteLine($"Float : {mySkillEffectTime}");
            Console.WriteLine($"Skill Int : {mySkillID}");
            Console.WriteLine($"Target Int : {mySkillTarget}");
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);//1 - not enough distance, 2 or above - unable to use skill: 2 error, 0 - success
            res.WriteFloat(1);//re-cast/Cooldown time in seconds
            res.WriteFloat(1);//Character Freeze time in seconds. (stiffen time?).  you can't move for this long after casting.  cast time?
            Router.Send(client, (ushort) AreaPacketId.recv_skill_exec_r, res);
            //Console.WriteLine($"res contents : {res}");
            //ThisTest(client);
            
            skillExec = true;
        }

        ////////////everything below this line is for testing other recvs.  should be removed before commit to master.
        
        int testIntIcrement = 0;
        private void ThisTest(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            IBuffer res2 = BufferProvider.Provide();
            //res.WriteInt64(10800405);
            res.WriteInt32(50100302); //Camp 50100302
            res.WriteFloat(1);
            res2.WriteInt32(888888888);
            
            Console.WriteLine($"This is a test : {client}");
            Router.Send(client, (ushort) AreaPacketId.recv_item_use_r, res);
            Router.Send(client, (ushort) AreaPacketId.recv_chara_update_hp, res2);
            testIntIcrement+=9; 
            //SendDataNotifyNpcData(client, testIntIcrement);
            
            
        }
 

        


        private void SendDataNotifyNpcData(NecClient client,int TestInt)
        {
            int x = -1;    
            
                   
            int[] NPCModelID = new int[] { 1911105, 1112101, 1122401, 1122101, 1311102, 1111301, 1121401, 1131401, 2073002, 1421101 };
            int[] NPCSerialID = new int[] { 10000101, 10000102, 10000103, 10000104, 10000105, 10000106, 10000107, 10000108, 80000009, 10000101 };
            int[] NPCX = new int[] { 1100, 1200, 1300, 1400, 1500, 1600, 1700, 1800, 1900, 1000 };
            int[] NPCY = new int[] { 0 , -100, -200, -300, 100, 200, 300, 400, 0, 500 };
            int[] NPCZ = new int[] { 25, -25, -25, 25, 25, -22, -25, 25, 0, 0 };
            int[] TestArrayInt = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            byte[] NPCViewAngle = new byte[] { 40, 41, 42, 43, 44, 45, 46, 47, 48, 49 };

            foreach (int y in NPCModelID)
            {
                x++;
                IBuffer res2 = BufferProvider.Provide();
                res2.WriteInt32(x+44444444);             // NPC ID (object id)

                res2.WriteInt32(237);      // NPC Serial ID from "npc.csv"

                res2.WriteByte(0);              // 0 - Clickable NPC (Active NPC, player can select and start dialog), 1 - Not active NPC (Player can't start dialog)

                res2.WriteCString($"NPC Name : {x}");//Name

                res2.WriteCString($"Emoticon : {TestArrayInt[x]+TestInt}");//Title

                res2.WriteFloat(NPCX[x]);//X Pos
                res2.WriteFloat(NPCY[x]);//Y Pos
                res2.WriteFloat(NPCZ[x]);//Z Pos
                res2.WriteByte(NPCViewAngle[x]);//view offset

                res2.WriteInt32(19);

                //this is an x19 loop but i broke it up
                res2.WriteInt32(24);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);

                res2.WriteInt32(19);


                int numEntries = 19;


                for (int i = 0; i < numEntries; i++)

                {
                    // loop start
                    res2.WriteInt32(210901); // this is a loop within a loop i went ahead and broke it up
                    res2.WriteByte(0);
                    res2.WriteByte(0);
                    res2.WriteByte(0);

                    res2.WriteInt32(10310503);
                    res2.WriteByte(0);
                    res2.WriteByte(0);
                    res2.WriteByte(0);

                    res2.WriteByte(0);
                    res2.WriteByte(0);
                    res2.WriteByte(1); // bool
                    res2.WriteByte(0);
                    res2.WriteByte(0);
                    res2.WriteByte(0);
                    res2.WriteByte(0);
                    res2.WriteByte(0);

                }

                res2.WriteInt32(19);

                //this is an x19 loop but i broke it up
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);

                res2.WriteInt32(NPCModelID[x]);   //NPC Model from file "model_common.csv"
                


                res2.WriteInt16(100);       //NPC Model Size

                res2.WriteByte(237);

                res2.WriteByte(237);

                res2.WriteByte(237);

                res2.WriteInt32(237);

                res2.WriteInt32(TestArrayInt[x]+TestInt); //npc Emoticon above head

                res2.WriteInt32(237);
                res2.WriteFloat(1000);
                res2.WriteFloat(1000);
                res2.WriteFloat(1000);

                res2.WriteInt32(128);

                int numEntries2 = 128;


                for (int i = 0; i < numEntries2; i++)

                {
                    res2.WriteInt32(237);
                    res2.WriteInt32(237);
                    res2.WriteInt32(237);

                }

                Router.Send(client, (ushort)AreaPacketId.recv_data_notify_npc_data, res2);
            }
        }

    }
}