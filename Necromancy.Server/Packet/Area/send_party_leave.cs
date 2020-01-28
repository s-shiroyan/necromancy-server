using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_leave : ClientHandler
    {
        public send_party_leave(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_party_leave;

        public override void Handle(NecClient client, NecPacket packet)
        {
            Party myParty = Server.Instances.GetInstance(client.Character.partyId) as Party;

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(client.Character.InstanceId);

            Router.Send(client, (ushort) AreaPacketId.recv_party_leave_r, res, ServerType.Area);
            Router.Send(myParty.PartyMembers, (ushort) AreaPacketId.recv_chara_notify_party_leave, res, ServerType.Area);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_charabody_notify_party_leave, res, ServerType.Area);


            myParty.Leave(client);

        }
    }
}
