using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_battle_attack_start : Handler
    {
        public send_battle_attack_start(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_battle_attack_start;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_battle_attack_start, res, client);

            SendNPCStateUpdateNotify(client);
            //SendDataNotifyCharaData(client);
            //SendDataNotifyCharabodyData(client); //needs to be fixed, causes disconnect on opposite client when moving
        }

        private void SendNPCStateUpdateNotify(NecClient client)
        {
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(1);
            res2.WriteInt32(1);
            Router.Send(client, (ushort)AreaPacketId.recv_npc_state_update_notify, res2);
        }

        private void SendDataNotifyCharaData(NecClient client)
        {
            IBuffer res3 = BufferProvider.Provide();

            //sub_read_int32
            res3.WriteInt32(-1);

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
            res3.WriteInt32(-1);

            //sub_483420
            res3.WriteInt32(1);//Criminal Status

            //sub_483470
            res3.WriteInt16(0xFFFF);

            //sub_483420
            int numEntries = 19;
            res3.WriteInt32(numEntries);//influences a loop that needs to be under 19

            //sub_483660
            for (int i = 0; i < numEntries; i++)
                res3.WriteInt32(-1);

            //sub_483420
            numEntries = 19;
            res3.WriteInt32(numEntries);//influences a loop that needs to be under 19

            //sub_4948C0
            for (int i = 0; i < numEntries; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    res3.WriteInt32(-1);
                    res3.WriteByte(0);
                    res3.WriteByte(0);
                    res3.WriteByte(0);
                }
                res3.WriteByte(0);
                res3.WriteByte(0);
                res3.WriteByte(1);//Bool
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
                res3.WriteInt32(-1);

            //sub_4835C0
            res3.WriteInt32(-1);

            //sub_484660
            res3.WriteInt32(0);//race
            res3.WriteInt32(1);//gender
            res3.WriteByte(2);//hair
            res3.WriteByte(3);//face
            res3.WriteByte(4);//hair color

            //sub_483420
            res3.WriteInt32(-1);

            //sub_4837C0
            res3.WriteInt32(-1);

            //sub_read_byte
            res3.WriteByte(0xFF);

            //sub_494890
            res3.WriteByte(1);//Bool Beginner Protection

            //sub_4835E0
            res3.WriteInt32(-1);

            //sub_483920
            res3.WriteInt32(-1);

            //sub_483440
            res3.WriteInt16(0xFF);

            //sub_read_byte
            res3.WriteByte(0xFF);//no change?

            //sub_read_byte
            res3.WriteByte(0xFF);//no change?

            //sub_read_int_32
            res3.WriteInt32(0);//title; 0 - display title, 1 - no title

            //sub_483580
            res3.WriteInt32(-1);

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
            res3.WriteCString("");//Comment string

            Router.Send(client.Map, (ushort)AreaPacketId.revc_data_notify_chara_data, res3, client);
        }

        private void SendDataNotifyCharabodyData(NecClient client)
        {
            IBuffer res3 = BufferProvider.Provide();

            res3.WriteInt32(1);

            res3.WriteCString("soulname");

            res3.WriteCString("charaname");

            res3.WriteFloat(23162);//X Pos
            res3.WriteFloat(-219);//Y Pos
            res3.WriteFloat(10);//Z Pos
            res3.WriteByte(1);//view offset

            res3.WriteInt32(1);

            int numEntries = 19;
            res3.WriteInt32(numEntries);//influences a loop that needs to be under 19

            for (int i = 0; i < numEntries; i++)
                res3.WriteInt32(-1);

            numEntries = 19;
            res3.WriteInt32(numEntries);//influences a loop that needs to be under 19

            //sub_4948C0
            for (int i = 0; i < numEntries; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    res3.WriteInt32(-1);
                    res3.WriteByte(0);
                    res3.WriteByte(0);
                    res3.WriteByte(0);
                }
                res3.WriteByte(0);
                res3.WriteByte(0);
                res3.WriteByte(1);//Bool
                res3.WriteByte(0);
                res3.WriteByte(0);
                res3.WriteByte(0);
                res3.WriteByte(0);
                res3.WriteByte(0);
            }

            numEntries = 19;
            res3.WriteInt32(numEntries);//influences a loop that needs to be under 19

            for (int i = 0; i < numEntries; i++)
                res3.WriteInt32(-1);

            res3.WriteInt32(1);//race
            res3.WriteInt32(1);//gender
            res3.WriteByte(2);//hair
            res3.WriteByte(3);//face
            res3.WriteByte(4);//hair color

            res3.WriteInt32(1);

            res3.WriteInt32(1);

            res3.WriteInt32(1);

            res3.WriteInt32(1);

            res3.WriteByte(1);

            res3.WriteByte(1);//Bool

            res3.WriteInt32(1);

            Router.Send(client.Map, (ushort)AreaPacketId.recv_data_notify_charabody_data, res3, client);
        }
    }
}