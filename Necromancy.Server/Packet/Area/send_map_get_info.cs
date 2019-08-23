using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_map_get_info : Handler
    {
        public send_map_get_info(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_map_get_info;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(1001001);


            Router.Send(client, (ushort)AreaPacketId.recv_map_get_info_r, res);

            SendDataNotifyNpcData(client);
        }
        int[] NPCModelID = new int[] { 1911105, 1112101, 1122401, 1122101, 1311102, 1111301, 1121401, 1131401 };
        int[] NPCSerialID = new int[] { 10000101, 10000102, 10000103, 10000104, 10000105, 10000106, 10000107, 10000108 };
        int[] NPCX = new int[] { 31455, 26562, 26757, 24136, 22906, 26583, 33065, 31737 };
        int[] NPCY = new int[] { 7398, -5580, 2480, 12, -928, -203, 3613, 6643 };
        int[] NPCZ = new int[] { 46, -134, -15, 2, 94, -24, -2, 8 };
        byte[] NPCViewAngle = new byte[] { 0, 15, 58, 100, 15, 58, 100, 15, };

        int x = 0;


        private void SendDataNotifyNpcData(NecClient client)
        {

            //IBuffer res2 = BufferProvider.Provide();

            foreach (int y in NPCModelID)
            {
                IBuffer res2 = BufferProvider.Provide();
                res2.WriteInt32(x);             // NPC ID (object id)

                res2.WriteInt32(237);      // NPC Serial ID from "npc.csv"

                res2.WriteByte(0);              // 0 - Clickable NPC (Active NPC, player can select and start dialog), 1 - Not active NPC (Player can't start dialog)

                res2.WriteCString("tester");//Name

                res2.WriteCString("tester");//Title

                res2.WriteFloat(NPCX[x]);//X Pos
                res2.WriteFloat(NPCY[x]);//Y Pos
                res2.WriteFloat(NPCZ[x]);//Z Pos
                res2.WriteByte(NPCViewAngle[x]);//view offset

                res2.WriteInt32(19);

                //this is an x19 loop but i broke it up
                res2.WriteInt32(10310503);
                res2.WriteInt32(-1);
                res2.WriteInt32(-1);
                res2.WriteInt32(-1);
                res2.WriteInt32(-1);
                res2.WriteInt32(-1);
                res2.WriteInt32(-1);
                res2.WriteInt32(-1);
                res2.WriteInt32(-1);
                res2.WriteInt32(-1);
                res2.WriteInt32(-1);
                res2.WriteInt32(-1);
                res2.WriteInt32(-1);
                res2.WriteInt32(-1);
                res2.WriteInt32(-1);
                res2.WriteInt32(-1);
                res2.WriteInt32(-1);
                res2.WriteInt32(-1);
                res2.WriteInt32(-1);

                res2.WriteInt32(19);


                int numEntries = 19;


                for (int i = 0; i < numEntries; i++)

                {
                    // loop start
                    res2.WriteInt32(10310503); // this is a loop within a loop i went ahead and broke it up
                    res2.WriteByte(0);
                    res2.WriteByte(0);
                    res2.WriteByte(0);

                    res2.WriteInt32(10310503);
                    res2.WriteByte(0);
                    res2.WriteByte(0);
                    res2.WriteByte(0);

                    res2.WriteByte(0);
                    res2.WriteByte(0);
                    res2.WriteByte(1); // bool
                    res2.WriteByte(0);
                    res2.WriteByte(0);
                    res2.WriteByte(0);
                    res2.WriteByte(0);
                    res2.WriteByte(0);

                }

                res2.WriteInt32(19);

                //this is an x19 loop but i broke it up
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);

                res2.WriteInt32(NPCModelID[x]);   //NPC Model from file "model_common.csv"
                x++;


                res2.WriteInt16(100);       //NPC Model Size

                res2.WriteByte(237);

                res2.WriteByte(237);

                res2.WriteByte(237);

                res2.WriteInt32(237);

                res2.WriteInt32(237); //npc state?

                res2.WriteInt32(237);
                res2.WriteFloat(1000);
                res2.WriteFloat(1000);
                res2.WriteFloat(1000);

                res2.WriteInt32(128);

                int numEntries2 = 128;


                for (int i = 0; i < numEntries2; i++)

                {
                    res2.WriteInt32(237);
                    res2.WriteInt32(237);
                    res2.WriteInt32(237);

                }

                Router.Send(client, (ushort)AreaPacketId.recv_data_notify_npc_data, res2);
            }
        }
    }
}