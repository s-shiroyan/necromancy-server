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
            int characterID = packet.Data.ReadInt32(); //Could be a Party ID value hidden as character-who-made-it's value

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            Router.Send(client, (ushort) AreaPacketId.recv_party_accept_to_invite_r, res, ServerType.Area);

            SendPartyNotifyAddMember(client);
        }

        private void SendPartyNotifyAddMember(NecClient client)
        {

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(client.Character.Id);
            res.WriteInt32(1);
            res.WriteInt32(2);
            res.WriteFixedString("soul?", 0x31); //soul name?
            res.WriteFixedString("chara?", 0x5B); //character name?
            res.WriteInt32(3);
            res.WriteByte(1);
            res.WriteByte(2);
            res.WriteInt32(4);
            res.WriteInt32(5);
            res.WriteInt32(6);
            res.WriteInt32(7);
            res.WriteInt32(8);
            res.WriteInt32(9);
            res.WriteInt32(10);
            res.WriteInt32(11);
            res.WriteInt32(12);
            res.WriteInt32(13);
            res.WriteFixedString("fixstr3", 0x61); // size is 0x61
            res.WriteFloat(1);
            res.WriteFloat(1);
            res.WriteFloat(1);
            res.WriteByte(3);
            res.WriteByte(4);
            res.WriteByte(5);
            res.WriteByte(6);

            Router.Send(client.Map, (ushort)MsgPacketId.recv_party_notify_add_member, res, ServerType.Msg);
        }
    }
}
