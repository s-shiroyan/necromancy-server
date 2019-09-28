using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_auction_search : Handler
    {
        public send_auction_search(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_auction_search;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
	
            res.WriteInt32(0x64); // cmp to 0x64 = 100
	
            int numEntries4 = 0x64;
            for (int i = 0; i < numEntries4; i++)
            {
                res.WriteInt32(0);
                res.WriteInt64(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString($"{client.Soul.Name}", 49);
                res.WriteByte(0);
                res.WriteFixedString("ToBeFound", 385);
                res.WriteInt16(0);
                res.WriteInt32(0);
            }
            Router.Send(client.Map, (ushort) AreaPacketId.recv_auction_search_r, res);
        }

    }
}