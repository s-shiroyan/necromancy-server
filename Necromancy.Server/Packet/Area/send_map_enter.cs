using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_map_enter : ClientHandler
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

            Router.Send(client, (ushort)AreaPacketId.recv_map_enter_r, res, ServerType.Area);
            Console.WriteLine($"{client.Character.Name} {client.Soul.Name} Entering Map: {client.Character.MapId}");
            Console.WriteLine($"At Entry Point XYZ  X: {client.Character.X}  Y: {client.Character.Y}  Z: {client.Character.Z}  ");

            //This makes each joined client re-send their notify when new clients connect.
            foreach (NecClient thisNecClient in client.Map.ClientLookup.GetAll())
            {
                SendDataNotifyCharaData(client, thisNecClient);
            }   
            //Commenting out code until a fix can be found.  This causes your character to be frozen upon map entry.   
           /* if (client.Character.NewCharaProtocol == true)
            {
                IBuffer res2 = BufferProvider.Provide();

                res2.WriteInt32(1); //1 = cinematic, 0 Just start the event without cinematic
                res2.WriteByte(0);
                client.Character.NewCharaProtocol = false;
                Router.Send(client, (ushort)AreaPacketId.recv_event_start, res2);
            }*/

        }
        private void SendDataNotifyCharaData(NecClient client, NecClient thisNecClient)
        {
            IBuffer res3 = BufferProvider.Provide();

            //sub_read_int32

            res3.WriteInt32(thisNecClient.Character.Id);//Character ID

            //sub_481AA0
            res3.WriteCString(thisNecClient.Soul.Name);

            //sub_481AA0
            res3.WriteCString(thisNecClient.Character.Name);

            //sub_484420
            res3.WriteFloat(thisNecClient.Character.X);//X Pos
            res3.WriteFloat(thisNecClient.Character.Y);//Y Pos
            res3.WriteFloat(thisNecClient.Character.Z);//Z Pos
            res3.WriteByte(thisNecClient.Character.viewOffset);//view offset

            //sub_read_int32
            res3.WriteInt32(6);

            //sub_483420

            res3.WriteInt32(0);//Character pose? 6 = guard, 8 = invisible,

            //sub_483470
            res3.WriteInt16(0);

            //sub_483420
            int numEntries = 19;
            res3.WriteInt32(numEntries);//has to be less than 19(defines how many int32s to read?)

            //Consolidated Frequently Used Code
            LoadEquip.SlotSetup(res3, thisNecClient.Character);


            //sub_483420
            numEntries = 19;
            res3.WriteInt32(numEntries);//has to be less than 19

            //Consolidated Frequently Used Code
            LoadEquip.EquipItems(res3, thisNecClient.Character);


            //sub_483420
            numEntries = 19;//influences a loop that needs to be under 19
            res3.WriteInt32(numEntries);

            //Consolidated Frequently Used Code
            LoadEquip.EquipSlotBitMask(res3, thisNecClient.Character);


            //sub_4835C0
            res3.WriteInt32(0);//1 here means crouching?

            //sub_484660
            LoadEquip.BasicTraits(res3, thisNecClient.Character);

            //sub_483420
            res3.WriteInt32(0); // party id?

            //sub_4837C0
            res3.WriteInt32(1); // party id? // i don't think sooo'

            //sub_read_byte
            res3.WriteByte(0);//Criminal name icon

            //sub_494890
            res3.WriteByte(0);//Bool Beginner Protection

            //sub_4835E0
            res3.WriteInt32(0);//pose, 1 = sitting, 0 = standing

            //sub_483920
            res3.WriteInt32(0);

            //sub_483440
            res3.WriteInt16(65);

            //sub_read_byte
            res3.WriteByte(255);//no change?

            //sub_read_byte
            res3.WriteByte(255);//no change?

            //sub_read_int_32
            res3.WriteInt32(1);//title; 0 - display title, 1 - no title

            //sub_483580
            res3.WriteInt32(244);

            //sub_483420
            numEntries = 1;
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


            Router.Send(thisNecClient.Map, (ushort)AreaPacketId.recv_data_notify_chara_data, res3, ServerType.Area, thisNecClient);

            SendMapBGM(client);
            client.Character.weaponEquipped = false;
        }

          private void SendMapBGM(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(100401);


            Router.Send(client.Map, (ushort)AreaPacketId.recv_map_update_bgm, res, ServerType.Area, client);


        } 
    }
}