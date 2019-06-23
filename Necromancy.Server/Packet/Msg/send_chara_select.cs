using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_chara_select : Handler
    {
        public send_chara_select(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_chara_select;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // Error

            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);

            res.WriteFixedString("127.0.0.1", 65);
            res.WriteInt16(60002);

            res.WriteFloat(100); // Coords ? - x,y,z
            res.WriteFloat(100);
            res.WriteFloat(100);

            res.WriteByte(0);

            Router.Send(client, (ushort) MsgPacketId.recv_chara_select_r, res);
        }
    }
}