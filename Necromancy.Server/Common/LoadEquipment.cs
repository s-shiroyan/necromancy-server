using Necromancy.Server.Model;
using System;
using Arrowgene.Buffers;
using Necromancy.Server.Model.ItemModel;

namespace Necromancy.Server.Common
{
    class LoadEquip
    {
        public static void BasicTraits(IBuffer res, Character myCharacter)
        {
            res.WriteUInt32(myCharacter.Raceid); //race
            res.WriteUInt32(myCharacter.Sexid);
            res.WriteByte(myCharacter.HairId); //hair
            res.WriteByte(myCharacter.HairColorId); //color
            res.WriteByte(myCharacter.FaceId); //face
        }
        public static void SlotSetup(IBuffer res, Character myCharacter, int numEntries)
        {
            if (myCharacter.Inventory._equippedItems.Count != 0) 
            {
                SlotSetupNew(res, myCharacter);
                return;
            }
            TemporaryCharacterSwitch(myCharacter, numEntries); //needed to instantiate Weapon ID for Weapon logic below
            int Armor = 25; //Armor 25
            int Accessory = 26; //Accessory 26
            int Shield = 21; //Shield 19-21
            int Quiver = 22; //Arrows 22 Bolt 23 Bullet 24
            int Other = 27; //None of the Above
            int Weapon =
                Convert.ToInt32(string.Concat($"{(myCharacter.EquipId[0])}"[1], $"{(myCharacter.EquipId[0])}"[2]));
            myCharacter.battlePose = (byte)Weapon;
            //Console.WriteLine($"Weapon : {Weapon}");

            //ItemType Select See str_table ID 100 SubID 121 for Item Type info. Increment by +1
            int[] EquipItemType = new int[]
            {
                Weapon, Shield, Quiver, Armor, Armor, Armor, Armor, Armor, Other /*Cape-Other*/, Accessory, Accessory,
                Accessory, Accessory, Other /*TalkRing-Other*/, Armor, Armor, Armor, Armor, Armor
            };

            //sub_483660 
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(EquipItemType[i]);
            }
        }

