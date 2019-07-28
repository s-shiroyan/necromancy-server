using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_map_enter : Handler
    {
        public send_map_enter(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_map_enter;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteByte(0);//Bool

            Router.Send(client, (ushort) AreaPacketId.recv_map_enter_r, res);

            SendDataNotifyCharaData(client);
        }

        private void SendDataNotifyCharaData(NecClient client)
        {
            IBuffer res3 = BufferProvider.Provide();

            //sub_read_int32
            res3.WriteInt32(0);//character state? 2 = soul? 3 = alive but took over NPC stuff, if this is 0 then no movement takes place in game, 7 makes dupes not spawn when sending it with movement

            //sub_481AA0
            res3.WriteCString("soulname");

            //sub_481AA0
            res3.WriteCString("charaname");
            
            //sub_484420
            res3.WriteFloat(23162);//X Pos
            res3.WriteFloat(-219);//Y Pos
            res3.WriteFloat(3);//Z Pos
            res3.WriteByte(1);//view offset

            //sub_read_int32
            res3.WriteInt32(1);

            //sub_483420
            res3.WriteInt32(1);//Criminal Status

            //sub_483470
            res3.WriteInt16(2);

            //sub_483420
            int numEntries = 19;
            res3.WriteInt32(numEntries);//influences a loop that needs to be under 19

            //sub_483660
            for (int i = 0; i < numEntries; i++)
                res3.WriteInt32(2);

            //sub_483420
            numEntries = 19;
            res3.WriteInt32(numEntries);//influences a loop that needs to be under 19

            //sub_4948C0
            for (int i = 0; i < numEntries; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    res3.WriteInt32(10310503);
                    res3.WriteByte(0);
                    res3.WriteByte(0);
                    res3.WriteByte(0);
                }
                res3.WriteByte(0);
                res3.WriteByte(0);
                res3.WriteByte(0);//Bool
                res3.WriteByte(0);
                res3.WriteByte(0);
                res3.WriteByte(0);
                res3.WriteByte(0);
                res3.WriteByte(0);
            }

            //sub_483420
            numEntries = 19;
            res3.WriteInt32(numEntries);//influences a loop that needs to be under 19

            //sub_483420
            for (int i = 0; i < numEntries; i++)
                res3.WriteInt32(3);

            //sub_4835C0
            res3.WriteInt32(0);//1 here means crouching?

            //sub_484660
            res3.WriteInt32(0);//race
            res3.WriteInt32(1);//gender
            res3.WriteByte(2);//hair
            res3.WriteByte(3);//face
            res3.WriteByte(4);//hair color

            //sub_483420
            res3.WriteInt32(10);

            //sub_4837C0
            res3.WriteInt32(100);

            //sub_read_byte
            res3.WriteByte(0);//Criminal name icon

            //sub_494890
            res3.WriteByte(1);//Bool Beginner Protection

            //sub_4835E0
            res3.WriteInt32(0);//pose, 1 = sitting, 0 = standing

            //sub_483920
            res3.WriteInt32(0);

            //sub_483440
            res3.WriteInt16(0);

            //sub_read_byte
            res3.WriteByte(1);//no change?

            //sub_read_byte
            res3.WriteByte(1);//no change?

            //sub_read_int_32
            res3.WriteInt32(0);//title; 0 - display title, 1 - no title

            //sub_483580
            res3.WriteInt32(0);

            //sub_483420
            numEntries = 128;
            res3.WriteInt32(numEntries);//influences a loop that needs to be under 128

            //sub_485A70
            for (int i = 0; i < numEntries; i++)
            {
                res3.WriteInt32(0);
                res3.WriteInt32(0);
                res3.WriteInt32(0);
            }

            //sub_481AA0
            res3.WriteCString("");//Comment string

            Router.Send(client.Map, (ushort)AreaPacketId.recv_data_notify_chara_data, res3, client);
        }
    }
}