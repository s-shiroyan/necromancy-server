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
        }

        private void SendChatNotifyMessage(NecClient client, string Message, int ChatType)
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
                    //case "Monster:":
                    //  AdminConsoleMonster(client);
                    //break;
                    //case "Teleport:":
                    //  AdminConsoleCharacter(client);
                    //break;
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

        private void AdminConsoleNPC(NecClient client, int ModelID)
        {
                if (ModelID <= 1) { ModelID = 1911105; }

                IBuffer res3 = BufferProvider.Provide();
                res3.WriteInt32(Util.GetRandomNumber(2222222, 22222300));   // NPC ID (object id)

                res3.WriteInt32(10000101);      // NPC Serial ID from "npc.csv"

                res3.WriteByte(0);              // 0 - Clickable NPC (Active NPC, player can select and start dialog), 1 - Not active NPC (Player can't start dialog)

                res3.WriteCString($"Belong Here");//Name

                res3.WriteCString($"I Don't");//Title

                res3.WriteFloat(client.Character.X + 100);//X Pos
                res3.WriteFloat(client.Character.Y + 100);//Y Pos
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

                res3.WriteInt32(1); //npc Emoticon above head 1 for skull

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


    }
}