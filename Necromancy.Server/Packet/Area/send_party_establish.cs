using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Msg;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_establish : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_party_establish));

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
            uint targetInstanceId = packet.Data.ReadUInt32();

            Party myFirstParty = new Party();
            Server.Instances.AssignInstance(myFirstParty);
            myFirstParty.PartyType = partyType;
            myFirstParty.NormalItemDist = normItemDist;
            myFirstParty.RareItemDist = rareItemDist;
            myFirstParty.TargetClientId = targetInstanceId;
            myFirstParty.Join(client);
            myFirstParty.PartyLeaderId = client.Character.InstanceId;
            client.Character.partyId = myFirstParty.InstanceId;

            //Sanity check.  Did the player who made the party make it in to the List of Party Members stored on the Party Model?
            foreach (NecClient necClient in myFirstParty.PartyMembers)
            {
                Logger.Debug(
                    $"my party with instance ID {myFirstParty.InstanceId} contains members {necClient.Character.Name}");
            }

            Logger.Debug($"Party Instance ID {myFirstParty.InstanceId}");

            RecvPartyNotifyEstablish recvPartyNotifyEstablish = new RecvPartyNotifyEstablish(client, myFirstParty);
            Router.Send(recvPartyNotifyEstablish, client);
            SendCharaBodyNotifyPartyJoin(client);
            SendCharaNotifyPartyJoin(client);


            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort)AreaPacketId.recv_party_establish_r, res, ServerType.Area);





            if (targetInstanceId != 0)
            {
                NecClient targetClient = Server.Clients.GetByCharacterInstanceId(targetInstanceId);
                RecvPartyNotifyInvite recvPartyNotifyInvite = new RecvPartyNotifyInvite(targetClient, myFirstParty);
                Router.Send(recvPartyNotifyInvite, targetClient);
            }
        }


        private void SendCharaBodyNotifyPartyJoin(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(client.Character.InstanceId); //Chara Instance ID
            res.WriteUInt32(client.Character.partyId); //Party InstancID
            res.WriteInt32(0); //Party Mode

            Router.Send(client.Map, (ushort)AreaPacketId.recv_charabody_notify_party_join, res, ServerType.Area);
        }

        private void SendCharaNotifyPartyJoin(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(client.Character.InstanceId); //Chara Instance ID
            res.WriteUInt32(client.Character.partyId); //Party InstancID
            res.WriteInt32(0); //Party Mode 

            Router.Send(client, (ushort)AreaPacketId.recv_chara_notify_party_join, res, ServerType.Area);
        }
    }
}
