using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_base_enter : Handler
    {
        public send_base_enter(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_base_enter;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            //res.WriteByte(1);
            //res.WriteByte(1);
            res.WriteInt32(0);  //  Error

            Router.Send(client, (ushort) AreaPacketId.recv_base_enter_r, res);
        }
    }
}