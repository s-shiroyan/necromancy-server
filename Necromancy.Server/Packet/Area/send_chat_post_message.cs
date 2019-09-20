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
            int x = 0;
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
            if(Message.Length >= 6)
            {
                switch (Message.Length)
                {
                    case 6:
                        x = Message[Message.Length - 1] - '0';
                        break;
                    case 7:
                        x += Message[Message.Length - 1] - '0';
                        x += (Message[Message.Length - 2] - '0') * 10;
                        break;
                    case 8:
                        x += Message[Message.Length - 1] - '0';
                        x += (Message[Message.Length - 2] - '0') * 10;
                        x += (Message[Message.Length - 3] - '0') * 100;
                        break;
                    case 9:
                        x += Message[Message.Length - 1] - '0';
                        x += (Message[Message.Length - 2] - '0') * 10;
                        x += (Message[Message.Length - 3] - '0') * 100;
                        x += (Message[Message.Length - 4] - '0') * 1000;
                        Console.WriteLine(x);
                        break;
                    case 10:
                        x += Message[Message.Length - 1] - '0';
                        x += (Message[Message.Length - 2] - '0') * 10;
                        x += (Message[Message.Length - 3] - '0') * 100;
                        x += (Message[Message.Length - 4] - '0') * 1000;
                        x += (Message[Message.Length - 5] - '0') * 10000;
                        break;
                    case 11:
                        x += Message[Message.Length - 1] - '0';
                        x += (Message[Message.Length - 2] - '0') * 10;
                        x += (Message[Message.Length - 3] - '0') * 100;
                        x += (Message[Message.Length - 4] - '0') * 1000;
                        x += (Message[Message.Length - 5] - '0') * 10000;
                        x += (Message[Message.Length - 6] - '0') * 100000;
                        break;
                    case 12:
                        x += Message[Message.Length - 1] - '0';
                        x += (Message[Message.Length - 2] - '0') * 10;
                        x += (Message[Message.Length - 3] - '0') * 100;
                        x += (Message[Message.Length - 4] - '0') * 1000;
                        x += (Message[Message.Length - 5] - '0') * 10000;
                        x += (Message[Message.Length - 6] - '0') * 100000;
                        x += (Message[Message.Length - 7] - '0') * 1000000;
                        break;
                    case 13:
                        x += Message[Message.Length - 1] - '0';
                        x += (Message[Message.Length - 2] - '0') * 10;
                        x += (Message[Message.Length - 3] - '0') * 100;
                        x += (Message[Message.Length - 4] - '0') * 1000;
                        x += (Message[Message.Length - 5] - '0') * 10000;
                        x += (Message[Message.Length - 6] - '0') * 100000;
                        x += (Message[Message.Length - 7] - '0') * 1000000;
                        x += (Message[Message.Length - 8] - '0') * 10000000;
                        break;
                    case 14:
                        x += Message[Message.Length - 1] - '0';
                        x += (Message[Message.Length - 2] - '0') * 10;
                        x += (Message[Message.Length - 3] - '0') * 100;
                        x += (Message[Message.Length - 4] - '0') * 1000;
                        x += (Message[Message.Length - 5] - '0') * 10000;
                        x += (Message[Message.Length - 6] - '0') * 100000;
                        x += (Message[Message.Length - 7] - '0') * 1000000;
                        x += (Message[Message.Length - 8] - '0') * 10000000;
                        x += (Message[Message.Length - 9] - '0') * 100000000;
                        break;
                    case 15:
                        x += Message[Message.Length - 1] - '0';
                        x += (Message[Message.Length - 2] - '0') * 10;
                        x += (Message[Message.Length - 3] - '0') * 100;
                        x += (Message[Message.Length - 4] - '0') * 1000;
                        x += (Message[Message.Length - 5] - '0') * 10000;
                        x += (Message[Message.Length - 6] - '0') * 100000;
                        x += (Message[Message.Length - 7] - '0') * 1000000;
                        x += (Message[Message.Length - 8] - '0') * 10000000;
                        x += (Message[Message.Length - 9] - '0') * 100000000;
                        x += (Message[Message.Length - 10] - '0') * 1000000000;
                        break;
                    default:
                        x = 0;
                        break;
                }
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
                case "itis":
                    SendItemInstance(client, x);
                    break;
                case "upit":
                    SendItemUpdateState(client);
                    break;
                case "stuf":
                    SendStallUpdateFeatureItem(client);
                    break;
                case "sssi":
                    SendStallSellItem(client);
                    break;
                default:
                    Message = "Unrecognized command";
                    break;
            }
        }

        /*private int intParse(int result, int place, string Message, int i)
        {
            if ( Message.Length == i)
                return 0;
            else
                return (result + intParse((Message[Message.Length - 1] - '0') * (10 * place), Message[1], Message, i++));
        }*///failed attempt at recursion

        private void SendStallSellItem(NecClient client)
        {
            //recv_stall_sell_item = 0x919C,
            IBuffer res = BufferProvider.Provide();

            res.WriteCString("C1Str"); // find max size Character name/Soul name
            res.WriteCString("CStr2"); // find max size Character name/Soul name
            res.WriteInt64(25);
            res.WriteByte(12);
            res.WriteByte(14);
            res.WriteInt16(16);
            res.WriteInt32(10200101);

            Router.Send(client, (ushort)AreaPacketId.recv_stall_sell_item, res);
        }

        private void SendStallUpdateFeatureItem(NecClient client)
        {
            //recv_stall_update_feature_item = 0xB195,
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(client.Character.Id);

            res.WriteInt32(10200101);
            res.WriteByte(2);
            res.WriteByte(2);
            res.WriteByte(2);

            res.WriteInt32(0);
            res.WriteInt16(0);
            res.WriteInt32(9);

            res.WriteByte(0); // bool

            res.WriteInt32(0);

            Router.Send(client.Map, (ushort)AreaPacketId.recv_stall_update_feature_item, res);
        }

        private void SendItemUpdateState(NecClient client)
        {
            //recv_item_update_state = 0x3247, 
            IBuffer res = BufferProvider.Provide();

        	res.WriteInt64(300000);//ItemID
            res.WriteInt32(200000);//Icon type, [x]00000 = certain armors, 1 = orb? 2 = helmet, up to 6

            Router.Send(client, (ushort)AreaPacketId.recv_item_update_state, res);
        }

        private void SendItemInstance(NecClient client, int x)
        {
            //recv_item_instance = 0x86EA,
            IBuffer res = BufferProvider.Provide();

	        res.WriteInt64(300000);//ItemID
            res.WriteInt32(300000);//Icon type, [x]00000 = certain armors, 1 = orb? 2 = helmet, up to 6
            res.WriteByte(0);//Number of "items"
            res.WriteInt32(0);//Changed icon to broken with 10200101, changed icon to have a 100% with 2 
            res.WriteFixedString("fixed", 0x10);
            res.WriteByte(1);//0/2 here causes crash, 255 causes item to go away
            res.WriteByte(1);//4 here causes crash
            res.WriteInt16(4);
            res.WriteInt32(-1);//Slot spots? 10200101 here caused certain spots to have an item, -1 for all slots(avatar included)
            res.WriteInt32(2);//Percentage stat, 9 max i think
            res.WriteByte(1);
            res.WriteByte(1);
            res.WriteCString("cstring"); // find max size 
            res.WriteInt16(1);
            res.WriteInt16(1);
            res.WriteInt32(1);//Divides max % by this number
            res.WriteByte(1);
            res.WriteInt32(0);
            int numEntries = 2;
            res.WriteInt32(numEntries); // less than or equal to 2

            //for (int i = 0; i < numEntries; i++)
                res.WriteInt32(0);
                res.WriteInt32(0);

                numEntries = 3;
            res.WriteInt32(numEntries); // less than or equal to 3
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteByte(0); //bool
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
            }
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt16(0);
            res.WriteInt32(0);//Guard protection toggle, 1 = on, everything else is off
            res.WriteInt16(0);

            Router.Send(client, (ushort)AreaPacketId.recv_item_instance, res);
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