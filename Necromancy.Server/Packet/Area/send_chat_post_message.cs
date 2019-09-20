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

        bool ConsoleActive = false;
        
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
            if (ConsoleActive == true)
            {
                string[] SplitMessage = Message.Split(':');

                switch (SplitMessage[0])
                {
                    case "NPC":
                        AdminConsoleNPC(client,Convert.ToInt32(SplitMessage[1]));
                        break;
                    case "Item":
                    AdminConsoleSoulMaterial(client);
                    break;
                    case "Died":
                        IBuffer res4 = BufferProvider.Provide();
                        Router.Send(client.Map, (ushort)AreaPacketId.recv_self_lost_notify, res4);
                         break;
                    case "GetLoot":
                        AdminConsoleLootAccessObject(client);
                        break;
                    case "GetMail":
                        AdminConsoleSelectPackageUpdate(client);
                        break;
                        

                    case "Jump":
                        AdminConsoleSuperJump(client, Convert.ToInt32(SplitMessage[1]));
                        break;
                    case "exit":
                        ConsoleActive = false;
                        break;
                    default:
                        Message = $"Unrecognized command '{SplitMessage[0]}' ";
                        break;
                }

                IBuffer res2 = BufferProvider.Provide();
                res2.WriteInt32(ChatType);
                res2.WriteInt32(1);      // todo, maybe, character id
                res2.WriteFixedString($"Console", 49);
                res2.WriteFixedString($"Admin", 37);
                res2.WriteFixedString($"Console Command - {Message} sent", 769);
                Router.Send(client.Map, (ushort)AreaPacketId.recv_chat_notify_message, res2);


            }

            if (ConsoleActive == false)
            {
                IBuffer res = BufferProvider.Provide();
                res.WriteInt32(ChatType);
                res.WriteInt32(1);      // todo, maybe, character id
                res.WriteFixedString($"{client.Soul.Name}", 49);
                res.WriteFixedString($"{client.Character.SoulId}", 37);
                res.WriteFixedString($"{Message}", 769);
                Router.Send(client.Map, (ushort)AreaPacketId.recv_chat_notify_message, res);

                if (Message == "GodModeConsole")
                {
                    ConsoleActive = true;
                    IBuffer res2 = BufferProvider.Provide();
                    res2.WriteInt32(ChatType);
                    res2.WriteInt32(1);      // todo, maybe, character id
                    res2.WriteFixedString($"Admin", 49);
                    res2.WriteFixedString($"Console", 37);
                    res2.WriteFixedString($"Admin Console Activated. Type 'exit' to escape", 769);
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_chat_notify_message, res2);
                }
            }
            


        }

        private void AdminConsoleSuperJump(NecClient client, int Height)
        {
            client.Character.Z += Height;  //It's not that easy.   Teleport to be done later.
        }
        private void AdminConsoleNPC(NecClient client, int ModelID)
        {
                if (ModelID <= 1) { ModelID = 1911105; }

                IBuffer res3 = BufferProvider.Provide();
                res3.WriteInt32(Util.GetRandomNumber(2222222, 22222300));   // NPC ID (object id)

                res3.WriteInt32(10000101);      // NPC Serial ID from "npc.csv"

                res3.WriteByte(0);              // 0 - Clickable NPC (Active NPC, player can select and start dialog), 1 - Not active NPC (Player can't start dialog)

                res3.WriteCString($"Belong Here");//Name

                res3.WriteCString($"I Don't");//Title

                res3.WriteFloat(client.Character.X + Util.GetRandomNumber(25, 150));//X Pos
                res3.WriteFloat(client.Character.Y + Util.GetRandomNumber(25, 150));//Y Pos
                res3.WriteFloat(client.Character.Z);//Z Pos
                res3.WriteByte(client.Character.viewOffset);//view offset

                res3.WriteInt32(19);

                //this is an x19 loop but i broke it up
                res3.WriteInt32(24);
                res3.WriteInt32(1);
                res3.WriteInt32(1);
                res3.WriteInt32(1);
                res3.WriteInt32(1);
                res3.WriteInt32(1);
                res3.WriteInt32(1);
                res3.WriteInt32(1);
                res3.WriteInt32(1);
                res3.WriteInt32(1);
                res3.WriteInt32(1);
                res3.WriteInt32(1);
                res3.WriteInt32(1);
                res3.WriteInt32(1);
                res3.WriteInt32(1);
                res3.WriteInt32(1);
                res3.WriteInt32(1);
                res3.WriteInt32(1);
                res3.WriteInt32(1);

                res3.WriteInt32(19);


                int numEntries = 19;


                for (int i = 0; i<numEntries; i++)

                {
                    // loop start
                    res3.WriteInt32(210901); // this is a loop within a loop i went ahead and broke it up
                    res3.WriteByte(0);
                    res3.WriteByte(0);
                    res3.WriteByte(0);

                    res3.WriteInt32(10310503);
                    res3.WriteByte(0);
                    res3.WriteByte(0);
                    res3.WriteByte(0);

                    res3.WriteByte(0);
                    res3.WriteByte(0);
                    res3.WriteByte(1); // bool
                    res3.WriteByte(0);
                    res3.WriteByte(0);
                    res3.WriteByte(0);
                    res3.WriteByte(0);
                    res3.WriteByte(0);

                }

                res3.WriteInt32(19);

                //this is an x19 loop but i broke it up
                res3.WriteInt32(3);
                res3.WriteInt32(3);
                res3.WriteInt32(3);
                res3.WriteInt32(3);
                res3.WriteInt32(3);
                res3.WriteInt32(3);
                res3.WriteInt32(3);
                res3.WriteInt32(3);
                res3.WriteInt32(3);
                res3.WriteInt32(3);
                res3.WriteInt32(3);
                res3.WriteInt32(3);
                res3.WriteInt32(3);
                res3.WriteInt32(3);
                res3.WriteInt32(3);
                res3.WriteInt32(3);
                res3.WriteInt32(3);
                res3.WriteInt32(3);
                res3.WriteInt32(3);

                res3.WriteInt32(ModelID);   //NPC Model from file "model_common.csv"
                


                res3.WriteInt16(100);       //NPC Model Size

                res3.WriteByte(237);

                res3.WriteByte(237);

                res3.WriteByte(237);

                res3.WriteInt32(237);

                res3.WriteInt32(Util.GetRandomNumber(1, 9)); //npc Emoticon above head 1 for skull

                res3.WriteInt32(237);
                res3.WriteFloat(1000);
                res3.WriteFloat(1000);
                res3.WriteFloat(1000);

                res3.WriteInt32(128);

                int numEntries2 = 128;


                for (int i = 0; i<numEntries2; i++)

                {
                    res3.WriteInt32(237);
                    res3.WriteInt32(237);
                    res3.WriteInt32(237);

                }

            Router.Send(client, (ushort) AreaPacketId.recv_data_notify_npc_data, res3);
            }
        private void AdminConsoleSoulMaterial(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(Util.GetRandomNumber(44,48));//Object ID
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

            res.WriteInt32(1);//Anim: 1 = fly from the source. (I think it has to do with odd numbers, some cause it to not be pickup-able)

            res.WriteInt32(0);
            
            Router.Send(client.Map, (ushort)AreaPacketId.recv_data_notify_itemobject_data, res);
        }

        private void AdminConsoleLootAccessObject(NecClient client)
        {
            for (int i = 0; i < 1; i++)
            {
                int objectID = itemIDs[i];

                IBuffer res = BufferProvider.Provide();
                res.WriteInt32(0);
                res.WriteInt32(18159);
                res.WriteFixedString("Bite", 0x31);
                res.WriteFixedString("My", 0x5B);
                res.WriteFixedString("Shiny", 0x5B);
                res.WriteFixedString("Metal", 0x259);
                res.WriteInt32(objectID);
                res.WriteInt16(1);
                res.WriteInt64(objectID);
                res.WriteInt32(objectID);
                res.WriteFixedString("Ass", 0x49);
                res.WriteFixedString("Human", 0x49);
                res.WriteInt32(objectID);
                res.WriteInt32(objectID);
                res.WriteInt32(objectID);
                res.WriteInt32(objectID);
                res.WriteFixedString("!", 0x10);
                res.WriteByte(1);
                res.WriteInt32(objectID);
                res.WriteInt32(objectID);
                for (int j = 0; j < 3; j++)
                {
                    res.WriteByte(1);//bool
                    res.WriteInt32(objectID);
                    res.WriteInt32(objectID);
                    res.WriteInt32(objectID);
                }
                res.WriteInt64(objectID);


                Router.Send(client, (ushort)AreaPacketId.recv_package_notify_add, res);
            }



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

        private void AdminConsoleSelectPackageUpdate(NecClient client)
        {
                int errcode = 0;
                IBuffer res = BufferProvider.Provide();

                res.WriteInt32(errcode);//Error message Call. 0 for success. see additional options in Sys_msg.csv
                /*
                1	You have unopened mails	SYSTEM_WARNING
                2	No mails to delete	SYSTEM_WARNING
                3	You have %d unreceived mails. Please check your inbox.	SYSTEM_WARNING
                -2414	Mail title includes banned words	SYSTEM_WARNING

                */

                Router.Send(client, (ushort)AreaPacketId.recv_select_package_update_r, res);
            
        }

        /////////Int array for testing Item ID's.  Poor formatting, for easy copying from excel
        int[] itemIDs = new int[] {    100101
                                    ,   100102
                                    ,   100103
                                    ,   100104
                                    ,   100105
                                    ,   100106
                                    ,   100107
                                    ,   100108
                                    ,   100109
                                    ,   100110
                                    ,   100191
                                    ,   100201
                                    ,   100202
                                    ,   100203
                                    ,   100204
                                    ,   100205
                                    ,   100206
                                    ,   100207
                                    ,   100208
                                    ,   100209
                                    ,   100210
                                    ,   100214
                                    ,   100301
                                    ,   100302
                                    ,   100303
                                    ,   100304
                                    ,   100305

                                    };

    }
}