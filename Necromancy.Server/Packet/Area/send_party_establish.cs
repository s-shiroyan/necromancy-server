using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_establish : ClientHandler
    {
        public send_party_establish(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_party_establish;

        public override void Handle(NecClient client, NecPacket packet)
        {
            /*int32, party type: 1 = open, 0 = closed
              int32, normal item distribution: 1 = random, 0 = do not distribute
              int32, rare item distribution: 1 = draw, 0 = do not distribute
              int32, target client(character) id*/

            int partyType = packet.Data.ReadInt32();
            int normItemDist = packet.Data.ReadInt32();
            int rareItemDist = packet.Data.ReadInt32();
            uint targetClientId = packet.Data.ReadUInt32();

            Party myFirstParty = new Party();
            Server.Instances.AssignInstance(myFirstParty);
            myFirstParty.PartyType = partyType;
            myFirstParty.NormalItemDist = normItemDist;
            myFirstParty.RareItemDist = rareItemDist;
            myFirstParty.TargetClientId = targetClientId;
            myFirstParty.Join(client);            
            myFirstParty.PartyLeaderId = client.Character.InstanceId;
            client.Character.partyId = myFirstParty.InstanceId;

            //Sanity check.  Did the player who made the party make it in to the List of Party Members stored on the Party Model?
            foreach (NecClient necClient in myFirstParty.PartyMembers) { Logger.Debug($"my party with instance ID {myFirstParty.InstanceId} contains members {necClient.Character.Name}"); }
            Logger.Debug($"Party Instance ID {myFirstParty.InstanceId}");

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort)AreaPacketId.recv_party_establish_r, res, ServerType.Area);

            SendPartyNotifyEstablish(client, partyType, normItemDist, rareItemDist, targetClientId, myFirstParty.InstanceId);
            SendCharaBodyNotifyPartyJoin(client);
            SendCharaNotifyPartyJoin(client);
            if (targetClientId != 0) { SendPartyNotifyInvite(client, targetClientId); }
        }

        private void SendPartyNotifyEstablish(NecClient client, int partyType, int normItemDist, int rareItemDist, uint targetClientId, uint partyInstanceId)
        {
            if (targetClientId == 0) { targetClientId = client.Character.InstanceId; }
            NecClient targetClient = Server.Clients.GetByCharacterInstanceId(targetClientId);

            IBuffer res = BufferProvider.Provide();

            res.WriteUInt32(partyInstanceId);//Party instance ID
            res.WriteInt32(partyType);//Party type
            res.WriteInt32(normItemDist);//Normal item distribution
            res.WriteInt32(rareItemDist);//Rare item distribution
            res.WriteUInt32(targetClientId); // ??
            res.WriteUInt32(client.Character.InstanceId); //Party Leader Instance ID.
            //for (int i = 0; i < 4; i++)
            {
                res.WriteInt32(1);
                res.WriteUInt32(targetClient.Character.InstanceId); //Instance Id?
                res.WriteFixedString($"{targetClient.Soul.Name}", 0x31); //Soul name
                res.WriteFixedString($"{targetClient.Character.Name}", 0x5B); //Chara name
                res.WriteUInt32(targetClient.Character.ClassId); //Class
                res.WriteByte(targetClient.Character.Level); //Level
                res.WriteByte(2); //Criminal Status
                res.WriteByte(1); //Beginner Protection (bool) 
                res.WriteByte(1); //Membership Status
                res.WriteByte(1);

                res.WriteInt32(2);
                res.WriteUInt32(targetClient.Character.InstanceId); //Instance Id?
                res.WriteFixedString($"{targetClient.Soul.Name}", 0x31); //Soul name
                res.WriteFixedString($"{targetClient.Character.Name}", 0x5B); //Chara name
                res.WriteUInt32(targetClient.Character.ClassId); //Class
                res.WriteByte(targetClient.Character.Level); //Level
                res.WriteByte(2); //Criminal Status
                res.WriteByte(1); //Beginner Protection (bool) 
                res.WriteByte(1); //Membership Status
                res.WriteByte(1);


                res.WriteInt32(3);
                res.WriteUInt32(targetClient.Character.InstanceId); //Instance Id?
                res.WriteFixedString($"{targetClient.Soul.Name}", 0x31); //Soul name
                res.WriteFixedString($"{targetClient.Character.Name}", 0x5B); //Chara name
                res.WriteUInt32(targetClient.Character.ClassId); //Class
                res.WriteByte(targetClient.Character.Level); //Level
                res.WriteByte(2); //Criminal Status
                res.WriteByte(1); //Beginner Protection (bool) 
                res.WriteByte(1); //Membership Status
                res.WriteByte(1);

                res.WriteInt32(4);
                res.WriteUInt32(targetClient.Character.InstanceId); //Instance Id?
                res.WriteFixedString($"{targetClient.Soul.Name}", 0x31); //Soul name
                res.WriteFixedString($"{targetClient.Character.Name}", 0x5B); //Chara name
                res.WriteUInt32(targetClient.Character.ClassId); //Class
                res.WriteByte(targetClient.Character.Level); //Level
                res.WriteByte(2); //Criminal Status
                res.WriteByte(1); //Beginner Protection (bool) 
                res.WriteByte(1); //Membership Status
                res.WriteByte(1);
            }
            res.WriteByte(0);

            Router.Send(client, (ushort)MsgPacketId.recv_party_notify_establish, res, ServerType.Msg);
        }

        private void SendPartyNotifyInvite(NecClient client, uint targetInstanceId)
        {
            NecClient targetClient = Server.Clients.GetByCharacterInstanceId(targetInstanceId);
            Party myParty = Server.Instances.GetInstance(client.Character.partyId) as Party;
            //Sanity check.  Who is in the party List at the time of sending the invite?
            foreach (NecClient necClient in myParty.PartyMembers) { Logger.Debug($"my party with instance ID {myParty.InstanceId} contains members {necClient.Character.Name}"); }

            NecClient partyClient1 = new NecClient(),
                      partyClient2 = new NecClient(),
                      partyClient3 = new NecClient();
            if (myParty.PartyMembers.Count >= 2) { partyClient1 = myParty.PartyMembers[1]; }
            if (myParty.PartyMembers.Count >= 3) { partyClient2 = myParty.PartyMembers[2]; }
            if (myParty.PartyMembers.Count >= 4) { partyClient3 = myParty.PartyMembers[3]; }



            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(client.Character.partyId);//Party maker client id // should be Party Instance ID
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
                res.WriteByte(client.Character.criminalState); //Criminal Status
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
                res.WriteByte(partyClient1.Character.criminalState); //Criminal Status
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
                res.WriteByte(partyClient3.Character.criminalState); //Criminal Status
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
                res.WriteByte(partyClient3.Character.criminalState); //Criminal Status
                res.WriteByte(1); //Beginner Protection (bool) 
                res.WriteByte(1); //Membership Status
                res.WriteByte(1);
            }
            res.WriteByte((byte)myParty.PartyMembers.Count); // number of above party member entries to display in invite
            res.WriteFixedString($"This is a Comment Box for Parties", 0xB5); //size is 0xB5

            Router.Send(Server.Clients.GetByCharacterInstanceId(targetInstanceId), (ushort)MsgPacketId.recv_party_notify_invite, res, ServerType.Msg);
        }

        private void SendCharaBodyNotifyPartyJoin(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(client.Character.InstanceId); //Chara Instance ID?
            res.WriteUInt32(client.Character.partyId); //Party InstancID?
            res.WriteInt32(0); //Party Leader InstanceId?

            Router.Send(client.Map, (ushort)AreaPacketId.recv_charabody_notify_party_join, res, ServerType.Area);
        }

        private void SendCharaNotifyPartyJoin(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(client.Character.InstanceId); //Chara Instance ID?
            res.WriteUInt32(client.Character.partyId); //Party InstancID?
            res.WriteInt32(0); //Party Leader InstanceId?

            Router.Send(client, (ushort)AreaPacketId.recv_chara_notify_party_join, res, ServerType.Area);
        }

    }
}
