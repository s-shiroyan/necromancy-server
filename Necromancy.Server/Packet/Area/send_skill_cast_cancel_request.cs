using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System.Threading;

namespace Necromancy.Server.Packet.Area
{
    public class send_skill_cast_cancel_request : ClientHandler
    {
        public send_skill_cast_cancel_request(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_skill_cast_cancel_request;

        public override void Handle(NecClient client, NecPacket packet)
        {
            Logger.Debug($"send_skill_cast_cancel_request");
            IBuffer res = BufferProvider.Provide();
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(client.Character.InstanceId);   


            Router.Send(client, (ushort)AreaPacketId.recv_skill_cast_cancel, res, ServerType.Area);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_start_notify, res2, ServerType.Area);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_action_skill_cancel, res, ServerType.Area);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_end_notify, res, ServerType.Area);



        }
    }
}
