using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Necromancy.Server.Packet.Area
{
    public class send_event_access_object : ClientHandler
    {
        public send_event_access_object(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_event_access_object;
        public override void Handle(NecClient client, NecPacket packet)
        {
            int objectID = packet.Data.ReadInt32();
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(objectID);
            Logger.Debug($"Packet int32 Contents in Decimal: {objectID} ");
            Router.Send(client, (ushort) AreaPacketId.recv_event_access_object_r, res, ServerType.Area);


            //Begin Event for all cases
            SentEventStart(client, objectID);
            client.Character.eventSelectReadyCode = objectID;

            var eventSwitchPerObjectID = new Dictionary<Func<int, bool>, Action>
            {
             { x => x < 10 ,    () => Logger.Debug($" Event Object switch for NPC ID {objectID} reached") },  
             { x => x < 100 ,    () => Logger.Debug($" Event Object switch for NPC ID {objectID} reached") },
             { x => x < 1000 ,    () => Logger.Debug($" Event Object switch for NPC ID {objectID} reached") },
             { x => x < 10000 ,   () => Logger.Debug($" Event Object switch for NPC ID {objectID} reached") },
             { x => x < 100000 ,  () => Logger.Debug($" Event Object switch for NPC ID {objectID} reached") },
             { x => x < 1000000 ,  () => Logger.Debug($" Event Object switch for NPC ID {objectID} reached") },
             { x => x < 90000010 ,  () => defaultEvent(client, objectID) },
             { x => x < 90009000 ,  () => RecoverySpring(client, objectID)        },
             
            };

            eventSwitchPerObjectID.First(sw => sw.Key(objectID)).Value();




        }
        private void SentEventStart(NecClient client, int obkectID)
        {
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(0); // 0 = normal 1 = cinematic
            res2.WriteByte(0);

            Router.Send(client, (ushort) AreaPacketId.recv_event_start, res2, ServerType.Area);
            // it's the event than permit to that all the code under
            // dont forget tu put a recv_event_end, at the end, if you don't want to get stuck, and do nothing.
        }

        private void SendEventShowBoardStart(NecClient client,int objectID)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString("I Hate Events"); // find max size
            res.WriteInt32(0);
            Router.Send(client, (ushort)AreaPacketId.recv_event_show_board_start, res, ServerType.Area);
        }
        private void SendEventShowBoardEnd(NecClient client,int objectID)
        {
            IBuffer res = BufferProvider.Provide();
            Router.Send(client, (ushort)AreaPacketId.recv_event_show_board_end, res, ServerType.Area);
        }
        private void SendEventEnd(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            Router.Send(client, (ushort)AreaPacketId.recv_event_end, res, ServerType.Area);

        }

        private void SendEventMessageNoObject(NecClient client,int objectID)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString($"NPC#:{objectID}"); // Npc name
            res.WriteCString("QuestChat");//Chat Window lable
            res.WriteCString("You've got 5 seconds before this window closes. Think Quick!'");// it's the npc text, switch automatically to an other window when text finish
            Router.Send(client, (ushort)AreaPacketId.recv_event_message_no_object, res, ServerType.Area);
        }

        private void SendEventMessage(NecClient client,int objectID)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(1);
            res.WriteCString("Hello world.");
            Router.Send(client, (ushort)AreaPacketId.recv_event_message, res, ServerType.Area);
        }

        private void SendEventBlockMessage(NecClient client, int objectID)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(objectID);
            res.WriteCString("Hello world.");
            Router.Send(client, (ushort)AreaPacketId.recv_event_block_message, res, ServerType.Area);
        }

        private void SendEventSelectMapAndChannel(NecClient client, int objectID)
        {
            
            IBuffer res7 = BufferProvider.Provide();

            int numEntries = mapIDs.Length; ; //Max of 0x20 : cmp ebx,20 
            res7.WriteInt32(numEntries);
            for (int i = 0; i < numEntries; i++)  
            {
                //sub_494c50
                res7.WriteInt32(nameIdx[i]); //Stage ID from Stage.CSV
                res7.WriteInt32(mapIDs[i]); //Map ID.  Cross Refrences Dungeun_info.csv to get X/Y value for map icon, and dungeun description. 
                res7.WriteInt32(partySize[i]); //max players
                res7.WriteInt16(levels[i]); ; //Recommended Level
                    //sub_4834C0
                    res7.WriteByte(19);
                    for (int j = 0; j < 0x80; j++) //j max 0x80
                    {
                        res7.WriteInt32(mapIDs[i]);
                        res7.WriteFixedString($"Channel-{i}:{j}", 0x61);  //Channel Names.  Variables let you know what Loop Iteration you're on
                        res7.WriteByte(1); //bool 1 | 0
                        res7.WriteInt16(0xFFFF); //Max players  -  Comment from other recv
                        res7.WriteInt16(7); //Current players  - Comment from other recv
                        res7.WriteByte(3); 
                        res7.WriteByte(6); //channel Emoticon - 6 for a Happy Face
                }
                    res7.WriteByte(6); //Number or Channels  - comment from other recv
            }
            
            Router.Send(client.Map, (ushort)AreaPacketId.recv_event_select_map_and_channel, res7, ServerType.Area);
        }
        private void defaultEvent(NecClient client, int objectID)
        {
            SendEventShowBoardStart(client, objectID);
            //SendEventMessageNoObject(client, objectID);
            //SendEventMessage(client, objectID);
            //SendEventBlockMessage(client, objectID);
            SendEventSelectMapAndChannel(client, objectID);

            Task.Delay(TimeSpan.FromMilliseconds((int)(15 * 1000))).ContinueWith
            (t1 =>
            {
                SendEventShowBoardEnd(client, objectID);
                        //SendEventEnd(client);
                    }
            );
        }
        private void RecoverySpring(NecClient client, int objectID)
            {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString("Fountain Of Healing"); // Title at top of Window
            res.WriteInt32(objectID); //should pull name of NPC,  doesnt currently
            Router.Send(client, (ushort)AreaPacketId.recv_event_show_board_start, res, ServerType.Area);
            /*
            IBuffer res10 = BufferProvider.Provide();
            res10.WriteCString("The fountain is brimmed with water. Has enough for 3 more drinks.");
            res10.WriteCString("The fountain is brimmed with water. Has enough for 4 more drinks.");
            res10.WriteCString("The fountain is brimmed with water. Has enough for 5 more drinks.");
            Router.Send(client, (ushort)AreaPacketId.recv_event_block_message_no_object, res10, ServerType.Area);
            */
            /*
            IBuffer res7 = BufferProvider.Provide();
            res7.WriteInt32(objectID);
            res7.WriteCString("The fountain is brimmed with water. Has enough for 5 more drinks.");
            Router.Send(client, (ushort)AreaPacketId.recv_event_block_message, res7, ServerType.Area);
            */
 
            /*
            IBuffer res9 = BufferProvider.Provide();
            res9.WriteCString($"NPC#:{objectID}"); // Npc name manually entered
            res9.WriteCString("QuestChat");//Chat Window lable
            res9.WriteCString("You've got 5 seconds before this window closes. Think Quick!'");// it's the npc text, switch automatically to an other window when text finish
            Router.Send(client, (ushort)AreaPacketId.recv_event_message_no_object, res9, ServerType.Area);
            */

            /*
            IBuffer res8 = BufferProvider.Provide();
            res8.WriteInt32(objectID); // used to pull Name/Title information from the NPC/object being interacted with
            res8.WriteCString("The fountain is brimmed with water. Has enough for 5 more drinks.");
            Router.Send(client, (ushort)AreaPacketId.recv_event_message, res8, ServerType.Area);
            */

            IBuffer res12 = BufferProvider.Provide();
            res12.WriteCString("The fountain is brimmed with water. Has enough for 5 more drinks."); // Length 0xC01
            Router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res12, ServerType.Area);// show system message on middle of the screen.

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
            res6.WriteInt32(objectID);
            Router.Send(client, (ushort)AreaPacketId.recv_event_change_type, res6, ServerType.Area);//????
            */

            IBuffer res3 = BufferProvider.Provide();
            res3.WriteCString("Drink"); //Length 0x601  // name of the choice 
            Router.Send(client, (ushort)AreaPacketId.recv_event_select_push, res3, ServerType.Area); // It's the first choice

            IBuffer res5 = BufferProvider.Provide();
            res5.WriteCString("Don't drink"); //Length 0x601 // name of the choice
            Router.Send(client, (ushort)AreaPacketId.recv_event_select_push, res5, ServerType.Area); // It's the second choice

            IBuffer res11 = BufferProvider.Provide();
            res11.WriteCString("Effect: Recover 50% of maximum HP and MP"); // Window Heading / Name
            res11.WriteInt32(objectID);
            Router.Send(client, (ushort)AreaPacketId.recv_event_select_exec, res11, ServerType.Area); // It's the windows that contain the multiple choice

            /*
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(objectID);
            res2.WriteByte(1); // bool
            Router.Send(client, (ushort)AreaPacketId.recv_event_select_ready, res2, ServerType.Area); //prevents the call to send_event_select_exec_r until you make a selection.
            */


        }


        int[] StageSelect = new int[]
        {
                     1  /* Ilfalo Port  */
            ,   100001	/*	Caligrase Sewers	*/
            ,   100002	/*	Kaoka Parrej Ruins	*/
            ,   100003	/*	Deltis Keep	*/
            ,   100004	/*	Golden Dragon Ruins	*/
            ,   100005	/*	Chikor Castle Site	*/
            ,   100006	/*	Aria Reservoir	*/
            ,   100007	/*	Temple of Oblivion	*/
            ,   100008	/*	Underground Sewers	*/
            ,   100009	/*	Descension Ruins	*/
            ,   100010	/*	Roswald Deep Fort	*/
            ,   100011	/*	Azarm Trial Grounds	*/
            ,   100012	/*	Ruined Chamber	*/
            ,   100013	/*	Facility 13	*/
            ,   100014	/*	Dark Roundtable	*/
            ,   100015	/*	Sangent Ruins	*/
            ,   100019	/*	Papylium Hanging Gardens	*/
            ,   100020	/*	Azarm Trial Grounds	*/
            ,   100022	/*	The Labyrinth of Apocrypha	*/
            ,   110001	/*	Dum Spiro Spero	*/
            ,   110002	/*	Trial of Fantasy	*/
        };
        int[] mapIDs = new int[] { 2001001, 2002001, 2001101, 2003101, 2001103, 2002101, 2002102, 2001105, 2003102, 2002104, 2003104, 2004106, 2004103, 2001014, 2004001 };
        short[] levels = new short[] { 1, 3, 5, 7, 9, 11, 12, 12, 14, 16, 16, 17, 18, 19, 20, 22 };
        int[] partySize = new int[] { 2, 3, 4, 5, 5, 5, 5, 4, 5, 4, 5, 3, 4, 5, 2, 4 };
        int[] nameIdx = new int[] { 100008, 100015, 100001, 100005, 100003, 100002, 100004, 100006, 100007, 100010, 100012, 100013, 100014, 110002, 100022 };

    }
}
