using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvDataNotifyMonsterData : PacketResponse
    {
        public RecvDataNotifyMonsterData()
            : base((ushort) AreaPacketId.recv_data_notify_monster_data, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            res.WriteCString("ToBeFound");

            res.WriteCString("ToBeFound_2");

            res.WriteFloat(0);
            res.WriteFloat(0);
            res.WriteFloat(0);
            res.WriteByte(0);

            res.WriteInt32(0);

            res.WriteInt32(0);

            res.WriteInt16(0);

            res.WriteInt32(0x10); // cmp to 0x10 = 16

            int numEntries = 0x10;
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(0);
            }

            res.WriteInt32(0x10); // cmp to 0x10 = 16

            int numEntries2 = 0x10;
            for (int i = 0; i < numEntries2; i++)
            {
                res.WriteInt32(0); // this was an x2 loop (i broke it down)
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0); // bool
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);
            }

            res.WriteInt32(0x10); // cmp to 0x10 = 16

            int numEntries3 = 0x10;
            for (int i = 0; i < numEntries3; i++)

            {
                res.WriteInt64(0);
            }

            res.WriteInt32(0);

            res.WriteInt64(0);

            res.WriteInt64(0);

            res.WriteInt64(0);

            res.WriteByte(0);

            res.WriteByte(0);

            res.WriteInt32(0);

            res.WriteInt32(0);

            res.WriteInt32(0x80); // cmp to 0x80 = 128

            int numEntries4 = 0x80;
            for (int i = 0; i < numEntries4; i++)

            {
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
            }
            return res;
        }
    }
}
