using Arrowgene.Buffers;
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
            res.WriteUInt32(partyInstanceId);
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

            SendPartyNotifyEstablish(client, myParty);
            SendCharaBodyNotifyPartyJoin(client, myParty); //Only send the Join Notify of the Accepting Client to the Map.  Existing members already did that when they joined.
            SendCharaNotifyPartyJoin(client, myParty); //Only send the Join of the Accepting Client to the Accepting Client. 

        }

        private void SendPartyNotifyAddMember(NecClient thisClient, List<NecClient> PartyMembersList)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(1); //Party Add Reason?
            res.WriteUInt32(thisClient.Character.InstanceId); //Object ID?
            res.WriteUInt32(thisClient.Character.InstanceId);
            res.WriteFixedString($"{thisClient.Soul.Name}", 0x31); //Soul name
            res.WriteFixedString($"{thisClient.Character.Name}", 0x5B); //Character name
            res.WriteUInt32(thisClient.Character.ClassId); //Class
            res.WriteByte(thisClient.Soul.Level); //Soul rank
            res.WriteByte(thisClient.Character.Level); //Character level
            res.WriteInt32(Util.GetRandomNumber(1,10));
            res.WriteInt32(Util.GetRandomNumber(1, 10));
            res.WriteInt32(Util.GetRandomNumber(1, 10));
            res.WriteInt32(Util.GetRandomNumber(1, 10));
            res.WriteInt32(Util.GetRandomNumber(1, 10));
            res.WriteInt32(Util.GetRandomNumber(1, 10));
            res.WriteInt32(0); //Might make the character selectable?
            res.WriteInt32(thisClient.Character.MapId); //One half of location? 1001902 = Illfalo Port but is actually Deep Sea Port
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteFixedString($"Channel {thisClient.Character.Channel}", 0x61); //Location of player if not in same zone
            res.WriteFloat(thisClient.Character.X);
            res.WriteFloat(thisClient.Character.Y);
            res.WriteFloat(thisClient.Character.Z);
            res.WriteByte(thisClient.Character.Heading);
            res.WriteByte(thisClient.Character.criminalState);//?
            res.WriteByte(1); //Beginner Protection (bool) ???
            res.WriteByte(1); //Membership Status???
            Router.Send(PartyMembersList, (ushort)MsgPacketId.recv_party_notify_add_member, res, ServerType.Msg, thisClient);

            /*
            PARTY_ADD	0	%s is in a party
            PARTY_ADD	1	%s has joined your party
            PARTY_ADD	10	You have joined the party of %s
            */
        }
        private void SendCharaBodyNotifyPartyJoin(NecClient client, Party myParty)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(client.Character.InstanceId); //Chara Instance ID
            res.WriteUInt32(client.Character.InstanceId); //Party InstancID?
            res.WriteUInt32(myParty.InstanceId); //Party Leader InstanceId?
            Router.Send(client.Map, (ushort)AreaPacketId.recv_charabody_notify_party_join, res, ServerType.Area);
        }

        private void SendCharaNotifyPartyJoin(NecClient client, Party myParty)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(client.Character.InstanceId); //object ID
            res.WriteUInt32(client.Character.InstanceId); //party ID
            res.WriteUInt32(myParty.InstanceId); //Party Leader InstanceId?
            Router.Send(client.Map, (ushort)AreaPacketId.recv_chara_notify_party_join, res, ServerType.Area);
        }

        private void SendPartyNotifyEstablish(NecClient client, Party myParty)
        {
            NecClient partyClient1 = new NecClient(),
                      partyClient2 = new NecClient(),
                      partyClient3 = new NecClient();
            if (myParty.PartyMembers.Count >= 2) { partyClient1 = myParty.PartyMembers[1]; }
            if (myParty.PartyMembers.Count >= 3) { partyClient2 = myParty.PartyMembers[2]; }
            if (myParty.PartyMembers.Count >= 4) { partyClient3 = myParty.PartyMembers[3]; }

            IBuffer res = BufferProvider.Provide();

            res.WriteUInt32(client.Character.partyId);//Party Instance ID
            res.WriteInt32(myParty.PartyType);//Party type; 0 = closed, 1 = open.
            res.WriteInt32(myParty.NormalItemDist);//Normal item distribution; 0 = do not distribute, 1 = random.
            res.WriteInt32(myParty.RareItemDist);//Rare item distribution; 0 = do not distribute, 1 = Draw.
            res.WriteUInt32(client.Character.InstanceId);
            res.WriteUInt32(myParty.PartyLeaderId);//From player instance ID (but doesn't work?)
            {
                res.WriteInt32(1);
                res.WriteUInt32(client.Character.InstanceId); //Instance Id?
                res.WriteFixedString($"{client.Soul.Name}", 0x31); //Soul name
                res.WriteFixedString($"{client.Character.Name}", 0x5B); //Chara name
                res.WriteUInt32(client.Character.ClassId); //Class
                res.WriteByte(client.Character.Level); //Level
                res.WriteByte(2); //Criminal Status
                res.WriteByte(1); //Beginner Protection (bool) 
                res.WriteByte(1); //Membership Status
                res.WriteByte(1);
            }
            {
                res.WriteInt32(2);
                res.WriteUInt32(partyClient1.Character.InstanceId); //Instance Id?
                res.WriteFixedString($"{partyClient1.Soul.Name}", 0x31); //Soul name
                res.WriteFixedString($"{partyClient1.Character.Name}", 0x5B); //Chara name
                res.WriteUInt32(partyClient1.Character.ClassId); //Class
                res.WriteByte(partyClient1.Character.Level); //Level
                res.WriteByte(2); //Criminal Status
                res.WriteByte(1); //Beginner Protection (bool) 
                res.WriteByte(1); //Membership Status
                res.WriteByte(1);
            }
            {
                res.WriteInt32(3);
                res.WriteUInt32(partyClient2.Character.InstanceId); //Instance Id?
                res.WriteFixedString($"{partyClient2.Soul.Name}", 0x31); //Soul name
                res.WriteFixedString($"{partyClient2.Character.Name}", 0x5B); //Chara name
                res.WriteUInt32(partyClient2.Character.ClassId); //Class
                res.WriteByte(partyClient2.Character.Level); //Level
                res.WriteByte(2); //Criminal Status
                res.WriteByte(1); //Beginner Protection (bool) 
                res.WriteByte(1); //Membership Status
                res.WriteByte(1);
            }
            {
                res.WriteInt32(4);
                res.WriteUInt32(partyClient3.Character.InstanceId); //Instance Id?
                res.WriteFixedString($"{partyClient3.Soul.Name}", 0x31); //Soul name
                res.WriteFixedString($"{partyClient3.Character.Name}", 0x5B); //Chara name
                res.WriteUInt32(partyClient3.Character.ClassId); //Class
                res.WriteByte(partyClient3.Character.Level); //Level
                res.WriteByte(2); //Criminal Status
                res.WriteByte(1); //Beginner Protection (bool) 
                res.WriteByte(1); //Membership Status
                res.WriteByte(1);
            }
            res.WriteByte((byte)myParty.PartyMembers.Count); // number of above party member entries to display in invite

            Router.Send(client, (ushort)MsgPacketId.recv_party_notify_establish, res, ServerType.Msg);
        }


    }
}
