using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_friend_accept_request_link : ClientHandler
    {
        public send_friend_accept_request_link(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)MsgPacketId.send_friend_accept_request_link;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int response = packet.Data.ReadInt32();
            int unknown = packet.Data.ReadByte();
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(response);  // 0 = sucess, 1 = Request was not approved
            res.WriteInt32(unknown); // ??
            Router.Send(client, (ushort)MsgPacketId.recv_friend_reply_to_link_r, res, ServerType.Msg);
            SendFriendNotifyAddMember(client);
            SendFriendNotifyAddMember(client, client.Character.friendRequest);
        }

        private void SendFriendNotifyAddMember(NecClient client)
        {
            NecClient targetClient = Server.Clients.GetByCharacterInstanceId(client.Character.friendRequest);
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(client.Character.friendRequest);
            res.WriteInt32(0);
            res.WriteFixedString($"{targetClient.Soul.Name}", 0x31); //soul name
            res.WriteFixedString($"{targetClient.Character.Name}", 0x5B); //character name
            res.WriteInt32(targetClient.Character.ClassId);  // Class 0 = Fighter, 1 = thief, ect....
            res.WriteByte(targetClient.Character.Level); // Level of the friend
            res.WriteInt32(targetClient.Character.MapId); // Location of your friend
            res.WriteInt32(0);
            res.WriteFixedString($"Channel {targetClient.Character.Channel}", 0x61); // Channel location
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            Router.Send(client, (ushort)MsgPacketId.recv_friend_notify_add_member_r, res, ServerType.Msg);
        }
        private void SendFriendNotifyAddMember(NecClient client, uint targetInstanceId)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(client.Character.InstanceId);
            res.WriteInt32(0);
            res.WriteFixedString($"{client.Soul.Name}", 0x31); //soul name
            res.WriteFixedString($"{client.Character.Name}", 0x5B); //character name
            res.WriteInt32(client.Character.ClassId);  // Class 0 = Fighter, 1 = thief, ect....
            res.WriteByte(client.Character.Level); // Level of the friend
            res.WriteInt32(client.Character.MapId); // Location of your friend
            res.WriteInt32(0);
            res.WriteFixedString($"Channel {client.Character.Channel}", 0x61); // When i put the channel it doesn't add the friend, need to fix that.
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            Router.Send(Server.Clients.GetByCharacterInstanceId(targetInstanceId), (ushort)MsgPacketId.recv_friend_notify_add_member_r, res, ServerType.Msg);
        }
    }
}
