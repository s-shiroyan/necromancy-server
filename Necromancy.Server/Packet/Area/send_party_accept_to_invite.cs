using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System.Collections.Generic;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_accept_to_invite : ClientHandler
    {
        public send_party_accept_to_invite(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_party_accept_to_invite;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint partyInstanceId = packet.Data.ReadUInt32();
            Logger.Debug($"character {client.Character.Name} accepted invite to party ID {partyInstanceId}");

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(partyInstanceId);
            Router.Send(client, (ushort)AreaPacketId.recv_party_accept_to_invite_r, res, ServerType.Area);

            Party myParty = Server.Instances.GetInstance(partyInstanceId) as Party;
            if (!myParty.PartyMembers.Contains(client)) { myParty.Join(client); } //add Accepting client to party list if it doesn't exist

            foreach (NecClient partyClient in myParty.PartyMembers)
            {
                //Sanity check.  Who is in the party List at the time of Accepting the invite?
                Logger.Debug($"my party with instance ID {myParty.InstanceId} contains members {partyClient.Character.Name}");
                List<NecClient> DisposableList = new List<NecClient>();
                foreach (NecClient DisposablePartyClient in myParty.PartyMembers)
                {
                    DisposableList.Add(DisposablePartyClient);
                    Logger.Debug($"Added {DisposablePartyClient.Character.Name} to disposable list");
                }
                SendPartyNotifyAddMember(partyClient, DisposableList);
                Logger.Debug($"Adding member {partyClient.Character.Name} to Roster ");
            }

            client.Character.partyId = myParty.InstanceId;

            //SendPartyEstablish(client, myParty.InstanceId);
            SendCharaBodyNotifyPartyJoin(client, partyInstanceId); //Only send the Join Notify of the Accepting Client to the Map.  Existing members already did that when they joined.
            SendCharaNotifyPartyJoin(client, myParty); //Only send the Join of the Accepting Client to the Accepting Client. 

        }
        private void SendPartyEstablish(NecClient thisClient, uint PartyInstanceId)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(PartyInstanceId);
            Router.Send(thisClient, (ushort)AreaPacketId.recv_party_establish_r, res, ServerType.Area);
        }
        private void SendPartyNotifyAddMember(NecClient thisClient, List<NecClient> PartyMembersList)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(thisClient.Character.InstanceId); //Party Add Reason?
            res.WriteInt32(thisClient.Character.InstanceId); //Object ID?
            res.WriteInt32(thisClient.Character.InstanceId);
            res.WriteFixedString($"{thisClient.Soul.Name}", 0x31); //Soul name
            res.WriteFixedString($"{thisClient.Character.Name}", 0x5B); //Character name
            res.WriteInt32(thisClient.Character.ClassId); //Class
            res.WriteByte(thisClient.Soul.Level); //Soul rank
            res.WriteByte(thisClient.Character.Level); //Character level
            res.WriteInt32(thisClient.Character.InstanceId);
            res.WriteInt32(thisClient.Character.InstanceId);
            res.WriteInt32(thisClient.Character.InstanceId);
            res.WriteInt32(thisClient.Character.InstanceId);
            res.WriteInt32(thisClient.Character.InstanceId);
            res.WriteInt32(thisClient.Character.InstanceId);
            res.WriteInt32(0); //Might make the character selectable?
            res.WriteInt32(thisClient.Character.MapId); //One half of location? 1001902 = Illfalo Port but is actually Deep Sea Port
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteFixedString("This is a Test", 0x61); //Location of player if not in same zone
            res.WriteFloat(thisClient.Character.X);
            res.WriteFloat(thisClient.Character.Y);
            res.WriteFloat(thisClient.Character.Z);
            res.WriteByte(thisClient.Character.Heading);
            res.WriteByte(1);
            res.WriteByte(1);
            res.WriteByte(1);
            Router.Send(PartyMembersList, (ushort)MsgPacketId.recv_party_notify_add_member, res, ServerType.Msg, thisClient);
        }
        private void SendCharaBodyNotifyPartyJoin(NecClient client, uint instanceId)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(client.Character.InstanceId); //Chara Instance ID
            res.WriteInt32(client.Character.partyId); //Party InstancID?
            res.WriteInt32(client.Character.InstanceId); //Party Leader InstanceId?
            Router.Send(client.Map, (ushort)AreaPacketId.recv_charabody_notify_party_join, res, ServerType.Area);
        }

        private void SendCharaNotifyPartyJoin(NecClient client, Party myParty)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(client.Character.InstanceId); //object ID
            res.WriteInt32(myParty.InstanceId); //party ID
            res.WriteInt32(client.Character.InstanceId); //Party Leader InstanceId?
            Router.Send(myParty.PartyMembers, (ushort)AreaPacketId.recv_chara_notify_party_join, res, ServerType.Area);
        }


    }
}
