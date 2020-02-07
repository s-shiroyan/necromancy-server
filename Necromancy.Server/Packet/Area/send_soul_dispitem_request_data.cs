using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_soul_dispitem_request_data : ClientHandler
    {
        public send_soul_dispitem_request_data(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_soul_dispitem_request_data;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);            

            Router.Send(client, (ushort) AreaPacketId.recv_soul_dispitem_request_data_r, res, ServerType.Area);

            //notify you of the soul item you got based on something above.
            IBuffer res19 = BufferProvider.Provide();
            res19.WriteInt32(Util.GetRandomNumber(62000001, 62000015)); //soul_dispitem.csv
            Router.Send(client, (ushort)AreaPacketId.recv_soul_dispitem_notify_data, res19, ServerType.Area);
        }
    }
}
