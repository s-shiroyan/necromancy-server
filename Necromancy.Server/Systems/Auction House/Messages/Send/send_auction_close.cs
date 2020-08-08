using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Systems.Auction_House
{
    public class send_auction_close : ClientHandler
    {
        public send_auction_close(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_auction_close;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_auction_close_r, res, ServerType.Area);
            SendAuctionNotifyClose(client);
        }

        private void SendAuctionNotifyClose(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            Router.Send(client.Map, (ushort) AreaPacketId.recv_auction_notify_close, res, ServerType.Area, client);
        }
    }
}
