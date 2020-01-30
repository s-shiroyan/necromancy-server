using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
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
            uint repliersInstanceId = packet.Data.ReadUInt32();
            uint resultAcceptOrDeny = packet.Data.ReadUInt32();

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(resultAcceptOrDeny); //Result
            res.WriteInt32(repliersInstanceId); //object id | Instance ID
            Router.Send(client.Map, (ushort) MsgPacketId.recv_union_reply_to_invite_r, res, ServerType.Msg);

            if (resultAcceptOrDeny == 0)
            {
                //client.Character.UnionId == inviterClient.Character.UnionId
                IBuffer res36 = BufferProvider.Provide();
                res36.WriteInt32(client.Character.InstanceId);
                res36.WriteInt32(8888 /*client.Character.UnionId*/);
                res36.WriteCString("Trade_Union" /*myUnion.Name*/); Router.Send(client.Map, (ushort)AreaPacketId.recv_chara_notify_union_data, res36, ServerType.Area);
            }
        }
    }
}
