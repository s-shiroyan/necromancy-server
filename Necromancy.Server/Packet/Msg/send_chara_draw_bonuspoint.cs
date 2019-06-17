using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_chara_draw_bonuspoint : Handler
    {
        public send_chara_draw_bonuspoint(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_chara_draw_bonuspoint;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint unknown = packet.Data.ReadByte();
            Logger.Info($"Unknown: {unknown}");

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteByte(8);

            Router.Send(client, (ushort) MsgPacketId.recv_chara_draw_bonuspoint_r, res);
        }
    }
}