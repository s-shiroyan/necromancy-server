using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
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
            res.WriteUInt32(_character.InstanceId);
            res.WriteCString(_soulName);
            res.WriteCString(_character.Name);
            res.WriteFloat(_character.X);
            res.WriteFloat(_character.Y);
            res.WriteFloat(_character.Z);
            res.WriteByte(_character.Heading);
            res.WriteInt32(_character.Level); //Player level (main gui)
            res.WriteUInt32((uint)_character.State);
            res.WriteInt16((short)_character.Hp.max); //Player max hp (main gui)

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
            res.WriteUInt32(_character.partyId); // party id?
            //sub_4837C0
            res.WriteUInt32(_character.partyRequest); // party id? // i don't think sooo'
            //sub_read_byte
            res.WriteByte(_character.criminalState); //Criminal name icon
            //sub_494890
            res.WriteByte((byte) _character.beginnerProtection); //Bool Beginner Protection
            //sub_4835E0
            res.WriteInt32(_character.movementPose); //pose, 1 = sitting, 0 = standing
            //sub_483920
            res.WriteInt32(0);
            //sub_483440
            res.WriteInt16(_character.Level); //Player level (stat gui)
            //sub_read_byte
            res.WriteByte(1); //no change?
            //sub_read_byte
            res.WriteByte(1); //no change?
            //sub_read_int_32
            res.WriteInt32(0); //title; 0 - display title, 1 - no title
            //sub_483580
            res.WriteUInt32(_character.ClassId); //Signifies character class
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
            res.WriteCString("" /*_character.comment*/); //Comment string
            return res;
        }
    }
}
