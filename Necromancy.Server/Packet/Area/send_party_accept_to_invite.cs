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
            uint instanceId = packet.Data.ReadUInt32(); //Could be a Party ID value hidden as character-who-made-it's value

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            Router.Send(client, (ushort) AreaPacketId.recv_party_accept_to_invite_r, res, ServerType.Area);
            
            SendPartyNotifyAddMember(client);
            SendPartyNotifyAddMember(client, instanceId);
        }
        int i = 0;
        private void SendPartyNotifyAddMember(NecClient client)
        {
            NecClient targetClient = Server.Clients.GetByCharacterInstanceId(client.Character.partyRequest);
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(targetClient.Character.InstanceId); //Most likely insanceId
            res.WriteInt32(i);
            res.WriteInt32(i);
            res.WriteFixedString($"{targetClient.Soul.Name}", 0x31); //Soul name
            res.WriteFixedString($"{targetClient.Character.Name}", 0x5B); //Character name
            res.WriteInt32(targetClient.Character.ClassId); //Class
            res.WriteByte(targetClient.Soul.Level); //Soul rank
            res.WriteByte(targetClient.Character.Level); //Character level
            res.WriteInt32(i);
            res.WriteInt32(i);
            res.WriteInt32(i);
            res.WriteInt32(i);
            res.WriteInt32(i);
            res.WriteInt32(i);
            res.WriteInt32(targetClient.Character.MapId); //Might make the character selectable?
            res.WriteInt32(targetClient.Character.MapId); //One half of location? 1001902 = Illfalo Port but is actually Deep Sea Port
            res.WriteInt32(i);
            res.WriteInt32(i);
            res.WriteFixedString("", 0x61); //Location of player if not in same zone
            res.WriteFloat(targetClient.Character.X);
            res.WriteFloat(targetClient.Character.Y);
            res.WriteFloat(targetClient.Character.Z);
            res.WriteByte(targetClient.Character.Heading);
            res.WriteByte((byte)i);
            res.WriteByte((byte)i);
            res.WriteByte((byte)i);
            i++;
            Router.Send(client, (ushort)MsgPacketId.recv_party_notify_add_member, res, ServerType.Msg);
        }

        private void SendPartyNotifyAddMember(NecClient client, uint instanceId)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(client.Character.InstanceId); //Most likely insanceId
            res.WriteInt32(10976456);
            res.WriteInt32(10976458);
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
            res.WriteInt32(client.Character.MapId); //Might make the character selectable?
            res.WriteInt32(client.Character.MapId); //One half of location? 1001902 = Illfalo Port but is actually Deep Sea Port
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
            Router.Send(Server.Clients.GetByCharacterInstanceId(instanceId), (ushort)MsgPacketId.recv_party_notify_add_member, res, ServerType.Msg);
        }
    }
}
