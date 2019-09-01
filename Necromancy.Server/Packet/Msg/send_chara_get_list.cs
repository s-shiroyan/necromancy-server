using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_chara_get_list : Handler
    {
        public send_chara_get_list(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)MsgPacketId.send_chara_get_list;


        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(1);
            Router.Send(client, (ushort)MsgPacketId.recv_chara_get_list_r, res);


            SendNotifyData(client);
            SendNotifyDataComplete(client);
        }

        private void SendNotifyDataComplete(NecClient client)
        {
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteByte(1);
            res2.WriteInt32(1);
            res2.WriteInt32(1);
            res2.WriteInt32(1);
            Router.Send(client, (ushort)MsgPacketId.recv_chara_notify_data_complete, res2);
        }

        byte[] slot = new byte[] { 0, 1, }; // 2, 3, 4, };

        int E = 0;



        private void SendNotifyData(NecClient client)

        {

            foreach (int y in slot)
            {
                IBuffer res = BufferProvider.Provide();
                res.WriteByte(1);//character slot, 0 for left, 1 for middle, 2 for right
                res.WriteInt32(1);    //  Character ID
                res.WriteFixedString("Character 1", 91); // 0x5B | 91x 1 byte

                res.WriteInt32(0); // 0 = Alive | 1 = Dead
                res.WriteInt32(0);//character level stat
                res.WriteInt32(0);//todo (unknown)
                res.WriteInt32(0);//class stat 
                                  //
                res.WriteInt32(1);//race flag
                res.WriteInt32(1);//gender flag
                res.WriteByte(0);//changing this byte makes hair and face change?
                res.WriteByte(0);//hair color? i think
                res.WriteByte(0);//changed nothing visibly
                                 //

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
                res.WriteInt32(Shield + 1); //1       					
                res.WriteInt32(22);  //0 


                //int[] EquipId = new int[] {15200601,15200601/*Shield* */,20000101/*Quiver*/,100101,200101,300101,400101,500101,690101/*Cape*/
                //,30102401,30200103,30300101,30400112,70000201/*talkring*/,160801,260801,360801,460801,10800405/*Weapon*/ };


                //sub_483420
                int numEntries = 19;

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

                    res.WriteInt32(EquipId[x]);//???
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0); //0  ????


                    res.WriteInt32(0);//???
                    res.WriteByte(0); //
                    res.WriteByte(0); //
                    res.WriteByte(0); //
                    x++;

                    res.WriteByte(00);// Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
                    res.WriteByte(00); //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
                    res.WriteByte(0); // testing
                    res.WriteByte(0); // testing
                    res.WriteByte(0); // testing
                    res.WriteByte(0); // testing
                    res.WriteByte(0); //Alternate texture for item model 
                    res.WriteByte(0); // seperate in assembly

                }

                //sub_483420
                numEntries = 19;//influences a loop that needs to be under 19


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


                //19x 4 byte //item quality(+#) or aura? 10 = +7, 19 = +6,(maybe just wep aura)
                res.WriteInt32(10); //Right Hand    //1 for weapon
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


                res.WriteByte(19);//changed nothing visibly

                res.WriteInt32(1001007);//map location ID

                E++;

                Router.Send(client, (ushort)MsgPacketId.recv_chara_notify_data, res);//SOE
                                                                                     //Router.Send(client,0xA535, res);//JP
            }
        }
    }
}