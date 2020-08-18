using System;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Receive;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Data.Setting;

namespace Necromancy.Server.Packet.Area
{
    public class send_event_access_object : ClientHandler
    {
        private static readonly NecLogger Logger =
            LogProvider.Logger<NecLogger>(typeof(send_event_access_object));

        public send_event_access_object(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_event_access_object;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint instanceId = packet.Data.ReadUInt32();
            client.Character.eventSelectReadyCode =
                instanceId; //Sends the NpcID to 'send_event_select_exec_r  logic gate.
            client.Character.takeover = false;

            //Begin Event for all cases
            SentEventStart(client, instanceId);


            IInstance instance = Server.Instances.GetInstance(instanceId);
            switch (instance)
            {
                case NpcSpawn npcSpawn:
                    client.Map.NpcSpawns.TryGetValue(npcSpawn.InstanceId, out npcSpawn);
                    Logger.Debug(
                        $"instanceId : {npcSpawn.InstanceId} |  npcSpawn.Id: {npcSpawn.Id}  |   npcSpawn.NpcId: {npcSpawn.NpcId}");
                    IBuffer res = BufferProvider.Provide();
                    res.WriteInt32(0);
                    Router.Send(client, (ushort) AreaPacketId.recv_event_access_object_r, res, ServerType.Area);

                    //logic to execute different actions based on the event that triggered this select execution.
                    var eventSwitchPerObjectID = new Dictionary<Func<int, bool>, Action>
                    {
                        {
                            x => x == 10000704, () => SendEventSelectMapAndChannel(client, instanceId)
                        }, //set to Manaphes in slums for testing.
                        {x => x == 10000005, () => SendEventSelectMapAndChannel(client, instanceId)},
                        {x => x == 10000012, () => SendEventSelectMapAndChannel(client, instanceId)},
                        {x => x == 10000912, () => SendEventSelectMapAndChannel(client, instanceId)},
                        {x => x == 80000018, () => AuctionHouse(client, npcSpawn)},
                        {x => x == 10000019, () => Abdul(client, npcSpawn)},
                        {
                            x => (x == 74000022) || (x == 74000024) || (x == 74000023),
                            () => RecoverySpring(client, npcSpawn)
                        },
                        {
                            x => (x == 10000044) || (x == 10000122) || (x == 10000329) || (x == 10000710) ||
                                 (x == 90000031),
                            () => Jeweler(client, npcSpawn)
                        },
                        {
                            x => (x == 10000033) || (x == 10000113) || (x == 10000305) || (x == 10000311) ||
                                 (x == 10000702),
                            () => Blacksmith(client, npcSpawn)
                        },
                        {x => x == 10000010, () => DonkeysItems(client, npcSpawn)},
                        {x => x == 80000003, () => CloakRoomShopClerk(client, npcSpawn)},
                        {x => x == 10000002, () => RegularInn(client, npcSpawn)},
                        {x => x == 10000703, () => CrimInn(client, npcSpawn)},
                        {x => x == 70000029, () => LostBBS(client, npcSpawn)},
                        {
                            x => (x == 70009008) || (x == 70000025) || (x == 70001001),
                            () => CharaChangeChannel(client, npcSpawn)
                        },
                        {x => x == 80000009, () => UnionWindow(client, npcSpawn)},
                        { x => x == 10000004 ,  () => SoulRankNPC(client, npcSpawn)},
                        {x => x < 10, () => Logger.Debug($" Event Object switch for NPC ID {npcSpawn.NpcId} reached")},
                        {x => x < 100, () => Logger.Debug($" Event Object switch for NPC ID {npcSpawn.NpcId} reached")},
                        {
                            x => x < 1000,
                            () => Logger.Debug($" Event Object switch for NPC ID {npcSpawn.NpcId} reached")
                        },
                        {
                            x => x < 10000,
                            () => Logger.Debug($" Event Object switch for NPC ID {npcSpawn.NpcId} reached")
                        },
                        {
                            x => x < 100000,
                            () => Logger.Debug($" Event Object switch for NPC ID {npcSpawn.NpcId} reached")
                        },
                        {
                            x => x < 1000000,
                            () => Logger.Debug($" Event Object switch for NPC ID {npcSpawn.NpcId} reached")
                        },
                        {x => x < 900000100, () => WorkInProgress(client, npcSpawn)}
                    };

                    eventSwitchPerObjectID.First(sw => sw.Key((int) npcSpawn.NpcId)).Value();

                    break;
                case MonsterSpawn monsterSpawn:
                    Logger.Debug($"MonsterId: {monsterSpawn.Id}");

                    IBuffer res2 = BufferProvider.Provide();
                    res2.WriteInt32(monsterSpawn.Id);
                    Router.Send(client, (ushort) AreaPacketId.recv_event_access_object_r, res2, ServerType.Area);

                    break;

                case GGateSpawn ggateSpawn:
                    //client.Map.GGateSpawns.TryGetValue(ggateSpawn.InstanceId, out ggateSpawn);
                    Logger.Debug(
                        $"instanceId : {ggateSpawn.InstanceId} |  ggateSpawn.Id: {ggateSpawn.Id}  |   ggateSpawn.NpcId: {ggateSpawn.SerialId}");
                    IBuffer res3 = BufferProvider.Provide();
                    res3.WriteInt32(0);
                    Router.Send(client, (ushort)AreaPacketId.recv_event_access_object_r, res3, ServerType.Area);

                    //logic to execute different actions based on the event that triggered this select execution.
                    var eventSwitchPerObjectID2 = new Dictionary<Func<int, bool>, Action>
                    {

                        {x => x == 74013071, () => SendGetWarpTarget(client, ggateSpawn)},
                        {x => x == 74013161, () => SendGetWarpTarget(client, ggateSpawn)},
                        {x => x == 74013271, () => SendGetWarpTarget(client, ggateSpawn)},

                        {x => x < 900000100, () => WorkInProgressGGate(client, ggateSpawn) }
                    };

                    eventSwitchPerObjectID2.First(sw => sw.Key((int)ggateSpawn.SerialId)).Value();
                    break;
                default:
                    Logger.Error($"Event Access logic for InstanceId: {instanceId} does not exist");
                    SendEventEnd(client);
                    break;
            }
        }

