using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_union_request_change_role : ClientHandler
    {
        public send_union_request_change_role(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_union_request_change_role;

        

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint roleModifierCharacterInstanceId = packet.Data.ReadUInt32();
            uint targetInstanceId = packet.Data.ReadUInt32();
            uint modification = packet.Data.ReadUInt32();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort) MsgPacketId.recv_union_request_change_role_r, res, ServerType.Msg);


            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(roleModifierCharacterInstanceId);
            res2.WriteInt32(targetInstanceId);
            res2.WriteInt32(modification);
            Router.Send(client.Map, (ushort)MsgPacketId.recv_union_notify_changed_role, res2, ServerType.Msg);

        }
    }
}
