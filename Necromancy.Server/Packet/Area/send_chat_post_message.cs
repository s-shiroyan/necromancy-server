using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

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
            uint unknown = packet.Data.ReadUInt32();
            string text = packet.Data.ReadCString();

            SendChatNotifyMessage(client, text);

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(1);
            //Router.Send(client, (ushort) AreaPacketId.recv_chat_post_message_r, res);
        }

        private void SendChatNotifyMessage(NecClient client, string chatText)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteFixedString("Soulname", 49);
            res.WriteFixedString("Charaname", 37);
            res.WriteFixedString($"{chatText}", 769);
            Router.Send(client, (ushort)AreaPacketId.recv_chat_notify_message, res);
        }
    }
}