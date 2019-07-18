using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_map_enter : Handler
    {
        public send_map_enter(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_map_enter;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteByte(0);//Bool

            Router.Send(client, (ushort) AreaPacketId.recv_map_enter_r, res);            
        }
    }
}