using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_accept_to_invite : ClientHandler
    {
        public send_party_accept_to_invite(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_party_accept_to_invite;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint partyInstanceId = packet.Data.ReadUInt32(); 
            Logger.Debug($"character {client.Character.Name} accepted invite to party ID {partyInstanceId}");

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(partyInstanceId);
            Router.Send(client, (ushort) AreaPacketId.recv_party_accept_to_invite_r, res, ServerType.Area);

            Party myParty = Server.Instances.GetInstance(partyInstanceId) as Party;
            if (!myParty.PartyMembers.Contains(client)) {myParty.Join(client); } //add client to party list if it doesn't exist

            foreach(NecClient partyClient in myParty.PartyMembers)
            {
                SendPartyNotifyAddMember(partyClient, myParty);
            }

            
            SendCharaBodyNotifyPartyJoin(client, partyInstanceId);
        }
        private void SendPartyNotifyAddMember(NecClient client, Party myParty)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(myParty.InstanceId); //Most likely insanceId
            res.WriteInt32(myParty.InstanceId);
            res.WriteInt32(client.Character.InstanceId);
            res.WriteFixedString($"{client.Soul.Name}", 0x31); //Soul name
            res.WriteFixedString($"{client.Character.Name}", 0x5B); //Character name
            res.WriteInt32(client.Character.ClassId); //Class
            res.WriteByte(client.Soul.Level); //Soul rank
            res.WriteByte(client.Character.Level); //Character level
            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteInt32(0); //Might make the character selectable?
            res.WriteInt32(client.Character.MapId); //One half of location? 1001902 = Illfalo Port but is actually Deep Sea Port
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteFixedString("This is a Test", 0x61); //Location of player if not in same zone
            res.WriteFloat(client.Character.X);
            res.WriteFloat(client.Character.Y);
            res.WriteFloat(client.Character.Z);
            res.WriteByte(client.Character.Heading);
            res.WriteByte(1);
            res.WriteByte(1);
            res.WriteByte(1);
            Router.Send(myParty.PartyMembers, (ushort)MsgPacketId.recv_party_notify_add_member, res, ServerType.Msg, client);
            //Router.Send(Server.Clients.GetByCharacterInstanceId(instanceId), (ushort)MsgPacketId.recv_party_notify_add_member, res, ServerType.Msg);
        }
        private void SendCharaBodyNotifyPartyJoin(NecClient client, uint instanceId)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(client.Character.InstanceId); //Chara Instance ID
            res.WriteInt32(11111111); //Party InstancID?
            res.WriteInt32(11111111); //Party Leader InstanceId?

            Router.Send(client.Map, (ushort)AreaPacketId.recv_charabody_notify_party_join, res, ServerType.Area);
        }

          
        }
}
