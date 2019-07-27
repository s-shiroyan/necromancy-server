using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_Player_To_Client : Handler
    {
        public send_Player_To_Client(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_chat_post_message;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res3 = BufferProvider.Provide();

            //sub_read_int32
            res3.WriteInt32(1);

            //sub_481AA0
            res3.WriteCString("soulname");

            //sub_481AA0
            res3.WriteCString("charaname");
            
            //sub_484420
            res3.WriteFloat(1);//X Pos
            res3.WriteFloat(2);//Y Pox
            res3.WriteFloat(3);//Z Pos
            res3.WriteByte(1);//View Offset

            //sub_read_int32
            res3.WriteInt32(1);

            //sub_483420
            res3.WriteInt32(1);

            //sub_483470
            res3.WriteInt16(1);

            //sub_483420
            int numEntries = 19;
            res3.WriteInt32(numEntries);//influences a loop that needs to be under 19

            //sub_483660
            for (int i = 0; i < numEntries; i++)
                res3.WriteInt32(i);

            //sub_483420
            numEntries = 19;
            res3.WriteInt32(numEntries);//influences a loop that needs to be under 19

            //sub_4948C0
            for (int i = 0; i < numEntries; i++)
            {
                for (int j = 0; j < numEntries; j++)
                {
                    res3.WriteInt32(j);
                    res3.WriteByte((byte)j);
                    res3.WriteByte((byte)(j + 1));
                    res3.WriteByte((byte)(j + 2));
                }
                res3.WriteByte(1);
                res3.WriteByte(2);
                res3.WriteByte(1);//Bool
                res3.WriteByte(3);
                res3.WriteByte(4);
                res3.WriteByte(5);
                res3.WriteByte(6);
                res3.WriteByte(7);
            }

            //sub_483420
            numEntries = 19;
            res3.WriteInt32(numEntries);//influences a loop that needs to be under 19

            //sub_483420
            for (int i = 0; i < numEntries; i++)
                res3.WriteInt32(i);

            //sub_4835C0
            res3.WriteInt32(3);

            //sub_484660
            res3.WriteInt32(1);
            res3.WriteInt32(1);
            res3.WriteByte(2);
            res3.WriteByte(3);
            res3.WriteByte(4);

            //sub_483420
            res3.WriteInt32(1);

            //sub_4837C0
            res3.WriteInt32(1);

            //sub_read_byte
            res3.WriteByte(5);

            //sub_494890
            res3.WriteByte(1);//Bool

            //sub_4835E0
            res3.WriteInt32(1);

            //sub_483920
            res3.WriteInt32(1);

            //sub_483440
            res3.WriteInt16(2);

            //sub_read_byte
            res3.WriteByte(6);

            //sub_read_byte
            res3.WriteByte(7);

            //sub_read_int_32
            res3.WriteInt32(1);

            //sub_483580
            res3.WriteInt32(1);

            //sub_483420
            numEntries = 128;
            res3.WriteInt32(numEntries);//influences a loop that needs to be under 128

            //sub_485A70
            for (int i = 0; i < numEntries; i++)
            {
                res3.WriteInt32(i);
                res3.WriteInt32(i+1);
                res3.WriteInt32(i+2);
            }

            //sub_481AA0
            res3.WriteCString("nofuckingideawhatthisis");

            Router.Send(client.Map, (ushort)AreaPacketId.recv_data_notify_chara_data, res3);
        }
    }
}