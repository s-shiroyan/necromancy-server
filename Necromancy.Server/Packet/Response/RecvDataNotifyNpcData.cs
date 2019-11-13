using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Response
{
    public class RecvDataNotifyNpcData : PacketResponse
    {
        private NpcSpawn _npcSpawn;

        public RecvDataNotifyNpcData(NpcSpawn npcSpawn)
            : base((ushort) AreaPacketId.recv_data_notify_npc_data, ServerType.Area)
        {
            _npcSpawn = npcSpawn;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_npcSpawn.InstanceId); // InstanceId 
            res.WriteInt32(_npcSpawn.NpcId); // NPC Serial ID from "npc.csv"
            res.WriteByte(0); // 0 - Clickable NPC (Active NPC, player can select and start dialog), 1 - Not active NPC (Player can't start dialog)
            res.WriteCString(_npcSpawn.Name); //Name
            res.WriteCString(_npcSpawn.Title); //Title
            res.WriteFloat(_npcSpawn.X); //X Pos
            res.WriteFloat(_npcSpawn.Y); //Y Pos
            res.WriteFloat(_npcSpawn.Z); //Z Pos
            res.WriteByte((byte)(_npcSpawn.Heading + Util.GetRandomNumber(0,180))); //view offset

            int numEquipments = 0; // 1 to 19 equipment.  Setting to 0 because NPCS don't wear gear.
            res.WriteInt32(numEquipments); // # Items to Equip
            for (int i = 0; i < numEquipments; i++)
            {
                res.WriteInt32(24);
            }

            res.WriteInt32(numEquipments); // # Items to Equip
            for (int i = 0; i < numEquipments; i++)
            {
                res.WriteInt32(210901);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(3);
                res.WriteInt32(10310503);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(3);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(1); // bool
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
            }

            res.WriteInt32(numEquipments); // # Items to Equip
            for (int i = 0; i < numEquipments; i++) // Item type bitmask per slot
            {
                res.WriteInt32(1);
            }

            res.WriteInt32(_npcSpawn.ModelId); //NPC Model from file "model_common.csv"
            res.WriteInt16(_npcSpawn.Size); //NPC Model Size
            res.WriteByte(0); //ToDo:  pull in map_symbol.csv column B
            res.WriteByte(0); //ToDo:  pull in map_symbol.csv column C
            res.WriteByte(0); //ToDo:  pull in map_symbol.csv column D
            res.WriteInt32(0);  //ToDo:  pull in map_symbol.csv column E or //ToDo:  pull in NPC.CSV column A //Hp Related Bitmask?  This setting makes the NPC "alive"    11111110 = npc flickering, 0 = npc alive
            res.WriteInt32(Util.GetRandomNumber(1, 9)); //npc Emoticon above head 1 for skull
            res.WriteInt32(Util.GetRandomNumber(5,15)); //ToDo:  pull in NPC.CSV column C  | 
            //horse: 4 TP machine:5 Ghost: 6 Illusion 7. Dungeun: 8 Stone 9. Ggate 1.  torch 13,14,15. power spot :22  event:23 ??:16,17,18
            res.WriteFloat(0); //x for particle effects from Int32 above //ToDo:  pull in NPC.CSV column D
            res.WriteFloat(0); //y for particle effects from Int32 above  //ToDo:  pull in NPC.CSV column E
            res.WriteFloat(0); //z for particle effects from Int32 above  //ToDo:  pull in NPC.CSV column F
            int numStatusEffects = 128;
            res.WriteInt32(numStatusEffects); //number of status effects. 128 Max.
            for (int i = 0; i < numStatusEffects; i++)
            {
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
            }

            return res;
        }
    }
}
