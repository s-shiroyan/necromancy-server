using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System.Threading;
using System;
using System.Threading.Tasks;

namespace Necromancy.Server.Packet.Area
{
    public class send_event_access_object : ClientHandler
    {
        public send_event_access_object(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_event_access_object;
        private byte byteIterator = 0;
        public override void Handle(NecClient client, NecPacket packet)
        {
            int objectID = packet.Data.ReadInt32();
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(objectID);
            Logger.Debug($"Packet int32 Contents in Decimal: {objectID} ");

            Router.Send(client, (ushort) AreaPacketId.recv_event_access_object_r, res, ServerType.Area);

            SentEventStart(client, objectID);
            //SendEventShowBoardStart(client, objectID);
            //SendEventMessageNoObject(client, objectID);
            Task.Delay(TimeSpan.FromMilliseconds((int)(1 * 1000))).ContinueWith 
            (t1 =>
                {
                   // SendEventShowBoardEnd(client, objectID);
                    SendEventEnd(client);
                }
            );
            
            //MapAndChannel(client, objectID);
            

        }
        private void SentEventStart(NecClient client, int obkectID)
        {
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32((int)byteIterator); // 1 = cinematic
            res2.WriteByte(0);
            Logger.Debug($"Testing Byte Iteration {byteIterator}");
            byteIterator++;

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

        private void MapAndChannel(NecClient client, int objectID)
        {
            
            ///////Giant Annoying Map Loading Recv that disconnects immediatly after
            IBuffer res7 = BufferProvider.Provide();

            int numEntries = 0x4; //Max of 0x20 : cmp ebx,20 
            res7.WriteInt32(numEntries);
            for (int i = 0; i < numEntries; i++)  
            {
                //sub_494c50
                res7.WriteInt32(99999999); //Unknown.  trying for clues from 'send_chara_select'  It uses the same Subs, and structure
                res7.WriteInt32(99999999);
                res7.WriteInt32(99999999);
                res7.WriteInt16(60001);
                    //sub_4834C0
                    res7.WriteByte(9);
                    for (int j = 0; j < 0x80; j++) //j max 0x80
                    {
                        res7.WriteInt32(1001007);
                        res7.WriteFixedString($"Loop{i}:{j}", 0x61);  //Channel Names.  Variables let you know what Loop Iteration you're on
                        res7.WriteByte(1); //bool 1 | 0
                        res7.WriteInt16(0xFFFF); //Max players  -  Comment from other recv
                        res7.WriteInt16(0xFF); //Current players  - Comment from other recv
                        res7.WriteByte(5);
                        res7.WriteByte(5);
                    }
                    res7.WriteByte(5); //Number or Channels  - comment from other recv
            }
            
            Router.Send(client.Map, (ushort)AreaPacketId.recv_event_select_map_and_channel, res7, ServerType.Area);

            ///////////////////////end  of problem


            /*
            IBuffer res6 = BufferProvider.Provide();
            res6.WriteInt32(1);
            Router.Send(client, (ushort)AreaPacketId.recv_event_change_type, res6, ServerType.Area); // It's the first choice


            IBuffer res3 = BufferProvider.Provide();
            res3.WriteCString("Hmmm.. Okay ?"); //Length 0x601  // name of the choice 
            Router.Send(client, (ushort)AreaPacketId.recv_event_select_push, res3, ServerType.Area); // It's the first choice


            IBuffer res5 = BufferProvider.Provide();
            res5.WriteCString("No, talk to me better"); //Length 0x601 // name of the choice
            Router.Send(client, (ushort)AreaPacketId.recv_event_select_push, res5, ServerType.Area); // It's the second choice

    
            IBuffer res4 = BufferProvider.Provide();

            res4.WriteCString("Hey you i need your help, help me now !"); // Window Heading / Name
            res4.WriteInt32(0);

            Router.Send(client, (ushort)AreaPacketId.recv_event_select_exec, res4, ServerType.Area); // It's the windows that contain the multiple choice


            
            */
        }


    }
}
