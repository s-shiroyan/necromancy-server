using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Systems.Auction_House
{
    public class send_auction_search : ClientHandler
    {
        public send_auction_search(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_auction_search;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // error

            int NUMBER_OF_ITEMS_DEBUG = 2;
            res.WriteInt32(NUMBER_OF_ITEMS_DEBUG); // number of loops

            for (int i = 0; i < NUMBER_OF_ITEMS_DEBUG; i++)
            {
                string hellothere = (i + i + 1).ToString();
                res.WriteInt32(i); // row identifier
                res.WriteInt64((long)10500501); // 1 = add, 2 blue icons, what is this ?
                res.WriteInt32(10); // Lowest
                res.WriteInt32(500); // Buy Now
                res.WriteFixedString(hellothere, 49); // Soul Name Of Sellers
                res.WriteByte(63); // 0 = nothing.    Other = Logo appear. maybe it's effect or rank, or somethiung else ?
                res.WriteFixedString("0 000000000 1111111111 2222222222 3333333333 44444444444 5555555555 6666666666 7777777777 88888888888 9999999999 1 000000000 1111111111 2222222222 3333333333 44444444444 5555555555 6666666666 7777777777 88888888888 9999999999 2 000000000 1111111111 2222222222 3333333333 44444444444 5555555555 6666666666 7777777777 88888888888 9999999999 3 000000000 1111111111 2222222222", 385); // Item Comment
                res.WriteInt16(0); // Bid
                res.WriteInt32(1000); // Item remaining time
            }

            Router.Send(client.Map, (ushort) AreaPacketId.recv_auction_search_r, res, ServerType.Area);
        }
    }
}
