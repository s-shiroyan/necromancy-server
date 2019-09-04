using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_map_enter : Handler
    {
        public send_map_enter(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_map_enter;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteByte(0);//Bool

            Router.Send(client, (ushort) AreaPacketId.recv_map_enter_r, res);

            SendDataNotifyCharaData(client);
        }

        private void SendDataNotifyCharaData(NecClient client)
        {
            IBuffer res3 = BufferProvider.Provide();

            //sub_read_int32

            res3.WriteInt32(client.Character.Id);//Character ID

            //sub_481AA0
            res3.WriteCString("soulname");

            //sub_481AA0
            res3.WriteCString("charaname");

            //sub_484420
            res3.WriteFloat(-5516);//X Pos
            res3.WriteFloat(-3896);//Y Pos
            res3.WriteFloat(0);//Z Pos
            res3.WriteByte(180);//view offset

            //sub_read_int32
            res3.WriteInt32(6);

            //sub_483420

            res3.WriteInt32(0);//Character pose? 6 = guard, 8 = invisible,

            //sub_483470
            res3.WriteInt16(0);

            //sub_483420
            int numEntries = 19;
            res3.WriteInt32(numEntries);//has to be less than 19(defines how many int32s to read?)

            //ItemType Select See str_table SubID 121 for Item Type info. Increment by +1
            int Armor = 25;         //Armor 25
            int Accessory = 27;     //Accessory 26
            int Shield = 21;        //Shield 19-21
            int Weapon = 4;         //0 Knuckle, 1 Dagger, 3 1hSword, 7 1h axe (broken), 8 2hAxe, 9 spear, 10 blunt, 13 staff, 15 crossbow
                                     //sub_483660 
            res3.WriteInt32(Weapon); //18	    				
            res3.WriteInt32(Shield); //17 	    		
            res3.WriteInt32(Armor); //16	        			
            res3.WriteInt32(Armor); //15	        				
            res3.WriteInt32(Armor); //14	        			
            res3.WriteInt32(Armor); //13	        			
            res3.WriteInt32(Armor); //12	        				
            res3.WriteInt32(Accessory); //11	  	
            res3.WriteInt32(Accessory); //10	    			
            res3.WriteInt32(Accessory); //9	    		
            res3.WriteInt32(Accessory); //8	    			
            res3.WriteInt32(Accessory); //7	    			
            res3.WriteInt32(Armor); //6          				
            res3.WriteInt32(Armor); //5          		
            res3.WriteInt32(Armor); //4	        					
            res3.WriteInt32(Armor); //3	        				
            res3.WriteInt32(Armor); //2          				
            res3.WriteInt32(Shield + 1); //1       					
            res3.WriteInt32(22);  //0 


            //int[] EquipId = new int[] {15200601,15200601/*Shield* */,20000101/*Quiver*/,100101,200101,300101,400101,500101,690101/*Cape*/
            //,30102401,30200103,30300101,30400112,70000201/*talkring*/,160801,260801,360801,460801,10800405/*Weapon*/ };


            //sub_483420
            numEntries = 19;
            res3.WriteInt32(numEntries);//has to be less than 19
            int x = 0;
            int[] EquipId = new int[19];

            string CharacterSet = "";

            switch (CharacterSet)
            {
                case "Xeno":
                    EquipId = new int[] {10800405/*Weapon*/,15200702/*Shield* */,260103/*Torso*/,110504/*head*/,360103/*legs*/,460103/*Arms*/,560103/*Feet*/,690101,690101/*Cape*/
                    ,690101,690101,690101,261401/*Avatar Torso*/,561401/*Avatar Feet*/,461401/*Avatar Arms */,361401/*Avatar Legs*/,161401/*Avatar Head*/,690101,20000101/*Weapon Related*/ };
                    break;
                case "Kadred":
                    EquipId = new int[] {10800405/*Weapon*/,15100901/*Shield* */,260103/*Torso*/,110504/*head*/,360103/*legs*/,460103/*Arms*/,560103/*Feet*/,690101,690101/*Cape*/
                    ,690101,690101,690101,260801/*Avatar Torso*/,560801/*Avatar Feet*/,460801/*Avatar Arms */,360801/*Avatar Legs*/,160801/*Avatar Head*/,690101,20000101/*Weapon Related*/ };
                    break;
                case "Zenkato":
                    EquipId = new int[] {11400403/*Weapon*/,0/*Shield* */,260103/*Torso*/,110504/*head*/,360103/*legs*/,460103/*Arms*/,510301/*Feet*/,690101,690101/*Cape*/
                    ,690101,690101,690101,260801/*Avatar Torso*/,510301/*Avatar Feet*/,460801/*Avatar Arms */,360801/*Avatar Legs*/,100403/*Avatar Head*/,690101,20000101/*Weapon Related*/ };
                    break;
                case "Ipa":
                    EquipId = new int[] {11300506/*Weapon*/,15100901/*Shield* */,260103/*Torso*/,110504/*head*/,360103/*legs*/,460103/*Arms*/,560103/*Feet*/,690101,690101/*Cape*/
                    ,690101,690101,690101,00252401/*Avatar Torso*/,560801/*Avatar Feet*/,460801/*Avatar Arms */,360801/*Avatar Legs*/,121901/*Avatar Head*/,690101,20000101/*Weapon Related*/ };
                    break;
                default:
                    EquipId = new int[] {10800405/*Weapon*/,15200702/*Shield* */,260103/*Torso*/,110504/*head*/,360103/*legs*/,460103/*Arms*/,560103/*Feet*/,690101,690101/*Cape*/
                    ,690101,690101,690101,261401/*Avatar Torso*/,561401/*Avatar Feet*/,461401/*Avatar Arms */,361401/*Avatar Legs*/,161401/*Avatar Head*/,690101,20000101/*Weapon Related*/ };
                    break;
            }

            //sub_4948C0
            for (int i = 0; i < numEntries; i++)
            {

                res3.WriteInt32(EquipId[x]);//???
                res3.WriteByte(0);
                res3.WriteByte(0);
                res3.WriteByte(0); //0  ????


                res3.WriteInt32(12341234);//???
                res3.WriteByte(0); //
                res3.WriteByte(4); //
                res3.WriteByte(1); //
                x++;

                res3.WriteByte(00);// Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
                res3.WriteByte(00); //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
                res3.WriteByte(4); // testing
                res3.WriteByte(4); // testing
                res3.WriteByte(4); // testing
                res3.WriteByte(4); // testing
                res3.WriteByte(4); //Alternate texture for item model 
                res3.WriteByte(4); // seperate in assembly

            }

            //sub_483420
            numEntries = 19;//influences a loop that needs to be under 19
            res3.WriteInt32(numEntries);

            int rr = 000;
            //sub_483420   // 2 shield 4accessory? 8Helmet 12belt? 16torso 32 pants 48torsopants 64 hands 96handpants 128 feet 192handfeet 
            res3.WriteInt32(001); //Right Hand    //1 for weapon
            res3.WriteInt32(002); //Left Hand     //2 for Shield
            res3.WriteInt32(016); //Torso         //16 for torso
            res3.WriteInt32(008); //Head          //08 for head
            res3.WriteInt32(032); //Legs          //32 for legs
            res3.WriteInt32(064); //Arms          //64 for Arms
            res3.WriteInt32(128); //Feet          //128 for feet
            res3.WriteInt32(004); //???Cape
            res3.WriteInt32(rr); //???Ring
            res3.WriteInt32(rr); //???Earring
            res3.WriteInt32(rr); //???Necklace
            res3.WriteInt32(rr); //???Belt
            res3.WriteInt32(016); //Avatar Torso
            res3.WriteInt32(128); //Avatar Feet
            res3.WriteInt32(064); //Avatar Arms
            res3.WriteInt32(032); //Avatar Legs
            res3.WriteInt32(008); //Avatar Head  
            res3.WriteInt32(004); //???
            res3.WriteInt32(000); //Right Hand 

            //sub_4835C0
            res3.WriteInt32(0);//1 here means crouching?

            //sub_484660
            res3.WriteInt32(4);//race
            res3.WriteInt32(1);//gender
            res3.WriteByte(2);//hair
            res3.WriteByte(3);//face
            res3.WriteByte(4);//hair color

            //sub_483420
            res3.WriteInt32(0); // party id?

            //sub_4837C0
            res3.WriteInt32(0); // party id?

            //sub_read_byte
            res3.WriteByte(0);//Criminal name icon

            //sub_494890
            res3.WriteByte(1);//Bool Beginner Protection

            //sub_4835E0
            res3.WriteInt32(0);//pose, 1 = sitting, 0 = standing

            //sub_483920
            res3.WriteInt32(0);

            //sub_483440
            res3.WriteInt16(65);

            //sub_read_byte
            res3.WriteByte(255);//no change?

            //sub_read_byte
            res3.WriteByte(255);//no change?

            //sub_read_int_32
            res3.WriteInt32(0);//title; 0 - display title, 1 - no title

            //sub_483580
            res3.WriteInt32(244);

            //sub_483420
            numEntries = 1;
            res3.WriteInt32(numEntries);//influences a loop that needs to be under 128

            //sub_485A70
            for (int i = 0; i < numEntries; i++)
            {
                res3.WriteInt32(0);
                res3.WriteInt32(0);
                res3.WriteInt32(0);
            }

            //sub_481AA0
            res3.WriteCString("");//Comment string

            Router.Send(client.Map, (ushort)AreaPacketId.recv_data_notify_chara_data, res3, client);
        }
    }
}