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

        public override ushort Id => (ushort) AreaPacketId.send_map_get_info;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(1001001);
            

            Router.Send(client, (ushort) AreaPacketId.recv_map_get_info_r, res);

            SendDataNotifyNpcData(client);
        }

        private void SendDataNotifyNpcData(NecClient client)
        {
            IBuffer res2 = BufferProvider.Provide();

            res2.WriteInt32(3);             // NPC ID (object id)

            res2.WriteInt32(10000108);      // NPC Serial ID from "npc.csv"

            res2.WriteByte(0);              // 0 - Clickable NPC (Active NPC, player can select and start dialog), 1 - Not active NPC (Player can't start dialog)

            res2.WriteCString("liar");//Name

            res2.WriteCString("training center personnel");//Title

            res2.WriteFloat(23162);//X Pos
            res2.WriteFloat(-200);//Y Pos
            res2.WriteFloat(3);//Z Pos
            res2.WriteByte(90);//view offset

            res2.WriteInt32(19);

            //this is an x19 loop but i broke it up
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

                res2.WriteInt32(0);
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

            res2.WriteInt32(1011101);   //NPC Model from file "model_common.csv"

            res2.WriteInt16(100);       //NPC Model Size

            res2.WriteByte(1);            

            res2.WriteByte(1);

            res2.WriteByte(1);

            res2.WriteInt32(100);

            res2.WriteInt32(100);

            res2.WriteInt32(100);
            res2.WriteFloat(1000);
            res2.WriteFloat(1000);
            res2.WriteFloat(1000);

            res2.WriteInt32(128);

            int numEntries2 = 128;


            for (int i = 0; i < numEntries2; i++)

            {
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);

            }

            Router.Send(client, (ushort)AreaPacketId.recv_data_notify_npc_data, res2);
        }
    }
}