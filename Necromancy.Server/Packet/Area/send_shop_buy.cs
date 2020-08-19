using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_shop_buy : ClientHandler
    {
        public send_shop_buy(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_shop_buy;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_shop_buy_r, res, ServerType.Area);
        }
    }
}
