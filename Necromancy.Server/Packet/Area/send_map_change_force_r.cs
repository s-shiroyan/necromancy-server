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
            Map map = client.Map;
            if (map != null)
            {
                map.Leave(client);
            }

            map = Server.Map.Get(client.Character.MapId);
            if (map == null)
            {
                Logger.Error($"Map {client.Character.MapId} does not exist");
                return;
            }

            map.Enter(client);
            client.Character.X = map.X;
            client.Character.Y = map.Y;
            client.Character.Z = map.Z;

            res.WriteInt32(client.Character.MapId);
            res.WriteInt32(client.Character.MapId);
            res.WriteFixedString("127.0.0.1", 65); //IP
            res.WriteInt16(60002); //Port

            res.WriteFloat(client.Character.X); //X Pos
            res.WriteFloat(client.Character.Y); //Y Pos
            res.WriteFloat(client.Character.Z); //Z Pos
            res.WriteByte(1); //view offset maybe?

            Router.Send(client, (ushort) AreaPacketId.recv_map_change_force, res, ServerType.Area);

            //SendMapChangeSyncOk(client);
        }

        private void SendMapChangeSyncOk(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_map_change_sync_ok, res, ServerType.Area);
        }
    }
}
