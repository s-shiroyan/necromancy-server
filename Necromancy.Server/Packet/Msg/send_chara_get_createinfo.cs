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

            byte entries = 6;
            res.WriteInt32(entries);
            for (int i = 0; i < 6; i++)
            {
                res.WriteByte((byte)i);//specifies colors A-F
            }

            res.WriteInt32(entries);//num of colors 6 being A-F

            for (int i = 0; i < entries; i++)
            {
                res.WriteByte((byte) i);//specifies colors A-F
            }

            res.WriteInt32(entries);//Specifies how many faces, 6 being A-F

            for (int i = 0; i < entries; i++)
            {
                res.WriteByte((byte)i); //face data, starts at 0 = A
            }

            entries = 5;//Specifies how many stat groups 5 = (Human, Elf, Dwarf, Porkul, Gnome)
            res.WriteInt32(entries);
            {
                wo_4E08E0_Human_Stats(res);//holds correct data for starting stats for Humans
                wo_4E08E0_Elf_Stats(res);//holds correct data for starting stats for Elfs
                wo_4E08E0_Dwarf_Stats(res);//holds correct data for starting stats for Dwarf
                wo_4E08E0_Porkul_Stats(res);//holds correct data for starting stats for Porkul
                wo_4E08E0_Gnome_Stats(res);//holds correct data for starting stats for Gnome
            }

            entries = 8;//num of entries for our group of models
            res.WriteInt32(entries); 

            {
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
            res.WriteInt32(entries);


            for (int i = 0; i < entries; i++)
            {
                wo_4E37F0(res);
            }

            //4bytes cmp, E ( < 14) -> JA
            entries = 4;
            res.WriteInt32(entries);//Specifies the number of Classes 4 being our max

            {
                wo_4E0970_Fighter(res);//Holds Stats and info for Fighter Class(need to fix bonus HP+MP+Stats)
                wo_4E0970_Priest(res);//Holds Stats and info for Priest Class(need to fix bonus HP+MP+Stats)
                wo_4E0970_Thief(res);//Holds Stats and info for Thief Class(need to fix bonus HP+MP+Stats)
                wo_4E0970_Mage(res);//Holds Stats and info for Mage Class(need to fix bonus HP+MP+Stats)
            }


            Router.Send(client, (ushort) MsgPacketId.recv_chara_get_createinfo_r, res);
        }

        private void wo_4E08E0_Human_Stats(IBuffer res)
        {
            res.WriteInt32(1);
            res.WriteInt32(60);//HP
            res.WriteInt32(20);//MP
            res.WriteInt32(0);

            res.WriteInt16(8);//STR
            res.WriteInt16(7);//VIT
            res.WriteInt16(7);//DEX
            res.WriteInt16(6);//AGI
            res.WriteInt16(8);//INT
            res.WriteInt16(5);//PIE
            res.WriteInt16(8);//LUK
        }

        private void wo_4E08E0_Elf_Stats(IBuffer res)
        {
            res.WriteInt32(1);
            res.WriteInt32(50);//HP
            res.WriteInt32(30);//MP
            res.WriteInt32(0);

            res.WriteInt16(6);//STR
            res.WriteInt16(5);//VIT
            res.WriteInt16(8);//DEX
            res.WriteInt16(8);//AGI
            res.WriteInt16(10);//INT
            res.WriteInt16(8);//PIE
            res.WriteInt16(4);//LUK
        }

        private void wo_4E08E0_Dwarf_Stats(IBuffer res)
        {
            res.WriteInt32(1);
            res.WriteInt32(80);//HP
            res.WriteInt32(15);//MP
            res.WriteInt32(0);

            res.WriteInt16(9);//STR
            res.WriteInt16(8);//VIT
            res.WriteInt16(8);//DEX
            res.WriteInt16(4);//AGI
            res.WriteInt16(5);//INT
            res.WriteInt16(9);//PIE
            res.WriteInt16(5);//LUK
        }

        private void wo_4E08E0_Porkul_Stats(IBuffer res)
        {
            res.WriteInt32(1);
            res.WriteInt32(50);//HP
            res.WriteInt32(20);//MP
            res.WriteInt32(0);

            res.WriteInt16(5);//STR
            res.WriteInt16(6);//VIT
            res.WriteInt16(9);//DEX
            res.WriteInt16(12);//AGI
            res.WriteInt16(7);//INT
            res.WriteInt16(7);//PIE
            res.WriteInt16(15);//LUK
        }

        private void wo_4E08E0_Gnome_Stats(IBuffer res)
        {
            res.WriteInt32(1);
            res.WriteInt32(70);//HP
            res.WriteInt32(25);//MP
            res.WriteInt32(0);

            res.WriteInt16(7);//STR
            res.WriteInt16(7);//VIT
            res.WriteInt16(4);//DEX
            res.WriteInt16(7);//AGI
            res.WriteInt16(6);//INT
            res.WriteInt16(10);//PIE
            res.WriteInt16(6);//LUK
        }

        private void wo_4E3700_Elf_Female(IBuffer res) //characater creation area?
        {
            res.WriteInt32(1);//race ID
            res.WriteInt32(1);//gender flag

            res.WriteByte(1);//bool


            for (int i = 0; i < 19; i++)
            {
                res.WriteInt32(0);
            }

            for (int i = 0; i < 19; i++)
            {
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);//bool
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
            }

            for (int i = 0; i < 19; i++)
            {
                res.WriteInt32(0);
            }

            res.WriteByte(0);
        }

        private void wo_4E3700_Human_Male(IBuffer res) //characater creation area?
        {
            // 4 byte
            res.WriteInt32(0);//race ID
            res.WriteInt32(0);//gender flag

            res.WriteByte(1);//bool

            for (int i = 0; i < 19; i++)
            {
                res.WriteInt32(0);
            }

            for (int i = 0; i < 19; i++)
            {
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);//bool
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
            }

            for (int i = 0; i < 19; i++)
            {
                res.WriteInt32(0);
            }

            res.WriteByte(0);
        }

        private void wo_4E3700_Human_Female(IBuffer res) //characater creation area?
        {
            // 4 byte
            res.WriteInt32(0);//race ID
            res.WriteInt32(1);//gender flag

            res.WriteByte(1);//bool

            for (int i = 0; i < 19; i++)
            {
                res.WriteInt32(0);
            }

            for (int i = 0; i < 19; i++)
            {
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);//bool
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
            }

            for (int i = 0; i < 19; i++)
            {
                res.WriteInt32(0);
            }

            res.WriteByte(0);
        }

        private void wo_4E3700_Elf_Male(IBuffer res) //characater creation area?
        {
            // 4 byte
            res.WriteInt32(1);//race ID
            res.WriteInt32(0);//gender flag

            res.WriteByte(1);//bool

            for (int i = 0; i < 19; i++)
            {
                res.WriteInt32(0);
            }

            for (int i = 0; i < 19; i++)
            {
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);//bool
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
            }

            for (int i = 0; i < 19; i++)
            {
                res.WriteInt32(0);
            }

            res.WriteByte(0);

        }

        private void wo_4E3700_Dwarf_Male(IBuffer res) //characater creation area?
        {
            // 4 byte
            res.WriteInt32(2);//race ID
            res.WriteInt32(0);//gender flag

            res.WriteByte(1);//bool

            for (int i = 0; i < 19; i++)
            {
                res.WriteInt32(0);
            }

            for (int i = 0; i < 19; i++)
            {
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);//bool
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
            }

            for (int i = 0; i < 19; i++)
            {
                res.WriteInt32(0);
            }

            res.WriteByte(0);
        }

        private void wo_4E3700_Gnome_Female(IBuffer res) //characater creation area?
        {
            // 4 byte
            res.WriteInt32(4);//race ID
            res.WriteInt32(1);//gender flag

            res.WriteByte(1);//bool

            for (int i = 0; i < 19; i++)
            {
                res.WriteInt32(0);
            }

            for (int i = 0; i < 19; i++)
            {
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);//bool
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
            }

            for (int i = 0; i < 19; i++)
            {
                res.WriteInt32(0);
            }

            res.WriteByte(0);
        }

        private void wo_4E3700_Prokul_Male(IBuffer res) //characater creation area?
        {
            // 4 byte
            res.WriteInt32(3);//race ID
            res.WriteInt32(0);//gender flag

            res.WriteByte(1);//bool


            for (int i = 0; i < 19; i++)
            {
                res.WriteInt32(0);
            }

            for (int i = 0; i < 19; i++)
            {
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);//bool
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
            }

            for (int i = 0; i < 19; i++)
            {
                res.WriteInt32(0);
            }

            res.WriteByte(0);
        }

        private void wo_4E3700_Porkul_Female(IBuffer res) //characater creation area?
        {
            // 4 byte
            res.WriteInt32(3);//race ID
            res.WriteInt32(1);//gender flag

            res.WriteByte(1);//bool


            for (int i = 0; i < 19; i++)
            {
                res.WriteInt32(0);
            }

            for (int i = 0; i < 19; i++)
            {
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);//bool
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
            }

            for (int i = 0; i < 19; i++)
            {
                res.WriteInt32(0);
            }

            res.WriteByte(0);
        }

        private void wo_4E37F0(IBuffer res)
        {
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);

            for (int i = 0; i < 19; i++)
            {
                res.WriteInt32(0);
            }

            for (int i = 0; i < 19; i++)
            {
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);//bool
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0); ;
            }


            for (int i = 0; i < 19; i++)
            {
                res.WriteInt32(0);
            }

            res.WriteByte(0);
        }

        private void wo_4E0970_Fighter(IBuffer res)
        {
            
            res.WriteInt32(0);//Class ID Fighter = 0, Thief = 1, Mage = 2, Priest - 3
            res.WriteByte(14); //Alignment Requirements 0/1 = Fully Locked, 2/3 = Lawful, 4/5 = Neutral. 6/7 = Lawful/Neutral,
                                //8/9 = Chaotic, 10/11 = Lawful/Chaotic, 12/13 = Neutral Chaotic, 14/15 = Fully unlocked, 16+ = fully locked

            res.WriteInt16(8);//STR Requirement
            res.WriteInt16(0);//VIT Requirement
            res.WriteInt16(0);//DEX Requirement
            res.WriteInt16(0);//AGI Requirement
            res.WriteInt16(0);//INT Requirement
            res.WriteInt16(0);//PIE Requirement
            res.WriteInt16(0);//LUK Requirement

            res.WriteInt32(30);//Class Bonus HP
            res.WriteInt32(0);//Class Bonus MP

            res.WriteInt16(2);//Class Bonus STR
            res.WriteInt16(2);//Class Bonus VIT
            res.WriteInt16(0);//Class Bonus DEX
            res.WriteInt16(0);//Class Bonux AGI
            res.WriteInt16(-1);;//Class Bonus INT
            res.WriteInt16(0);//Class Bonus PIE
            res.WriteInt16(0);//Class Bonus LUK

            res.WriteInt32(11101);// states what is in skill slot 0 (left most), 0 = nothing
            res.WriteInt32(11201);// states what is in skill slot 1 (middle), 0 = nothing              
            res.WriteInt32(0);// states what is in skill slot 2 (right most), 0 = nothing              

            res.WriteByte(0);//bonus roll number store?

        }

        private void wo_4E0970_Thief(IBuffer res)
        {
            res.WriteInt32(1);//Class ID Fighter = 0, Thief = 1, Mage = 2, Priest - 3
            res.WriteByte(12);//Alignment Requirements 0/1 = Fully Locked, 2/3 = Lawful, 4/5 = Neutral. 6/7 = Lawful/Neutral,
                              //8/9 = Chaotic, 10/11 = Lawful/Chaotic, 12/13 = Neutral Chaotic, 14/15 = Fully unlocked, 16+ = fully locked

            res.WriteInt16(0);//STR Requirement
            res.WriteInt16(0);//VIT Requirement
            res.WriteInt16(8);//DEX Requirement
            res.WriteInt16(0);//AGI Requirement
            res.WriteInt16(0);//INT Requirement
            res.WriteInt16(0);//PIE Requirement
            res.WriteInt16(0);//LUK Requirement

            res.WriteInt32(10);//Class Bonus HP
            res.WriteInt32(0);//Class Bonus MP

            res.WriteInt16(1);//Class Bonus STR
            res.WriteInt16(0);//Class Bonus VIT
            res.WriteInt16(1);//Class Bonus DEX
            res.WriteInt16(1);//Class Bonux AGI
            res.WriteInt16(-1); ;//Class Bonus INT
            res.WriteInt16(-1);//Class Bonus PIE
            res.WriteInt16(1);//Class Bonus LUK

            res.WriteInt32(14101);// states what is in skill slot 0 (left most), 0 = nothing
            res.WriteInt32(14302);// states what is in skill slot 1 (middle), 0 = nothing              
            res.WriteInt32(14803);// states what is in skill slot 2 (right most), 0 = nothing              

            res.WriteByte(0);//bonus roll number store?
        }

        private void wo_4E0970_Mage(IBuffer res)
        {
            res.WriteInt32(2);//Class ID Fighter = 0, Thief = 1, Mage = 2, Priest - 3
            res.WriteByte(14); //Alignment Requirements 0/1 = Fully Locked, 2/3 = Lawful, 4/5 = Neutral. 6/7 = Lawful/Neutral,
                               //8/9 = Chaotic, 10/11 = Lawful/Chaotic, 12/13 = Neutral Chaotic, 14/15 = Fully unlocked, 16+ = fully locked

            res.WriteInt16(0);//STR Requirement
            res.WriteInt16(0);//VIT Requirement
            res.WriteInt16(0);//DEX Requirement
            res.WriteInt16(0);//AGI Requirement
            res.WriteInt16(8);//INT Requirement
            res.WriteInt16(0);//PIE Requirement
            res.WriteInt16(0);//LUK Requirement

            res.WriteInt32(0);//Class Bonus HP
            res.WriteInt32(50);//Class Bonus MP

            res.WriteInt16(-1);//Class Bonus STR
            res.WriteInt16(0);//Class Bonus VIT
            res.WriteInt16(0);//Class Bonus DEX
            res.WriteInt16(0);//Class Bonux AGI
            res.WriteInt16(+2); ;//Class Bonus INT
            res.WriteInt16(+1);//Class Bonus PIE
            res.WriteInt16(0);//Class Bonus LUK

            res.WriteInt32(13101);// states what is in skill slot 0 (left most), 0 = nothing
            res.WriteInt32(13404);// states what is in skill slot 1 (middle), 0 = nothing              
            res.WriteInt32(0);// states what is in skill slot 2 (right most), 0 = nothing              

            res.WriteByte(0);//bonus roll number store?
        }

        private void wo_4E0970_Priest(IBuffer res)
        {
            
            res.WriteInt32(3);//Class ID Fighter = 0, Thief = 1, Mage = 2, Priest - 3
            res.WriteByte(10); //Alignment Requirements 0/1 = Fully Locked, 2/3 = Lawful, 4/5 = Neutral. 6/7 = Lawful/Neutral,
                               //8/9 = Chaotic, 10/11 = Lawful/Chaotic, 12/13 = Neutral Chaotic, 14/15 = Fully unlocked, 16+ = fully locked

            res.WriteInt16(0);//STR Requirement
            res.WriteInt16(0);//VIT Requirement
            res.WriteInt16(0);//DEX Requirement
            res.WriteInt16(0);//AGI Requirement
            res.WriteInt16(0);//INT Requirement
            res.WriteInt16(8);//PIE Requirement
            res.WriteInt16(0);//LUK Requirement

            res.WriteInt32(20);//Class Bonus HP
            res.WriteInt32(40);//Class Bonus MP

            res.WriteInt16(0);//Class Bonus STR
            res.WriteInt16(1);//Class Bonus VIT
            res.WriteInt16(0);//Class Bonus DEX
            res.WriteInt16(-1);//Class Bonux AGI
            res.WriteInt16(0); ;//Class Bonus INT
            res.WriteInt16(2);//Class Bonus PIE
            res.WriteInt16(0);//Class Bonus LUK

            res.WriteInt32(12501);// states what is in skill slot 0 (left most), 0 = nothing
            res.WriteInt32(12601);// states what is in skill slot 1 (middle), 0 = nothing              
            res.WriteInt32(0);// states what is in skill slot 2 (right most), 0 = nothing              

            res.WriteByte(0);//bonus roll number store?
        }


       
        
        
    }
}