using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;
using System;
using System.Text;

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

            int NUMBER_OF_ITEMS_DEBUG = 64;
            res.WriteInt32(NUMBER_OF_ITEMS_DEBUG); // number of loops

            for (int i = 0; i < NUMBER_OF_ITEMS_DEBUG; i++)
            {
                string hellothere = i.ToString() + " " + Convert.ToString(i, 2).PadLeft(8, '0'); ;
                //START ROW IDENTIFIER
                res.WriteByte((byte)i);                
                res.WriteByte((byte) 255); //seemingly ignored
                res.WriteByte((byte) 255); //seemingly ignored               
                res.WriteByte((byte) 255); //seemingly ignored

                //START UNKNOWN
                //res.WriteByte((byte)0); 
                //res.WriteByte((byte)0);
                //res.WriteByte((byte)255);
                //res.WriteByte((byte)255);
                //res.WriteByte((byte)0);
                //res.WriteByte((byte)0);
                //res.WriteByte((byte)0);
                //res.WriteByte((byte)0);

                res.WriteInt64(i + 64); // 1 = add, 2 blue icons, what is this ? probably serial id | test: 10500501, 100114
                res.WriteInt32(17); // Lowest
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