        public static void EquipItems(IBuffer res, Character myCharacter, int numEntries)
        {
            if (myCharacter.Inventory._equippedItems.Count != 0)
            {
                EquipItemsNew(res, myCharacter);
                return;
            }
            //sub_483420
            int x = 0;

            int[] headSlot = TemporaryCharacterSwitch(myCharacter, numEntries);

            //sub_4948C0
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(myCharacter.EquipId[x]); //Sets your Item ID per Iteration
                res.WriteByte(0); // 
                res.WriteByte(0); // (theory bag)
                res.WriteByte(0); // (theory Slot)

                res.WriteInt32(myCharacter.EquipId[x]); //testing (Theory, Icon related)
                res.WriteByte(0); //
                res.WriteByte(0); // (theory bag)
                res.WriteByte(0); // (theory Slot)

                res.WriteByte(
                    (byte)headSlot[
                        x]); // Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
                res.WriteByte(
                    00); //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
                res.WriteByte(0); // testing (Theory Torso Tex)
                res.WriteByte(0); // testing (Theory Pants Tex)
                res.WriteByte(0); // testing (Theory Hands Tex)
                res.WriteByte(0); // testing (Theory Feet Tex)
                res.WriteByte(0); //Alternate texture for item model 

                res.WriteByte(0); // separate in assembly
                x++;
            }
        }

        public static void EquipSlotBitMask(IBuffer res, Character myCharacter, int numEntries)
        {
            if (myCharacter.Inventory._equippedItems.Count != 0)
            {
                EquipSlotBitMaskNew(res, myCharacter);
                return;
            }
            int[] EquipBitMask = new int[] //Correct Bit Mask
            {
                1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768, 65536, 131072, 262144
            };
            EquipBitMask = new int[] //Temporary Bit Mask until i re-figure out Avatar Item Display Precedence.
            {
                1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 8, 16, 32, 64, 128
            };
            for (int i = 0; i < numEntries; i++)
            {
                //sub_483420   
                res.WriteInt32(EquipBitMask[i]); //bitmask per equipment slot
            }
        }

        public static void SlotSetupNew(IBuffer res, Character character)
        {
            //sub_483660 
            foreach (InventoryItem inventoryItem in character.Inventory._equippedItems.Values)
            {
                res.WriteInt32((int)inventoryItem.Item.LoadEquipType);
            }
        }

        public static void EquipItemsNew(IBuffer res, Character character)
        {
            //sub_4948C0
            foreach (InventoryItem inventoryItem in character.Inventory._equippedItems.Values)
            {
                res.WriteInt32(inventoryItem.Item.Id); //Sets your Item ID per Iteration
                res.WriteByte(0); // 
                res.WriteByte(0); // (theory bag)
                res.WriteByte(0); // (theory Slot)

                res.WriteInt32(inventoryItem.Item.Id); //testing (Theory, Icon related)
                res.WriteByte(0); //
                res.WriteByte(0); // (theory bag)
                res.WriteByte(0); // (theory Slot)

                res.WriteByte(0); // Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
                res.WriteByte(00); //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
                res.WriteByte(0); // testing (Theory Torso Tex)
                res.WriteByte(0); // testing (Theory Pants Tex)
                res.WriteByte(0); // testing (Theory Hands Tex)
                res.WriteByte(0); // testing (Theory Feet Tex)
                res.WriteByte(0); //Alternate texture for item model 

                res.WriteByte(0); // separate in assembly
            }
        }
        
        public static void EquipSlotBitMaskNew(IBuffer res, Character character)
        {
            //sub_483420 
            foreach (InventoryItem inventoryItem in character.Inventory._equippedItems.Values)
            {
                res.WriteInt32((int)inventoryItem.Item.EquipmentSlotType); //bitmask per equipment slot
            }
        }


        //This goes away soon.  as soon as NPC gear is stored in a CSV or database table.  and character equipment is finished.

        private static int[] TemporaryCharacterSwitch(Character myCharacter, int numEntries)
        {
            string CharacterSet = myCharacter.Name;
            int[] headSlot = new int[numEntries];
            int[] EquipBitMask = {1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 8, 16, 32, 64, 128};

            if (myCharacter.hadDied == true) //removes all gear in soul form
            {
                myCharacter.EquipId = new int[]
                {
                    11200301 /*Weapon*/, 0 /*Shield* */, 0 /*Arrow*/, 0 /*head*/, 0 /*Torso*/,
                    0 /*Pants*/, 0 /*Hands*/, 0 /*Feet*/, 0 /*Cape*/, 0 /*Necklace*/,
                    0 /*Earring*/, 0 /*Belt*/, 0 /*Ring*/, 0 /*Talk Ring*/, 0 /*Avatar Head */,
                    0 /*Avatar Torso*/, 0 /*Avatar Pants*/, 0 /*Avatar Hands*/, 0 /*Avatar Feet*/
                };
                headSlot = new int[19] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            }
            else
            {
                switch (CharacterSet)
                {
                    case "Talin":
                        myCharacter.EquipId = new int[]
                        {
                            10800405 /*Weapon*/, 15100901 /*Shield* */, 20000101 /*Arrow*/, 110301 /*head*/,
                            210701 /*Torso*/,
                            360103 /*Pants*/, 410505 /*Hands*/, 560103 /*Feet*/, 600101 /*Cape*/, 30300101 /*Necklace*/,
                            30200107 /*Earring*/, 30400105 /*Belt*/, 30100106 /*Ring*/, 70000101 /*Talk Ring*/,
                            110301 /*Avatar Head */,
                            210701 /*Avatar Torso*/, 360103 /*Avatar Pants*/, 410505 /*Avatar Hands*/,
                            560103 /*Avatar Feet*/
                        };
                        headSlot = new int[19] {0, 0, 0, 00, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 00, 0, 0, 0, 0};
                        break;
                    case "Kadred":
                        myCharacter.EquipId = new int[]
                        {
                            10100301 /*Weapon*/, 15100901 /*Shield* */, 20000101 /*Arrow*/, 110504 /*head*/,
                            260103 /*Torso*/,
                            360103 /*Pants*/, 460103 /*Hands*/, 510301 /*Feet*/, 690101 /*Cape*/, 30300101 /*Necklace*/,
                            30200107 /*Earring*/, 30400105 /*Belt*/, 30100106 /*Ring*/, 70000101 /*Talk Ring*/,
                            160801 /*Avatar Head */,
                            260801 /*Avatar Torso*/, 360801 /*Avatar Pants*/, 460801 /*Avatar Hands*/,
                            560801 /*Avatar Feet*/
                        };
                        headSlot = new int[19] {0, 0, 0, 42, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 42, 0, 0, 0, 0};
                        break;
                    case "Zakura":
                        myCharacter.EquipId = new int[]
                        {
                            11400303 /*Weapon*/, 0 /*Shield* */, 20000101 /*Arrow*/, 110504 /*head*/, 260103 /*Torso*/,
                            360103 /*Pants*/, 460103 /*Hands*/, 510301 /*Feet*/, 690101 /*Cape*/, 30300101 /*Necklace*/,
                            30200107 /*Earring*/, 30400105 /*Belt*/, 30100106 /*Ring*/, 70000101 /*Talk Ring*/,
                            100403 /*Avatar Head */,
                            260801 /*Avatar Torso*/, 360801 /*Avatar Pants*/, 460801 /*Avatar Hands*/,
                            510301 /*Avatar Feet*/
                        };
                        headSlot = new int[19] {0, 0, 0, 42, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 42, 0, 0, 0, 0};
                        break;
                    case "Test1":
                        myCharacter.EquipId = new int[]
                        {
                            11500102 /*Weapon*/, 15100801 /*Shield* */, 20000101 /*Arrow*/, 110504 /*head*/,
                            260103 /*Torso*/,
                            360103 /*Pants*/, 460103 /*Hands*/, 560103 /*Feet*/, 690101 /*Cape*/, 30300101 /*Necklace*/,
                            30200107 /*Earring*/, 30400105 /*Belt*/, 30100106 /*Ring*/, 70000101 /*Talk Ring*/,
                            161101 /*Avatar Head */,
                            261101 /*Avatar Torso*/, 361101 /*Avatar Pants*/, 461101 /*Avatar Hands*/,
                            561101 /*Avatar Feet*/
                        };
                        headSlot = new int[19] {0, 0, 0, 00, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 00, 0, 0, 0, 0};
                        break;
                    case "One":
                        myCharacter.EquipId = new int[]
                        {
                            10800405 /*Weapon*/, 15100901 /*Shield* */, 20000101 /*Arrow*/, 110301 /*head*/,
                            210701 /*Torso*/,
                            360103 /*Pants*/, 401201 /*Hands*/, 560103 /*Feet*/, 690101 /*Cape*/, 30300101 /*Necklace*/,
                            30200107 /*Earring*/, 30400105 /*Belt*/, 30100106 /*Ring*/, 70000101 /*Talk Ring*/,
                            160801 /*Avatar Head */,
                            260801 /*Avatar Torso*/, 360801 /*Avatar Pants*/, 460801 /*Avatar Hands*/,
                            560801 /*Avatar Feet*/
                        };
                        headSlot = new int[19] {0, 0, 0, 00, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 00, 0, 0, 0, 0};
                        break;
                    case "Wolfzen":
                        myCharacter.EquipId = new int[]
                        {
                            10800405 /*Weapon*/, 15100901 /*Shield* */, 20000101 /*Arrow*/, 110501 /*head*/,
                            260103 /*Torso*/,
                            360103 /*Pants*/, 460103 /*Hands*/, 510301 /*Feet*/, 690101 /*Cape*/, 30300101 /*Necklace*/,
                            30200107 /*Earring*/, 30400105 /*Belt*/, 30100106 /*Ring*/, 70000101 /*Talk Ring*/,
                            160801 /*Avatar Head */,
                            260801 /*Avatar Torso*/, 360801 /*Avatar Pants*/, 460801 /*Avatar Hands*/,
                            560801 /*Avatar Feet*/
                        };
                        headSlot = new int[19] {0, 0, 0, 42, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
                        //EquipBitMask = new int[] {1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 0, 0, 0, 0, 0};
                        break;
                    case "Wolf":
                        myCharacter.EquipId = new int[]
                        {
                            11300404 /*Weapon*/, 0 /*Shield* */, 20000101 /*Arrow*/, 110504 /*head*/, 260103 /*Torso*/,
                            360103 /*Pants*/, 460103 /*Hands*/, 510301 /*Feet*/, 690101 /*Cape*/, 30300101 /*Necklace*/,
                            30200107 /*Earring*/, 30400105 /*Belt*/, 30100106 /*Ring*/, 70000101 /*Talk Ring*/,
                            100403 /*Avatar Head */,
                            260801 /*Avatar Torso*/, 360801 /*Avatar Pants*/, 460801 /*Avatar Hands*/,
                            510301 /*Avatar Feet*/
                        };
                        headSlot = new int[19] {0, 0, 0, 42, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 42, 0, 0, 0, 0};
                        break;
                    case "Hades":
                        myCharacter.EquipId = new int[]
                        {
                            11300706 /*Weapon*/, 0 /*Shield* */, 0 /*Arrow*/, 161101 /*head*/, 220401 /*Torso*/,
                            320401 /*Pants*/, 420302 /*Hands*/, 520302 /*Feet*/, 600101 /*Cape*/, 30300101 /*Necklace*/,
                            30200107 /*Earring*/, 30400105 /*Belt*/, 30100106 /*Ring*/, 70000101 /*Talk Ring*/,
                            161101 /*Avatar Head */,
                            220401 /*Avatar Torso*/, 320101 /*Avatar Pants*/, 420101 /*Avatar Hands*/,
                            520101 /*Avatar Feet*/
                        };
                        headSlot = new int[19] {0, 0, 0, 00, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 00, 0, 0, 0, 0};
                        break;
                    case "Quopaty Monk":
                    case "Quopaty Priest":
                        myCharacter.EquipId = new int[]
                        {
                            11200301 /*Weapon*/, 0 /*Shield* */, 0 /*Arrow*/, 0 /*head*/, 220601 /*Torso*/,
                            320101 /*Pants*/, 400101 /*Hands*/, 500101 /*Feet*/, 0 /*Cape*/, 0 /*Necklace*/,
                            0 /*Earring*/, 0 /*Belt*/, 0 /*Ring*/, 0 /*Talk Ring*/, 0 /*Avatar Head */,
                            220601 /*Avatar Torso*/, 320601 /*Avatar Pants*/, 400101 /*Avatar Hands*/,
                            500101 /*Avatar Feet*/
                        };
                        headSlot = new int[19] {0, 0, 0, 004, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 04, 0, 0, 0, 0};
                        break;
                    default:
                        myCharacter.EquipId = new int[]
                        {
                            10300102 /*Weapon*/, 15000101 /*Shield* */, 0 /*Arrow*/, 0 /*head*/, 200101 /*Torso*/,
                            300101 /*Pants*/, 400101 /*Hands*/, 500101 /*Feet*/, 0 /*Cape*/, 0 /*Necklace*/,
                            0 /*Earring*/, 0 /*Belt*/, 0 /*Ring*/, 0 /*Talk Ring*/, 0 /*Avatar Head */,
                            200101 /*Avatar Torso*/, 300101 /*Avatar Pants*/, 400101 /*Avatar Hands*/,
                            500101 /*Avatar Feet*/
                        };
                        headSlot = new int[19] {0, 0, 0, 004, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 04, 0, 0, 0, 0};
                        break;
                }
            }

            return headSlot;
        }
    }
}
