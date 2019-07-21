using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_base_check_version_msg : Handler
    {
        public send_base_check_version_msg(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_base_check_version;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint unknown = packet.Data.ReadUInt32();
            uint major = packet.Data.ReadUInt32();
            uint minor = packet.Data.ReadUInt32();
            Logger.Info($"{major} - {minor}");

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(unknown);
            res.WriteInt32(major);
            res.WriteInt32(minor);

            Router.Send(client,  (ushort) MsgPacketId.recv_base_check_version_r, res);
        }
    }
}