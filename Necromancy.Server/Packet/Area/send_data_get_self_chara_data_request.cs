using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_data_get_self_chara_data_request : Handler
    {
        public send_data_get_self_chara_data_request(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_data_get_self_chara_data_request;

        public override void Handle(NecClient client, NecPacket packet)
        {
            SendDataGetSelfCharaData(client);
            SendDataNotifyCharaData(client);

            IBuffer res2 = BufferProvider.Provide();
            Router.Send(client, (ushort)AreaPacketId.recv_data_get_self_chara_data_request_r, res2);
        }

        private void SendDataGetSelfCharaData(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();

            //sub_4953B0
            res.WriteInt32(1);
            res.WriteInt32(2);
            res.WriteByte(1);
            res.WriteByte(2);
            res.WriteByte(3);

            //sub_484720
            res.WriteInt32(3);
            res.WriteInt32(4);
            res.WriteInt16(4);
            res.WriteInt64(1);
            res.WriteInt64(2);
            res.WriteInt64(3);
            res.WriteInt64(4);
            res.WriteInt32(5);
            res.WriteInt32(6);
            res.WriteInt32(7);
            res.WriteInt32(8);
            res.WriteInt32(9);
            res.WriteInt32(10);
            res.WriteInt32(11);
            res.WriteInt32(12);
            res.WriteInt32(13);
            res.WriteInt32(14);
            res.WriteByte(4);
            for (int i = 0; i < 7; i++)
                res.WriteInt16((byte)i); //loop 7x
            for (int i = 0; i < 9; i++)
                res.WriteInt16((byte)i); //loop 9x
            for (int i = 0; i < 9; i++)
                res.WriteInt16((byte)i); //loop 9x
            for (int i = 0; i < 11; i++)
                res.WriteInt16((byte)i); //loop 11x
            res.WriteInt64(5);
            res.WriteInt32(15);
            res.WriteInt32(16);
            res.WriteInt32(17);
            res.WriteInt32(18);
            res.WriteInt32(19);

            //sub_484980
            res.WriteInt32(20);
            res.WriteInt32(21);
            res.WriteInt32(22);
            for (int i = 0; i < 7; i++)
                res.WriteInt16((byte)i); //loop 7x
            for (int i = 0; i < 9; i++)
                res.WriteInt16((byte)i); //loop 9x
            for (int i = 0; i < 9; i++)
                res.WriteInt16((byte)i); //loop 9x
            for (int i = 0; i < 11; i++)
                res.WriteInt16((byte)i); //loop 11x

            //sub_484B00
            res.WriteInt32(23);
            res.WriteInt32(24);
            res.WriteFixedString("hello", 65);
            res.WriteInt16(5);

            //sub_484420
            res.WriteFloat(1);
            res.WriteFloat(2);
            res.WriteFloat(3);
            res.WriteByte(5);

            //sub_read_int32
            res.WriteInt32(25);

            //sub_483420
            res.WriteInt32(26);

            //sub_494AC0
            res.WriteByte(6);
            res.WriteInt32(27);
            res.WriteInt32(28);
            res.WriteInt32(29);
            res.WriteByte(7);
            res.WriteByte(1);//Bool
            res.WriteByte(7);
            res.WriteByte(7);
            res.WriteByte(7);
            res.WriteByte(7);

            //sub_read_3-int16
            res.WriteInt16(5);
            res.WriteInt16(6);
            res.WriteInt16(7);

            //sub_4833D0
            res.WriteInt64(6);

            //sub_4833D0
            res.WriteInt64(7);

            //sub_4834A0
            res.WriteFixedString("Channel", 97);

            //sub_4834A0
            res.WriteFixedString("Name", 385);

            //sub_494890
            res.WriteByte(1);//Bool

            //sub_4834A0
            res.WriteFixedString("Lastname", 385);

            //sub_494890
            res.WriteByte(1);//Bool

            //sub_483420
            int numEntries = 19;
            res.WriteInt32(numEntries);//has to be less than 19(defines how many int32s to read?)

            //sub_483660
            for (int i = 0; i < numEntries; i++)
                res.WriteInt32(i + 3);


            //sub_483420
            numEntries = 19;
            res.WriteInt32(numEntries);//has to be less than 19(i think this defines the next subs #of loops)

            //sub_4948C0
            for (int i = 0; i < numEntries; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    res.WriteInt32(j);
                    res.WriteByte((byte)j);
                    res.WriteByte((byte)(j + 1));
                    res.WriteByte((byte)(j + 2));
                }
                res.WriteByte(1);
                res.WriteByte(2);
                res.WriteByte(1);//Bool
                res.WriteByte(3);
                res.WriteByte(4);
                res.WriteByte(5);
                res.WriteByte(6);
                res.WriteByte(7);
            }

            //sub_483420
            numEntries = 19;
            res.WriteInt32(numEntries);

            //sub_483420
            for (int i = 0; i < numEntries; i++)
                res.WriteInt32(i);

            //sub_483420
            numEntries = 128;
            res.WriteInt32(numEntries);//has to be less than 128

            //sub_485A70
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(1);
                res.WriteInt32(2);
                res.WriteInt32(3);
            }

            Router.Send(client, (ushort)AreaPacketId.recv_data_get_self_chara_data_r, res);
        }

        private void SendDataNotifyCharaData(NecClient client)
        {
            IBuffer res3 = BufferProvider.Provide();

            //sub_read_int32
            res3.WriteInt32(1);

            //sub_481AA0
            res3.WriteCString("soulname");

            //sub_481AA0
            res3.WriteCString("charaname");
            
            //sub_484420
            res3.WriteFloat(1);
            res3.WriteFloat(2);
            res3.WriteFloat(3);
            res3.WriteByte(1);

            //sub_read_int32
            res3.WriteInt32(1);

            //sub_483420
            res3.WriteInt32(2);

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
            res3.WriteInt32(4);
            res3.WriteInt32(5);
            res3.WriteByte(2);
            res3.WriteByte(3);
            res3.WriteByte(4);

            //sub_483420
            res3.WriteInt32(6);

            //sub_4837C0
            res3.WriteInt32(7);

            //sub_read_byte
            res3.WriteByte(5);

            //sub_494890
            res3.WriteByte(1);//Bool

            //sub_4835E0
            res3.WriteInt32(8);

            //sub_483920
            res3.WriteInt32(9);

            //sub_483440
            res3.WriteInt16(2);

            //sub_read_byte
            res3.WriteByte(6);

            //sub_read_byte
            res3.WriteByte(7);

            //sub_read_int_32
            res3.WriteInt32(10);

            //sub_483580
            res3.WriteInt32(11);

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

            Router.Send(client, (ushort)AreaPacketId.revc_data_notify_chara_data, res3);
        }
    }
}