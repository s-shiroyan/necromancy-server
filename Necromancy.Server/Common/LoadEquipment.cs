using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Common
{
    class LoadEquip
    {
        public static void BasicTraits(IBuffer res, Character myCharacter)
        {
            res.WriteInt32(myCharacter.Raceid); //race
            res.WriteInt32(myCharacter.Sexid); ; //gender
            res.WriteByte(myCharacter.HairId); //hair
            res.WriteByte(myCharacter.HairColorId); //color
            res.WriteByte(myCharacter.FaceId); //face
        }

            public static void SlotSetup( IBuffer res, Character myCharacter)
        {
            //Static Weapon types for your character slots until weapon type is databased.
            int[] MyWeaponType = new int[] { 14, 8, 15, 8, 10, 10 };
            //ItemType Select See str_table SubID 121 for Item Type info. Increment by +1
            int Armor = 25;         //Armor 25
            int Accessory = 27;     //Accessory 26
            int Shield = 21;        //Shield 19-21
                                    //int Weapon = 14;         //0 Knuckle, 1 Dagger, 3 1hSword, 7 1h axe (broken), 8 2hAxe, 9 spear, 10 blunt, 13 staff, 15 crossbow
                                    //sub_483660 
            res.WriteInt32(MyWeaponType[myCharacter.Characterslotid]); //18	    				
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
        }

        public static void EquipItems(IBuffer res, Character myCharacter)
        {
            //sub_483420
            int numEntries = 19;

            int x = 0;
            int[] EquipId = new int[19];
            byte[] headSlot = new byte[19];

            string CharacterSet = myCharacter.Name;

            switch (CharacterSet)
            {
                case "Talin":
                    EquipId = new int[] {10800405/*Weapon*/,15100901/*Shield* */,210701/*Torso*/,110301/*head*/,360103/*legs*/,410505/*Arms*/,560103/*Feet*/,690101,690101/*Cape*/
                    ,690101,690101,690101,210701/*Avatar Torso*/,560103/*Avatar Feet*/,410505/*Avatar Arms */,360103/*Avatar Legs*/,110301/*Avatar Head*/,690101,20000101/*Weapon Related*/ };
                    headSlot = new byte[19] { 0, 0, 0, 00, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 00, 0, 0 };
                    break;
                case "Kadred":
                    EquipId = new int[] {10800405/*Weapon*/,15100901/*Shield* */,260103/*Torso*/,110504/*head*/,360103/*legs*/,460103/*Arms*/,560103/*Feet*/,690101,690101/*Cape*/
                    ,690101,690101,690101,260801/*Avatar Torso*/,560801/*Avatar Feet*/,460801/*Avatar Arms */,360801/*Avatar Legs*/,160801/*Avatar Head*/,690101,20000101/*Weapon Related*/ };
                    headSlot = new byte[19] { 0, 0, 0, 42, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 42, 0, 0 };
                    break;
                case "Zakura":
                    EquipId = new int[] {11400403/*Weapon*/,0/*Shield* */,260103/*Torso*/,110504/*head*/,360103/*legs*/,460103/*Arms*/,510301/*Feet*/,690101,690101/*Cape*/
                    ,690101,690101,690101,260801/*Avatar Torso*/,510301/*Avatar Feet*/,460801/*Avatar Arms */,360801/*Avatar Legs*/,100403/*Avatar Head*/,690101,20000101/*Weapon Related*/ };
                    headSlot = new byte[19] { 0, 0, 0, 42, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 42, 0, 0 };
                    break;
                case "Ipa":
                    EquipId = new int[] {11300506/*Weapon*/,15100901/*Shield* */,260103/*Torso*/,110504/*head*/,360103/*legs*/,460103/*Arms*/,560103/*Feet*/,690101,690101/*Cape*/
                    ,690101,690101,690101,00252401/*Avatar Torso*/,560801/*Avatar Feet*/,460801/*Avatar Arms */,360801/*Avatar Legs*/,121901/*Avatar Head*/,690101,20000101/*Weapon Related*/ };
                    headSlot = new byte[19] { 0, 0, 0, 42, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 42, 0, 0 };
                    break;
                case "Test1":
                    EquipId = new int[] {11500102/*Weapon*/,15100801/*Shield* */,260103/*Torso*/,110504/*head*/,360103/*legs*/,460103/*Arms*/,560103/*Feet*/,690101,690101/*Cape*/
                        ,690101,690101,690101,261101/*Avatar Torso*/,561101/*Avatar Feet*/,461101/*Avatar Arms */,361101/*Avatar Legs*/,161101/*Avatar Head*/,690101,20000101/*Weapon Related*/ };
                    break;
                default:
                    EquipId = new int[] {10800405/*Weapon*/,15200702/*Shield* */,260103/*Torso*/,110504/*head*/,360103/*legs*/,460103/*Arms*/,560103/*Feet*/,690101,690101/*Cape*/
                    ,690101,690101,690101,261401/*Avatar Torso*/,561401/*Avatar Feet*/,461401/*Avatar Arms */,361401/*Avatar Legs*/,161401/*Avatar Head*/,690101,20000101/*Weapon Related*/ };
                    headSlot = new byte[19] { 0, 0, 0, 004, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 004, 0, 0 };
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

                res.WriteByte(headSlot[x]);// Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
                res.WriteByte(00); //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
                res.WriteByte(0); // testing (Theory Torso Tex)
                res.WriteByte(0); // testing (Theory Pants Tex)
                res.WriteByte(0); // testing (Theory Hands Tex)
                res.WriteByte(0); // testing (Theory Feet Tex)
                res.WriteByte(0); //Alternate texture for item model 
                res.WriteByte(0); // seperate in assembly
                x++;
            }

        }

        public static void EquipSlotBitMask(IBuffer res, Character myCharacter)
        {

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
            res.WriteInt32(-1);//016); //Avatar Torso
            res.WriteInt32(-1);//128); //Avatar Feet
            res.WriteInt32(-1);//064); //Avatar Arms
            res.WriteInt32(-1);//032); //Avatar Legs
            res.WriteInt32(-1);//008); //Avatar Head  
            res.WriteInt32(004); //???Talk Ring
            res.WriteInt32(000); //???Quiver  

            
        }
        
    }

}


