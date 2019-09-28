using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_chara_get_createinfo : Handler
    {
        public send_chara_get_createinfo(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_chara_get_createinfo;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(6);
            for (int i = 0; i < 6; i++)
            {
                res.WriteByte((byte)i);//specifies colors A-F
            }

            // 4bytes (004E90F6) cmp, 8 -> ja
            byte entries = 6;
            res.WriteByte(entries);//num of colors 6 being A-F
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            // 004E913F (jb loop)
            for (int i = 0; i < entries; i++)
            {
                res.WriteByte((byte) i);//specifies colors A-F
            }

            // 4bytes (004E9174) cmp, 8 -> ja
            entries = 6;
            res.WriteByte(entries); //Specifies how many faces, 6 being A-F
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            // 004E91BF (jb loop)
            for (int i = 0; i < entries; i++)
            {
                res.WriteByte((byte)i); //face data, starts at 0 = A
            }

            // 4bytes (004E91F4) cmp, 5 -> ja
            entries = 5;//Specifies how many stat groups 5 = (Human, Elf, Dwarf, Porkul, Gnome)
            res.WriteByte(entries);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            // note: advance 5 * 0x20 | 5 * 32


            // 4bytes 004E921C | E8 BF76FFFF | call wizardryonline_no_encryption.4E08E0 
            //for (int i = 0; i < entries; i++)
            {
                wo_4E08E0_Human_Stats(res);//holds correct data for starting stats for Humans
                wo_4E08E0_Elf_Stats(res);//holds correct data for starting stats for Elfs
                wo_4E08E0_Dwarf_Stats(res);//holds correct data for starting stats for Dwarf
                wo_4E08E0_Porkul_Stats(res);//holds correct data for starting stats for Porkul
                wo_4E08E0_Gnome_Stats(res);//holds correct data for starting stats for Gnome
            }

            // end  call wizardryonline_no_encryption.4E08E0 

            // 4 byte cmp, 10 -> ja (0019F954) loop create class? | 004E92B3 loop read data?
            entries = 8;//num of entries for our group of models
            res.WriteByte(entries); //50
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //for (int i = 0; i < entries; i++)
            {
                // 004E9297
                wo_4E3700_Elf_Female(res);//Holds Female Elf model details
                wo_4E3700_Human_Male(res);//Holds Male Human model details
                wo_4E3700_Human_Female(res);//Holds Female Human model details
                wo_4E3700_Elf_Male(res);//Holds Male Elf model details
                wo_4E3700_Dwarf_Male(res);//Holds Male Dwarf model details
                wo_4E3700_Gnome_Female(res);//Holds Female Gnome model details
                wo_4E3700_Prokul_Male(res);//Holds Male Porkul model details
                wo_4E3700_Porkul_Female(res);//Holds Female Porkul model details
            }


            //Read 4 byte (004E92E8) cmp,140(0x8C) -> JA (320)
            entries = 2;
            res.WriteByte(entries); // 0 = no elf , 1 = elf
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            for (int i = 0; i < entries; i++)
            {
                wo_4E37F0(res);
            }

            //4bytes cmp, E ( < 14) -> JA
            entries = 4;
            res.WriteByte(entries);//Specifies the number of Classes 4 being our max
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            
            //for (int i = 0; i < entries; i++)
            {
                wo_4E0970_Fighter(res);//Holds Stats and info for Fighter Class(need to fix bonus HP+MP+Stats)
                wo_4E0970_Priest(res);//Holds Stats and info for Priest Class(need to fix bonus HP+MP+Stats)
                wo_4E0970_Thief(res);//Holds Stats and info for Thief Class(need to fix bonus HP+MP+Stats)
                wo_4E0970_Mage(res);//Holds Stats and info for Mage Class(need to fix bonus HP+MP+Stats)
            }


            Router.Send(client, (ushort) MsgPacketId.recv_chara_get_createinfo_r, res);
        }

        private void wo_4E0970_Fighter(IBuffer res)
        {
            //4bytes
              res.WriteByte(0);//Class ID Fighter = 0, Thief = 1, Mage = 2, Priest - 3
              res.WriteByte(0);
              res.WriteByte(0);
              res.WriteByte(0); 
              
              //1bytes
              res.WriteByte(14); //Alignment Requirements 0/1 = Fully Locked, 2/3 = Lawful, 4/5 = Neutral. 6/7 = Lawful/Neutral,
              //8/9 = Chaotic, 10/11 = Lawful/Chaotic, 12/13 = Neutral Chaotic, 14/15 = Fully unlocked, 16+ = fully locked

              // 2byte 7x loop
              // 2 bytes
              res.WriteByte(8);//STR Requirement
              res.WriteByte(0);

              // 2 byte
              res.WriteByte(0);//VIT Requirement
              res.WriteByte(0);

              //2 byte
              res.WriteByte(0); //40  //DEX Requirement
              res.WriteByte(0);

              // 2 byte
              res.WriteByte(0);//AGI Requirement
              res.WriteByte(0);

              // 2 byte
              res.WriteByte(0);//INT Requirement
              res.WriteByte(0); //45

              // 2 byte
              res.WriteByte(0);//PIE Requirement
              res.WriteByte(0);

              // 2 byte
              res.WriteByte(0); //48  //LUK Requirement
              res.WriteByte(0);
              //end loop
              
              
              //4bytes
              res.WriteByte(12);//Class Bonus HP
              res.WriteByte(0);
              res.WriteByte(0);
              res.WriteByte(0); 
              
              //4bytes
              res.WriteByte(12);//Class Bonus MP
              res.WriteByte(0);
              res.WriteByte(0);
              res.WriteByte(0); 
              
              
              // 2byte 7x loop
              // 2 bytes
              res.WriteByte(12);//Class Bonus STR
              res.WriteByte(0);

              // 2 byte
              res.WriteByte(12);//Class Bonus VIT
              res.WriteByte(0);

              //2 byte
              res.WriteByte(12); //40 //Class Bonus DEX
              res.WriteByte(0);

              // 2 byte
              res.WriteByte(12);//Class Bonux AGI
              res.WriteByte(0);

              // 2 byte
              res.WriteByte(12);//Class Bonus INT
              res.WriteByte(0); //45

              // 2 byte
              res.WriteByte(12);//Class Bonus PIE
              res.WriteByte(0);

              // 2 byte
              res.WriteByte(12); //48 //Class Bonus LUK
              res.WriteByte(0);
            //end loop
            

            //4bytes
            res.WriteInt32(11101);
            //res.WriteInt32(110001);// states what is in skill slot 0 (left most), 0 = nothing
              
              //4bytes
              res.WriteInt32(11201);// states what is in skill slot 1 (middle), 0 = nothing              
                            
              //4bytes
              res.WriteInt32(0);// states what is in skill slot 2 (right most), 0 = nothing              
              
              //1bytes
              res.WriteByte(5);//bonus roll number store?

        }

        private void wo_4E0970_Thief(IBuffer res)
        {
            //4bytes
            res.WriteByte(1);//Class ID Fighter = 0, Thief = 1, Mage = 2, Priest - 3
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //1bytes
            res.WriteByte(12); //Alignment Requirements 0/1 = Fully Locked, 2/3 = Lawful, 4/5 = Neutral. 6/7 = Lawful/Neutral,
                               //8/9 = Chaotic, 10/11 = Lawful/Chaotic, 12/13 = Neutral Chaotic, 14/15 = Fully unlocked, 16+ = fully locked


            // 2byte 7x loop
            // 2 bytes
            res.WriteByte(0);//STR Requirement
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(0);//VIT Requirement
            res.WriteByte(0);

            //2 byte
            res.WriteByte(8); //40  //DEX Requirement
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(0);//AGI Requirement
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(0);//INT Requirement
            res.WriteByte(0); //45

            // 2 byte
            res.WriteByte(0);//PIE Requirement
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(0); //48  //LUK Requirement
            res.WriteByte(0);
            //end loop


            //4bytes
            res.WriteByte(12);//Class Bonus HP
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //4bytes
            res.WriteByte(12);//Class Bonus MP
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);


            // 2byte 7x loop
            // 2 bytes
            res.WriteByte(12);//Class Bonus STR
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(12);//Class Bonus VIT
            res.WriteByte(0);

            //2 byte
            res.WriteByte(12); //40 //Class Bonus DEX
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(12);//Class Bonux AGI
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(12);//Class Bonus INT
            res.WriteByte(0); //45

            // 2 byte
            res.WriteByte(12);//Class Bonus PIE
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(12); //48 //Class Bonus LUK
            res.WriteByte(0);
            //end loop



            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //4bytes
            res.WriteByte(4);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);


            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);


            //1bytes
            res.WriteByte(2);
        }

        private void wo_4E0970_Mage(IBuffer res)
        {
            //4bytes
            res.WriteByte(2);//Class ID Fighter = 0, Thief = 1, Mage = 2, Priest - 3
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //1bytes
            res.WriteByte(14); //Alignment Requirements 0/1 = Fully Locked, 2/3 = Lawful, 4/5 = Neutral. 6/7 = Lawful/Neutral,
                               //8/9 = Chaotic, 10/11 = Lawful/Chaotic, 12/13 = Neutral Chaotic, 14/15 = Fully unlocked, 16+ = fully locked


            // 2byte 7x loop
            // 2 bytes
            res.WriteByte(0);//STR Requirement
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(0);//VIT Requirement
            res.WriteByte(0);

            //2 byte
            res.WriteByte(0); //40  //DEX Requirement
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(0);//AGI Requirement
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(8);//INT Requirement
            res.WriteByte(0); //45

            // 2 byte
            res.WriteByte(0);//PIE Requirement
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(0); //48  //LUK Requirement
            res.WriteByte(0);
            //end loop


            //4bytes
            res.WriteByte(12);//Class Bonus HP
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //4bytes
            res.WriteByte(12);//Class Bonus MP
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);


            // 2byte 7x loop
            // 2 bytes
            res.WriteByte(12);//Class Bonus STR
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(12);//Class Bonus VIT
            res.WriteByte(0);

            //2 byte
            res.WriteByte(12); //40 //Class Bonus DEX
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(12);//Class Bonux AGI
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(12);//Class Bonus INT
            res.WriteByte(0); //45

            // 2 byte
            res.WriteByte(12);//Class Bonus PIE
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(12); //48 //Class Bonus LUK
            res.WriteByte(0);
            //end loop



            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //4bytes
            res.WriteByte(4);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);


            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);


            //1bytes
            res.WriteByte(2);
        }

        private void wo_4E0970_Priest(IBuffer res)
        {
            //4bytes
            res.WriteByte(3);//Class ID Fighter = 0, Thief = 1, Mage = 2, Priest - 3
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //1bytes
            res.WriteByte(10); //Alignment Requirements 0/1 = Fully Locked, 2/3 = Lawful, 4/5 = Neutral. 6/7 = Lawful/Neutral,
                               //8/9 = Chaotic, 10/11 = Lawful/Chaotic, 12/13 = Neutral Chaotic, 14/15 = Fully unlocked, 16+ = fully locked


            // 2byte 7x loop
            // 2 bytes
            res.WriteByte(0);//STR Requirement
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(0);//VIT Requirement
            res.WriteByte(0);

            //2 byte
            res.WriteByte(0); //40  //DEX Requirement
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(0);//AGI Requirement
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(0);//INT Requirement
            res.WriteByte(0); //45

            // 2 byte
            res.WriteByte(8);//PIE Requirement
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(0); //48  //LUK Requirement
            res.WriteByte(0);
            //end loop


            //4bytes
            res.WriteByte(12);//Class Bonus HP
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //4bytes
            res.WriteByte(12);//Class Bonus MP
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);


            // 2byte 7x loop
            // 2 bytes
            res.WriteByte(12);//Class Bonus STR
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(12);//Class Bonus VIT
            res.WriteByte(0);

            //2 byte
            res.WriteByte(12); //40 //Class Bonus DEX
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(12);//Class Bonux AGI
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(12);//Class Bonus INT
            res.WriteByte(0); //45

            // 2 byte
            res.WriteByte(12);//Class Bonus PIE
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(12); //48 //Class Bonus LUK
            res.WriteByte(0);
            //end loop



            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //4bytes
            res.WriteByte(4);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);


            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);


            //1bytes
            res.WriteByte(2);
        }


        private void wo_4E37F0(IBuffer res)
        {
            //4bytes cmp,14 -> JA
            res.WriteByte(0x14);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //4bytes
            res.WriteByte(3);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //4bytes
            res.WriteByte(4);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);


            for (int i = 0; i < 19; i++)
            {
                //4bytes
                res.WriteByte((byte)i);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
            }

            for (int i = 0; i < 19; i++)
            {
                wo_4948C0(res);
            }


            for (int i = 0; i < 19; i++)
            {
                //4bytes
                res.WriteByte(1);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
            }


            // Read Byte
            res.WriteByte(0);
        }

        private void wo_4948C0(IBuffer res)
        {
            // loop 19x
            // 004E3863 | E8 5810FBFF              | call wizardryonline_no_encryption.4948C0            |
            // start loop 19x
            // loop 2x
            //4bytes
            res.WriteByte(2);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //4bytes - 004948EE
            res.WriteByte(1);
            // Read Byte
            res.WriteByte(1);
            // Read Byte
            res.WriteByte(1);

            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            // Read Byte
            res.WriteByte(1);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop

            // Read Byte
            res.WriteByte(2);
            // Read Byte
            res.WriteByte(1);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(1);
            // Read Byte
            res.WriteByte(2);
            // Read Byte
            res.WriteByte(3);
            // Read Byte
            res.WriteByte(4);
            // Read Byte
            res.WriteByte(5);
            // Read Byte
            res.WriteByte(6);
            // end     
        }


        private void wo_4E3700_Elf_Female(IBuffer res) //characater creation area?
        {
            // 4 byte
            res.WriteByte(1);//race ID
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //4bytes
            res.WriteByte(1);//gender flag
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //1 byte cmp,1 -> sete (bool)
            res.WriteByte(1);

            Equipment.Setup(25, 27, 21, 14, 4, res);

            //int[] EquipId = new int[] {15200601,15200601/*Shield* */,20000101/*Quiver*/,100101,200101,300101,400101,500101,690101/*Cape*/
            //,30102401,30200103,30300101,30400112,70000201/*talkring*/,160801,260801,360801,460801,10800405/*Weapon*/ };

            //Read 1 byte (004E37C7)
            res.WriteByte(19);
        }

        private void wo_4E3700_Human_Male(IBuffer res) //characater creation area?
        {
            // 4 byte
            res.WriteByte(0);//race ID
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //4bytes
            res.WriteByte(0);//gender flag
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //1 byte cmp,1 -> sete (bool)
            res.WriteByte(1);


            //ItemType Select See str_table SubID 121 for Item Type info. Increment by +1
            Equipment.Setup(25, 27, 21, 14, 1, res);


            //int[] EquipId = new int[] {15200601,15200601/*Shield* */,20000101/*Quiver*/,100101,200101,300101,400101,500101,690101/*Cape*/
            //,30102401,30200103,30300101,30400112,70000201/*talkring*/,160801,260801,360801,460801,10800405/*Weapon*/ };

            //Read 1 byte (004E37C7)
            res.WriteByte(19);
        }

        private void wo_4E3700_Human_Female(IBuffer res) //characater creation area?
        {
            // 4 byte
            res.WriteByte(0);//race ID
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //4bytes
            res.WriteByte(1);//gender flag
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //1 byte cmp,1 -> sete (bool)
            res.WriteByte(1);


            //ItemType Select See str_table SubID 121 for Item Type info. Increment by +1
            Equipment.Setup(25, 27, 21, 14, 2, res);

            //int[] EquipId = new int[] {15200601,15200601/*Shield* */,20000101/*Quiver*/,100101,200101,300101,400101,500101,690101/*Cape*/
            //,30102401,30200103,30300101,30400112,70000201/*talkring*/,160801,260801,360801,460801,10800405/*Weapon*/ };
            //Read 1 byte (004E37C7)
            res.WriteByte(19);
        }

        private void wo_4E3700_Elf_Male(IBuffer res) //characater creation area?
        {
            // 4 byte
            res.WriteByte(1);//race ID
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //4bytes
            res.WriteByte(0);//gender flag
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //1 byte cmp,1 -> sete (bool)
            res.WriteByte(1);


            //ItemType Select See str_table SubID 121 for Item Type info. Increment by +1
            Equipment.Setup(25, 27, 21, 14, 3, res);
            //Equipment.Bitmask(res);


            //int[] EquipId = new int[] {15200601,15200601/*Shield* */,20000101/*Quiver*/,100101,200101,300101,400101,500101,690101/*Cape*/
            //,30102401,30200103,30300101,30400112,70000201/*talkring*/,160801,260801,360801,460801,10800405/*Weapon*/ };
            //Read 1 byte (004E37C7)
            res.WriteByte(19);
            
        }

        private void wo_4E3700_Dwarf_Male(IBuffer res) //characater creation area?
        {
            // 4 byte
            res.WriteByte(2);//race ID
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //4bytes
            res.WriteByte(0);//gender flag
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //1 byte cmp,1 -> sete (bool)
            res.WriteByte(1);


            //ItemType Select See str_table SubID 121 for Item Type info. Increment by +1
            Equipment.Setup(25, 27, 21, 14, 5, res);


            //int[] EquipId = new int[] {15200601,15200601/*Shield* */,20000101/*Quiver*/,100101,200101,300101,400101,500101,690101/*Cape*/
            //,30102401,30200103,30300101,30400112,70000201/*talkring*/,160801,260801,360801,460801,10800405/*Weapon*/ };
            //Read 1 byte (004E37C7)
            res.WriteByte(19);
        }

        private void wo_4E3700_Gnome_Female(IBuffer res) //characater creation area?
        {
            // 4 byte
            res.WriteByte(4);//race ID
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //4bytes
            res.WriteByte(1);//gender flag
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //1 byte cmp,1 -> sete (bool)
            res.WriteByte(1);


            //ItemType Select See str_table SubID 121 for Item Type info. Increment by +1
            Equipment.Setup(25, 27, 21, 14, 8, res);


            //int[] EquipId = new int[] {15200601,15200601/*Shield* */,20000101/*Quiver*/,100101,200101,300101,400101,500101,690101/*Cape*/
            //,30102401,30200103,30300101,30400112,70000201/*talkring*/,160801,260801,360801,460801,10800405/*Weapon*/ };
            //Read 1 byte (004E37C7)
            res.WriteByte(19);
        }

        private void wo_4E3700_Prokul_Male(IBuffer res) //characater creation area?
        {
            // 4 byte
            res.WriteByte(3);//race ID
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //4bytes
            res.WriteByte(0);//gender flag
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //1 byte cmp,1 -> sete (bool)
            res.WriteByte(1);


            //ItemType Select See str_table SubID 121 for Item Type info. Increment by +1
            Equipment.Setup(25, 27, 21, 14, 6, res);


            //int[] EquipId = new int[] {15200601,15200601/*Shield* */,20000101/*Quiver*/,100101,200101,300101,400101,500101,690101/*Cape*/
            //,30102401,30200103,30300101,30400112,70000201/*talkring*/,160801,260801,360801,460801,10800405/*Weapon*/ };

            //Read 1 byte (004E37C7)
            res.WriteByte(19);
        }

        private void wo_4E3700_Porkul_Female(IBuffer res) //characater creation area?
        {
            // 4 byte
            res.WriteByte(3);//race ID
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //4bytes
            res.WriteByte(1);//gender flag
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //1 byte cmp,1 -> sete (bool)
            res.WriteByte(1);

            //ItemType Select See str_table SubID 121 for Item Type info. Increment by +1
            Equipment.Setup(25, 27, 21, 14, 7, res);


            //int[] EquipId = new int[] {15200601,15200601/*Shield* */,20000101/*Quiver*/,100101,200101,300101,400101,500101,690101/*Cape*/
            //,30102401,30200103,30300101,30400112,70000201/*talkring*/,160801,260801,360801,460801,10800405/*Weapon*/ };

            //Read 1 byte (004E37C7)
            res.WriteByte(19);
        }

        private void wo_4E08E0_Human_Stats(IBuffer res)
        {
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            // 4bytes
            res.WriteByte(60);//HP
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            // 4bytes
            res.WriteByte(20);//MP
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            // 4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            // 2byte 7x loop    /////character stats start here
            // 2 bytes
            res.WriteByte(8);//STR
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(7);//VIT
            res.WriteByte(0);

            //2 byte
            res.WriteByte(7); //40  //DEX
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(6);//AGI
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(8);//INT
            res.WriteByte(0); //45

            // 2 byte
            res.WriteByte(5);///PIE
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(8); //48 //LUK
            res.WriteByte(0);
            //end loop
        }

        private void wo_4E08E0_Elf_Stats(IBuffer res)
        {
            res.WriteByte(3);//not sure
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            // 4bytes
            res.WriteByte(50);//HP
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            // 4bytes
            res.WriteByte(30);//MP
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            // 4bytes
            res.WriteByte(1);//
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            // 2byte 7x loop    /////character stats start here
            // 2 bytes
            res.WriteByte(6);//STR
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(5);//VIT
            res.WriteByte(0);

            //2 byte
            res.WriteByte(8); //40  //DEX
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(8);//AGI
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(10);//INT
            res.WriteByte(0); //45

            // 2 byte
            res.WriteByte(8);///PIE
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(4); //48 //LUK
            res.WriteByte(0);
            //end loop
        }

        private void wo_4E08E0_Dwarf_Stats(IBuffer res)
        {
            res.WriteByte(1);//not sure
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            // 4bytes
            res.WriteByte(80);//HP
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            // 4bytes
            res.WriteByte(15);//MP
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            // 4bytes
            res.WriteByte(1);//dont know
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            // 2byte 7x loop    /////character stats start here
            // 2 bytes
            res.WriteByte(9);//STR
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(8);//VIT
            res.WriteByte(0);

            //2 byte
            res.WriteByte(8); //40  //DEX
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(4);//AGI
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(5);//INT
            res.WriteByte(0); //45

            // 2 byte
            res.WriteByte(9);///PIE
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(5); //48 //LUK
            res.WriteByte(0);
            //end loop
        }

        private void wo_4E08E0_Porkul_Stats(IBuffer res)
        {
            res.WriteByte(1);//not sure
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            // 4bytes
            res.WriteByte(50);//HP
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            // 4bytes
            res.WriteByte(20);//MP
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            // 4bytes
            res.WriteByte(1);//dont know
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            // 2byte 7x loop    /////character stats start here
            // 2 bytes
            res.WriteByte(5);//STR
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(6);//VIT
            res.WriteByte(0);

            //2 byte
            res.WriteByte(9); //40  //DEX
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(12);//AGI
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(7);//INT
            res.WriteByte(0); //45

            // 2 byte
            res.WriteByte(7);///PIE
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(15); //48 //LUK
            res.WriteByte(0);
            //end loop
        }

        private void wo_4E08E0_Gnome_Stats(IBuffer res)
        {
            res.WriteByte(1);//not sure
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            // 4bytes
            res.WriteByte(70);//HP
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            // 4bytes
            res.WriteByte(25);//MP
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            // 4bytes
            res.WriteByte(1);//dont know
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            // 2byte 7x loop    /////character stats start here
            // 2 bytes
            res.WriteByte(7);//STR
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(7);//VIT
            res.WriteByte(0);

            //2 byte
            res.WriteByte(4); //40  //DEX
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(7);//AGI
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(6);//INT
            res.WriteByte(0); //45

            // 2 byte
            res.WriteByte(10);///PIE
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(6); //48 //LUK
            res.WriteByte(0);
            //end loop
        }
    }
}