using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_base_check_version_area : ClientHandler
    {
        public send_base_check_version_area(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_base_check_version;

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

            Router.Send(client, (ushort) AreaPacketId.recv_base_check_version_r, res);
        }
    }
}