using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvAuctionNotifyOpen : PacketResponse
    {
        public RecvAuctionNotifyOpen()
            : base((ushort) AreaPacketId.recv_auction_notify_open, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(5); // must be <= 5

            int numEntries = 0x5;
            for (int i = 0; i < numEntries; i++)
            {

                res.WriteByte(1);
                // res.WriteInt64(0xAAAAAAAA); // this is shown in the structure but for some reason isnt read

                res.WriteInt32(0);
                res.WriteInt64(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString("soulname", 49);
                res.WriteByte(1);
                res.WriteFixedString("ToBeFound", 385);
                res.WriteInt16(0);
                res.WriteInt32(0);

                res.WriteInt32(0);
                res.WriteInt32(0);

            }

            res.WriteInt32(8); // must be< = 8

            int numEntries2 = 0x8;
            for (int i = 0; i < numEntries2; i++)
            {
                res.WriteByte(0);
                // res.WriteInt64(0xAAAAAAAA); // this is shown in the structure but for some reason isnt read

                res.WriteInt32(0);
                res.WriteInt64(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString("soulname", 49);
                res.WriteByte(0);
                res.WriteFixedString("ToBeFound", 385);
                res.WriteInt16(0);
                res.WriteInt32(0);

                res.WriteInt32(0);
                res.WriteInt32(0);

            }

            res.WriteByte(0); // bool
            return res;
        }
    }
}
