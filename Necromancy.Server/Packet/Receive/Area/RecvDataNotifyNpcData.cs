using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvDataNotifyNpcData : PacketResponse
    {
        public RecvDataNotifyNpcData()
            : base((ushort) AreaPacketId.recv_data_notify_npc_data, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(3);             // NPC ID (object id)

            res.WriteInt32(10000108);      // NPC Serial ID from "npc.csv"

            res.WriteByte(0);              // 0 - Clickable NPC (Active NPC, player can select and start dialog), 1 - Not active NPC (Player can't start dialog)

            res.WriteCString("liar");//Name

            res.WriteCString("training center personnel");//Title

            res.WriteFloat(23200);//X Pos
            res.WriteFloat(-50);//Y Pos
            res.WriteFloat(3);//Z Pos
            res.WriteByte(90);//view offset

            res.WriteInt32(19);

            //this is an x19 loop but i broke it up
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);

            res.WriteInt32(19);


            int numEntries = 19;


            for (int i = 0; i < numEntries; i++)

            {
                // loop start
                res.WriteInt32(10310503); // this is a loop within a loop i went ahead and broke it up
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(1); // bool
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

            }

            res.WriteInt32(19);

            //this is an x19 loop but i broke it up
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);

            res.WriteInt32(1011101);   //NPC Model from file "model_common.csv"

            res.WriteInt16(100);       //NPC Model Size

            res.WriteByte(1);

            res.WriteByte(1);

            res.WriteByte(1);

            res.WriteInt32(100);

            res.WriteInt32(100);

            res.WriteInt32(100);
            res.WriteFloat(1000);
            res.WriteFloat(1000);
            res.WriteFloat(1000);

            res.WriteInt32(128);

            int numEntries2 = 128;


            for (int i = 0; i < numEntries2; i++)

            {
                res.WriteInt32(1);
                res.WriteInt32(1);
                res.WriteInt32(1);

            }
            return res;
        }
    }
}