        private void SentEventStart(NecClient client, uint obkectID)
        {
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(0); // 0 = normal 1 = cinematic
            res2.WriteByte(0);

            Router.Send(client, (ushort) AreaPacketId.recv_event_start, res2, ServerType.Area);
            // it's the event than permit to that all the code under
            // dont forget tu put a recv_event_end, at the end, if you don't want to get stuck, and do nothing.
        }

        private void SendEventShowBoardStart(NecClient client, uint instanceId)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString("Select a Map!. just not the town"); // find max size
            res.WriteInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_event_show_board_start, res, ServerType.Area);
        }

        private void SendEventShowBoardEnd(NecClient client, uint instanceId)
        {
            IBuffer res = BufferProvider.Provide();
            Router.Send(client, (ushort) AreaPacketId.recv_event_show_board_end, res, ServerType.Area);
        }

        private void SendEventEnd(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            Router.Send(client, (ushort) AreaPacketId.recv_event_end, res, ServerType.Area);
        }

        private void SendEventMessageNoObject(NecClient client, int instanceId)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString($"NPC#:{instanceId}"); // Npc name
            res.WriteCString("QuestChat"); //Chat Window lable
            res.WriteCString(
                "You've got 5 seconds before this window closes. Think Quick!'"); // it's the npc text, switch automatically to an other window when text finish
            Router.Send(client, (ushort) AreaPacketId.recv_event_message_no_object, res, ServerType.Area);
        }

        private void SendEventMessage(NecClient client, int instanceId)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(1);
            res.WriteCString("Hello world.");
            Router.Send(client, (ushort) AreaPacketId.recv_event_message, res, ServerType.Area);
        }

        private void SendEventBlockMessage(NecClient client, int instanceId)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(instanceId);
            res.WriteCString("Hello world.");
            Router.Send(client, (ushort) AreaPacketId.recv_event_block_message, res, ServerType.Area);
        }

        private void SendEventSelectMapAndChannel(NecClient client, uint instanceId)
        {
            IBuffer res7 = BufferProvider.Provide();

            int numEntries = mapIDs.Length;
            ; //Max of 0x20 : cmp ebx,20 
            res7.WriteInt32(numEntries);
            for (int i = 0; i < numEntries; i++)
            {
                //sub_494c50
                res7.WriteInt32(nameIdx[i]); //Stage ID from Stage.CSV
                res7.WriteInt32(
                    mapIDs[i]); //Map ID.  Cross Refrences Dungeun_info.csv to get X/Y value for map icon, and dungeun description. 
                res7.WriteInt32(partySize[i]); //max players
                res7.WriteInt16(levels[i]);
                ; //Recommended Level
                //sub_4834C0
                res7.WriteByte(19);
                for (int j = 0; j < 0x80; j++) //j max 0x80
                {
                    res7.WriteInt32(mapIDs[i]);
                    res7.WriteFixedString($"Channel-{j}",
                        0x61); //Channel Names.  Variables let you know what Loop Iteration you're on
                    res7.WriteByte(1); //bool 1 | 0
                    res7.WriteUInt16(0xFFFF); //Max players  -  Comment from other recv
                    res7.WriteInt16(7); //Current players  - Comment from other recv
                    res7.WriteByte(3);
                    res7.WriteByte(6); //channel Emoticon - 6 for a Happy Face
                }

                res7.WriteByte(6); //Number or Channels  - comment from other recv
            }

            Router.Send(client, (ushort) AreaPacketId.recv_event_select_map_and_channel, res7, ServerType.Area);
        }


        private void Abdul(NecClient client, NpcSpawn npcSpawn)

        {
            if (client.Character.helperTextAbdul)

            {
                IBuffer res2 = BufferProvider.Provide();
                res2.WriteCString($"{npcSpawn.Name}"); //Name
                res2.WriteCString($"{npcSpawn.Title}"); //Title (inside chat box)
                res2.WriteCString("I used to drive a cab."); //Text block
                Router.Send(client, (ushort) AreaPacketId.recv_event_message_no_object, res2, ServerType.Area);

                IBuffer res6 = BufferProvider.Provide();
                Router.Send(client, (ushort) AreaPacketId.recv_event_sync, res6, ServerType.Area);

                client.Character.helperTextAbdul = false;
            }
            else
            {
                IBuffer res = BufferProvider.Provide();
                res.WriteCString(npcSpawn.Title); // Title at top of Window
                res.WriteUInt32(npcSpawn.InstanceId); //should pull name of NPC,  doesnt currently
                Router.Send(client, (ushort) AreaPacketId.recv_event_show_board_start, res, ServerType.Area);

                IBuffer res3 = BufferProvider.Provide();
                res3.WriteCString("Accept Mission"); //Length 0x601  // name of the choice 
                Router.Send(client, (ushort) AreaPacketId.recv_event_select_push, res3,
                    ServerType.Area); // It's the first choice

                IBuffer res4 = BufferProvider.Provide();
                res4.WriteCString("Report Mission"); //Length 0x601 // name of the choice
                Router.Send(client, (ushort) AreaPacketId.recv_event_select_push, res4,
                    ServerType.Area); // It's the second choice 

                IBuffer res5 = BufferProvider.Provide();
                res5.WriteCString("Back"); //Length 0x601 // name of the choice
                Router.Send(client, (ushort) AreaPacketId.recv_event_select_push, res5,
                    ServerType.Area); // It's the second choice 

                IBuffer res11 = BufferProvider.Provide();
                res11.WriteCString("Pick a Button..  What are you waiting for"); // Window Heading / Name
                res11.WriteUInt32(npcSpawn.InstanceId);
                Router.Send(client, (ushort) AreaPacketId.recv_event_select_exec, res11,
                    ServerType.Area); // It's the windows that contain the multiple choice
            }
        }

        // I added these to Npc database for now, should it be handled differently?????
        private void SendGetWarpTarget(NecClient client, GGateSpawn ggateSpawn)
        {
            client.Character.eventSelectExecCode = -1;
            Logger.Debug(
                $"ggateSpawn.Id: {ggateSpawn.Id}  |   ggateSpawn.NpcId: {ggateSpawn.SerialId} client.Character.eventSelectExecCode: {client.Character.eventSelectExecCode}");
            if (client.Character.eventSelectExecCode == -1)
            {
                IBuffer res3 = BufferProvider.Provide();
                if (client.Character.MapId == 2002104) // Roswald Fort #1 to #2
                {
                    res3.WriteCString("Isolated Hall"); //Length 0x601
                }
                else if (client.Character.MapId == 2002105 || client.Character.MapId == 2002106
                ) // Roswald Fort #2/#3 to #1
                {
                    res3.WriteCString("Rusted Gate"); //Length 0x601
                }

                Router.Send(client, (ushort) AreaPacketId.recv_event_select_push, res3,
                    ServerType.Area); // It's the first choice

                IBuffer res70 = BufferProvider.Provide();
                if (client.Character.MapId == 2002104 || client.Character.MapId == 2002105) // Roswald Fort #1/#2 to #3
                {
                    res70.WriteCString("Severed Corridor"); //Length 0x601
                }
                else if (client.Character.MapId == 2002106) // Roswald Fort #3 to #2
                {
                    res70.WriteCString("Isolated Hall"); //Length 0x601
                }

                Router.Send(client, (ushort) AreaPacketId.recv_event_select_push, res70,
                    ServerType.Area); // It's the second choice

                IBuffer res1 = BufferProvider.Provide();
                res1.WriteCString("Select area to travel to"); // It's the title dude
                res1.WriteUInt32(ggateSpawn.InstanceId); // This is the Event Type.  0xFFFD sends a 58 byte packet
                Router.Send(client, (ushort) AreaPacketId.recv_event_select_exec, res1,
                    ServerType.Area); // Actual map change is handled by send_event_select_exec_r, need to figure out how to handle this better
            }
        }

        private void defaultEvent(NecClient client, uint instanceId)
        {
            SendEventShowBoardStart(client, instanceId);
            //SendEventMessageNoObject(client, instanceId);
            //SendEventMessage(client, instanceId);
            //SendEventBlockMessage(client, instanceId);
            SendEventSelectMapAndChannel(client, instanceId);

            Task.Delay(TimeSpan.FromMilliseconds((int) (15 * 1000))).ContinueWith
            (t1 =>
                {
                    SendEventShowBoardEnd(client, instanceId);
                    //SendEventEnd(client);
                }
            );
        }

        private void RecoverySpring(NecClient client, NpcSpawn npcSpawn)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString(npcSpawn.Title); // Title at top of Window
            res.WriteUInt32(npcSpawn.InstanceId); //should pull name of NPC,  doesnt currently
            Router.Send(client, (ushort) AreaPacketId.recv_event_show_board_start, res, ServerType.Area);


            IBuffer res12 = BufferProvider.Provide();
            res12.WriteCString("The fountain is brimmed with water. Has enough for 5 more drinks."); // Length 0xC01
            Router.Send(client, (ushort) AreaPacketId.recv_event_system_message, res12,
                ServerType.Area); // show system message on middle of the screen.


            IBuffer res3 = BufferProvider.Provide();
            res3.WriteCString("Drink"); //Length 0x601  // name of the choice 
            Router.Send(client, (ushort) AreaPacketId.recv_event_select_push, res3,
                ServerType.Area); // It's the first choice

            IBuffer res5 = BufferProvider.Provide();
            res5.WriteCString("Don't drink"); //Length 0x601 // name of the choice
            Router.Send(client, (ushort) AreaPacketId.recv_event_select_push, res5,
                ServerType.Area); // It's the second choice

            IBuffer res11 = BufferProvider.Provide();
            res11.WriteCString("Effect: Recover 50% of maximum HP and MP"); // Window Heading / Name
            res11.WriteUInt32(npcSpawn.InstanceId);
            Router.Send(client, (ushort) AreaPacketId.recv_event_select_exec, res11,
                ServerType.Area); // It's the windows that contain the multiple choice
        }

        private void WorkInProgress(NecClient client, NpcSpawn npcSpawn)
        {
            String[] Text1 = new string[]
            {
                $"Welcome to the test server for Wizardry Online {client.Character.Name}!",
                "Go Away!",
                "Hey there good lookin. that's a nice hat you have there!",
                "i heard there's a secret green door in white town",
                "there might be some beetles in caligrase",
                $"{client.Soul.Name}.... were you born with that name?",
                "ありがとうございます", //game client can't render japanese text
                "мы ценим вас"                
            };
            String[] Text2 = new string[]
            {
                $"This NPC is still under development",
                "  ..no seriously, go away!",
                "Be a shame if somebody..... Took it!",
                "see if you can find it",
                "go kill those beetles!",
                "or did you choose it?  ",
                " 参加していただきありがとうございます",
                "Спасибо, что присоединились"
            };
            int randomTextChoice = Util.GetRandomNumber(0, Text1.Length-1);

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteCString($"{npcSpawn.Name}"); //Name
            res2.WriteCString($"{npcSpawn.Title}"); //Title (inside chat box)
            res2.WriteCString(Text1[randomTextChoice]); 
            Router.Send(client, (ushort)AreaPacketId.recv_event_message_no_object, res2, ServerType.Area);

            IBuffer res3 = BufferProvider.Provide();
            res3.WriteCString($"{npcSpawn.Name}"); //Name
            res3.WriteCString($"{npcSpawn.Title}"); //Title (inside chat box)
            res3.WriteCString(Text2[randomTextChoice]);
            Router.Send(client, (ushort)AreaPacketId.recv_event_message_no_object, res3, ServerType.Area);

            IBuffer res6 = BufferProvider.Provide();
            Router.Send(client, (ushort)AreaPacketId.recv_event_sync, res6, ServerType.Area);
        }

        private void WorkInProgressGGate(NecClient client, GGateSpawn npcSpawn)
        {
            String[] Text1 = new string[]
            {
                $"Here lies {client.Character.Name}!",
                "Go Away!",
                "I am an inanimate Object!",
                "Sorry, device is broke",
                "$5.99 early access special *thwack*...",
                $"{client.Soul.Name}.... please help us?",
                "ありがとうございます", //game client can't render japanese text
                "мы ценим вас"
            };

            int randomTextChoice = Util.GetRandomNumber(0, Text1.Length - 1);

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteCString($"{npcSpawn.Name}"); //Name
            res2.WriteCString($"{npcSpawn.Title}"); //Title (inside chat box)
            res2.WriteCString(Text1[randomTextChoice]);
            Router.Send(client, (ushort)AreaPacketId.recv_event_message_no_object, res2, ServerType.Area);

            IBuffer res6 = BufferProvider.Provide();
            Router.Send(client, (ushort)AreaPacketId.recv_event_sync, res6, ServerType.Area);
        }

        //Use this as a default event if we ever need to do some serious NPC model updating and heading setting again.
        private void UpdateNPC(NecClient client, NpcSpawn npcSpawn)
        {
            IBuffer res3 = BufferProvider.Provide();
            res3.WriteCString("Set the NPC Heading in Database"); //Length 0x601  // name of the choice 
            Router.Send(client, (ushort) AreaPacketId.recv_event_select_push, res3,
                ServerType.Area); // It's the first choice

            IBuffer res5 = BufferProvider.Provide();
            res5.WriteCString("Update the Model ID of NPC in Database"); //Length 0x601 // name of the choice
            Router.Send(client, (ushort) AreaPacketId.recv_event_select_push, res5,
                ServerType.Area); // It's the second choice

            IBuffer res11 = BufferProvider.Provide();
            res11.WriteCString("Which Admin function would you like to do?"); // Window Heading / Name
            res11.WriteUInt32(npcSpawn.InstanceId);
            Router.Send(client, (ushort) AreaPacketId.recv_event_select_exec, res11,
                ServerType.Area); // It's the windows that contain the multiple choice
        }

        private void Blacksmith(NecClient client, NpcSpawn npcSpawn)
        {
            if (client.Character.helperTextBlacksmith)
            {
                IBuffer res2 = BufferProvider.Provide();
                res2.WriteCString($"{npcSpawn.Name}"); //Name
                res2.WriteCString($"{npcSpawn.Title}"); //Title (inside chat box)
                res2.WriteCString(
                    "By forging, you can use the same equipment for a long time. The equipment will get more powerful the more you forge. Of course,"); //Text block
                Router.Send(client, (ushort)AreaPacketId.recv_event_message_no_object, res2, ServerType.Area);

                IBuffer res3 = BufferProvider.Provide();
                res3.WriteCString($"{npcSpawn.Name}"); //Name
                res3.WriteCString($"{npcSpawn.Title}"); //Title (inside chat box)
                res3.WriteCString("sometimes the process fails."); //Text block
                Router.Send(client, (ushort)AreaPacketId.recv_event_message_no_object, res3, ServerType.Area);

                IBuffer res6 = BufferProvider.Provide();
                Router.Send(client, (ushort)AreaPacketId.recv_event_sync, res6, ServerType.Area);

                client.Character.helperTextBlacksmith = false;
            }
            else
            {



                IBuffer res = BufferProvider.Provide();
                //recv_shop_notify_open = 0x52FD, // Parent = 0x5243 // Range ID = 02
                res.WriteInt16(
                    0b11111010); //Shop type, 1 = remove curse; 2 = purchase list; 3 = 1 and 2; 4 = sell; 5 = 1 and 4; 6 = 2 and 4; 7 = 1, 2, and 4; 8 = identify; 16 = repair; 32 = special; 64 = meal
                res.WriteUInt32(1);//tabs num
                res.WriteInt32(10800405);// cash
                res.WriteByte(0); //Items num for expected recv_shop_notify_open
                Router.Send(client, (ushort)AreaPacketId.recv_shop_notify_open, res, ServerType.Area);

                IBuffer res5 = BufferProvider.Provide();
                res5.WriteCString($"{npcSpawn.Name} the {npcSpawn.Title}");
                Router.Send(client, (ushort)AreaPacketId.recv_shop_title_push, res5, ServerType.Area);
            }
        }
        private void Jeweler(NecClient client, NpcSpawn npcSpawn)
        {
            if (client.Character.helperTextBlacksmith)
            {
                IBuffer res2 = BufferProvider.Provide();
                res2.WriteCString($"{npcSpawn.Name}"); //Name
                res2.WriteCString($"{npcSpawn.Title}"); //Title (inside chat box)
                res2.WriteCString(
                    "This will be the Jeweler NPC"); //Text block
                Router.Send(client, (ushort)AreaPacketId.recv_event_message_no_object, res2, ServerType.Area);

                IBuffer res3 = BufferProvider.Provide();
                res3.WriteCString($"{npcSpawn.Name}"); //Name
                res3.WriteCString($"{npcSpawn.Title}"); //Title (inside chat box)
                res3.WriteCString("sometimes the process fails."); //Text block
                Router.Send(client, (ushort)AreaPacketId.recv_event_message_no_object, res3, ServerType.Area);

                IBuffer res6 = BufferProvider.Provide();
                Router.Send(client, (ushort)AreaPacketId.recv_event_sync, res6, ServerType.Area);

                client.Character.helperTextBlacksmith = false;
            }
            else
            {
                IBuffer res4 = BufferProvider.Provide();
                //recv_shop_notify_open = 0x52FD, // Parent = 0x5243 // Range ID = 02
                res4.WriteInt16(0b110000000000000); 
                //Shop type, 1 = remove curse; 2 = purchase list; 3 = 1 and 2; 4 = sell; 5 = 1 and 4; 6 = 2 and 4; 7 = 1, 2, and 4; 8 = identify; 16 = repair; 32 = special; 64 = meal
                res4.WriteInt32(10800405);
                res4.WriteInt32(10800405);
                res4.WriteByte(0);
                Router.Send(client, (ushort)AreaPacketId.recv_shop_notify_open, res4, ServerType.Area);

                IBuffer res5 = BufferProvider.Provide();
                res5.WriteCString($"{npcSpawn.Name} the {npcSpawn.Title}");
                Router.Send(client, (ushort)AreaPacketId.recv_shop_title_push, res5, ServerType.Area);
            }
        }

        private void DonkeysItems(NecClient client, NpcSpawn npcSpawn)
        {
            if (client.Character.helperTextDonkey)
            {
                IBuffer res2 = BufferProvider.Provide();
                res2.WriteCString($"{npcSpawn.Name}"); //Name
                res2.WriteCString($"{npcSpawn.Title}"); //Title (inside chat box)
                res2.WriteCString(
                    "Wee! There's plenty of weapons and armor at the specialty shops. The weapon and armor shops are in Bustling Market. *Hiccup*"); //Text block
                Router.Send(client, (ushort)AreaPacketId.recv_event_message_no_object, res2, ServerType.Area);

                IBuffer res6 = BufferProvider.Provide();
                Router.Send(client, (ushort)AreaPacketId.recv_event_sync, res6, ServerType.Area);

                client.Character.helperTextDonkey = false;
            }
            else
            {
                int numItems = 6;
                int[] DonkeyItems = new int[] { 100101, 50100301, 50100302, 50100401, 50100402, 70000301, 100101, 110101, 120101, 200101, 210101, 220101, 300101, 310101, 320101, 400101, 410101, 420101, 500101, 510101, 520101, 10200101, 10300101, 11000101, 11300101, 10210003, 15000101,15300003 };
                int[] DonkeyPrices = new int[] {100,02,100,10,500,500,400,280,350,1100,1000,1000,500,450,450,300,350,250,450,400,450,1450,1500,1400,1550,1000,1500,1500 };
                IBuffer res = BufferProvider.Provide();
                //recv_shop_notify_open = 0x52FD, // Parent = 0x5243 // Range ID = 02
                res.WriteInt16(14); //Shop type, 1 = remove curse; 2 = purchase list; 3 = 1 and 2; 4 = sell; 5 = 1 and 4; 6 = 2 and 4; 7 = 1, 2, and 4; 8 = identify; 14 = purchase, sell, identify; 16 = repair;
                res.WriteInt32(1);
                res.WriteInt32(45);
                res.WriteByte((byte)numItems);
                Router.Send(client, (ushort)AreaPacketId.recv_shop_notify_open, res, ServerType.Area);
                Character character = client.Character;

                for (int i = 0; i < numItems; i++)
                {

                    Item item = Server.Items[DonkeyItems[i]];

                    // Create InventoryItem
                    InventoryItem inventoryItem = new InventoryItem();
                    inventoryItem.Id = i;
                    inventoryItem.Item = item;
                    inventoryItem.ItemId = item.Id;



                    itemStats(inventoryItem, client);


                    res = BufferProvider.Provide();
                    res.WriteByte((byte)i); //idx
                    res.WriteInt32(item.Id); // item Serial id
                    res.WriteInt64(DonkeyPrices[i]); // item price
                    res.WriteFixedString($"{inventoryItem.Item.Name}", 0x10); // ?
                    Router.Send(client, (ushort)AreaPacketId.recv_shop_notify_item, res, ServerType.Area);


                }
                res = BufferProvider.Provide();

                int numEntries3 = 0xA;
                res.WriteInt32(numEntries3);
                for (int j = 0; j < numEntries3; j++)
                {
                    res.WriteInt16((short)j);
                    res.WriteInt64(10200101);
                    res.WriteFixedString("UNKNOWN", 0xC1);
                }

               // Router.Send(client, (ushort)0xBA61, res, ServerType.Area);

                res = BufferProvider.Provide();

                int numEntries4 = 0xA;// <=0xA
                res.WriteInt32(numEntries4); //// <=0xA
                for (int k = 0; k < numEntries4; k++)
                {
                    res.WriteInt16((short)k);
                    res.WriteInt32(10500501);
                }
                //Router.Send(client, (ushort)0x8D62, res, ServerType.Area);

                IBuffer res5 = BufferProvider.Provide();
                res5.WriteCString($"{npcSpawn.Name}'s Goods");
                Router.Send(client, (ushort)AreaPacketId.recv_shop_title_push, res5, ServerType.Area);
            }
        }
        private void AuctionHouse(NecClient client, NpcSpawn npcSpawn)
        {

            IBuffer res = BufferProvider.Provide();
            int numEntries = 0x5;
            res.WriteInt32(numEntries); // must be <= 5    // item information tab
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteByte((byte)i);

                res.WriteInt32(i);
                res.WriteInt64(10500501);
                res.WriteInt32(2);
                res.WriteInt32(3);
                res.WriteFixedString($"{client.Soul.Name}", 49);
                res.WriteByte(1);
                res.WriteFixedString("Item Description", 385);
                res.WriteInt16(4);
                res.WriteInt32(5);

                res.WriteInt32(6);
                res.WriteInt32(7);

            }
            int numEntries2 = 0x8;
            res.WriteInt32(numEntries2); // must be< = 8   //bid info tab

            for (int i = 0; i < numEntries2; i++)
            {
                res.WriteByte((byte)i);

                res.WriteInt32(i);
                res.WriteInt64(10500501);
                res.WriteInt32(2);
                res.WriteInt32(3);
                res.WriteFixedString("soulname", 49);
                res.WriteByte(1);
                res.WriteFixedString("ToBeFound", 385);
                res.WriteInt16(4);
                res.WriteInt32(5);

                res.WriteInt32(6);
                res.WriteInt32(7);

            }

            res.WriteByte(0); // bool
            Router.Send(client, (ushort)AreaPacketId.recv_auction_notify_open, res, ServerType.Area);
            
        }

        private void CloakRoomShopClerk(NecClient client, NpcSpawn npcSpawn)
        {
            if (client.Character.helperTextCloakRoom)
            {
                IBuffer res2 = BufferProvider.Provide();
                res2.WriteCString($"{npcSpawn.Name}"); //Name
                res2.WriteCString($"{npcSpawn.Title}"); //Title (inside chat box)
                res2.WriteCString(
                    "Welcome! We take care of your belongings and money."); //Text block
                Router.Send(client, (ushort) AreaPacketId.recv_event_message_no_object, res2, ServerType.Area);

                IBuffer res6 = BufferProvider.Provide();
                Router.Send(client, (ushort) AreaPacketId.recv_event_sync, res6, ServerType.Area);

                client.Character.helperTextCloakRoom = false;
            }
            else
            {
                IBuffer res4 = BufferProvider.Provide();
                //recv_event_soul_storage_open = 0x3DD0, 
                res4.WriteInt64(client.Soul.WarehouseGold); // Gold in the storage
                Router.Send(client, (ushort) AreaPacketId.recv_event_soul_storage_open, res4, ServerType.Area);
            }
        }

        private void RegularInn(NecClient client, NpcSpawn npcSpawn)
        {
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteCString("While Beginner (Usable until SR 2) : 100 G"); //Length 0x601  // name of the choice 
            Router.Send(client, (ushort) AreaPacketId.recv_event_select_push, res2, ServerType.Area); // 0       

            IBuffer res3 = BufferProvider.Provide();
            res3.WriteCString("Floor : Free!"); //Length 0x601  // name of the choice 
            Router.Send(client, (ushort) AreaPacketId.recv_event_select_push, res3, ServerType.Area); // 1

            IBuffer res4 = BufferProvider.Provide();
            res4.WriteCString("Simple Bed : 60 G"); //Length 0x601 // name of the choice
            Router.Send(client, (ushort) AreaPacketId.recv_event_select_push, res4, ServerType.Area); // 2

            IBuffer res5 = BufferProvider.Provide();
            res5.WriteCString("Economy Room : 300 G"); //Length 0x601 // name of the choice
            Router.Send(client, (ushort) AreaPacketId.recv_event_select_push, res5, ServerType.Area); // 3

            IBuffer res6 = BufferProvider.Provide();
            res6.WriteCString("Suite Room : 1,200 G"); //Length 0x601 // name of the choice
            Router.Send(client, (ushort) AreaPacketId.recv_event_select_push, res6, ServerType.Area); // 4

            IBuffer res7 = BufferProvider.Provide();
            res7.WriteCString("Royal Suite : 3000 G"); //Length 0x601 // name of the choice
            Router.Send(client, (ushort) AreaPacketId.recv_event_select_push, res7, ServerType.Area); // 5

            IBuffer res8 = BufferProvider.Provide();
            res8.WriteCString("Back"); //Length 0x601 // name of the choice
            Router.Send(client, (ushort) AreaPacketId.recv_event_select_push, res8, ServerType.Area); // 6

            IBuffer res9 = BufferProvider.Provide();
            res9.WriteCString("Welcome! Please choose a room to stay in!"); // Window Heading / Name
            res9.WriteUInt32(npcSpawn.InstanceId);
            Router.Send(client, (ushort) AreaPacketId.recv_event_select_exec, res9,
                ServerType.Area); // It's the windows that contain the multiple choice

            /*IBuffer res2 = BufferProvider.Provide();
            res2.WriteCString($"{npcSpawn.Name}");//Name
            res2.WriteCString($"{npcSpawn.Title}");//Title (inside chat box)
            res2.WriteCString("Wee! There's plenty of weapons and armor at the specialty shops. The weapon and armor shops are in Bustling Market. *Hiccup*");//Text block
            Router.Send(client, (ushort)AreaPacketId.recv_event_message_no_object, res2, ServerType.Area);

            //IBuffer res6 = BufferProvider.Provide();
            //Router.Send(client, (ushort)AreaPacketId.recv_event_sync, res6, ServerType.Area);

            IBuffer res4 = BufferProvider.Provide();
            //recv_shop_notify_open = 0x52FD, // Parent = 0x5243 // Range ID = 02
            res4.WriteInt16(14); //Shop type, 1 = remove curse; 2 = purchase list; 3 = 1 and 2; 4 = sell; 5 = 1 and 4; 6 = 2 and 4; 7 = 1, 2, and 4; 8 = identify; 14 = purchase, sell, identify; 16 = repair;
            res4.WriteInt32(0);
            res4.WriteInt32(0);
            res4.WriteByte(0);
            Router.Send(client, (ushort)AreaPacketId.recv_shop_notify_open, res4, ServerType.Area);*/
        }

        private void CrimInn(NecClient client, NpcSpawn npcSpawn)
        {
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteCString("While Beginner (Usable until SR 2) 100 G"); //Length 0x601  // name of the choice 
            Router.Send(client, (ushort) AreaPacketId.recv_event_select_push, res2, ServerType.Area); // 0

            IBuffer res3 = BufferProvider.Provide();
            res3.WriteCString("Pig stable : Free!"); //Length 0x601  // name of the choice 
            Router.Send(client, (ushort) AreaPacketId.recv_event_select_push, res3, ServerType.Area); // 1

            IBuffer res4 = BufferProvider.Provide();
            res4.WriteCString("Storage room : 60 G"); //Length 0x601 // name of the choice
            Router.Send(client, (ushort) AreaPacketId.recv_event_select_push, res4, ServerType.Area); // 2

            IBuffer res5 = BufferProvider.Provide();
            res5.WriteCString("Sleeper : 300 G"); //Length 0x601 // name of the choice
            Router.Send(client, (ushort) AreaPacketId.recv_event_select_push, res5, ServerType.Area); // 3

            IBuffer res6 = BufferProvider.Provide();
            res6.WriteCString("Slum Suite : 10,000 G"); //Length 0x601 // name of the choice
            Router.Send(client, (ushort) AreaPacketId.recv_event_select_push, res6, ServerType.Area); // 4

            IBuffer res8 = BufferProvider.Provide();
            res8.WriteCString("Back"); //Length 0x601 // name of the choice
            Router.Send(client, (ushort) AreaPacketId.recv_event_select_push, res8, ServerType.Area); // 5

            IBuffer res9 = BufferProvider.Provide();
            res9.WriteCString("Welcome! Please choose a room to stay in!"); // Window Heading / Name
            res9.WriteUInt32(npcSpawn.InstanceId);
            Router.Send(client, (ushort) AreaPacketId.recv_event_select_exec, res9,
                ServerType.Area); // It's the windows that contain the multiple choice
        }

        private void LostBBS(NecClient client, NpcSpawn npcSpawn)
        {
            IBuffer res = BufferProvider.Provide();
            //recv_message_board_notify_open = 0x170F, 

            res.WriteInt32(2); //String lookup inside str_table.csv around line 3366

            res.WriteInt16(2);
            res.WriteInt16(4); // Lost souls yesterday.
            res.WriteInt16(6);
            res.WriteInt16(8);

            res.WriteInt16(10);
            res.WriteInt16(12); // Lost souls last month.
            res.WriteInt16(14);
            res.WriteInt16(16);

            Router.Send(client.Map, (ushort) AreaPacketId.recv_message_board_notify_open, res, ServerType.Area);
        }

        private void UnionWindow(NecClient client, NpcSpawn npcSpawn)
        {
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(0);
            //Router.Send(client, (ushort)AreaPacketId.recv_union_request_establish_r, res2, ServerType.Area);
            IBuffer res = BufferProvider.Provide();
            //recv_union_open_window = 0x7D75,
            //no structure
            Router.Send(client, (ushort) AreaPacketId.recv_union_open_window, res, ServerType.Area);
        }

        private void EventChangeChannel(NecClient client, NpcSpawn npcSpawn)
        {
            IBuffer res2 = BufferProvider.Provide();
            //res2.WriteInt32(0); // error check
            //res2.WriteInt32(client.Character.InstanceId); // ??
            //sub_494c50
            res2.WriteInt32(client.Character.MapId); //Stage ID from Stage.CSV
            res2.WriteInt32(client.Character
                .MapId); //Map ID. Cross Refrences Dungeun_info.csv to get X/Y value for map icon, and dungeun description. 
            res2.WriteInt32(partySize[2]); //max players
            res2.WriteInt16(levels[2]);
            //sub_4834C0
            res2.WriteByte(10); //loops to display
            //sub_494B90 - for loop
            for (int i = 0; i < 0x80; i++)
            {
                res2.WriteInt32(i); //Channel ID for passing to Send_Channel_Select
                res2.WriteFixedString($"Channel {i}", 97);
                res2.WriteByte(1); //bool 1 | 0
                res2.WriteInt16(0xF); //Max players
                res2.WriteInt16((short) i); //Current players
                res2.WriteByte(3);
                res2.WriteByte(6); //channel Emoticon - 6 for a Happy Face
                //
            }

            res2.WriteByte(10); //# of channels
            Router.Send(client, (ushort) AreaPacketId.recv_event_select_channel, res2, ServerType.Area);
        }

        private void CharaChangeChannel(NecClient client, NpcSpawn npcSpawn) //Usage TBD. calls up Send_Channel_Select
        {
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(0); // error check
            res2.WriteUInt32(client.Character.InstanceId); // ??
            //sub_494c50
            res2.WriteInt32(client.Character.MapId); //Stage ID from Stage.CSV
            res2.WriteInt32(client.Character
                .MapId); //Map ID. Cross Refrences Dungeun_info.csv to get X/Y value for map icon, and dungeun description. 
            res2.WriteInt32(partySize[2]); //max players
            res2.WriteInt16(levels[2]);
            //sub_4834C0
            res2.WriteByte(10); //loops to display
            //sub_494B90 - for loop
            for (int i = 0; i < 0x80; i++)
            {
                res2.WriteInt32(i); //Channel ID for passing to Send_Channel_Select
                res2.WriteFixedString($"Channel {i}", 97);
                res2.WriteByte(1); //bool 1 | 0
                res2.WriteInt16(0xF); //Max players
                res2.WriteInt16((short) i); //Current players
                res2.WriteByte(3);
                res2.WriteByte(6); //channel Emoticon - 6 for a Happy Face
                //
            }

            res2.WriteByte(10); //# of channels
            Router.Send(client, (ushort) MsgPacketId.recv_chara_select_channel_r, res2, ServerType.Msg);
        }

        private void SoulRankNPC(NecClient client, NpcSpawn npcSpawn)
        {
            IBuffer res1 = BufferProvider.Provide();
            res1.WriteCString($"{npcSpawn.Name}");//need to find max size; Name
            res1.WriteCString($"{npcSpawn.Title}");//need to find max size; Title (inside chat box)
            res1.WriteCString("I'm Calarde, the Maiden of Isic. I look after the souls of you adventurers.");//Text block
            Router.Send(client, (ushort)AreaPacketId.recv_event_message_no_object, res1, ServerType.Area);

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteCString($"{npcSpawn.Name}");//need to find max size; Name
            res2.WriteCString($"{npcSpawn.Title}");//need to find max size; Title (inside chat box)
            res2.WriteCString("Your soul has the ability to shine brighter. I look forward to seeing you in the future.");//Text block
            Router.Send(client, (ushort)AreaPacketId.recv_event_message_no_object, res2, ServerType.Area);

            IBuffer res3 = BufferProvider.Provide();
            res3.WriteCString($"{npcSpawn.Name}");//need to find max size; Name
            res3.WriteCString($"{npcSpawn.Title}");//need to find max size; Title (inside chat box)
            res3.WriteCString("Come back when you've gaind more experience. I'll unlock your soul's power.");//Text block
            Router.Send(client, (ushort)AreaPacketId.recv_event_message_no_object, res3, ServerType.Area);

            IBuffer res4 = BufferProvider.Provide();
            res4.WriteCString("Ask about Souls"); //Length 0x601 // name of the choice
            Router.Send(client, (ushort)AreaPacketId.recv_event_select_push, res4, ServerType.Area); // 1

            IBuffer res5 = BufferProvider.Provide();
            res5.WriteCString("Trade Soul Material"); //Length 0x601 // name of the choice
            Router.Send(client, (ushort)AreaPacketId.recv_event_select_push, res5, ServerType.Area); // 2

            IBuffer res6 = BufferProvider.Provide();
            res6.WriteCString("Increase Soul Amount"); //Length 0x601 // name of the choice
            Router.Send(client, (ushort)AreaPacketId.recv_event_select_push, res6, ServerType.Area); // 3

            IBuffer res7 = BufferProvider.Provide();
            res7.WriteCString("Increase Soul Limit"); //Length 0x601 // name of the choice
            Router.Send(client, (ushort)AreaPacketId.recv_event_select_push, res7, ServerType.Area); // 4

            IBuffer res8 = BufferProvider.Provide();
            res8.WriteCString("Akashic Record"); //Length 0x601 // name of the choice
            Router.Send(client, (ushort)AreaPacketId.recv_event_select_push, res8, ServerType.Area); // 5

            IBuffer res9 = BufferProvider.Provide();
            res9.WriteCString("Leave"); //Length 0x601 // name of the choice
            Router.Send(client, (ushort)AreaPacketId.recv_event_select_push, res9, ServerType.Area); // 6

            IBuffer res10 = BufferProvider.Provide();
            res10.WriteCString("How can I help you?"); // Window Heading / Name
            res10.WriteUInt32(npcSpawn.InstanceId);
            Router.Send(client, (ushort)AreaPacketId.recv_event_select_exec, res10, ServerType.Area); // It's the windows that contain the multiple choice
        }

        private void SpareEventParts(NecClient client, NpcSpawn npcSpawn)
        {
            //Move all this event stuff to an appropriate file/handler for re-use of common code
            ;
            /*
            IBuffer res10 = BufferProvider.Provide();
            res10.WriteCString("The fountain is brimmed with water. Has enough for 3 more drinks.");
            res10.WriteCString("The fountain is brimmed with water. Has enough for 4 more drinks.");
            res10.WriteCString("The fountain is brimmed with water. Has enough for 5 more drinks.");
            Router.Send(client, (ushort)AreaPacketId.recv_event_block_message_no_object, res10, ServerType.Area);
            */
            /*
            IBuffer res7 = BufferProvider.Provide();
            res7.WriteInt32(instanceId);
            res7.WriteCString("The fountain is brimmed with water. Has enough for 5 more drinks.");
            Router.Send(client, (ushort)AreaPacketId.recv_event_block_message, res7, ServerType.Area);
            */

            /*
            IBuffer res9 = BufferProvider.Provide();
            res9.WriteCString($"NPC#:{instanceId}"); // Npc name manually entered
            res9.WriteCString("QuestChat");//Chat Window lable
            res9.WriteCString("You've got 5 seconds before this window closes. Think Quick!'");// it's the npc text, switch automatically to an other window when text finish
            Router.Send(client, (ushort)AreaPacketId.recv_event_message_no_object, res9, ServerType.Area);
            */

            /*
            IBuffer res8 = BufferProvider.Provide();
            res8.WriteInt32(instanceId); // used to pull Name/Title information from the NPC/object being interacted with
            res8.WriteCString("The fountain is brimmed with water. Has enough for 5 more drinks.");
            Router.Send(client, (ushort)AreaPacketId.recv_event_message, res8, ServerType.Area);
            */

            /*
            IBuffer res4 = BufferProvider.Provide();
            res4.WriteCString("The fountain is brimmed with water. Has enough for 5 more drinks.");
            res4.WriteInt32(0);
            res4.WriteInt32(0);
            res4.WriteInt32(0);
            Router.Send(client, (ushort)AreaPacketId.recv_event_select_exec_winpos, res4, ServerType.Area); // It's the windows that contain the multiple choice
            */

            /*
            IBuffer res6 = BufferProvider.Provide();
            res6.WriteInt32(instanceId);
            Router.Send(client, (ushort)AreaPacketId.recv_event_change_type, res6, ServerType.Area);//????
            */

            /*
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(instanceId);
            res2.WriteByte(1); // bool
            Router.Send(client, (ushort)AreaPacketId.recv_event_select_ready, res2, ServerType.Area); //prevents the call to send_event_select_exec_r until you make a selection.
            */
        }


        int[] StageSelect = new int[]
        {
            1 /* Ilfalo Port  */, 100001 /*	Caligrase Sewers	*/, 100002 /*	Kaoka Parrej Ruins	*/,
            100003 /*	Deltis Keep	*/, 100004 /*	Golden Dragon Ruins	*/, 100005 /*	Chikor Castle Site	*/,
            100006 /*	Aria Reservoir	*/, 100007 /*	Temple of Oblivion	*/, 100008 /*	Underground Sewers	*/,
            100009 /*	Descension Ruins	*/, 100010 /*	Roswald Deep Fort	*/, 100011 /*	Azarm Trial Grounds	*/,
            100012 /*	Ruined Chamber	*/, 100013 /*	Facility 13	*/, 100014 /*	Dark Roundtable	*/,
            100015 /*	Sangent Ruins	*/, 100019 /*	Papylium Hanging Gardens	*/, 100020 /*	Azarm Trial Grounds	*/,
            100022 /*	The Labyrinth of Apocrypha	*/, 110001 /*	Dum Spiro Spero	*/, 110002 /*	Trial of Fantasy	*/
        };

        int[] mapIDs = new int[]
        {
            2001001, 2002001, 2001101, 2003101, 2001103, 2002101, 2002102, 2001105, 2003102, 2002104, 2003104, 2004106,
            2004103, 2001014, 2004001
        };

        short[] levels = new short[] {1, 3, 5, 7, 9, 11, 12, 12, 14, 16, 16, 17, 18, 19, 20, 22};
        int[] partySize = new int[] {2, 3, 4, 5, 5, 5, 5, 4, 5, 4, 5, 3, 4, 5, 2, 4};

        int[] nameIdx = new int[]
        {
            100008, 100015, 100001, 100005, 100003, 100002, 100004, 100006, 100007, 100010, 100012, 100013, 100014,
            110002, 100022
        };

        public void itemStats(InventoryItem inventoryItem, NecClient client)
        {
            Server.SettingRepository.ItemLibrary.TryGetValue(inventoryItem.Item.Id, out ItemLibrarySetting itemLibrarySetting);
            if (itemLibrarySetting == null) return;

            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteInt32(itemLibrarySetting.Durability); // MaxDura points
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_maxdur, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteInt32(itemLibrarySetting.Durability - 10); // Durability points
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_durability, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteInt32((int)itemLibrarySetting.Weight * 100); // Weight points
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_weight, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteInt16((short)itemLibrarySetting.PhysicalAttack); // Defense and attack points
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_physics, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteInt16((short)itemLibrarySetting.MagicalAttack); // Magic def and attack Points
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_magic, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteInt32(itemLibrarySetting.SpecialPerformance); // for the moment i don't know what it change
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_enchantid, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteInt16((short)Util.GetRandomNumber(50, 100)); // Shwo GP on certain items
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_ac, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteInt32(1); // for the moment i don't know what it change
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_date_end_protect, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteByte((byte)itemLibrarySetting.Hardness); // Hardness
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_hardness, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteByte(1); //Level requirement
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_level, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteByte(0); //sp Level requirement
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_sp_level, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteInt32(0b000000); // State bitmask
                                      //Router.Send(client, (ushort)AreaPacketId.recv_item_update_state, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id); // id?
            res.WriteInt64(Util.GetRandomNumber(10, 10000)); // price
            res.WriteInt64(Util.GetRandomNumber(10, 100)); // identify
            res.WriteInt64(Util.GetRandomNumber(10, 1000)); // curse?
            res.WriteInt64(Util.GetRandomNumber(10, 500)); // repair?
            Router.Send(client, (ushort)AreaPacketId.recv_shop_notify_item_sell_price, res, ServerType.Area);

        }

    }
}
