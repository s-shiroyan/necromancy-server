using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_chat_post_message : Handler
    {
        public send_chat_post_message(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_chat_post_message;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int ChatType = packet.Data.ReadInt32();     // 0 - Area, 1 - Shout, other todo...
            string To = packet.Data.ReadCString();
            string Message = packet.Data.ReadCString();

            SendChatNotifyMessage(client, Message, ChatType);

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort)AreaPacketId.recv_chat_post_message_r, res);
            Console.WriteLine("Chat Type: " + ChatType + "\nTo: " + To + "\nMessage: " + Message);

            if (Message[0] == '!')
                commandParse(client, Message);
        }

        private void commandParse(NecClient client, string Message)
        {
            string command = null;
            int i = 1;
            bool cont = true;
            while (cont)
            {
                if (i == 4)
                {
                    cont = false;
                }
                command += Message[i];
                i++;
            }
            switch (command)
            {
                case "rbox":
                    SendRandomBoxNotifyOpen(client);
                    break;
                case "tbox":
                    SendEventTreasureboxBegin(client);
                    break;
                case "item":
                    SendDataNotifyItemObjectData(client);
                    break;
                case "mail":
                    SendMailOpenR(client);
                    break;
                case "chid":
                    SendCharacterId(client);
                    break;
                case "accs":
                    SendLootAccessObject(client);
                    break;
                case "move":
                    SendItemMove(client);
                    break;
                default:
                    Message = "Unrecognized command";
                    break;
            }
        }

        private void SendItemMove(NecClient client)
        {
            //recv_item_move_r = 0x708B,
            IBuffer res = BufferProvider.Provide();

	        res.WriteInt32(69);//Error check?

            Router.Send(client, (ushort)AreaPacketId.recv_item_move_r, res);
        }

        private void SendLootAccessObject(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(69);

            Router.Send(client, (ushort)AreaPacketId.recv_loot_access_object_r, res);
        }


        private void SendCharacterId(NecClient client)
        {
            string chid = client.Character.Id.ToString();
            SendChatNotifyMessage(client, chid, 0);
        }

        private void SendMailOpenR(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            //recv_mail_open_r = 0xCE7,

            res.WriteInt32(client.Character.Id);

            Router.Send(client, (ushort) AreaPacketId.recv_mail_open_r, res);
        }

        private void SendRandomBoxNotifyOpen(NecClient client)
        {
            //recv_random_box_notify_open = 0xC374,
            IBuffer res = BufferProvider.Provide();

            int numEntries = 10;
            res.WriteInt32(numEntries);//less than or equal to 10

            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt64(160801);
            }

            res.WriteInt32(client.Character.Id);

            Router.Send(client, (ushort)AreaPacketId.recv_random_box_notify_open, res);
        }

        private void SendEventTreasureboxBegin(NecClient client)
        {
            //recv_event_tresurebox_begin = 0xBD7E,
            IBuffer res = BufferProvider.Provide();

            int numEntries = 0x10;
            res.WriteInt32(numEntries);

            //for loop of 0x10
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            res.WriteInt32(100101);

            Router.Send(client, (ushort)AreaPacketId.recv_event_tresurebox_begin, res);
        }

        private void SendDataNotifyItemObjectData(NecClient client)
        {
            //SendDataNotifyItemObjectData
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

            Router.Send(client.Map, (ushort)AreaPacketId.recv_data_notify_itemobject_data, res);
        }

        private void SendChatNotifyMessage(NecClient client, string Message, int ChatType)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(ChatType);//Char type
            res.WriteInt32(client.Character.Id);//Character ID
            res.WriteFixedString("Soulname", 49);//Soul name
            res.WriteFixedString("Charaname", 37);//Character name
            res.WriteFixedString($"{Message}", 769);//Message contents
            Router.Send(client.Map, (ushort)AreaPacketId.recv_chat_notify_message, res);
        }
    }
}