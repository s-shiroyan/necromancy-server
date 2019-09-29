/*
 * Equipment management class, meant to contain armor related functions
 *
 * creation date: 27/09/2019
 * last update: 27/09/2019
 */

using System;
using System.Collections.Generic;
using System.Text;
using Arrowgene.Services.Buffers;

namespace Necromancy.Server.Common
{
    class Equipment
    {
        public static void Setup(int armor, int accessory, int shield, int weapon, int raceAndSex, IBuffer res)
        {
            /*
             *Armor initializing function used when a character appears for the first time,
             * or when every piece is changed at once.
             * 
             * ItemType Select See str_table SubID 121 for Item Type info. Increment by +1
             * 
             * the raceAndSex value are:
             * 1/2: humans (M/F)
             * 3/4: elfs (M/F)
             * 5: Dwarves
             * 6/7: Prokuls (M/F)
             * 8: Gnomes
            */
            int RASByte = 0; // Race and Sex byte
            switch (raceAndSex)
            {
                case 1:
                    RASByte = 00752101;
                    break;
                case 2:
                    RASByte = 00752201;
                    break;
                case 3:
                    RASByte = 00752301;
                    break;
                case 4:
                    RASByte = 00752401;
                    break;
                case 5:
                    RASByte = 00752501;
                    break;
                case 6:
                    RASByte = 00752701;
                    break;
                case 7:
                    RASByte = 00752801;
                    break;
                case 8:
                    RASByte = 00753001;
                    break;
            }

            //send equipment bytes
            res.WriteInt32(weapon); //18	    				
            res.WriteInt32(shield); //17 	    		
            res.WriteInt32(armor); //16	        			
            res.WriteInt32(armor); //15	        				
            res.WriteInt32(armor); //14	        			
            res.WriteInt32(armor); //13	        			
            res.WriteInt32(armor); //12	        				
            res.WriteInt32(accessory); //11	  	
            res.WriteInt32(accessory); //10	    			
            res.WriteInt32(accessory); //9	    		
            res.WriteInt32(accessory); //8	    			
            res.WriteInt32(accessory); //7	    			
            res.WriteInt32(armor); //6          				
            res.WriteInt32(armor); //5          		
            res.WriteInt32(armor); //4	        					
            res.WriteInt32(armor); //3	        				
            res.WriteInt32(armor); //2          				
            res.WriteInt32(shield + 1); //1       					
            res.WriteInt32(22);  //0 

            //skim through equipment models to gather info (I think)
            //sub_483420
            int numEntries = 19;


            //sub_4948C0
            for (int i = 0; i < numEntries; i++)
            {
                /* the next bytes determine the Race/sex combination that'll be affected. 
                 * the first "00752" is common to everyone except gnomes (more on that later) then
                 * 
                 * 101: Human/Male
                 * 201: Human/female
                 * 301: Elf/Male
                 * 401: Elf/Female
                 * 501: Dwarves
                 * 701: Porkul/Male
                 * 801: Porkul/Female
                 * 
                 * the gnomes uses 00753001 for some reason
                 */
                res.WriteInt32(RASByte); //Equipment ID from iteminfo.csv
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0); 

                res.WriteInt32(12341234); //No Effect
                res.WriteByte(0); //
                res.WriteByte(4); //
                res.WriteByte(1); //

                res.WriteByte(00);// Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
                res.WriteByte(00); //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
                res.WriteByte(4); // testing
                res.WriteByte(4); // testing
                res.WriteByte(4); // testing
                res.WriteByte(4); // testing
                res.WriteByte(2); //Alternate texture for item model 
                res.WriteByte(4); // seperate in assembly
            }

                //Equipement's bitmasks
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

        }
    }

}


