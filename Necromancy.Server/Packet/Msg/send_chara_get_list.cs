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

        byte[] slot = new byte[] {0,1,2,3,4};//3,4};

        
        string[] MyCharacters = new string[] {"Zenkato","Test1","Xeno","Kadred","Ipa","Diablo",};
        int[] MyRace = new int[]{0,4,3,3,0,2};
        int[] MyGender = new int[]{1,1,0,0,1,1};
        int[] MyWeaponType = new int[]{14,8,8,8,10,10};
        byte[] MyHair = new byte[] { 5,0,0,0,5,5};
        


        private void SendNotifyData(NecClient client)

        {
            int E = 0;
            foreach (int y in slot)
            {
                IBuffer res = BufferProvider.Provide();
                res.WriteByte(slot[E]);//character slot, 0 for left, 1 for middle, 2 for right
                res.WriteInt32(E+256);    //  Character ID
                res.WriteFixedString(MyCharacters[E], 91); // 0x5B | 91x 1 byte

                res.WriteInt32(0); // 0 = Alive | 1 = Dead
                res.WriteInt32(0);//character level stat
                res.WriteInt32(0);//todo (unknown)
                res.WriteInt32(0);//class stat 
                                  //
                res.WriteInt32(MyRace[E]);//race flag
                res.WriteInt32(MyGender[E]);//gender flag
                res.WriteByte(0);//Hair Style
                res.WriteByte(5);//Hair Color
                res.WriteByte(0);//Face Style
                                 

                //ItemType Select See str_table SubID 121 for Item Type info. Increment by +1
                int Armor = 25;         //Armor 25
                int Accessory = 27;     //Accessory 26
                int Shield = 21;        //Shield 19-21
                //int Weapon = 14;         //0 Knuckle, 1 Dagger, 3 1hSword, 7 1h axe (broken), 8 2hAxe, 9 spear, 10 blunt, 13 staff, 15 crossbow
                                         //sub_483660 
                res.WriteInt32(MyWeaponType[E]); //18	    				
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


                //sub_483420
                int numEntries = 19;

                int x = 0;
                int[] EquipId = new int[19];

                string CharacterSet = MyCharacters[E];

                switch (CharacterSet)
                {
                    case "Xeno":
                        EquipId = new int[] {10800405/*Weapon*/,15100901/*Shield* */,210701/*Torso*/,110301/*head*/,360103/*legs*/,410505/*Arms*/,560103/*Feet*/,690101,690101/*Cape*/
                    ,690101,690101,690101,210701/*Avatar Torso*/,560103/*Avatar Feet*/,410505/*Avatar Arms */,360103/*Avatar Legs*/,110301/*Avatar Head*/,690101,20000101/*Weapon Related*/ };
                        break;
                    case "Kadred":
                        EquipId = new int[] {10800405/*Weapon*/,15100901/*Shield* */,260103/*Torso*/,110504/*head*/,360103/*legs*/,460103/*Arms*/,560103/*Feet*/,690101,690101/*Cape*/
                    ,690101,690101,690101,260801/*Avatar Torso*/,560801/*Avatar Feet*/,460801/*Avatar Arms */,360801/*Avatar Legs*/,160801/*Avatar Head*/,690101,20000101/*Weapon Related*/ };
                        break;
                    case "Zenkato":
                        EquipId = new int[] {11400303/*Weapon*/,0/*Shield* */,260103/*Torso*/,110504/*head*/,360103/*legs*/,460103/*Arms*/,510301/*Feet*/,690101,690101/*Cape*/
                    ,690101,690101,690101,260801/*Avatar Torso*/,510301/*Avatar Feet*/,460801/*Avatar Arms */,360801/*Avatar Legs*/,100403/*Avatar Head*/,690101,20000101/*Weapon Related*/ };
                        break;
                    case "Ipa":
                     EquipId = new int[] {11300705/*Weapon*/,0/*Shield* */,260101/*Torso*/,110504/*head*/,360103/*legs*/,460103/*Arms*/,560103/*Feet*/,690101,690101/*Cape*/
                    ,690101,690101,690101,260104/*Avatar Torso*/,560801/*Avatar Feet*/,460801/*Avatar Arms */,361001/*Avatar Legs*/,160104/*Avatar Head*/,690101,20000101/*Weapon Related*/ };
                    break;
                    case "Diablo":
                     EquipId = new int[] {11400401/*Weapon*/,0/*Shield* */,260103/*Torso*/,110504/*head*/,360103/*legs*/,460103/*Arms*/,560103/*Feet*/,690101,690101/*Cape*/
                    ,690101,690101,690101,260701/*Avatar Torso*/,560801/*Avatar Feet*/,460801/*Avatar Arms */,360701/*Avatar Legs*/,160104/*Avatar Head*/,690101,20000101/*Weapon Related*/ };
                    break;
                    case "Test1":
                     EquipId = new int[] {10910405/*Weapon*/,15100801/*Shield* */,260103/*Torso*/,110504/*head*/,360103/*legs*/,460103/*Arms*/,560103/*Feet*/,690101,690101/*Cape*/
                    ,690101,690101,690101,261101/*Avatar Torso*/,561101/*Avatar Feet*/,461101/*Avatar Arms */,361101/*Avatar Legs*/,161101/*Avatar Head*/,690101,20000101/*Weapon Related*/ };
                    break;
                    default:
                     EquipId = new int[] {10800405/*Weapon*/,15100901/*Shield* */,260103/*Torso*/,110504/*head*/,360103/*legs*/,460103/*Arms*/,560103/*Feet*/,690101,690101/*Cape*/
                    ,690101,690101,690101,260901/*Avatar Torso*/,560901/*Avatar Feet*/,460901/*Avatar Arms */,360901/*Avatar Legs*/,160901/*Avatar Head*/,690101,20000101/*Weapon Related*/ };
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

                    res.WriteByte(MyHair[E]);// Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
                    res.WriteByte(00); //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
                    res.WriteByte(0); // testing (Theory Torso Tex)
                    res.WriteByte(0); // testing (Theory Pants Tex)
                    res.WriteByte(0); // testing (Theory Hands Tex)
                    res.WriteByte(0); // testing (Theory Feet Tex)
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
                res.WriteInt32(004); //???Talk Ring
                res.WriteInt32(000); //???Quiver   


                //19x 4 byte //item quality(+#) or aura? 10 = +7, 19 = +6,(maybe just wep aura)
                res.WriteInt32(10); //Right Hand    //1 for weapon
                res.WriteInt32(10); //Left Hand     //2 for Shield
                res.WriteInt32(10); //Torso         //16 for torso
                res.WriteInt32(10); //Head          //08 for head
                res.WriteInt32(10); //Legs          //32 for legs
                res.WriteInt32(10); //Arms          //64 for Arms
                res.WriteInt32(10); //Feet          //128 for feet
                res.WriteInt32(004); //???Cape
                res.WriteInt32(rr); //???Ring
                res.WriteInt32(rr); //???Earring
                res.WriteInt32(rr); //???Necklace
                res.WriteInt32(rr); //???Belt
                res.WriteInt32(10); //Avatar Torso
                res.WriteInt32(10); //Avatar Feet
                res.WriteInt32(10); //Avatar Arms
                res.WriteInt32(10); //Avatar Legs
                res.WriteInt32(10); //Avatar Head  
                res.WriteInt32(10); //???Talk Ring
                res.WriteInt32(00); //???Quiver    


                res.WriteByte(19);//changed nothing visibly

                res.WriteInt32(1001007);//map location ID

                E++;

                Router.Send(client, (ushort)MsgPacketId.recv_chara_notify_data, res);//SOE
                                                                                     //Router.Send(client,0xA535, res);//JP
            }
        }
    }
}