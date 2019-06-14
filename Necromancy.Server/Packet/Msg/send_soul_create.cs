using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_soul_create : Handler
    {
        public send_soul_create(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_soul_create;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte unknown = packet.Data.ReadByte();
            string soulName = packet.Data.ReadCString();
            Logger.Info($"Created SoulName: {soulName}");
        }
    }
}