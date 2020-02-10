using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.Union;

using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_union_reply_to_invite2 : ClientHandler
    {
        public send_union_reply_to_invite2(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_union_reply_to_invite2;

        

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint replyToInstanceId = packet.Data.ReadUInt32();
            uint resultAcceptOrDeny = packet.Data.ReadUInt32();
            NecClient replyToClient = Server.Clients.GetByCharacterInstanceId(replyToInstanceId);
            client.Character.unionId = replyToClient.Character.unionId;

            Union myUnion = Server.Instances.GetInstance(replyToClient.Character.unionId) as Union;
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(resultAcceptOrDeny); //Result
            res.WriteInt32(replyToInstanceId); //object id | Instance ID
            Router.Send(replyToClient, (ushort) MsgPacketId.recv_union_reply_to_invite_r, res, ServerType.Msg);

            if (resultAcceptOrDeny == 0)
            {
                //client.Character.UnionId == inviteCharacter.UnionId
                IBuffer res36 = BufferProvider.Provide();
                res36.WriteInt32(client.Character.InstanceId);
                res36.WriteInt32(client.Character.unionId);
                res36.WriteCString(myUnion.Name); 
                Router.Send(client.Map, (ushort)AreaPacketId.recv_chara_notify_union_data, res36, ServerType.Area);
            }
        }
    }
}
