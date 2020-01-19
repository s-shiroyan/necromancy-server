using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Necromancy.Server.Packet.Area
{
    public class send_event_select_exec_r : ClientHandler
    {
        public send_event_select_exec_r(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort)AreaPacketId.send_event_select_exec_r;

        public override void Handle(NecClient client, NecPacket packet)
        {
            client.Character.eventSelectExecCode = packet.Data.ReadInt32();
            Logger.Debug($" The packet contents are :{client.Character.eventSelectExecCode}");


            if (client.Character.eventSelectExecCode == -1)
            {
                IBuffer res2 = BufferProvider.Provide();
                res2.WriteByte(0);
                Router.Send(client.Map, (ushort)AreaPacketId.recv_event_end, res2, ServerType.Area);
            }
            else
            {
                //logic to execute different actions based on the event that triggered this select execution.
                 IInstance instance = Server.Instances.GetInstance(client.Character.eventSelectReadyCode);


                switch (instance)
                {
                    case NpcSpawn npcSpawn:
                        client.Map.NpcSpawns.TryGetValue(npcSpawn.InstanceId, out npcSpawn);
                        Logger.Debug($"instanceId : {client.Character.eventSelectReadyCode} |  npcSpawn.Id: {npcSpawn.Id}  |   npcSpawn.NpcId: {npcSpawn.NpcId}");

                        var eventSwitchPerObjectID = new Dictionary<Func<int, bool>, Action>
                        {
                         { x => x == 10000704, () => defaultEvent(client, npcSpawn.NpcId) }, //set to Manaphes in slums for testing.
                         { x => x == 10000012 ,  () => defaultEvent(client, npcSpawn.NpcId) },
                         { x => x == 74000022 ,  () => RecoverySpring(client, npcSpawn.NpcId) },
                         { x => x == 74013071 ,  () => ChangeMap(client, npcSpawn.NpcId) },
                         { x => x == 74013161 ,  () => ChangeMap(client, npcSpawn.NpcId) },
                         { x => x == 74013271 ,  () => ChangeMap(client, npcSpawn.NpcId) },
                         { x => x == 10000002, () => RegularInn(client, npcSpawn.NpcId, npcSpawn) },
                         { x => x == 10000703, () => CrimInn(client, npcSpawn.NpcId, npcSpawn) },
                         {x => x == 16969692, () => ResolveInn(client, npcSpawn.NpcId, npcSpawn) },
                         { x => x < 10 ,    () => Logger.Debug($" Event Object switch for NPC ID {npcSpawn.NpcId} reached") },
                         { x => x < 100 ,    () => Logger.Debug($" Event Object switch for NPC ID {npcSpawn.NpcId} reached") },
                         { x => x < 1000 ,    () => Logger.Debug($" Event Object switch for NPC ID {npcSpawn.NpcId} reached") },
                         { x => x < 10000 ,   () => Logger.Debug($" Event Object switch for NPC ID {npcSpawn.NpcId} reached") },
                         { x => x < 100000 ,  () => Logger.Debug($" Event Object switch for NPC ID {npcSpawn.NpcId} reached") },
                         { x => x < 1000000 ,  () => Logger.Debug($" Event Object switch for NPC ID {npcSpawn.NpcId} reached") },
                         { x => x < 900000100 ,  () => UpdateNPC(client, npcSpawn) }

                        };

                        eventSwitchPerObjectID.First(sw => sw.Key(npcSpawn.NpcId)).Value();



                        break;
                    case MonsterSpawn monsterSpawn:
                        Logger.Debug($"MonsterId: {monsterSpawn.Id}");
                        break;
                    default:
                        Logger.Error($"Instance with InstanceId: {client.Character.eventSelectReadyCode} does not exist");
                        RecvEventEnd(client);
                        break;
                }

            }
        }
        private void RecvEventEnd(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            Router.Send(client, (ushort)AreaPacketId.recv_event_show_board_end, res, ServerType.Area);
            Task.Delay(TimeSpan.FromMilliseconds((int)(2 * 1000))).ContinueWith
           (t1 =>
               {
                IBuffer res = BufferProvider.Provide();
                res.WriteByte(0);
                Router.Send(client.Map, (ushort)AreaPacketId.recv_event_end, res, ServerType.Area);
               }
           );

        }
        private void SendEventEnd(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            Router.Send(client, (ushort)AreaPacketId.recv_event_end, res, ServerType.Area);
        }

        private void RecoverySpring(NecClient client, int objectID)
        {
            if (client.Character.eventSelectExecCode == 0)
            {

                if ((client.Character.currentHp == client.Character.maxHp) && (client.Character.currentMp == client.Character.maxMp))
                {
                    IBuffer res12 = BufferProvider.Provide();
                    res12.WriteCString("You try drinking the water but it doesn't seem to have an effect."); // Length 0xC01
                    Router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res12, ServerType.Area);// show system message on middle of the screen.
                }
                else
                {
                    IBuffer res12 = BufferProvider.Provide();
                    res12.WriteCString("You drink The water and it replenishes you"); // Length 0xC01
                    Router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res12, ServerType.Area);// show system message on middle of the screen.

                    IBuffer res7 = BufferProvider.Provide();
                    res7.WriteInt32((client.Character.maxHp)); //To-Do : Math for Max gain of 50% MaxHp
                    Router.Send(client, (ushort)AreaPacketId.recv_chara_update_hp, res7, ServerType.Area);
                    client.Character.currentHp = client.Character.maxHp;

                    IBuffer res9 = BufferProvider.Provide();
                    res9.WriteInt32(client.Character.maxMp); //To-Do : Math for Max gain of 50% MaxMp
                    Router.Send(client, (ushort)AreaPacketId.recv_chara_update_mp, res9, ServerType.Area);
                    client.Character.currentMp = client.Character.maxMp;

                }


            }
            else if (client.Character.eventSelectExecCode == 1)
            {

                IBuffer res12 = BufferProvider.Provide();
                res12.WriteCString("You Say no to random Dungeun water"); // Length 0xC01
                Router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res12, ServerType.Area);// show system message on middle of the screen.
            }

            IBuffer res13 = BufferProvider.Provide();
            res13.WriteCString("To raise your level, you need 1337 more exp."); // Length 0xC01
            Router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res13, ServerType.Area);// show system message on middle of the screen.

            RecvEventEnd(client); //End The Event 
        }
        private void ChangeMap(NecClient client, int objectID)
        {
            Map map = null;
            switch (objectID)
            {
                case 74013071:
                    Logger.Debug($"objectId[{objectID}] selected {client.Character.eventSelectExecCode}");
                    if (client.Character.eventSelectExecCode == 0)
                    {
                        map = Server.Maps.Get(2002105);
                    }
                    else if (client.Character.eventSelectExecCode == 1)
                    {
                        map = Server.Maps.Get(2002106);
                    }
                    break;
                case 74013161:
                    Logger.Debug($"objectId[{objectID}] selected {client.Character.eventSelectExecCode}");
                    if (client.Character.eventSelectExecCode == 0)
                    {
                        map = Server.Maps.Get(2002104);
                    }
                    else if (client.Character.eventSelectExecCode == 1)
                    {
                        map = Server.Maps.Get(2002106);
                    }
                    break;
                case 74013271:
                    Logger.Debug($"objectId[{objectID}] selected {client.Character.eventSelectExecCode}");
                    if (client.Character.eventSelectExecCode == 0)
                    {
                        map = Server.Maps.Get(2002104);
                    }
                    else if (client.Character.eventSelectExecCode == 1)
                    {
                        map = Server.Maps.Get(2002105);
                    }
                    break;
                default:
                    return;
            }
            map.EnterForce(client);
            SendEventEnd(client);
        }

        private void defaultEvent(NecClient client, int objectID)
        {
            RecvEventEnd(client);
        }

        private void UpdateNPC(NecClient client, NpcSpawn npcSpawn)
        {
            if (client.Character.eventSelectExecCode == 0)
            {

                npcSpawn.Heading = (byte)(client.Character.Heading + 90);
                npcSpawn.Heading = (byte)(npcSpawn.Heading % 180);
                if(npcSpawn.Heading < 0)
                {
                    npcSpawn.Heading += 180;

                }
                    npcSpawn.Updated = DateTime.Now;


                if (!Server.Database.UpdateNpcSpawn(npcSpawn))
                {
                    IBuffer res12 = BufferProvider.Provide();
                    res12.WriteCString("Could not update the database"); // Length 0xC01
                    Router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res12, ServerType.Area);// show system message on middle of the screen.
                    return;
                }


            }
            else if (client.Character.eventSelectExecCode == 1)
            {

                IBuffer res12 = BufferProvider.Provide();
                res12.WriteCString("This Feature is under development"); // Length 0xC01
                Router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res12, ServerType.Area);// show system message on middle of the screen.
            }

            IBuffer res13 = BufferProvider.Provide();
            res13.WriteCString("NPC Updated"); // Length 0xC01
            Router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res13, ServerType.Area);// show system message on middle of the screen.

            RecvEventEnd(client); //End The Event 
        }

        private void RegularInn(NecClient client, int objectID, NpcSpawn npcSpawn)
        {
            if(client.Character.eventSelectExecCode == 0)
            {
                IBuffer res7 = BufferProvider.Provide();
                res7.WriteCString("Stay"); //Length 0x601 // name of the choice
                Router.Send(client, (ushort)AreaPacketId.recv_event_select_push, res7, ServerType.Area); // It's the fifth choice

                IBuffer res8 = BufferProvider.Provide();
                res8.WriteCString("Back"); //Length 0x601 // name of the choice
                Router.Send(client, (ushort)AreaPacketId.recv_event_select_push, res8, ServerType.Area); // It's the sixth choice

                IBuffer res9 = BufferProvider.Provide();
                res9.WriteCString("Effect: Recover all HP, all MP, and Beginner Condition"); // Window Heading / Name
                res9.WriteInt32(0);
                Router.Send(client, (ushort)AreaPacketId.recv_event_select_exec, res9, ServerType.Area); // It's the windows that contain the multiple choice
                
                client.Character.eventSelectExecCode = 16969692;
            }
            else if (client.Character.eventSelectExecCode == 1)
            {
                IBuffer res7 = BufferProvider.Provide();
                res7.WriteCString("Stay"); //Length 0x601 // name of the choice
                Router.Send(client, (ushort)AreaPacketId.recv_event_select_push, res7, ServerType.Area); // It's the fifth choice

                IBuffer res8 = BufferProvider.Provide();
                res8.WriteCString("Back"); //Length 0x601 // name of the choice
                Router.Send(client, (ushort)AreaPacketId.recv_event_select_push, res8, ServerType.Area); // It's the sixth choice

                IBuffer res9 = BufferProvider.Provide();
                res9.WriteCString("Effect: Recover all HP, all MP, and 1 Condition"); // Window Heading / Name
                res9.WriteInt32(0);
                Router.Send(client, (ushort)AreaPacketId.recv_event_select_exec, res9, ServerType.Area); // It's the windows that contain the multiple choice
                
                client.Character.eventSelectExecCode = 16969692;
            }
            else if (client.Character.eventSelectExecCode == 2)
            {
                IBuffer res7 = BufferProvider.Provide();
                res7.WriteCString("Stay"); //Length 0x601 // name of the choice
                Router.Send(client, (ushort)AreaPacketId.recv_event_select_push, res7, ServerType.Area); // It's the fifth choice

                IBuffer res8 = BufferProvider.Provide();
                res8.WriteCString("Back"); //Length 0x601 // name of the choice
                Router.Send(client, (ushort)AreaPacketId.recv_event_select_push, res8, ServerType.Area); // It's the sixth choice

                IBuffer res9 = BufferProvider.Provide();
                res9.WriteCString("Effect: Recover all HP, all MP, and 2 Condition"); // Window Heading / Name
                res9.WriteInt32(0);
                Router.Send(client, (ushort)AreaPacketId.recv_event_select_exec, res9, ServerType.Area); // It's the windows that contain the multiple choice

                client.Character.eventSelectExecCode = 16969692;
                npcSpawn.NpcId = 16969692;
            }
            else if (client.Character.eventSelectExecCode == 3)
            {
                IBuffer res7 = BufferProvider.Provide();
                res7.WriteCString("Stay"); //Length 0x601 // name of the choice
                Router.Send(client, (ushort)AreaPacketId.recv_event_select_push, res7, ServerType.Area); // It's the fifth choice

                IBuffer res8 = BufferProvider.Provide();
                res8.WriteCString("Back"); //Length 0x601 // name of the choice
                Router.Send(client, (ushort)AreaPacketId.recv_event_select_push, res8, ServerType.Area); // It's the sixth choice

                IBuffer res9 = BufferProvider.Provide();
                res9.WriteCString("Effect: Recover all HP, all MP, and 3 Condition"); // Window Heading / Name
                res9.WriteInt32(0);
                Router.Send(client, (ushort)AreaPacketId.recv_event_select_exec, res9, ServerType.Area); // It's the windows that contain the multiple choice

                client.Character.eventSelectExecCode = 16969692;
            }
            else if (client.Character.eventSelectExecCode == 4)
            {
                IBuffer res7 = BufferProvider.Provide();
                res7.WriteCString("Stay"); //Length 0x601 // name of the choice
                Router.Send(client, (ushort)AreaPacketId.recv_event_select_push, res7, ServerType.Area); // It's the fifth choice

                IBuffer res8 = BufferProvider.Provide();
                res8.WriteCString("Back"); //Length 0x601 // name of the choice
                Router.Send(client, (ushort)AreaPacketId.recv_event_select_push, res8, ServerType.Area); // It's the sixth choice

                IBuffer res9 = BufferProvider.Provide();
                res9.WriteCString("Effect: Recover all HP, all MP, and 4 Condition"); // Window Heading / Name
                res9.WriteInt32(0);
                Router.Send(client, (ushort)AreaPacketId.recv_event_select_exec, res9, ServerType.Area); // It's the windows that contain the multiple choice

                client.Character.eventSelectExecCode = 16969692;
            }
            else if (client.Character.eventSelectExecCode == 5)
            {
                IBuffer res7 = BufferProvider.Provide();
                res7.WriteCString("Stay"); //Length 0x601 // name of the choice
                Router.Send(client, (ushort)AreaPacketId.recv_event_select_push, res7, ServerType.Area); // It's the fifth choice

                IBuffer res8 = BufferProvider.Provide();
                res8.WriteCString("Back"); //Length 0x601 // name of the choice
                Router.Send(client, (ushort)AreaPacketId.recv_event_select_push, res8, ServerType.Area); // It's the sixth choice

                IBuffer res9 = BufferProvider.Provide();
                res9.WriteCString("Effect: Recover all HP, all MP, and 5 Condition"); // Window Heading / Name
                res9.WriteInt32(0);
                Router.Send(client, (ushort)AreaPacketId.recv_event_select_exec, res9, ServerType.Area); // It's the windows that contain the multiple choice

                client.Character.eventSelectExecCode = 16969692;
            }
            else if(client.Character.eventSelectExecCode == 6)
            {
                SendEventEnd(client);
            }
        }
        private void CrimInn(NecClient client, int objectID, NpcSpawn npcSpawn)
        {
            
        }

        private void ResolveInn(NecClient client, int objectId, NpcSpawn npcSpawn)
        {
            if (client.Character.eventSelectExecCode == 0)
            {

                SendEventEnd(client);
                //we need the sleep script to fire here
                //this is a placeholder for now until we find how to do so
                npcSpawn.NpcId = 10000002;
            }
            else if (client.Character.eventSelectExecCode == 1)
            {
                SendEventEnd(client);
            }
        }

    }
}
