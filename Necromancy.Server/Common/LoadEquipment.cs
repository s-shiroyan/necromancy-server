using Necromancy.Server.Model;
using System;
using Arrowgene.Buffers;
using Necromancy.Server.Model.ItemModel;
using Arrowgene.Logging;
using Necromancy.Server.Logging;

namespace Necromancy.Server.Common
{
    class LoadEquip
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(LoadEquip));

        public static void BasicTraits(IBuffer res, Character myCharacter)
        {
            res.WriteUInt32(myCharacter.Raceid); //race
            res.WriteUInt32(myCharacter.Sexid);
            res.WriteByte(myCharacter.HairId); //hair
            res.WriteByte(myCharacter.HairColorId); //color
            res.WriteByte(myCharacter.FaceId); //face
            res.WriteByte(1);//unknown
            res.WriteByte(2);//unknown
        }
        public static void SlotSetup(IBuffer res, Character character, int numEntries)
        {
            int i = 0;
            //sub_483660 
            foreach (InventoryItem inventoryItem in character.Inventory._equippedItems.Values)
            {
                res.WriteInt32((int)inventoryItem.Item.LoadEquipType);
                Logger.Debug($"Loading {inventoryItem.CurrentEquipmentSlotType} | {inventoryItem.Item.LoadEquipType}");
                i++;
            }
            while (i < numEntries)
            {
                //sub_483660   
                res.WriteInt32(0); //Must have 25 on recv_chara_notify_data
                i++;
            }
        }

        public static void EquipItems(IBuffer res, Character character, int numEntries)
        {
            int i = 0;
            //sub_4948C0
            foreach (InventoryItem inventoryItem in character.Inventory._equippedItems.Values)
            {
                res.WriteInt32(inventoryItem.Item.Id); //Sets your Item ID per Iteration
                res.WriteByte(0); //hair
                res.WriteByte(0); //color
                res.WriteByte(0); //face

                res.WriteInt32(inventoryItem.Item.Id); //testing (Theory, Icon related)
                res.WriteByte(character.HairId); //hair
                res.WriteByte(character.HairColorId); //color
                res.WriteByte(character.FaceId); //face

                res.WriteByte(0); // Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
                res.WriteByte(10); //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
                res.WriteByte((byte)Util.GetRandomNumber(1, 5)); // testing (Theory Torso Tex)
                res.WriteByte((byte)Util.GetRandomNumber(1, 5)); // testing (Theory Pants Tex)
                res.WriteByte((byte)Util.GetRandomNumber(1, 5)); // testing (Theory Hands Tex)
                res.WriteByte((byte)Util.GetRandomNumber(1, 5)); // testing (Theory Feet Tex)
                res.WriteByte(0); //Alternate texture for item model  0 normal : 1 Pink 

                res.WriteByte((byte)Util.GetRandomNumber(1, 5)); // separate in assembly
                res.WriteByte((byte)Util.GetRandomNumber(1, 5)); // separate in assembly
                i++;
            }
            while (i < numEntries)//Must have 25 on recv_chara_notify_data
            {
                res.WriteInt32(0); //Sets your Item ID per Iteration
                res.WriteByte(0); // 
                res.WriteByte(0); // (theory bag)
                res.WriteByte(0); // (theory Slot)

                res.WriteInt32(0); //testing (Theory, Icon related)
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

                res.WriteByte(1); // separate in assembly
                res.WriteByte(1); // separate in assembly
                i++;
            }

        }
        
        public static void EquipSlotBitMask(IBuffer res, Character character, int numEntries)
        {
            int i = 0;
            //sub_483420 
            foreach (InventoryItem inventoryItem in character.Inventory._equippedItems.Values)
            {
                res.WriteInt32((int)inventoryItem.Item.EquipmentSlotType); //bitmask per equipment slot
                i++;
            }
            while (i < numEntries)
            {
                //sub_483420   
                res.WriteInt32(0); //Must have 25 on recv_chara_notify_data
                i++;
            }
        }

        public static void SlotUpgradeLevel(IBuffer res, Character character, int numEntries)
        {
            int i = 0;
            foreach (InventoryItem inventoryItem in character.Inventory._equippedItems.Values)
            {
                res.WriteInt32(10); ///item quality(+#) or aura? 10 = +7, 19 = +6,(maybe just wep aura)
                i++;
            }
            while (i < numEntries)
            {
                //sub_483420   
                res.WriteInt32(10); //Must have 25 on recv_chara_notify_data
                i++;
            }
        }

    }
}
