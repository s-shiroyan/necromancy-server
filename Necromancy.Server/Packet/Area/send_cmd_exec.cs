using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_cmd_exec : ClientHandler
    {
        public send_cmd_exec(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_cmd_exec;

       public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            Router.Send(client, (ushort)AreaPacketId.recv_cmd_exec_r, res, ServerType.Area);
        }
    }
}