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

            res.WriteInt32(0); // 0 = Alive | 1 = Dead
            res.WriteInt32(_character.Level); //character level stat
            res.WriteInt32(1); //TODO (unknown)
            res.WriteUInt32(_character.ClassId); //class stat 

            res.WriteUInt32(_character.Raceid);
            res.WriteUInt32(_character.Sexid);
            res.WriteByte(_character.HairId);
            res.WriteByte(_character.HairColorId);
            res.WriteByte(_character.FaceId);
            
            res.WriteInt32(ItemEquipDisplayType.KATANA_1H); //TODO
            res.WriteInt32(ItemEquipDisplayType.KATANA_1H);
            res.WriteInt32(ItemEquipDisplayType.quiver);

            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            for (int i = 0; i < 19; i++)
            {
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
            for (int i = 0; i < 19; i++)
                res.WriteInt32(0);
            for (int i = 0; i < 19; i++)
                res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
