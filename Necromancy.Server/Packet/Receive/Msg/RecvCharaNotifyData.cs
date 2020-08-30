using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Items;
using System.Collections.Generic;
using System.Linq;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvCharaNotifyData : PacketResponse
    {
        private readonly Character _character;
        public RecvCharaNotifyData(Character character)
            : base((ushort) MsgPacketId.recv_chara_notify_data, ServerType.Msg)
        {
            _character = character;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(_character.Slot); //character slot, 0 for left, 1 for middle, 2 for right
            res.WriteInt32(_character.Id); //  Character ID
            res.WriteFixedString(_character.Name, 91); // 0x5B | 91x 1 byte

            res.WriteInt32(0); // 0 = Alive | 1 = Dead  probably not 0 but probably a bitmask for status
            res.WriteInt32(_character.Level); //character level stat
            res.WriteInt32(1); //TODO (unknown)
            res.WriteUInt32(_character.ClassId); //class stat 

            res.WriteUInt32(_character.RaceId);
            res.WriteUInt32(_character.SexId);
            res.WriteByte(_character.HairId);
            res.WriteByte(_character.HairColorId);
            res.WriteByte(_character.FaceId);

            byte numOfEquippedItems = (byte)_character.EquippedItems.Count;
            for (int i = 0; i < numOfEquippedItems; i++)
            {
                res.WriteInt32(0); //TODO figure out what the fuck this is for Item display type? something with spagetti load equipment
            }
            for (int i = 0; i < numOfEquippedItems; i++)
            { //todo textures
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
                res.WriteByte(0); //bool
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
            }
            for (int i = 0; i < numOfEquippedItems; i++)
                res.WriteInt32(0); //TODOequipslot?
            for (int i = 0; i < numOfEquippedItems; i++)
                res.WriteInt32(0); //TODOupgrade level?
            res.WriteByte(numOfEquippedItems); //number of equpped items
            res.WriteInt32(0); //map id
            return res;
        }
    }
}
