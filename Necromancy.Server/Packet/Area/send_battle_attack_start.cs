using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_battle_attack_start : Handler
    {
        public send_battle_attack_start(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_battle_attack_start;

        int i = 0;
        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_battle_attack_start, res, client);

            SendNPCStateUpdateNotify(client);
            if(client.Character != null)
            {
                client.Character.X = packet.Data.ReadFloat();
                client.Character.Y = packet.Data.ReadFloat();
                client.Character.Z = packet.Data.ReadFloat();
                client.Character.charaPose = packet.Data.ReadByte();
            }

            //SendDataNotifyCharaData(client);
            //SendDataNotifyCharabodyData(client, charaX, charaY, charaZ);
            //SendCharaUpdateForm(client);
            SendObjectPointMoveNotify(client);
            //SendDataNotifyItemObjectData(client);
        }

        private void SendDataNotifyItemObjectData(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(251001);//Object ID
            res.WriteFloat(client.Character.X);//Initial X
            res.WriteFloat(client.Character.Y);//Initial Y
            res.WriteFloat(client.Character.Z);//Initial Z

            res.WriteFloat(client.Character.X);//Final X
            res.WriteFloat(client.Character.Y);//Final Y
            res.WriteFloat(client.Character.Z);//Final Z
            res.WriteByte(client.Character.viewOffset);//View offset

            res.WriteInt32(0);// 0 here gives an indication (blue pillar thing) and makes it pickup-able
            res.WriteInt32(0);
            res.WriteInt32(0);

            res.WriteInt32(255);//Anim: 1 = fly from the source. (I think it has to do with odd numbers, some cause it to not be pickup-able)

            res.WriteInt32(0);
            i++;
            Router.Send(client.Map, (ushort)AreaPacketId.recv_data_notify_itemobject_data, res);
        }
        private void SendCharaUpdateForm(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            Router.Send(client, (ushort) AreaPacketId.recv_chara_update_form, res);
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
            res3.WriteInt32(-1);// character id

            //sub_481AA0
            res3.WriteCString("soulname");

            //sub_481AA0
            res3.WriteCString("charaname");
            
            //sub_484420
            res3.WriteFloat(client.Character.X);//X Pos     23162
            res3.WriteFloat(client.Character.Y);//Y Pos     -219
            res3.WriteFloat(client.Character.Z);//Z Pos     3
            res3.WriteByte(client.Character.viewOffset);//view offset

            //sub_read_int32
            res3.WriteInt32(0);

            //sub_483420
            res3.WriteInt32(1);//Criminal Status

            //sub_483470
            res3.WriteInt16((short)0);

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
            res3.WriteInt32(0);

            //sub_484660
            res3.WriteInt32(0);//race
            res3.WriteInt32(1);//gender
            res3.WriteByte(2);//hair
            res3.WriteByte(3);//face
            res3.WriteByte(4);//hair color

            //sub_483420
            res3.WriteInt32(0);

            //sub_4837C0
            res3.WriteInt32(0);

            //sub_read_byte
            res3.WriteByte((byte)0);

            //sub_494890
            res3.WriteByte(1);//Bool Beginner Protection

            //sub_4835E0
            res3.WriteInt32(0);

            //sub_483920
            res3.WriteInt32(0);

            //sub_483440
            res3.WriteInt16((short)0);

            //sub_read_byte
            res3.WriteByte((byte)0);

            //sub_read_byte
            res3.WriteByte((byte)0);

            //sub_read_int_32
            res3.WriteInt32(0);//title; 0 - display title, 1 - no title

            //sub_483580
            res3.WriteInt32(0);

            //sub_483420
            numEntries = 1;
            res3.WriteInt32(numEntries);//influences a loop that needs to be under 128

            //sub_485A70
            for (int i = 0; i < numEntries; i++)
            {
                res3.WriteInt32(123);
                res3.WriteInt32(125);
                res3.WriteInt32(1);
            }

            //sub_481AA0
            res3.WriteCString("");//Comment string

            Router.Send(client.Map, (ushort)AreaPacketId.recv_data_notify_chara_data, res3, client);
        }


        private void SendDataNotifyCharabodyData(NecClient client, float charaX, float charaY, float charaZ)
        {
            IBuffer res3 = BufferProvider.Provide();

            res3.WriteInt32(0);//Has to be 0 for the character movement to take place, -1 spawns a new one on each movement,

            res3.WriteInt32(0);//Character state? 2 = dead?


            res3.WriteCString("soulMeme");

            res3.WriteCString("charaMeme");

            res3.WriteFloat(charaX);//X Pos     23162
            res3.WriteFloat(charaY);//Y Pos     -219
            res3.WriteFloat(charaZ);//Z Pos     3
            res3.WriteByte(1);//view offset

            res3.WriteInt32(100);

            int numEntries = 19;
            res3.WriteInt32(numEntries);//influences a loop that needs to be under 19

            for (int i = 0; i < numEntries; i++)
                res3.WriteInt32(2);

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
                res3.WriteInt32(3);

            res3.WriteInt32(0);//race
            res3.WriteInt32(1);//gender
            res3.WriteByte(2);//hair
            res3.WriteByte(3);//face
            res3.WriteByte(4);//hair color

            res3.WriteInt32(1);//  Pose? 0/8 = backpack, 1/9 = laying down also makes the model no longer a backpack, 2/3/4/5/6/7/10 = no updates

            res3.WriteInt32(1); //Character state? 4 = ash, otherwise is regular?

            res3.WriteInt32(0);

            res3.WriteInt32(7);// 7 and up makes the character stand up here

            res3.WriteByte(0);// Criminal level 0 = white, 1 = yellow, 2+ = red

            res3.WriteByte(1);//Bool

            res3.WriteInt32(0);

            Router.Send(client.Map, (ushort)AreaPacketId.recv_data_notify_charabody_data, res3, client);
        }

        private void SendObjectPointMoveNotify(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(client.Character.Id);//Character ID
            res.WriteFloat(client.Character.X);
            res.WriteFloat(client.Character.Y);
            res.WriteFloat(client.Character.Z);
            res.WriteByte(client.Character.viewOffset);//View offset
            res.WriteByte(0);//Character state?
            
            Router.Send(client.Map, (ushort)AreaPacketId.recv_object_point_move_notify, res, client);
        }
    }
}