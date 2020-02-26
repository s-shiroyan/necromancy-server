using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;


namespace Necromancy.Server.Packet.Response
{
    public class RecvDataNotifyCharaBodyData : PacketResponse
    {
        private readonly DeadBody _deadBody;
        private readonly Character _character;
        private readonly Soul _soul;

        public RecvDataNotifyCharaBodyData(DeadBody deadBody, NecClient client)// Character character, client)
            : base((ushort)AreaPacketId.recv_data_notify_charabody_data, ServerType.Area)
        {
            _deadBody = deadBody;
            _character = client.Character;
            _soul = client.Soul;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res14 = BufferProvider.Provide();
            res14.WriteInt32(_deadBody.InstanceId); //Instance ID of dead body
            res14.WriteInt32(_deadBody.CharacterInstanceId); //Reference to actual player's instance ID
            res14.WriteCString($"{_soul.Name}"); // Soul name 
            res14.WriteCString($"{_deadBody.CharaName}"); // Character name
            res14.WriteFloat(_deadBody.X); // X
            res14.WriteFloat(_deadBody.Y); // Y
            res14.WriteFloat(_deadBody.Z); // Z
            res14.WriteByte(_deadBody.Heading); // Heading
            res14.WriteInt32(8);

            int numEntries = 19;
            res14.WriteInt32(numEntries);//less than or equal to 19
            //Consolidated Frequently Used Code
            LoadEquip.SlotSetup(res14, _character, numEntries);

            numEntries = 19;
            res14.WriteInt32(numEntries);
            //Consolidated Frequently Used Code
            LoadEquip.EquipItems(res14, _character, numEntries);

            numEntries = 19;
            res14.WriteInt32(numEntries);
            //Consolidated Frequently Used Code
            LoadEquip.EquipSlotBitMask(res14, _character, numEntries);

            //Consolidated Frequently Used Code
            LoadEquip.BasicTraits(res14, _character);

            res14.WriteInt32(_deadBody.ConnectionState);// 0 = bag, 1 for dead? (Can't enter soul form if this isn't 0 or 1 i think).
            res14.WriteInt32(_deadBody.ModelType);//4 = ash pile, not sure what this is.
            res14.WriteInt32(0);
            res14.WriteInt32(_deadBody.deathPose); //death pose 0 = faced down, 1 = head chopped off, 2 = no arm, 3 = faced down, 4 = chopped in half, 5 = faced down, 6 = faced down, 7 and up "T-pose" the body (ONLY SEND 1 IF YOU ARE CALLING THIS FOR THE FIRST TIME)
            res14.WriteByte(_deadBody.CriminalStatus);//crim status (changes icon on the end also), 0 = white, 1 = yellow, 2 = red, 3 = red with crim icon, 
            res14.WriteByte(_deadBody.BeginnerProtection);// (bool) Beginner protection
            res14.WriteInt32(1);

            return res14;
        }
    }
}
