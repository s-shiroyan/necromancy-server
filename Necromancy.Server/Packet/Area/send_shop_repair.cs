using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class Send_shop_repair : ClientHandler
    {
        public Send_shop_repair(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort)AreaPacketId.send_shop_repair;

        public override void Handle(NecClient client, NecPacket packet)
        {

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_shop_repair_r, res, ServerType.Area);
 
            IBuffer res3 = BufferProvider.Provide();
            res3.WriteInt64(10200101);
            res3.WriteInt32(100); // Current durability points
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_durability, res3, ServerType.Area);
            //When repairing this should be item's max durability.
        }

    }
}
