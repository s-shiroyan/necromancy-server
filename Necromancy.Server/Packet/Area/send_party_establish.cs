using Arrowgene.Services.Buffers;
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

            client.Character.partyId = myFirstParty.InstanceId;
            Logger.Debug($"Party Instance ID {myFirstParty.InstanceId}");

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(myFirstParty.InstanceId);

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

            res.WriteInt32(partyInstanceId);//Party maker client id
            res.WriteInt32(partyType);//Party type
            res.WriteInt32(normItemDist);//Normal item distribution
            res.WriteInt32(rareItemDist);//Rare item distribution
            res.WriteInt32(client.Character.InstanceId); // ??
            res.WriteInt32(targetClient.Character.InstanceId); //client 2 instance ID?
            //for (int i = 0; i < 4; i++)
            {
                res.WriteInt32(1);
                res.WriteInt32(targetClient.Character.InstanceId); //Instance Id?
                res.WriteFixedString($"{targetClient.Soul.Name}", 0x31); //Soul name
                res.WriteFixedString($"{targetClient.Character.Name}", 0x5B); //Chara name
                res.WriteInt32(targetClient.Character.ClassId); //Class
                res.WriteByte(targetClient.Character.Level); //Level
                res.WriteByte(2); //Criminal Status
                res.WriteByte(1); //Beginner Protection (bool) 
                res.WriteByte(1); //Membership Status
                res.WriteByte(1);

                res.WriteInt32(2);
                res.WriteInt32(targetClient.Character.InstanceId); //Instance Id?
                res.WriteFixedString($"{targetClient.Soul.Name}", 0x31); //Soul name
                res.WriteFixedString($"{targetClient.Character.Name}", 0x5B); //Chara name
                res.WriteInt32(targetClient.Character.ClassId); //Class
                res.WriteByte(targetClient.Character.Level); //Level
                res.WriteByte(2); //Criminal Status
                res.WriteByte(1); //Beginner Protection (bool) 
                res.WriteByte(1); //Membership Status
                res.WriteByte(1);


                res.WriteInt32(3);
                res.WriteInt32(targetClient.Character.InstanceId); //Instance Id?
                res.WriteFixedString($"{targetClient.Soul.Name}", 0x31); //Soul name
                res.WriteFixedString($"{targetClient.Character.Name}", 0x5B); //Chara name
                res.WriteInt32(targetClient.Character.ClassId); //Class
                res.WriteByte(targetClient.Character.Level); //Level
                res.WriteByte(2); //Criminal Status
                res.WriteByte(1); //Beginner Protection (bool) 
                res.WriteByte(1); //Membership Status
                res.WriteByte(1);

                res.WriteInt32(4);
                res.WriteInt32(targetClient.Character.InstanceId); //Instance Id?
                res.WriteFixedString($"{targetClient.Soul.Name}", 0x31); //Soul name
                res.WriteFixedString($"{targetClient.Character.Name}", 0x5B); //Chara name
                res.WriteInt32(targetClient.Character.ClassId); //Class
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
            //recv_party_notify_invite
            NecClient targetClient = Server.Clients.GetByCharacterInstanceId(targetInstanceId);
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(client.Character.partyId);//Party maker client id // should be Party Instance ID
            res.WriteInt32(1);//Party type; 0 = closed, 1 = open.
            res.WriteInt32(1);//Normal item distribution; 0 = do not distribute, 1 = random.
            res.WriteInt32(1);//Rare item distribution; 0 = do not distribute, 1 = Draw.
            res.WriteInt32(client.Character.partyId);
            res.WriteInt32(targetInstanceId);//From player instance ID (but doesn't work?)
            {
                res.WriteInt32(0);
                res.WriteInt32(targetClient.Character.InstanceId); //Instance Id?
                res.WriteFixedString($"{targetClient.Soul.Name}", 0x31); //Soul name
                res.WriteFixedString($"{targetClient.Character.Name}", 0x5B); //Chara name
                res.WriteInt32(targetClient.Character.ClassId); //Class
                res.WriteByte(targetClient.Character.Level); //Level
                res.WriteByte(2); //Criminal Status
                res.WriteByte(1); //Beginner Protection (bool) 
                res.WriteByte(1); //Membership Status
                res.WriteByte(1);
            }
            {
                res.WriteInt32(0);
                res.WriteInt32(2); //Instance Id for member 2
                res.WriteFixedString($"{targetClient.Soul.Name}", 0x31); //Soul name
                res.WriteFixedString($"{targetClient.Character.Name}", 0x5B); //Chara name
                res.WriteInt32(targetClient.Character.ClassId); //Class
                res.WriteByte(targetClient.Character.Level); //Level
                res.WriteByte(2); //Criminal Status
                res.WriteByte(1); //Beginner Protection (bool)
                res.WriteByte(0); //Membership Status
                res.WriteByte(2);
            }
            {
                res.WriteInt32(0);
                res.WriteInt32(3); //Instance Id?
                res.WriteFixedString($"{targetClient.Soul.Name}", 0x31); //Soul name
                res.WriteFixedString($"{targetClient.Character.Name}", 0x5B); //Chara name
                res.WriteInt32(2); //Class
                res.WriteByte(69); //Level
                res.WriteByte(2); //Criminal Status
                res.WriteByte(1); //Beginner Protection (bool)
                res.WriteByte(1); //Membership Status
                res.WriteByte(3);
            }
            {
                res.WriteInt32(0);
                res.WriteInt32(0); //Instance Id?
                res.WriteFixedString($"{""/*targetClient.Soul.Name*/}", 0x31); //Soul name
                res.WriteFixedString($"{""/*targetClient.Character.Name*/}", 0x5B); //Chara name
                res.WriteInt32(0); //Class
                res.WriteByte(0); //Level
                res.WriteByte(0); //Criminal Status
                res.WriteByte(0); //Beginner Protection (bool)
                res.WriteByte(0); //Membership Status
                res.WriteByte(0);
            }
            res.WriteByte(4); // Size of party
            res.WriteFixedString($"This is a Comment Box for Parties", 0xB5); //size is 0xB5

            Router.Send(Server.Clients.GetByCharacterInstanceId(targetInstanceId), (ushort)MsgPacketId.recv_party_notify_invite, res, ServerType.Msg);
        }

        private void SendCharaBodyNotifyPartyJoin(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(client.Character.InstanceId); //Chara Instance ID?
            res.WriteInt32(client.Character.partyId); //Party InstancID?
            res.WriteInt32(client.Character.InstanceId); //Party Leader InstanceId?

            Router.Send(client.Map, (ushort)AreaPacketId.recv_charabody_notify_party_join, res, ServerType.Area);
        }

        private void SendCharaNotifyPartyJoin(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(client.Character.InstanceId); //Chara Instance ID?
            res.WriteInt32(client.Character.partyId); //Party InstancID?
            res.WriteInt32(client.Character.InstanceId); //Party Leader InstanceId?

            Router.Send(client.Map, (ushort)AreaPacketId.recv_chara_notify_party_join, res, ServerType.Area);
        }

        private void SendPartyRegistMemberRecruit(NecClient client)
        {
            //recv_party_regist_member_recruit_r = 0xA7BF,

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort)AreaPacketId.recv_party_establish_r, res, ServerType.Area);
        }

        private void SendPartyNotifyRecruitRequest(NecClient client, int partyType, int normItemDist, int rareItemDist, int targetClient)
        {
            //recv_party_notify_recruit_request = 0x9F8F, // Parent = 0x9F70 // Range ID = 02 // after -> send_party_regist_party_recruit

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(partyType);
            res.WriteInt32(normItemDist);
            res.WriteInt32(rareItemDist);
            res.WriteInt32(targetClient);
            Router.Send(client, (ushort)AreaPacketId.recv_party_notify_recruit_request, res, ServerType.Area);
        }

        private void SendPartyInvite(NecClient client, int targetClient)
        {
            //recv_party_invite_r = 0x300A, 
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            Router.Send(client, (ushort)AreaPacketId.recv_party_invite_r, res, ServerType.Area);
        }

        private void SendPartyApply(NecClient client, int targetClient)
        {
            //recv_party_apply_r = 0x5F1A,

            IBuffer res = BufferProvider.Provide();

        	res.WriteInt32(targetClient);

            Router.Send(client.Map, (ushort)AreaPacketId.recv_party_apply_r, res, ServerType.Area);
        }

        private void SendPartyChangeLeader(NecClient client)
        {
            //recv_party_change_leader_r = 0x7BB3,
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(client.Character.InstanceId);

            Router.Send(client.Map, (ushort)AreaPacketId.recv_party_change_leader_r, res, ServerType.Area);
        }
    }
}
