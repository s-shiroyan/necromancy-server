using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_map_change_force_r : ClientHandler
    {
        public send_map_change_force_r(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_map_change_force_r;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(client.Character.MapId);
            res.WriteInt32(client.Character.MapId);
            res.WriteFixedString("127.0.0.1", 65);//IP
            res.WriteInt16(60002);//Port

            res.WriteFloat(100);//x coord
            res.WriteFloat(100);//y coord
            res.WriteFloat(100);//z coord
            res.WriteByte(1);//view offset maybe?

            Router.Send(client, (ushort)AreaPacketId.recv_map_change_force, res);

            SendMapChangeSyncOk(client);
        }

        private void SendMapChangeSyncOk(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            Router.Send(client, (ushort)AreaPacketId.recv_map_change_sync_ok,res);
        }
    }
}