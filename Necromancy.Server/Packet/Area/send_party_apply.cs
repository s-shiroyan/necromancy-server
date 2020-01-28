using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_apply : ClientHandler
    {
        public send_party_apply(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_party_apply;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int PartyLeaderInstanceId = packet.Data.ReadInt32();
            uint targetPartyInstanceId = packet.Data.ReadUInt32();
            Logger.Debug($"{client.Character.Name }Applied to party {targetPartyInstanceId}");

            Party myParty = Server.Instances.GetInstance(targetPartyInstanceId) as Party;
            NecClient myPartyLeaderClient = Server.Clients.GetByCharacterInstanceId(myParty.PartyLeaderId);


            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(myParty.PartyLeaderId);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_party_apply_r, res, ServerType.Area);

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(client.Character.partyId); //Party ID?
            res2.WriteInt32(client.Character.InstanceId);
            res2.WriteFixedString($"{client.Soul.Name}", 0x31);
            res2.WriteFixedString($"{client.Character.Name}", 0x5B);
            res2.WriteInt32(client.Character.ClassId);
            res2.WriteByte(client.Character.Level);
            res2.WriteByte(2); //Criminal Status
            res2.WriteByte(1); //Beginner Protection (bool) 
            res2.WriteByte(1); //Membership Status
            res2.WriteByte(1);
            Router.Send(myPartyLeaderClient, (ushort)MsgPacketId.recv_party_notify_apply, res2, ServerType.Msg);
        }
    }
}
