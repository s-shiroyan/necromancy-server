using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Packet.Receive.Msg;
using System.Collections.Generic;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_accept_to_apply : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_party_accept_to_apply));

        public send_party_accept_to_apply(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_party_accept_to_apply;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint applicantInstanceId =
                packet.Data.ReadUInt32(); //Could be a Party ID value hidden as character-who-made-it's value
            Logger.Debug(
                $"character {client.Character.Name} accepted Application to party from character Instance ID {applicantInstanceId}");

            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_party_accept_to_apply_r, res, ServerType.Area);

            Party myParty = Server.Instances.GetInstance(client.Character.partyId) as Party;
            NecClient applicantClient = Server.Clients.GetByCharacterInstanceId(applicantInstanceId);
            myParty.Join(applicantClient);




            foreach (NecClient partyClient in myParty.PartyMembers)
            {
                //Sanity check.  Who is in the party List at the time of Accepting the invite?
                Logger.Debug(
                    $"my party with instance ID {myParty.InstanceId} contains members {partyClient.Character.Name}");
                List<NecClient> DisposableList = new List<NecClient>();
                foreach (NecClient DisposablePartyClient in myParty.PartyMembers)
                {
                    DisposableList.Add(DisposablePartyClient);
                    Logger.Debug($"Added {DisposablePartyClient.Character.Name} to disposable list");
                }

                RecvPartyNotifyAddMember recvPartyNotifyAddMember = new RecvPartyNotifyAddMember(partyClient);
                Router.Send(recvPartyNotifyAddMember, DisposableList);

                Logger.Debug($"Adding member {partyClient.Character.Name} to Roster ");
            }

            RecvCharaBodyNotifyPartyJoin recvCharaBodyNotifyPartyJoin = new RecvCharaBodyNotifyPartyJoin(client.Character.InstanceId, myParty.InstanceId, myParty.PartyType);
            Router.Send(client.Map, recvCharaBodyNotifyPartyJoin);//Only send the Join Notify of the Accepting Client to the Map.  Existing members already did that when they joined.
        }

        private void SendPartyNotifyAddMember(NecClient client, Party myParty)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(myParty.InstanceId); //Most likely insanceId
            res.WriteUInt32(10976456);
            res.WriteUInt32(client.Character.InstanceId);
            res.WriteFixedString($"{client.Soul.Name}", 0x31); //Soul name
            res.WriteFixedString($"{client.Character.Name}", 0x5B); //Character name
            res.WriteUInt32(client.Character.ClassId); //Class
            res.WriteByte(client.Soul.Level); //Soul rank
            res.WriteByte(client.Character.Level); //Character level
            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteInt32(client.Character.MapId); //Might make the character selectable?
            res.WriteInt32(client.Character
                .MapId); //One half of location? 1001902 = Illfalo Port but is actually Deep Sea Port
            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteFixedString("", 0x61); //Location of player if not in same zone
            res.WriteFloat(1);
            res.WriteFloat(1);
            res.WriteFloat(1);
            res.WriteByte(3);
            res.WriteByte(4);
            res.WriteByte(5);
            res.WriteByte(6);
            Router.Send(myParty.PartyMembers, (ushort) MsgPacketId.recv_party_notify_add_member, res, ServerType.Msg);
            //Router.Send(Server.Clients.GetByCharacterInstanceId(instanceId), (ushort)MsgPacketId.recv_party_notify_add_member, res, ServerType.Msg);
        }

        private void SendCharaBodyNotifyPartyJoin(NecClient client, uint instanceId)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(client.Character.InstanceId); //Chara Instance ID
            res.WriteUInt32(client.Character.InstanceId); //Party InstancID?
            res.WriteUInt32(client.Character.InstanceId); //Party Leader InstanceId?

            Router.Send(client.Map, (ushort) AreaPacketId.recv_charabody_notify_party_join, res, ServerType.Area);
        }
    }
}
