using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_kick : ClientHandler
    {
        public send_party_kick(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_party_kick;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint kickTargetInstanceID = packet.Data.ReadUInt32();
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //Kick Reason?  error check?  probably error check
            Router.Send(client, (ushort) AreaPacketId.recv_party_kick_r, res, ServerType.Area);


            NecClient targetClient = Server.Clients.GetByCharacterInstanceId(kickTargetInstanceID);

            Router.Send(targetClient, (ushort)MsgPacketId.recv_party_notify_kick, BufferProvider.Provide(), ServerType.Msg);

        }
    }
}
