using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvDataNotifyMonsterData : PacketResponse
    {
        private MonsterSpawn _monsterSpawn;
        private Character _character = new Character();

        public RecvDataNotifyMonsterData(MonsterSpawn monsterSpawn)
            : base((ushort) AreaPacketId.recv_data_notify_monster_data, ServerType.Area)
        {
            _monsterSpawn = monsterSpawn;
            _character.Name = monsterSpawn.Name;
        }

        protected override IBuffer ToBuffer()
        {
            int numEntries = 16; //Max of 16 Equipment Slots for Monster.  cmp to 0x10
            int numStatusEffects = 0x80; //Statuses effects. Max 128

            if (_monsterSpawn.ModelId > 52000 /*CharacterModelUpperLimit*/)
            {
                numEntries = 0;
            } //ToDo find any videos with monsters holding weapons.

            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_monsterSpawn.InstanceId);
            res.WriteCString(_monsterSpawn.Name);
            res.WriteCString(_monsterSpawn.Title);
            res.WriteFloat(_monsterSpawn.X);
            res.WriteFloat(_monsterSpawn.Y);
            res.WriteFloat(_monsterSpawn.Z);
            res.WriteByte(_monsterSpawn.Heading);
            res.WriteInt32(_monsterSpawn.MonsterId); //Monster Serial ID
            res.WriteInt32(_monsterSpawn.ModelId); //Monster Model ID
            res.WriteInt16(_monsterSpawn.Size);

            res.WriteByte(0);//new
            res.WriteByte(0);//new
            res.WriteByte(0);//new

            res.WriteInt32(numEntries); // Number of equipment Slots
            int i = 0;
            while (i < numEntries)
            {
                //sub_483660   
                res.WriteInt32(0); //Must have 16 on recv_data_notify_monster_data
                i++;
            }

            //sub_483420
            res.WriteInt32(numEntries); // Number of equipment Slots
            i = 0;
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
            //Equipment bitmask 
            res.WriteInt32(numEntries); // cmp to 0x10 = 16
            for (i = 0; i < numEntries; i++)
            {
                res.WriteInt64(0);
            }

            res.WriteInt32(0b00000000); //BITMASK for Monster State
            //0bxxxxxxx1 - 1 Dead / 0 Alive  | 
            //0bxxxxxx1x - 1 crouching / 0 standing
            //0bxxxxx1xx - 
            //0bxxxx1xxx - 1 crouching / 0 standing
            //0bxxx1xxxx -
            //0bxx1xxxxx - 
            //0bx1xxxxxx - 1 Aggro Battle  / 0 Normal    | (for when you join a map and the monster is in battle)
            //0b1xxxxxxx - 
            res.WriteInt64(0); //
            res.WriteInt64(0); //
            res.WriteInt64(0); //
            res.WriteByte(231);
            res.WriteByte(232);
            res.WriteByte(0);//new
            res.WriteInt32(_monsterSpawn.Hp.current); //Current HP
            res.WriteInt32(_monsterSpawn.Hp.max); //Max HP
            res.WriteInt32(numStatusEffects); // cmp to 0x80 = 128
            for (i = 0; i < numStatusEffects; i++)
            {
                res.WriteInt32(0); // status effect ID. set to i
                res.WriteInt32(0); //1 on 0 off
                res.WriteInt32(0);
                res.WriteInt32(0);//new
            }

            return res;
        }
    }
}
