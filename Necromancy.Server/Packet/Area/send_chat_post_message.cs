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

        public override ushort Id => (ushort) AreaPacketId.send_chat_post_message;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int ChatType = packet.Data.ReadInt32();     // 0 - Area, 1 - Shout, other todo...
            string To = packet.Data.ReadCString();
            string Message = packet.Data.ReadCString();

            
      
            SendChatNotifyMessage(client, Message, ChatType);

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_chat_post_message_r, res);
            Console.WriteLine("Chat Type: " + ChatType + "\nTo: " + To + "\nMessage: " + Message);
        }

        private void SendChatNotifyMessage(NecClient client, string Message, int ChatType)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(ChatType);
            res.WriteInt32(1);      // todo, maybe, character id
            res.WriteFixedString("Soulname", 49);
            res.WriteFixedString("Charaname", 37);
            res.WriteFixedString($"{Message}", 769);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_chat_notify_message, res);
            /*
            for (int i = 0; i < 9; i++)
            {
                IBuffer res = BufferProvider.Provide();

                res.WriteInt32(i);
                res.WriteInt32(1);      // todo, maybe, character id
                res.WriteFixedString("Soulname", 49);
                res.WriteFixedString("Charaname", 37);
                res.WriteFixedString($"Chat Type: " + i, 769);
                Router.Send(client, (ushort)AreaPacketId.recv_chat_notify_message, res);
                System.Threading.Thread.Sleep(5000);
            }
            */
        }
    }
}