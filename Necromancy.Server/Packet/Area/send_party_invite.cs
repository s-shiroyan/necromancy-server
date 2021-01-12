using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_invite : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_party_invite));

        public send_party_invite(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_party_invite;

        public override void Handle(NecClient client, NecPacket packet)
        {
            /*int32, unknown
              int32, target client(character) id[i think this is the sends structure]*/
            int unknown = packet.Data.ReadInt32();
            uint targetInstanceId = packet.Data.ReadUInt32();
            NecClient targetClient = Server.Clients.GetByCharacterInstanceId(targetInstanceId);
            targetClient.Character.partyRequest = client.Character.InstanceId;

            if (targetInstanceId == 0)
            {
                targetInstanceId = client.Character.InstanceId;
            } //band-aid for null reference errors while testing. to-do Delete this line.

            Logger.Debug(
                $"ID {client.Character.InstanceId} {client.Character.Name} sent a party invite to {targetClient.Character.Name} with instance ID {targetInstanceId}");

            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(targetInstanceId);
            Router.Send(client, (ushort) AreaPacketId.recv_party_invite_r, res, ServerType.Area);

            SendPartyNotifyInvite(client, targetInstanceId);
        }

        private void SendPartyNotifyInvite(NecClient client, uint targetInstanceId)
        {
            Party myParty = Server.Instances.GetInstance(client.Character.partyId) as Party;
            //Sanity check.  Who is in the party List at the time of sending the invite?
            foreach (NecClient necClient in myParty.PartyMembers)
            {
                Logger.Debug(
                    $"my party with instance ID {myParty.InstanceId} contains members {necClient.Character.Name}");
            }

            NecClient partyClient1 = new NecClient(),
                        partyClient2 = new NecClient(),
                        partyClient3 = new NecClient(),
                        partyClient4 = new NecClient(),
                        partyClient5 = new NecClient();
            if (myParty.PartyMembers.Count >= 2)
            {
                partyClient1 = myParty.PartyMembers[1];
            }

            if (myParty.PartyMembers.Count >= 3)
            {
                partyClient2 = myParty.PartyMembers[2];
            }

            if (myParty.PartyMembers.Count >= 4)
            {
                partyClient3 = myParty.PartyMembers[3];
            }

            if (myParty.PartyMembers.Count >= 5)
            {
                partyClient4 = myParty.PartyMembers[4];
            }

            if (myParty.PartyMembers.Count >= 6)
            {
                partyClient5 = myParty.PartyMembers[5];
            }


            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(client.Character.partyId); //Party maker client id // should be Party Instance ID
            res.WriteInt32(myParty.PartyType); //Party type; 0 = closed, 1 = open.
            res.WriteInt32(myParty.NormalItemDist); //Normal item distribution; 0 = do not distribute, 1 = random.
            res.WriteInt32(myParty.RareItemDist); //Rare item distribution; 0 = do not distribute, 1 = Draw.
            res.WriteUInt32(client.Character.InstanceId);
            res.WriteUInt32(myParty.PartyLeaderId); //From player instance ID (but doesn't work?)
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
                res.WriteByte(1);
            }
            {
                res.WriteInt32(5);
                res.WriteUInt32(partyClient4.Character.InstanceId); //Instance Id?
                res.WriteFixedString($"{partyClient4.Soul.Name}", 0x31); //Soul name
                res.WriteFixedString($"{partyClient4.Character.Name}", 0x5B); //Chara name
                res.WriteUInt32(partyClient4.Character.ClassId); //Class
                res.WriteByte(partyClient4.Character.Level); //Level
                res.WriteByte(partyClient4.Character.criminalState); //Criminal Status
                res.WriteByte(1); //Beginner Protection (bool) 
                res.WriteByte(1); //Membership Status
                res.WriteByte(1);
                res.WriteByte(1);
            }
            {
                res.WriteInt32(6);
                res.WriteUInt32(partyClient5.Character.InstanceId); //Instance Id?
                res.WriteFixedString($"{partyClient5.Soul.Name}", 0x31); //Soul name
                res.WriteFixedString($"{partyClient5.Character.Name}", 0x5B); //Chara name
                res.WriteUInt32(partyClient5.Character.ClassId); //Class
                res.WriteByte(partyClient5.Character.Level); //Level
                res.WriteByte(partyClient5.Character.criminalState); //Criminal Status
                res.WriteByte(1); //Beginner Protection (bool) 
                res.WriteByte(1); //Membership Status
                res.WriteByte(1);
                res.WriteByte(1);
            }
            res.WriteByte((byte)myParty.PartyMembers
                .Count); // number of above party member entries to display in invite
            res.WriteByte(0); //Bool
            res.WriteFixedString($"This is a Comment Box for Parties", 0xB5); //size is 0xB5

            Router.Send(Server.Clients.GetByCharacterInstanceId(targetInstanceId),
                (ushort) MsgPacketId.recv_party_notify_invite, res, ServerType.Msg);
        }
    }
}
