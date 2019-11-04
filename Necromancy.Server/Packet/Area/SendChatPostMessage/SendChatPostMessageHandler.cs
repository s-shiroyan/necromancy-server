using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;
using Necromancy.Server.Chat;

namespace Necromancy.Server.Packet.Area.SendChatPostMessage
{
    public class SendChatPostMessageHandler : ClientHandlerDeserializer<ChatMessage>
    {
        public SendChatPostMessageHandler(NecServer server) : base(server, new SendChatPostMessageDeserializer())
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_chat_post_message;

        int x = 0;

        public override void HandleRequest(NecClient client, ChatMessage request)
        {
            Server.Chat.Handle(client, request);
        }
        
        /// <summary>
        /// Begin Console commands and Test funtions below.   To be or restricted to GM use at a later time.
        /// </summary>
        private void SendChatNotifyMessage(NecClient client, string Message, int ChatType)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(ChatType);
            res.WriteInt32(client.Character.Id);
            res.WriteFixedString($"{client.Soul.Name}", 49);
            res.WriteFixedString($"{client.Character.Name}", 37);
            res.WriteFixedString($"{Message}", 769);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_chat_notify_message, res, ServerType.Area);
        }

        private string commandParse(NecClient client, string Message)
        {
            string command = null;
            string[] SplitMessage = Message.Split('!', ':');
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

            if (Message.Length >= 6)
            {
                string newString = null;

                for (int k = 0; k < Message.Length - 5; k++)
                {
                    newString += Message[k + 5];
                }

                Int64.TryParse(newString, out long newInt);

                x = newInt;
            }

            switch (SplitMessage[1])
            {
                case "NPC": //spawns an NPC by your location.  Add an ID to spawn a specific model
                    if (SplitMessage[2] == "")
                    {
                        SplitMessage[2] = "0";
                    }

                    AdminConsoleNPC(client, Convert.ToInt32(SplitMessage[2]));
                    break;
                case "Monster": //Spawns a monster near you
                    AdminConsoleRecvDataNotifyMonsterData(client);
                    break;
                case "Died": //displays message that you died
                    IBuffer res4 = BufferProvider.Provide();
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_self_lost_notify, res4, ServerType.Area);
                    break;
                case "GetUItem": //Equips your whole character with gear based on settings in LoadEquipment.cs
                    AdminConsoleRecvItemInstanceUnidentified(client);
                    break;
                case "GetItem": //puts items in your inventory
                    AdminConsoleRecvItemInstance(client);
                    break;
                case "GetMail": //cant remember
                    AdminConsoleSelectPackageUpdate(client);
                    break;
                case "logout": //logs you out
                    LogOut(client);
                    break;
                case "ReadFile": //runs a command from FileReader.CS for testing output
                    FileReader.GameFileReader(client);
                    break;
                case "MapChange": //Changes your Map.  Union room by default.  add a mapId to a specific map.
                    if (SplitMessage[2] == "")
                    {
                        SplitMessage[2] = "1001010";
                    }

                    SendMapChangeForce(client, Convert.ToInt32(SplitMessage[2]));
                    break;
                case "MapEntry": //adds your client to the list of clients in a map
                    SendMapEntry(client, Convert.ToInt32(SplitMessage[2]));
                    break;
                case "OnHit": //  battle report attack on hit. 
                    IBuffer res = BufferProvider.Provide();
                    Router.Send(client, (ushort) AreaPacketId.recv_battle_report_action_attack_onhit, res,
                        ServerType.Area);
                    break;
                case "EndEvent": //failsafe to end events when frozen
                    SendEventEnd(client);
                    break;
                case "GGate": //Makes a random statue or "Gaurdian Gate"
                    if (SplitMessage[2] == "")
                    {
                        SplitMessage[2] = "100";
                    }

                    SendDataNotifiyGGateStoneData(client, Convert.ToInt32(SplitMessage[2]));
                    break;
                case "MapLink":
                    if (SplitMessage[2] == "")
                    {
                        SplitMessage[2] = "10";
                    }

                    SendMapLink(client, Convert.ToInt32(SplitMessage[2]));
                    break;
                default:
                    SplitMessage[1] = "unrecognized";
                    //Message = $"Unrecognized command '{SplitMessage[1]}' ";
                    break;
            }

            if (command == "unrecognized" && SplitMessage[1] == "unrecognized")
            {
                Message = $"Unrecognized command - {Message}";
            }
            else
            {
                Message = $"Sent command - {Message}";
            }

            return Message;
        }

        private void SendDataNotifiyGGateStoneData(NecClient client, int GGateChoice)
        {
            if (GGateChoice == 100)
            {
                GGateChoice = Util.GetRandomNumber(0, GGateModelIds.Length);
            } // pick random model if you don't specify an ID.

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(
                GGateModelIds[GGateChoice]); // Unique Object ID.  Crash if already in use (dont use your character ID)
            res.WriteInt32(GGateChoice); // Serial ID for Interaction? from npc.csv????
            res.WriteByte((byte) Util.GetRandomNumber(0, 2)); // 0 = Text, 1 = F to examine  , 2 or above nothing
            res.WriteCString($"The number of GGate you picked from array is : {GGateChoice}"); //"0x5B" //Name
            res.WriteCString($"The Model ID of your GGate is: {GGateModelIds[GGateChoice]}"); //"0x5B" //Title
            res.WriteFloat(client.Character.X + Util.GetRandomNumber(25, 150)); //X Pos
            res.WriteFloat(client.Character.Y + Util.GetRandomNumber(25, 150)); //Y Pos
            res.WriteFloat(client.Character.Z); //Z Pos
            res.WriteByte(client.Character.viewOffset); //view offset
            res.WriteInt32(
                GGateModelIds[
                    GGateChoice]); // Optional Model ID. Warp Statues. Gaurds, Pedistals, Etc., to see models refer to the model_common.csv

            res.WriteInt16(100); //  size of the object

            res.WriteInt32(0); // 0 = collision, 1 = no collision  (active/Inactive?)

            res.WriteInt32(
                EquipBitMask[
                    Util.GetRandomNumber(0,
                        4)]); //0= no effect color appear, //Red = 0bxx1x   | Gold = obxxx1   |blue = 0bx1xx


            Router.Send(client, (ushort) AreaPacketId.recv_data_notify_ggate_stone_data, res, ServerType.Area);
        }

        private void AdminConsoleNPC(NecClient client, int ModelID)
        {
            if (ModelID <= 1)
            {
                ModelID = NPCModelID[Util.GetRandomNumber(1, 10)];
            } // pick random model if you don't specify an ID.

            int numEntries = 0; // 1 to 19 equipment.  Setting to 0 because NPCS don't wear gear.
            IBuffer res3 = BufferProvider.Provide();
            res3.WriteInt32(NPCModelID[Util.GetRandomNumber(1, 10)]); // NPC ID (object id)

            res3.WriteInt32((NPCSerialID[Util.GetRandomNumber(1, 10)])); // NPC Serial ID from "npc.csv"

            res3.WriteByte(0); // 0 - Clickable NPC (Active NPC, player can select and start dialog), 1 - Not active NPC (Player can't start dialog)

            res3.WriteCString($"Name"); //Name

            res3.WriteCString($"Title"); //Title

            res3.WriteFloat(client.Character.X + Util.GetRandomNumber(25, 150)); //X Pos
            res3.WriteFloat(client.Character.Y + Util.GetRandomNumber(25, 150)); //Y Pos
            res3.WriteFloat(client.Character.Z); //Z Pos
            res3.WriteByte(client.Character.viewOffset); //view offset

            res3.WriteInt32(numEntries); // # Items to Equip

            for (int i = 0; i < numEntries; i++)

            {
                res3.WriteInt32(24);
            }

            res3.WriteInt32(numEntries); // # Items to Equip

            for (int i = 0; i < numEntries; i++)

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

            res3.WriteInt32(numEntries); // # Items to Equip

            for (int i = 0; i < numEntries; i++) // Item type bitmask per slot

            {
                res3.WriteInt32(1);
            }

            res3.WriteInt32(ModelID); //NPC Model from file "model_common.csv"

            res3.WriteInt16(100); //NPC Model Size

            res3.WriteByte(2);

            res3.WriteByte(5);

            res3.WriteByte(6);

            res3.WriteInt32(
                0); //Hp Related Bitmask?  This setting makes the NPC "alive"    11111110 = npc flickering, 0 = npc alive

            res3.WriteInt32(Util.GetRandomNumber(1, 9)); //npc Emoticon above head 1 for skull

            res3.WriteInt32(8); // add strange light on certain npc
            res3.WriteFloat(0); //x for icons
            res3.WriteFloat(0); //y for icons
            res3.WriteFloat(50); //z for icons

            res3.WriteInt32(128);

            int numEntries2 = 128;


            for (int i = 0; i < numEntries2; i++)

            {
                res3.WriteInt32(0);
                res3.WriteInt32(0);
                res3.WriteInt32(0);
            }

            Router.Send(client, (ushort) AreaPacketId.recv_data_notify_npc_data, res3, ServerType.Area);
        }

        private void LogOut(NecClient client)
        {
            byte[] byteArr = new byte[8] {0x00, 0x06, 0xEE, 0x91, 0, 0, 0, 0};

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            res.SetPositionStart();

            for (int i = 4; i < 8; i++)
            {
                byteArr[i] += res.ReadByte();
            }

            // TODO use packet format 
            //  client.MsgConnection.Send(byteArr);

            System.Threading.Thread.Sleep(4000);

            byte[] byteArrr = new byte[9] {0x00, 0x07, 0x52, 0x56, 0, 0, 0, 0, 0};

            IBuffer res2 = BufferProvider.Provide();

            res2.WriteInt32(0);
            res2.WriteByte(0);

            res2.SetPositionStart();

            for (int i = 4; i < 9; i++)
            {
                byteArrr[i] += res2.ReadByte();
            }

            // TODO use packet format 
            //  client.MsgConnection.Send(byteArr);
        }

        private void AdminConsoleSelectPackageUpdate(NecClient client)
        {
            int errcode = 0;
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(errcode); //Error message Call. 0 for success. see additional options in Sys_msg.csv
            /*
            1	You have unopened mails	SYSTEM_WARNING
            2	No mails to delete	SYSTEM_WARNING
            3	You have %d unreceived mails. Please check your inbox.	SYSTEM_WARNING
            -2414	Mail title includes banned words	SYSTEM_WARNING

            */

            Router.Send(client, (ushort) AreaPacketId.recv_select_package_update_r, res, ServerType.Area);
        }

        private void AdminConsoleRecvDataNotifyMonsterData(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            int MonsterUniqueId = Util.GetRandomNumber(55566, 55888);
            res.WriteInt32(MonsterUniqueId);

            res.WriteCString($"Demon Bardock{MonsterUniqueId}"); //Name while spawning

            res.WriteCString($"Titan"); //Title

            res.WriteFloat(client.Character.X + Util.GetRandomNumber(25, 150)); //X Pos
            res.WriteFloat(client.Character.Y + Util.GetRandomNumber(25, 150)); //Y Pos
            res.WriteFloat(client.Character.Z); //Z Pos
            res.WriteByte(client.Character.viewOffset); //view offset

            res.WriteInt32(
                900102); // Monster serial ID.  70101 for Lesser Demon.  If this is invalid, you can't "loot" the monster or see it's first CString

            res.WriteInt32(2016001); // Model from model_common.csv  2070001 for Lesser Demon

            res.WriteInt16(100); //model size

            res.WriteInt32(0x10); // cmp to 0x10 = 16

            int numEntries = 0x10;
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(16);
            }

            res.WriteInt32(0x10); // cmp to 0x10 = 16

            int numEntries2 = 0x10;
            for (int i = 0; i < numEntries2; i++)
            {
                res.WriteInt32(16); // this was an x2 loop (i broke it down)
            }

            for (int i = 0; i < numEntries2; i++)
            {
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteInt32(0);
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
                res.WriteInt64(100);
            }

            res.WriteInt32(900102); //1000 0000 here makes it stand up and not be dead.   or 0 = alive, 1 = dead

            res.WriteInt64(itemIDs[x]);

            res.WriteInt64(itemIDs[x]);

            res.WriteInt64(itemIDs[x]);

            res.WriteByte(0);

            res.WriteByte(0);

            res.WriteInt32(900); //Current HP

            res.WriteInt32(1000); //Max HP

            res.WriteInt32(0x80); // cmp to 0x80 = 128

            int numEntries4 = 0x80; //Statuses?
            for (int i = 0; i < numEntries4; i++)

            {
                res.WriteInt32(900102); // ID ?
                res.WriteInt32(1);
                res.WriteInt32(0);
            }

            Router.Send(client.Map, (ushort) AreaPacketId.recv_data_notify_monster_data, res, ServerType.Area);


            IBuffer res5 = BufferProvider.Provide();
            res5.WriteInt32(11);
            res5.WriteInt32(MonsterUniqueId);
            Router.Send(client, (ushort) AreaPacketId.recv_monster_hate_on, res5, ServerType.Area);

            IBuffer res6 = BufferProvider.Provide();
            res6.WriteInt32(11);
            res6.WriteInt32(MonsterUniqueId);
            Router.Send(client, (ushort) AreaPacketId.recv_battle_report_notify_damage_hp, res6, ServerType.Area);


            IBuffer res12 = BufferProvider.Provide();
            res12.WriteInt32(0);

            res12.WriteInt32(MonsterUniqueId);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_monster_state_update_notify, res12, ServerType.Area);

            /*IBuffer res81 = BufferProvider.Provide();
            res81.WriteInt32(11);

            res81.WriteFloat(45);
            res81.WriteFloat(0);
            res81.WriteFloat(0);
            res81.WriteByte(0);

            res81.WriteFloat(0);
            res81.WriteFloat(0);
            res81.WriteInt32(3);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_data_notify_maplink, res81);

            /*
                        IBuffer res5 = BufferProvider.Provide();
                        res5.WriteInt32(8);
                        res5.WriteInt32(0);
                        res5.WriteFloat(1);
                        Router.Send(client, (ushort)AreaPacketId.recv_battle_report_action_monster_skill_start_cast, res5);


                        IBuffer res6 = BufferProvider.Provide();
                        res6.WriteInt32(0);
                        Router.Send(client, (ushort)AreaPacketId.recv_battle_report_action_monster_skill_exec, res6);

                        IBuffer res8 = BufferProvider.Provide();
                        res8.WriteInt32(client.Character.Id);
                        res8.WriteInt32(1);
                        Router.Send(client, (ushort)AreaPacketId.recv_battle_report_notify_damage_hp, res8);

                        IBuffer res4 = BufferProvider.Provide();
                        res4.WriteInt32(1);
                        Router.Send(client, (ushort)AreaPacketId.recv_battle_report_notify_hit_effect, res4);

                        IBuffer res10 = BufferProvider.Provide();
                        res10.WriteByte(1);
                        res10.WriteInt16(0);
                        Router.Send(client, (ushort)AreaPacketId.recv_chara_target_move_side_speed_per, res10);

                        IBuffer res9 = BufferProvider.Provide();
                        res9.WriteInt32(8);

                        res9.WriteInt32(8); // 1 = no reactive ?
                        Router.Send(client, (ushort)AreaPacketId.recv_monster_state_update_notify, res9); */


            //3100102 attack monster, where to put it ?
        }

        private void AdminConsoleRecvItemInstanceUnidentified(NecClient client)
        {
            int i = 0;
            x = -1;
            for (i = 0; i < 19; i++)
            {
                x++;
                System.Threading.Thread.Sleep(100);
                //recv_item_instance_unidentified = 0xD57A,
                IBuffer res = BufferProvider.Provide();

                res.WriteInt64(client.Character.EquipId[x]);

                res.WriteCString($"ID:{itemIDs[x]} MSK:{EquipBitMask[x]} Type:{EquipItemType[x]} lvl"); // Item Name

                res.WriteInt32(EquipItemType[x] -
                               1); // Item Type. Refer To ItemType.csv   // This controls Item Type.  61 ( minus 1) makes everything Type "Avatar"
                res.WriteInt32(EquipBitMask[x]); //Slot Limiting Bitmask.  Limits  Slot Item can be Equiped.

                res.WriteByte(1); // Numbers of items

                res.WriteInt32(
                    EquipBitMask[
                        Util.GetRandomNumber(4,
                            4)]); /* 10001003 Put The Item Unidentified. 0 put the item Identified 1-2-4-8-16 follow this patterns (8 cursed, 16 blessed)*/


                res.WriteInt32(client.Character.EquipId[x]); //Item ID for Icon
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteInt32(1);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(1); // bool
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0); // 0 = adventure bag. 1 = character equipment, 2 = royal bag
                res.WriteByte(0); // 0~2
                res.WriteInt16((short) x); // bag index 0 to 24

                res.WriteInt32(EquipBitMask[x]); //bit mask. This indicates where to put items.  

                res.WriteInt64(client.Character.EquipId[x]);

                res.WriteInt32(1);


                Router.Send(client, (ushort) AreaPacketId.recv_item_instance_unidentified, res, ServerType.Area);


                IBuffer res0 = BufferProvider.Provide();
                res0.WriteInt64(client.Character.EquipId[x]);
                res0.WriteInt32(Util.GetRandomNumber(199, 200)); // MaxDura points
                Router.Send(client, (ushort) AreaPacketId.recv_item_update_maxdur, res0, ServerType.Area);


                IBuffer res2 = BufferProvider.Provide(); // Maybe not the good one ?
                res2.WriteInt64(client.Character.EquipId[x]);
                res2.WriteInt32(Util.GetRandomNumber(1, 200)); // Durability points
                Router.Send(client, (ushort) AreaPacketId.recv_item_update_durability, res2, ServerType.Area);


                IBuffer res4 = BufferProvider.Provide();
                res4.WriteInt64(client.Character.EquipId[x]);
                res4.WriteInt32(Util.GetRandomNumber(800, 10000)); // Weight points
                Router.Send(client, (ushort) AreaPacketId.recv_item_update_weight, res4, ServerType.Area);


                IBuffer res5 = BufferProvider.Provide();
                res5.WriteInt64(client.Character.EquipId[x]);
                res5.WriteInt16((short) Util.GetRandomNumber(5, 500)); // Defense and attack points
                Router.Send(client, (ushort) AreaPacketId.recv_item_update_physics, res5, ServerType.Area);


                IBuffer res6 = BufferProvider.Provide();
                res6.WriteInt64(client.Character.EquipId[x]);
                res6.WriteInt16((short) Util.GetRandomNumber(5, 500)); // Magic def and attack Points
                Router.Send(client, (ushort) AreaPacketId.recv_item_update_magic, res6, ServerType.Area);


                IBuffer res7 = BufferProvider.Provide();
                res7.WriteInt64(client.Character.EquipId[x]);
                res7.WriteInt32(Util.GetRandomNumber(1, 10)); // for the moment i don't know what it change
                Router.Send(client, (ushort) AreaPacketId.recv_item_update_enchantid, res7, ServerType.Area);


                IBuffer res8 = BufferProvider.Provide();
                res8.WriteInt64(client.Character.EquipId[x]);
                res8.WriteInt16((short) Util.GetRandomNumber(0, 100000)); // Shwo GP on certain items
                Router.Send(client, (ushort) AreaPacketId.recv_item_update_ac, res8, ServerType.Area);

                IBuffer res9 = BufferProvider.Provide();
                res9.WriteInt64(client.Character.EquipId[x]);
                res9.WriteInt32(Util.GetRandomNumber(1, 50)); // for the moment i don't know what it change
                Router.Send(client, (ushort) AreaPacketId.recv_item_update_date_end_protect, res9, ServerType.Area);


                IBuffer res11 = BufferProvider.Provide();
                res11.WriteInt64(client.Character.EquipId[x]);
                res11.WriteByte((byte) Util.GetRandomNumber(0, 100)); // Hardness
                Router.Send(client, (ushort) AreaPacketId.recv_item_update_hardness, res11, ServerType.Area);


                IBuffer res1 = BufferProvider.Provide();
                res1.WriteInt64(client.Character
                    .EquipId[x]); //client.Character.EquipId[x]   put stuff unidentified and get the status equipped  , 0 put stuff identified
                res1.WriteInt32(0);
                Router.Send(client, (ushort) AreaPacketId.recv_item_update_state, res1, ServerType.Area);


                IBuffer res13 = BufferProvider.Provide();
                //95 torso ?
                //55 full armor too ?
                //93 full armor ?
                // 27 full armor ?
                //11 under ?
                // 38 = boots and cape
                //byte y = unchecked((byte)110111);
                //byte y = unchecked ((byte)Util.GetRandomNumber(0, 100)); // for the moment i only get the armor on this way :/

                res13.WriteInt64(client.Character.EquipId[x]);
                res13.WriteInt32(EquipBitMask[x]); // Permit to get the armor on the chara

                res13.WriteInt32(client.Character.EquipId[x]); // List of items that gonna be equip on the chara
                res13.WriteByte(0); // ?? when you change this the armor dissapear, apparently
                res13.WriteByte(0);
                res13.WriteByte(0); //need to find the right number, permit to get the armor on the chara

                res13.WriteInt32(1);
                res13.WriteByte(0);
                res13.WriteByte(0);
                res13.WriteByte(0);

                res13.WriteByte(0);
                res13.WriteByte(0);
                res13.WriteByte(0); //bool
                res13.WriteByte(0);
                res13.WriteByte(0);
                res13.WriteByte(0);
                res13.WriteByte(0); // 1 = body pink texture
                res13.WriteByte(0);
                Router.Send(client.Map, (ushort) AreaPacketId.recv_item_update_eqmask, res13, ServerType.Area);


                IBuffer res17 = BufferProvider.Provide();
                res17.WriteInt64(itemIDs[x]);
                res17.WriteInt32(EquipBitMask[x]);
                Router.Send(client.Map, (ushort) AreaPacketId.recv_item_update_spirit_eqmask, res17, ServerType.Area,
                    client);


                /*IBuffer res19 = BufferProvider.Provide();
                res19.WriteInt32(itemIDs[x]);
                res19.WriteInt32(EquipBitMask[x]);

                int numEntries = 0x2;
                for (i = 0; i < numEntries; i++)
                {
                    res19.WriteInt32(0);
                    res19.WriteByte(0);
                    res19.WriteByte(0);
                    res19.WriteByte(0);

                }

                res19.WriteByte(0);

                res19.WriteByte(0);
                res19.WriteByte(0);
                res19.WriteByte(0);
                res19.WriteByte(0);
                res19.WriteByte(0);
                res19.WriteByte(0);

                res19.WriteByte(0);

                res19.WriteInt32(EquipBitMask[x]);
                Router.Send(client.Map, (ushort)AreaPacketId.recv_dbg_chara_equipped, res19, ServerType.Area);*/
            }
        }


        private void AdminConsoleRecvItemInstance(NecClient client)
        {
            x = 0;
            for (int j = 0; j < 19; j++)
            {
                IBuffer res = BufferProvider.Provide();
                //recv_item_instance = 0x86EA,
                x++;
                res.WriteInt64(client.Character
                    .EquipId[x]); //  Assume Unique ID instance identifier. 1 here makes item green icon
                res.WriteInt32(EquipItemType[x] - 1);
                res.WriteByte(1); //number of items in stack
                res.WriteInt32(client.Character.EquipId[x]); //
                res.WriteFixedString("WhatIsThis", 0x10);
                res.WriteByte(0); // 0 = adventure bag. 1 = character equipment
                res.WriteByte(0); // 0~2
                res.WriteInt16((short) x); // bag index
                res.WriteInt32(EquipBitMask[x]); //bit mask. This indicates where to put items.   ??
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteCString("ThisIsThis"); // find max size 
                res.WriteInt16(0);
                res.WriteInt16(0);
                res.WriteInt32(client.Character.EquipId[x]);
                res.WriteByte(0);
                res.WriteInt32(client.Character.EquipId[x]);
                int numEntries = 2;
                res.WriteInt32(numEntries); // less than or equal to 2
                for (int i = 0; i < numEntries; i++)
                {
                    res.WriteInt32(client.Character.EquipId[x]);
                }

                numEntries = 3;
                res.WriteInt32(numEntries); // less than or equal to 3
                for (int i = 0; i < numEntries; i++)
                {
                    res.WriteByte(0); //bool
                    res.WriteInt32(client.Character.EquipId[x]);
                    res.WriteInt32(client.Character.EquipId[x]);
                    res.WriteInt32(client.Character.EquipId[x]);
                }

                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt16(0);
                res.WriteInt32(1); //1 here lables the item "Gaurd".   no effect from higher numbers
                res.WriteInt16(0);

                Router.Send(client, (ushort) AreaPacketId.recv_item_instance, res, ServerType.Area);
            }
        }

        private void SendMapEntry(NecClient client, int myMapId)
        {
            int mapId = myMapId;


            Map map = Server.Map.Get(mapId);

            //map.Enter(client);

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_map_entry_r, res, ServerType.Area);
        }

        private void SendMapChangeForce(NecClient client, int MapID)
        {
            IBuffer res = BufferProvider.Provide();
            client.Character.MapId = MapID;
            Map map = Server.Map.Get(client.Character.MapId);

            client.Character.X = map.X;
            client.Character.Y = map.Y;
            client.Character.Z = map.Z;

            //sub_4E4210_2341  // impacts map spawn ID
            res.WriteInt32(MapID); //MapSerialID
            res.WriteInt32(MapID); //MapID
            res.WriteFixedString("127.0.0.1", 65); //IP
            res.WriteInt16(60002); //Port

            //sub_484420   //  does not impact map spawn coord
            res.WriteFloat(client.Character.X); //X Pos
            res.WriteFloat(client.Character.Y); //Y Pos
            res.WriteFloat(client.Character.Z); //Z Pos
            res.WriteByte(1); //View offset

            Router.Send(client, (ushort) AreaPacketId.recv_map_change_force, res, ServerType.Area);

            //SendMapChangeSyncOk(client);
            SendMapEntry(client, MapID);
        }

        private void SendEventEnd(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            Router.Send(client, (ushort) AreaPacketId.recv_event_end, res, ServerType.Area);
        }

        private void SendMapLink(NecClient client, int colorChoice)
        {
            if (colorChoice == 10)
            {
                colorChoice = EquipBitMask[Util.GetRandomNumber(0, 4)];
            } // pick random model if you don't specify an ID.

            IBuffer res1 = BufferProvider.Provide(); // it's the aura portal for map
            res1.WriteInt32(Util.GetRandomNumber(1000, 1010)); // Unique ID

            res1.WriteFloat(client.Character.X); //x
            res1.WriteFloat(client.Character.Y); //y
            res1.WriteFloat(client.Character.Z + 2); //z
            res1.WriteByte(client.Character.viewOffset); // offset

            res1.WriteFloat(1000); // Height
            res1.WriteFloat(100); // Width

            res1.WriteInt32(
                colorChoice); // Aura color 0=blue 1=gold 2=white 3=red 4=purple 5=black  0 to 5, crash above 5
            Router.Send(client, (ushort) AreaPacketId.recv_data_notify_maplink, res1, ServerType.Area);
        }


        /////////Int array for testing Item ID's. 
        int[] itemIDs = new int[]
        {
            10800405 /*Weapon*/, 15100901 /*Shield* */, 20000101 /*Arrow*/, 110301 /*head*/, 210701 /*Torso*/,
            360103 /*Pants*/, 401201 /*Hands*/, 560103 /*Feet*/, 690101 /*Cape*/, 30300101 /*Necklace*/,
            30200107 /*Earring*/, 30400105 /*Belt*/, 30100106 /*Ring*/, 70000101 /*Talk Ring*/, 160801 /*Avatar Head */,
            260801 /*Avatar Torso*/, 360801 /*Avatar Pants*/, 460801 /*Avatar Hands*/, 560801 /*Avatar Feet*/, 1, 2, 3
        };

        int[] NPCModelID = new int[]
            {1911105, 1112101, 1122401, 1122101, 1311102, 1111301, 1121401, 1131401, 2073002, 1421101};

        int[] NPCSerialID = new int[]
            {10000101, 10000102, 10000103, 10000104, 10000105, 10000106, 10000107, 10000108, 80000009, 10000101};

        int[] EquipBitMask = new int[]
        {
            1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768, 65536, 131072, 262144, 524288,
            1048576, 2097152
        };

        int[] EquipItemType = new int[]
            {9, 20, 23, 28, 31, 32, 36, 40, 41, 44, 43, 45, 42, 54, 61, 61, 61, 61, 61, 61, 0, 0};

        int[] EquipStatus = new int[] {0, 1, 2, 4, 8, 16};

        int[] GGateModelIds = new int[]
        {
            1800000, /*	Stone slab of guardian statue	*/
            1801000, /*	Bulletin board	*/
            1802000, /*	Sign	*/
            1803000, /*	Stone board	*/
            1804000, /*	Guardians Gate	*/
            1805000, /*	Warp device	*/
            1806000, /*	Puddle	*/
            1807000, /*	machine	*/
            1808000, /*	Junk mountain	*/
            1809000, /*	switch	*/
            1810000, /*	Statue	*/
            1811000, /*	Horse statue	*/
            1812000, /*	Agate balance	*/
            1813000, /*	Dagger scale	*/
            1814000, /*	Apple balance	*/
            1815000, /*	torch	*/
            1816000, /*	Royal shop sign	*/
            1817000, /*	Witch pot	*/
            1818000, /*	toilet	*/
            1819000, /*	Abandoned tree	*/
            1820000, /*	Pedestal with fire	*/
            1900000, /*	For transparency	*/
            1900001, /*	For transparency	*/
        };
    }
}
