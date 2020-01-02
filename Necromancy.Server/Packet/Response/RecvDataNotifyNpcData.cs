using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;


namespace Necromancy.Server.Packet.Response
{
    public class RecvDataNotifyNpcData : PacketResponse
    {
        private NpcSpawn _npcSpawn;
        int[] BitMask = new int[] //Correct Bit Mask
        {
            1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768, 65536, 131072, 262144,/* 524288,1048576, 2097152*/
        };

        public RecvDataNotifyNpcData(NpcSpawn npcSpawn)
            : base((ushort) AreaPacketId.recv_data_notify_npc_data, ServerType.Area)
        {
            _npcSpawn = npcSpawn;
        }

        protected override IBuffer ToBuffer()
        {
            int testi = BitMask[Util.GetRandomNumber(0, 18)];
            string binary = Convert.ToString(testi, 2);
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_npcSpawn.InstanceId); // InstanceId 
            res.WriteInt32(_npcSpawn.NpcId); // NPC Serial ID from "npc.csv"
            res.WriteByte(0); // interaction type. 0:chat bubble. 1:none 2:press f
            res.WriteCString(_npcSpawn.Name); //Name 
            res.WriteCString(_npcSpawn.Title); //Title
            res.WriteFloat(_npcSpawn.X); //X Pos
            res.WriteFloat(_npcSpawn.Y); //Y Pos
            res.WriteFloat(_npcSpawn.Z); //Z Pos
            res.WriteByte(_npcSpawn.Heading); //view offset

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
            res.WriteByte(4); //Hair ID for Character models
            res.WriteByte(5); //Hair Color ID for Character models
            res.WriteByte(3); //Face ID for Character models
            res.WriteInt32(0b11110); //BITMASK
                                   //0bxxxxx1 - 1 dead / 0 alive | for character models only 
                                   //0bxxxx1x - 1 Soul form visible / 0 soul form invisible
                                   //0bxxx1xx -
                                   //0bxx1xxx - 1 Show Emoticon / 0 Hide Emoticon
                                   //0bx1xxxx -
                                   //0b1xxxxx - 1 blinking  / 0 solid
            res.WriteInt32(Util.GetRandomNumber(1, 9));  //npc Emoticon above head 1 for skull. 2-9 different hearts
            res.WriteInt32(_npcSpawn.Status); //From  NPC.CSV column C  |   //horse: 4 TP machine:5 Ghost: 6 Illusion 7. Dungeun: 8 Stone 9. Ggate 1.  torch 13,14,15. power spot :22  event:23 ??:16,17,18
            res.WriteFloat(_npcSpawn.Status_X); //x for particle effects from Int32 above From NPC.CSV column D
            res.WriteFloat(_npcSpawn.Status_Y); //y for particle effects from Int32 above From NPC.CSV column E
            res.WriteFloat(_npcSpawn.Status_Z); //z for particle effects from Int32 above From NPC.CSV column F
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
