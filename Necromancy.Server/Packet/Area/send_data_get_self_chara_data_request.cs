using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_data_get_self_chara_data_request : Handler
    {
        public send_data_get_self_chara_data_request(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_data_get_self_chara_data_request;

        public override void Handle(NecClient client, NecPacket packet)
        {
            SendDataGetSelfCharaData(client);

            IBuffer res2 = BufferProvider.Provide();
            Router.Send(client, (ushort)AreaPacketId.recv_data_get_self_chara_data_request_r, res2);

        }

        private void SendDataGetSelfCharaData(NecClient client)
        {
                IBuffer res = BufferProvider.Provide();

                //sub_4953B0 - characteristics
                res.WriteInt32(4); //race
                res.WriteInt32(1); //gender
                res.WriteByte(0); //hair
                res.WriteByte(5); //face
                res.WriteByte(0); //color

                //sub_484720 - combat/leveling info
                res.WriteInt32(client.Character.Id);  // ? character ID maybe?
                res.WriteInt32(1); // class
                res.WriteInt16(1); // current level
                res.WriteInt64(0); // current exp
                res.WriteInt64(12); // soul exp
                res.WriteInt64(33); // exp needed to level
                res.WriteInt64(44); // soul exp needed to level
                res.WriteInt32(123); // current hp
                res.WriteInt32(100); // current mp
                res.WriteInt32(111); // current od
                res.WriteInt32(123); // max hp
                res.WriteInt32(124); // maxmp
                res.WriteInt32(189); // max od
                res.WriteInt32(164); // current gp
                res.WriteInt32(155); // map gp
                res.WriteInt32(1238); // value/100 = current weight
                res.WriteInt32(1895); // value/100 = max weight
                res.WriteByte(200); // condition

                // total stat level includes bonus'?
                res.WriteInt16(24); // str
                res.WriteInt16(28); // vit
                res.WriteInt16(35); // dex
                res.WriteInt16(89); // agi
                res.WriteInt16(42); // int
                res.WriteInt16(52); // pie
                res.WriteInt16(90); // luk

                // mag atk atrb
                res.WriteInt16(5); // fire
                res.WriteInt16(52); // water
                res.WriteInt16(58); // wind
                res.WriteInt16(45); // earth
                res.WriteInt16(33); // light
                res.WriteInt16(12); // dark
                res.WriteInt16(145); // changed nothing visably
                res.WriteInt16(85); // changed nothing visably
                res.WriteInt16(96); // changed nothing visably

                // mag def atrb
                res.WriteInt16(5); // fire
                res.WriteInt16(52); // water
                res.WriteInt16(58); // wind
                res.WriteInt16(45); // earth
                res.WriteInt16(33); // light
                res.WriteInt16(12); // dark
                res.WriteInt16(145); // changed nothing visably
                res.WriteInt16(85); // changed nothing visably
                res.WriteInt16(96); // changed nothing visably

                //status change resistance
                res.WriteInt16(215); // fire
                res.WriteInt16(99); // water
                res.WriteInt16(88); // wind
                res.WriteInt16(455); // earth
                res.WriteInt16(333); // light
                res.WriteInt16(1222); // dark
                res.WriteInt16(1445); // changed nothing visably
                res.WriteInt16(858); // changed nothing visably
                res.WriteInt16(962); // changed nothing visably
                res.WriteInt16(968); // changed nothing visably
                res.WriteInt16(9688); // changed nothing visably

                // gold and alignment?
                res.WriteInt64(214587); // gold
                res.WriteInt32(187); // changed nothing visably
                res.WriteInt32(5); // lawful
                res.WriteInt32(3); // neutral
                res.WriteInt32(1); // chaos
                res.WriteInt32(113); // changed nothing visably

                //sub_484980
                res.WriteInt32(1);// changed nothing visably
                res.WriteInt32(1);// changed nothing visably
                res.WriteInt32(1);// changed nothing visably

                // characters stats
                res.WriteInt16(24); // str
                res.WriteInt16(28); // vit
                res.WriteInt16(35); // dex
                res.WriteInt16(89); // agi
                res.WriteInt16(42); // int
                res.WriteInt16(52); // pie
                res.WriteInt16(90); // luk

                // nothing
                res.WriteInt16(51); // changed nothing visably
                res.WriteInt16(25); // changed nothing visably
                res.WriteInt16(10); // changed nothing visably
                res.WriteInt16(25); // changed nothing visably
                res.WriteInt16(87); // changed nothing visably
                res.WriteInt16(122); // changed nothing visably
                res.WriteInt16(14); // changed nothing visably
                res.WriteInt16(73); // changed nothing visably
                res.WriteInt16(69); // changed nothing visably


                // nothing
                res.WriteInt16(51); // changed nothing visably
                res.WriteInt16(25); // changed nothing visably
                res.WriteInt16(10); // changed nothing visably
                res.WriteInt16(25); // changed nothing visably
                res.WriteInt16(87); // changed nothing visably
                res.WriteInt16(122); // changed nothing visably
                res.WriteInt16(14); // changed nothing visably
                res.WriteInt16(73); // changed nothing visably
                res.WriteInt16(69); // changed nothing visably

                // nothing
                res.WriteInt16(51); // changed nothing visably
                res.WriteInt16(25); // changed nothing visably
                res.WriteInt16(10); // changed nothing visably
                res.WriteInt16(25); // changed nothing visably
                res.WriteInt16(87); // changed nothing visably
                res.WriteInt16(122); // changed nothing visably
                res.WriteInt16(14); // changed nothing visably
                res.WriteInt16(73); // changed nothing visably
                res.WriteInt16(69); // changed nothing visably
                res.WriteInt16(73); // changed nothing visably
                res.WriteInt16(69); // changed nothing visably

                //sub_484B00 map ip and connection
                res.WriteInt32(1001007);//MapSerialID
                res.WriteInt32(1001007);//MapID
                res.WriteFixedString("127.0.0.1", 65);//IP
                res.WriteInt16(60002);//Port
            
                //sub_484420
                res.WriteFloat(-5516);//X Pos
                res.WriteFloat(-3896);//Y Pos
                res.WriteFloat(0);//Z Pos
                res.WriteByte(180);//view offset

                //sub_read_int32 skill point
                res.WriteInt32(101); // skill point

                //sub_483420 character state like alive/dead/invis
                res.WriteInt32(0); //-254 GM

                //sub_494AC0
                res.WriteByte(78); // soul level
                res.WriteInt32(22); // current soul points
                res.WriteInt32(29); // changed nothing visably
                res.WriteInt32(12); // max soul points
                res.WriteByte(0); // 0 is white,1 yellow 2 red 3+ skull
                res.WriteByte(0); //Bool
                res.WriteByte(12); // changed nothing visably
                res.WriteByte(72); // changed nothing visably
                res.WriteByte(43); // changed nothing visably
                res.WriteByte(75); // changed nothing visably

                //sub_read_3-int16 unknown
                res.WriteInt16(55); // changed nothing visably
                res.WriteInt16(66); // changed nothing visably
                res.WriteInt16(77); // changed nothing visably

                //sub_4833D0
                res.WriteInt64(6);// changed nothing visably

                //sub_4833D0
                res.WriteInt64(7);// changed nothing visably

                //sub_4834A0
                res.WriteFixedString("", 97);//Shopname

                //sub_4834A0
                res.WriteFixedString("", 385);//Comment

                //sub_494890
                res.WriteByte(1);//Bool

                //sub_4834A0
                res.WriteFixedString("aaaa", 385);//Chatbox?

                //sub_494890
                res.WriteByte(1);//Bool

                //sub_483420
                int numEntries = 19;
                res.WriteInt32(numEntries);//has to be less than 19(defines how many int32s to read?)

                //ItemType Select See str_table SubID 121 for Item Type info. Increment by +1
                int Armor = 25;         //Armor 25
                int Accessory = 27;     //Accessory 26
                int Shield = 21;        //Shield 19-21
                int Weapon = 14;         //0 Knuckle, 1 Dagger, 3 1hSword, 7 1h axe (broken), 8 2hAxe, 9 spear, 10 blunt, 13 staff, 15 crossbow
                //sub_483660 
                res.WriteInt32(Weapon); //18	    				
                res.WriteInt32(Shield); //17 	    		
                res.WriteInt32(Armor); //16	        			
                res.WriteInt32(Armor); //15	        				
                res.WriteInt32(Armor); //14	        			
                res.WriteInt32(Armor); //13	        			
                res.WriteInt32(Armor); //12	        				
                res.WriteInt32(Accessory); //11	  	
                res.WriteInt32(Accessory); //10	    			
                res.WriteInt32(Accessory); //9	    		
                res.WriteInt32(Accessory); //8	    			
                res.WriteInt32(Accessory); //7	    			
                res.WriteInt32(Armor); //6          				
                res.WriteInt32(Armor); //5          		
                res.WriteInt32(Armor); //4	        					
                res.WriteInt32(Armor); //3	        				
                res.WriteInt32(Armor); //2          				
                res.WriteInt32(Shield+1); //1       					
                res.WriteInt32(22);  //0 
                
                
                //int[] EquipId = new int[] {15200601,15200601/*Shield* */,20000101/*Quiver*/,100101,200101,300101,400101,500101,690101/*Cape*/
                //,30102401,30200103,30300101,30400112,70000201/*talkring*/,160801,260801,360801,460801,10800405/*Weapon*/ };


                //sub_483420
                numEntries = 19;
                res.WriteInt32(numEntries);//has to be less than 19
                int x=0;
                int[] EquipId = new int[19]; 

                string CharacterSet = "Zenkato";       
                
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
                     
                    res.WriteInt32(EquipId[x]);//???
                    res.WriteByte(0); 
                    res.WriteByte(0);
                    res.WriteByte(0); //0  ????

                    
                    res.WriteInt32(12341234);//???
                    res.WriteByte(0); //
                    res.WriteByte(4); //
                    res.WriteByte(1); //
                        x++;
                    
                    res.WriteByte(00);// Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
                    res.WriteByte(00); //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
                    res.WriteByte(4); // testing
                    res.WriteByte(4); // testing
                    res.WriteByte(4); // testing
                    res.WriteByte(4); // testing
                        res.WriteByte(4); //Alternate texture for item model 
                res.WriteByte(4); // seperate in assembly
                
                }

                //sub_483420
                numEntries = 19;//influences a loop that needs to be under 19
                res.WriteInt32(numEntries);

                int rr = 000;
                //sub_483420   // 2 shield 4accessory? 8Helmet 12belt? 16torso 32 pants 48torsopants 64 hands 96handpants 128 feet 192handfeet 
                res.WriteInt32(001); //Right Hand    //1 for weapon
                res.WriteInt32(002); //Left Hand     //2 for Shield
                res.WriteInt32(016); //Torso         //16 for torso
                res.WriteInt32(008); //Head          //08 for head
                res.WriteInt32(032); //Legs          //32 for legs
                res.WriteInt32(064); //Arms          //64 for Arms
                res.WriteInt32(128); //Feet          //128 for feet
                res.WriteInt32(004); //???Cape
                res.WriteInt32(rr); //???Ring
                res.WriteInt32(rr); //???Earring
                res.WriteInt32(rr); //???Necklace
                res.WriteInt32(rr); //???Belt
                res.WriteInt32(016); //Avatar Torso
                res.WriteInt32(128); //Avatar Feet
                res.WriteInt32(064); //Avatar Arms
                res.WriteInt32(032); //Avatar Legs
                res.WriteInt32(008); //Avatar Head  
                res.WriteInt32(004); //???
                res.WriteInt32(000); //Right Hand   

                //sub_483420
                numEntries = 128;
                res.WriteInt32(numEntries);//has to be less than 128
                
                //sub_485A70
                for (int imac = 0; imac < numEntries; imac++)//status buffs / debuffs
                {
                    res.WriteInt32(10); //[eax]:&L"i.dllext-ms-mf-pal-l2-1-0"
                    res.WriteInt32(20);
                    res.WriteInt32(30);
                }

                Router.Send(client, (ushort)AreaPacketId.recv_data_get_self_chara_data_r, res);
            

            
        }
        
       
    }
}