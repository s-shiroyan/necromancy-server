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

        int ConsoleActive = 0;
        int x = 0;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int ChatType = packet.Data.ReadInt32();     // 0 - Area, 1 - Shout, other todo...
            string To = packet.Data.ReadCString();
            string Message = packet.Data.ReadCString();


            SendChatNotifyMessage(client, Message, ChatType);
            

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // errcode : 0 for success
            /*
            CHAT_MSG	GENERIC	Unknown statement error
            CHAT_MSG	2	You are unable to whisper to yourself
            CHAT_MSG	-802	You are not able to speak since you are not in a party
            CHAT_MSG	-1400	Unable to find Soul.  Reason: <errmsg>
            CHAT_MSG	-1401	You do not have an all-chat item
            CHAT_MSG	-1402	Action failed since it is on cool down.
            CHAT_MSG	-1403	You may not repeat phrases over and over in chat
            CHAT_MSG	-1404	You may not shout at Soul Rank 1
            CHAT_MSG	-1405	Action failed since it is on cool down.
            CHAT_MSG	-2201	Target refused to accept your whisper
            CHAT_MSG	-2202	Unable to whisper as you are on the recipient's Block List.
            CHAT_MSG	-3000	You may not chat in all-chat during trades.
            CHAT_MSG	-3001	You may not chat in all-chat while you have a shop open.
             */

            Router.Send(client, (ushort)AreaPacketId.recv_chat_post_message_r, res);
            Console.WriteLine("Character X: " + client.Character.X + "\nCharacter Y: " + client.Character.Y + "\nCharacter Z: " + client.Character.Z);
            if (Message[0] == '!')
                commandParse(client, Message);
        }

        private void commandParse(NecClient client, string Message)
        {
            string command = null;
            int i = 1;
            long x = 0;
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
                string newString = null;

                for(int k = 0; k < Message.Length-5; k++)
                {
                    newString += Message[k + 5];
                }

                Int64.TryParse(newString, out long newInt);

                x = newInt;
            }
            switch (command)
            {
                case "rbox":
                    SendRandomBoxNotifyOpen(client);
                    break;
                case "tbox":
                    SendEventTreasureboxBegin(client);
                    break;
                case "auct":
                    SendAuctionNotifyOpen(client);
                    break;
                case "shop":
                    SendShopNotifyOpen(client);
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
                /*case "move":
                    SendItemMove(client);
                    break;*/
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

        private void SendChatNotifyMessage(NecClient client, string Message, int ChatType)
        {
            if (ConsoleActive == client.Character.Id)
            {
                string[] SplitMessage = Message.Split(':');

                switch (SplitMessage[0])
                {
                    case "NPC":
                        if (SplitMessage[1] == "") { SplitMessage[1] = "0"; }
                            AdminConsoleNPC(client,Convert.ToInt32(SplitMessage[1]));
                        break;
                    case "Monster":
                            AdminConsoleRecvDataNotifyMonsterData(client);
                        break;
                    case "Item":
                            SendDataNotifyItemObjectData(client);
                        break;
                    case "Died":
                            IBuffer res4 = BufferProvider.Provide();
                            Router.Send(client.Map, (ushort)AreaPacketId.recv_self_lost_notify, res4);
                         break; 
                    case "GetUItem":
                            AdminConsoleRecvItemInstanceUnidentified(client);
                        break;
                    case "GetItem":
                            AdminConsoleRecvItemInstance(client);
                        break;
                    case "SendMail":
                            SendMailOpenR(client);
                        break;
                    case "GetMail":
                            AdminConsoleSelectPackageUpdate(client);
                        break;                 
                    case "Jump":
                            AdminConsoleSuperJump(client, Convert.ToInt32(SplitMessage[1]));
                        break;
                    case "exit":
                            ConsoleActive = 0;
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
                res2.WriteFixedString($"Console Command - {Message} sent{x}", 769);
                Router.Send(client.Map, (ushort)AreaPacketId.recv_chat_notify_message, res2);


            }

            if (ConsoleActive != client.Character.Id)
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
                    ConsoleActive = client.Character.Id;
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
                res3.WriteInt32(10000101);   // NPC ID (object id)

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
                    res3.WriteByte(3);

                    res3.WriteInt32(10310503);
                    res3.WriteByte(0);
                    res3.WriteByte(0);
                    res3.WriteByte(3);

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

                res3.WriteByte(1);

                res3.WriteByte(1);

                res3.WriteByte(1);

                res3.WriteInt32(11111000);

                res3.WriteInt32(Util.GetRandomNumber(1, 9)); //npc Emoticon above head 1 for skull

                res3.WriteInt32(11111110);
                res3.WriteFloat(1000);
                res3.WriteFloat(1000);
                res3.WriteFloat(1000);

                res3.WriteInt32(128);

                int numEntries2 = 128;


                for (int i = 0; i<numEntries2; i++)

                {
                    res3.WriteInt32(1);
                    res3.WriteInt32(1);
                    res3.WriteInt32(1);

                }

            Router.Send(client, (ushort) AreaPacketId.recv_data_notify_npc_data, res3);
            }

        private void SendStallSellItem(NecClient client)
        {
            //recv_stall_sell_item = 0x919C,
            IBuffer res = BufferProvider.Provide();

            res.WriteCString("C1Str"); // find max size Character name/Soul name
            res.WriteCString("CStr2"); // find max size Character name/Soul name
            res.WriteInt64(25);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteInt16(16);
            res.WriteInt32(client.Character.Id); //Item id

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

        private void SendItemInstance(NecClient client, long x)
        {
            //recv_item_instance = 0x86EA,
            IBuffer res = BufferProvider.Provide();

	        res.WriteInt64(69);//ItemID
            res.WriteInt32((int)x);//Icon type, [x]00000 = certain armors, 1 = orb? 2 = helmet, up to 6
            res.WriteByte(0);//Number of "items"
            res.WriteInt32(8);//Item status, in multiples of numbers, 8 = blessed/cursed/both 
            res.WriteFixedString("fixed", 0x10);
            res.WriteByte(0); // 0 = adventure bag. 1 = character equipment
            res.WriteByte(0); // 0~2 // maybe.. more bag index?
            res.WriteInt16(1);// bag index
            res.WriteInt32(0);//Slot spots? 10200101 here caused certain spots to have an item, -1 for all slots(avatar included)
            res.WriteInt32(1);//Percentage stat, 9 max i think
            res.WriteByte(1);
            res.WriteByte(3);
            res.WriteCString("cstring"); // find max size 
            res.WriteInt16(2);
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

        private void SendLootAccessObject(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(-10);
            /*
                LOOT	-1	It is carrying nothing
                LOOT	-10	No one can be looted nearby
                LOOT	-207	No space available in inventory
                LOOT	-1500	No permission to loot
            */

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
        
        private void SendShopNotifyOpen(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt16(17); 
            /* Shop ID, 0 it's forge, 1 it's cursed, 2 Purchase shop, 3 purchase and curse, 4 it's sell, 
                        5 sell and curse. 6 purchase and sell. 7 Purchase, Sell, Curse.
                        
                        8 Identify. 9 identify & curse. 10 Purchase & Identify.
                        
                        11 Purchase, Identify & Curse. 12 Sell And Identify
                        
                        13 Sell, Identify & Curse, 14 Purchase, Sell & Identify
                        
                        15 All of what i say before except the forge.
                        
                        16 Repair !
           */
            res.WriteInt32(0); // don't know what this shit do for the moment
            res.WriteInt32(0); // don't know too
            res.WriteByte(0); // Don't know too
            Router.Send(client, (ushort)AreaPacketId.recv_shop_notify_open, res);
        }

        private void SendAuctionNotifyOpen(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(5); // must be <= 5

            int numEntries = 0x5;
            for (int i = 0; i < numEntries; i++)
            {

                res.WriteByte(1); // Slots 0 to 2
                // res.WriteInt64(0xAAAAAAAA); // this is shown in the structure but for some reason isnt read

                res.WriteInt32(0);
                res.WriteInt64(0);
                res.WriteInt32(0); // Lowest
                res.WriteInt32(0); // Buy Now
                res.WriteFixedString($"{client.Soul.Name}", 49);
                res.WriteByte(1);
                res.WriteFixedString("Comment", 385); // Comment in the item information
                res.WriteInt16(0); // Bid
                res.WriteInt32(0);

                res.WriteInt32(0); 
                res.WriteInt32(0);

            }

            res.WriteInt32(8); // must be< = 8

            int numEntries2 = 0x8;
            for (int i = 0; i < numEntries2; i++)
            {
                res.WriteByte(0); // Slots 0 to 3
                // res.WriteInt64(0xAAAAAAAA); // this is shown in the structure but for some reason isnt read

                res.WriteInt32(0);
                res.WriteInt64(0);
                res.WriteInt32(0); // Lowest bid info
                res.WriteInt32(0); // Buy now bid info
                res.WriteFixedString($"{client.Soul.Name}", 49);
                res.WriteByte(1);
                res.WriteFixedString("Zgeg", 385); // Comment in the bid info
                res.WriteInt16(0);
                res.WriteInt32(0);

                res.WriteInt32(0); // Bid Amount (bid info seciton)
                res.WriteInt32(0);

            }

            res.WriteByte(0); // bool
            Router.Send(client, (ushort)AreaPacketId.recv_auction_notify_open, res);
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

        private void AdminConsoleRecvDataNotifyMonsterData(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(Util.GetRandomNumber(55566, 55888));

            res.WriteCString($"Belong Here");//Name while spawning

            res.WriteCString($"I Don't");//Title

            res.WriteFloat(client.Character.X + Util.GetRandomNumber(25, 150));//X Pos
            res.WriteFloat(client.Character.Y + Util.GetRandomNumber(25, 150));//Y Pos
            res.WriteFloat(client.Character.Z);//Z Pos
            res.WriteByte(client.Character.viewOffset);//view offset

            res.WriteInt32(70101); // Monster serial ID.  70101 for Lesser Demon.  If this is invalid, you can't "loot" the monster or see it's first CString

            res.WriteInt32(2070001); // Model from model_common.csv  2070001 for Lesser Demon

            res.WriteInt16(100); //model size

            res.WriteInt32(0x10); // cmp to 0x10 = 16

            int numEntries = 0x10;
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(client.Character.Id);
            }

            res.WriteInt32(0x10); // cmp to 0x10 = 16

            int numEntries2 = 0x10;
            for (int i = 0; i < numEntries2; i++)
            {
                res.WriteInt32(70101); // this was an x2 loop (i broke it down)
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteInt32(70101);
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);

                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1); // bool
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);

                res.WriteByte(1);
            }

            res.WriteInt32(0x10); // cmp to 0x10 = 16

            int numEntries3 = 0x10; //equipment slots to display?
            for (int i = 0; i < numEntries3; i++)

            {
                res.WriteInt64(1111111111111111);
            }

            res.WriteInt32(11111110); //1000 0000 here makes it stand up and not be dead.

            res.WriteInt64(9999999999999999);

            res.WriteInt64(9999999999999999);

            res.WriteInt64(9999999999999999);

            res.WriteByte(1);

            res.WriteByte(1);

            res.WriteInt32(client.Character.Id);

            res.WriteInt32(client.Character.Id);

            res.WriteInt32(0x80); // cmp to 0x80 = 128

            int numEntries4 = 0x80;  //Statuses?
            for (int i = 0; i < numEntries4; i++)

            {
                res.WriteInt32(10000000);
                res.WriteInt32(10000000);
                res.WriteInt32(10000000);
            }

            Router.Send(client, (ushort)AreaPacketId.recv_data_notify_monster_data, res);
        }

        private void AdminConsoleRecvItemInstanceUnidentified(NecClient client)
        {
            //recv_item_instance_unidentified = 0xD57A,
            IBuffer res = BufferProvider.Provide();

            res.WriteInt64(10001000100010002);

            res.WriteCString("Have Fun in Texas Hiraeth!"); // Item Name

            res.WriteInt32(7); // Item Type.   10001005 Mask Item type and stats

            res.WriteInt32(10001004); 

            res.WriteByte(0); // Numbers of items

            res.WriteInt32(0); // 10001003 Put The Item Unidentified. 0 put the item Identified

            res.WriteInt32(10800405);  //Item ID for Item?
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteInt32(10800405);  //Item ID for Icon?
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(1); // bool
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteByte(0);                       // 0 = adventure bag. 1 = character equipment
            res.WriteByte(0);                       // 0~2
            res.WriteInt16(3);                      // bag index

            res.WriteInt32(0);              //bit mask. This indicates where to put items.   e.g. 01 head 010 arm 0100 feet etc (0 for not equipped)

            res.WriteInt64(1000100010001000);

            res.WriteInt32(10001002);

            Router.Send(client, (ushort)AreaPacketId.recv_item_instance_unidentified, res);
        }
        
        private void AdminConsoleRecvItemInstance(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            //recv_item_instance = 0x86EA,
            x++;
	        res.WriteInt64(1000200030004001);  //  Assume Unique ID instance identifier. 1 here makes item green icon
            res.WriteInt32(itemIDs[x]);
            res.WriteByte(1);               //number of items in stack
            res.WriteInt32(itemIDs[x]);       //0b1 normal 0b10 disconnect 
            res.WriteFixedString("WhatIsThis", 0x10);
            res.WriteByte(0);                       // 0 = adventure bag. 1 = character equipment
            res.WriteByte(0);                       // 0~2
            res.WriteInt16(3);                      // bag index
            res.WriteInt32(0b1000000000000000000000000000001);              //bit mask. This indicates where to put items.   e.g. 01 head 010 arm 0100 feet etc
            res.WriteInt32(1);
            res.WriteByte(3);
            res.WriteByte(3);
            res.WriteCString("ThisIsThis"); // find max size 
            res.WriteInt16(4);
            res.WriteInt16(5);
            res.WriteInt32(1);
            res.WriteByte(3);
            res.WriteInt32(1);
            int numEntries = 2;
            res.WriteInt32(numEntries); // less than or equal to 2
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(11400403);
            }
            numEntries = 3;
            res.WriteInt32(numEntries); // less than or equal to 3
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteByte(16); //bool
                res.WriteInt32(11400403);
                res.WriteInt32(11400403);
                res.WriteInt32(11400403);
            }
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt16(-1);
            res.WriteInt32(1);  //1 here lables the item "Gaurd".   no effect from higher numbers
            res.WriteInt16(-1);

            Router.Send(client, (ushort)AreaPacketId.recv_item_instance, res);

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