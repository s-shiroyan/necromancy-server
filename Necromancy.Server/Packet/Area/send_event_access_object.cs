using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_event_access_object : Handler
    {
        public send_event_access_object(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_event_access_object;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(10000121);
            Router.Send(client, (ushort) AreaPacketId.recv_event_access_object_r, res);

            //SendEventMessage(client);
            SendEventBlockMessage(client);
        }

        private void SendEventMessage(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(10000121);
            res.WriteCString("Hello world.");
            Router.Send(client, (ushort)AreaPacketId.recv_event_message, res);
        }

        private void SendEventBlockMessage(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(10000121);
            res.WriteCString("Hello world.");
            Router.Send(client, (ushort)AreaPacketId.recv_event_block_message, res);
        }
    }
}