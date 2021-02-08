using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvDataNotifyCharaBodyData : PacketResponse
    {
        private readonly DeadBody _deadBody;
        private readonly Character _character;

        public RecvDataNotifyCharaBodyData(DeadBody deadBody, Character character, NecClient client) 
            : base((ushort) AreaPacketId.recv_data_notify_charabody_data, ServerType.Area)
        {
            _deadBody = deadBody;
            _character = character;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_deadBody.InstanceId); //Instance ID of dead body
            res.WriteUInt32(_deadBody.CharacterInstanceId); //Reference to actual player's instance ID
            res.WriteCString($"{_deadBody.SoulName}"); // Soul name 
            res.WriteCString($"{_deadBody.CharaName}"); // Character name
            res.WriteFloat(_deadBody.X); // X
            res.WriteFloat(_deadBody.Y); // Y
            res.WriteFloat(_deadBody.Z); // Z
            res.WriteByte(_deadBody.Heading); // Heading
            res.WriteInt32(_deadBody.Level);

            res.WriteInt16(01); //new
            int i = 0;
            int numEntries = 19;
            res.WriteInt32(numEntries); //less than or equal to 19
                                          //Consolidated Frequently Used Code
                                          //LoadEquip.SlotSetup(res, _character, numEntries);
            foreach (ItemInstance itemInstance in _character.EquippedItems.Values)
            {
                res.WriteInt32((int)itemInstance.Type);
                //Logger.Debug($"Loading {i}:{itemInstance.Type} | {itemInstance.UnidentifiedName}");
                i++;
            }
            while (i < numEntries)
            {
                //sub_483660   
                res.WriteInt32(0); //Must have 25 on recv_chara_notify_data
                //Logger.Debug($"Loading {i}: blank");
                i++;
            }
            res.WriteInt32(numEntries);
            //Consolidated Frequently Used Code
            //LoadEquip.EquipItems(res, _character, numEntries);
            i = 0;
            //sub_4948C0
            foreach (ItemInstance itemInstance in _character.EquippedItems.Values)
            {
                res.WriteInt32(itemInstance.BaseID); //Item Base Model ID
                res.WriteByte(00); //? TYPE data/chara/##/ 00 is character model, 01 is npc, 02 is monster
                res.WriteByte(byte.Parse($"{_character.RaceId}{_character.SexId}")); //Race and gender tens place is race 1= human, 2= elf 3=dwarf 4=gnome 5=porkul, ones is gender 1 = male 2 = female
                res.WriteByte(16); //??item version

                res.WriteInt32(itemInstance.BaseID); //testing (Theory, texture file related)
                res.WriteByte(0); //hair
                res.WriteByte(1); //color
                res.WriteByte(0); //face

                res.WriteByte(45); // Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
                res.WriteByte((byte)(_character.FaceId * 10));  //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
                res.WriteByte(00); // testing (Theory Torso Tex)
                res.WriteByte(0); // testing (Theory Pants Tex)
                res.WriteByte(0); // testing (Theory Hands Tex)
                res.WriteByte(0); // testing (Theory Feet Tex)
                res.WriteByte(1); //Alternate texture for item model  0 normal : 1 Pink 

                res.WriteByte(0); // separate in assembly
                res.WriteByte(0); // separate in assembly
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

                res.WriteByte(0); // separate in assembly
                res.WriteByte(0); // separate in assembly
                i++;
            }
            res.WriteInt32(numEntries);
            //Consolidated Frequently Used Code
            //LoadEquip.EquipSlotBitMask(res, _character, numEntries);
            i = 0;
            //sub_483420 
            foreach (ItemInstance itemInstance in _character.EquippedItems.Values)
            {
                res.WriteInt32((int)itemInstance.CurrentEquipSlot); //bitmask per equipment slot
                i++;
            }
            while (i < numEntries)
            {
                //sub_483420   
                res.WriteInt32(0); //Must have 25 on recv_chara_notify_data
                i++;
            }

            //Traits
            res.WriteUInt32(_character.RaceId); //race
            res.WriteUInt32(_character.SexId);
            res.WriteByte(_character.HairId); //hair
            res.WriteByte(_character.HairColorId); //color
            res.WriteByte(_character.FaceId); //face

            res.WriteInt32(_deadBody.ConnectionState); // 0 = bag, 1 for dead? (Can't enter soul form if this isn't 0 or 1 i think).
            res.WriteInt32(_deadBody.ModelType); //4 = ash pile, not sure what this is.
            res.WriteInt32(0);
            res.WriteInt32(_deadBody.deathPose); //death pose 0 = faced down, 1 = head chopped off, 2 = no arm, 3 = faced down, 4 = chopped in half, 5 = faced down, 6 = faced down, 7 and up "T-pose" the body (ONLY SEND 1 IF YOU ARE CALLING THIS FOR THE FIRST TIME)
            res.WriteByte(_deadBody.CriminalStatus); //crim status (changes icon on the end also), 0 = white, 1 = yellow, 2 = red, 3 = red with crim icon, 
            res.WriteByte(_deadBody.BeginnerProtection); // (bool) Beginner protection
            res.WriteInt32(1);

            return res;
        }
    }
}
