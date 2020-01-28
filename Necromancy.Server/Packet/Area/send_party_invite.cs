using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_invite : ClientHandler
    {
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

            if (targetInstanceId == 0) { targetInstanceId = client.Character.InstanceId; } //band-aid for null reference errors while testing. to-do Delete this line.
            Logger.Debug($"ID {client.Character.InstanceId} {client.Character.Name} sent a party invite to {targetClient.Character.Name} with instance ID {targetInstanceId}");

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(targetInstanceId);

            Router.Send(client, (ushort) AreaPacketId.recv_party_invite_r, res, ServerType.Area);

            SendPartyNotifyInvite(client, targetInstanceId);
        }
        int i = 0;
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
    }
}
