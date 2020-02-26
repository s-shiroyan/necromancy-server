using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Response
{
    public class RecvDataNotifyCharaData : PacketResponse
    {
        private readonly Character _character;
        private readonly string _soulName;

        public RecvDataNotifyCharaData(Character character, string soulName)
            : base((ushort) AreaPacketId.recv_data_notify_chara_data, ServerType.Area)
        {
            _character = character;
            _soulName = soulName;
        }

        protected override IBuffer ToBuffer()
        {
            int numEntries = 19; //Max of 19 Equipment Slots for Character Player
            int numStatusEffects = 0x80; //Statuses effects. Max 128

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_character.InstanceId);
            res.WriteCString(_soulName);
            res.WriteCString(_character.Name);
            res.WriteFloat(_character.X);
            res.WriteFloat(_character.Y);
            res.WriteFloat(_character.Z);
            res.WriteByte(_character.Heading);
            res.WriteInt32(0);
            res.WriteInt32(_character.state); //BITMASK for Character State
                                        //0bxxxxxxx1 - 1 Soul Form / 0 Normal  | (Soul form is Glowing with No armor) 
                                        //0bxxxxxx1x - 1 Battle Pose / 0 Normal
                                        //0bxxxxx1xx - 1 Block Pose / 0 Normal | (for coming out of stealth while blocking)
                                        //0bxxxx1xxx - 1 transparent / 0 solid  | (stealth in party partial visibility)
                                        //0bxxx1xxxx -
                                        //0bxx1xxxxx - 1 invisible / 0 visible  | (Stealth to enemies)
                                        //0bx1xxxxxx - 1 blinking  / 0 solid    | (10  second invulnerability blinking)
                                        //0b1xxxxxxx - 

            res.WriteInt16(0); //Character Size/Radius?

            res.WriteInt32(numEntries); // Number of equipment Slots
            //Consolidated Frequently Used Code
            LoadEquip.SlotSetup(res, _character, numEntries);
            //sub_483420
            res.WriteInt32(numEntries); // Number of equipment Slots
            //Consolidated Frequently Used Code
            LoadEquip.EquipItems(res, _character, numEntries);
            //sub_483420
            res.WriteInt32(numEntries); // Number of equipment Slots
            //Consolidated Frequently Used Code
            LoadEquip.EquipSlotBitMask(res, _character, numEntries);

            //sub_4835C0
            res.WriteInt32(0); //1 here means crouching?
            //sub_484660
            LoadEquip.BasicTraits(res, _character);
            //sub_483420
            res.WriteInt32(0); // party id?
            //sub_4837C0
            res.WriteInt32(1); // party id? // i don't think sooo'
            //sub_read_byte
            res.WriteByte(0); //Criminal name icon
            //sub_494890
            res.WriteByte(0); //Bool Beginner Protection
            //sub_4835E0
            res.WriteInt32(0); //pose, 1 = sitting, 0 = standing
            //sub_483920
            res.WriteInt32(0);
            //sub_483440
            res.WriteInt16(65);
            //sub_read_byte
            res.WriteByte(255); //no change?
            //sub_read_byte
            res.WriteByte(255); //no change?
            //sub_read_int_32
            res.WriteInt32(1); //title; 0 - display title, 1 - no title
            //sub_483580
            res.WriteInt32(244);
            //sub_483420
            res.WriteInt32(numStatusEffects); //Number of Status Effects to display 128 Max
            //sub_485A70
            for (int i = 0; i < numStatusEffects; i++)
            {
                res.WriteInt32(0); //Status effect ID
                res.WriteInt32(0);
                res.WriteInt32(0);
            }
            //sub_481AA0
            res.WriteCString(""/*_character.comment*/); //Comment string
            return res;
        }
    }
}
