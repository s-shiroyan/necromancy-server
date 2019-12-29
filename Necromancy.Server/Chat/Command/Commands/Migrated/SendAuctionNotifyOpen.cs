using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    //opens auction house
    public class SendAuctionNotifyOpen : ServerChatCommand
    {
        public SendAuctionNotifyOpen(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(5); // must be <= 5

            int numEntries = 0x5;
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteByte((byte)i); // Slots 0 to 2
                // res.WriteInt64(0xAAAAAAAA); // this is shown in the structure but for some reason isnt read

                res.WriteInt32(0);
                res.WriteInt64(0);
                res.WriteInt32(0); // Lowest
                res.WriteInt32(0); // Buy Now
                res.WriteFixedString($"{client.Soul.Name}", 49);
                res.WriteByte(1); // 1 permit to show item in the search section ??
                res.WriteFixedString("It's a bit weird, what is this ?!", 385); // Comment in the item information
                res.WriteInt16(0); // Bid
                res.WriteInt32(0);

                res.WriteInt32(5); // Bid Price.
                res.WriteInt32(1); /*

                0. Remove item
                1. Bid accepted (Reveice button appear)

                */
            }

            res.WriteInt32(8); // must be< = 8

            int numEntries2 = 0x8;
            for (int i = 0; i < numEntries2; i++)
            {
                res.WriteByte((byte)i); // Slots 0 to 3
                // res.WriteInt64(0xAAAAAAAA); // this is shown in the structure but for some reason isnt read

                res.WriteInt32(1); /*

                (affect Search section)


                0. Re-Bid.
                1. Bid.

                 */
                res.WriteInt64(0);
                res.WriteInt32(0); // Lowest bid info
                res.WriteInt32(0); // Buy now bid info
                res.WriteFixedString($"{client.Soul.Name}", 49);
                res.WriteByte(1); // 0 = nothing, 1 = logo appear
                res.WriteFixedString("Seem like a object ?", 385); // Comment in the bid info
                res.WriteInt16(18); // Bid (Bid info section)
                res.WriteInt32(800); // Time remaining, when status 1

                res.WriteInt32(1); // Bid Amount (bid info seciton, if you rebid in the search section, the bid amount change.) 
                res.WriteInt32(0); /* Statuses
                0 = show time remaining 
                1 = Your bid Was Sucessful  button change to receive (you supposed to receive the item)
                2 = look like someone outbid you, and button change to return money
                3 = Cancel selling
                finish.
                 */
            }

            res.WriteByte(0); // bool
            Router.Send(client, (ushort) AreaPacketId.recv_auction_notify_open, res, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "auct";
    }
}
