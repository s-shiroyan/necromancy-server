using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_channel_select : Handler
    {
        public send_channel_select(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_channel_select;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);//Error

            //sub_4E4210_2341
            res.WriteInt32(1001002);//MapSerialID
            res.WriteInt32(1001002);//MapID
            res.WriteFixedString("127.0.0.1", 65);//IP
            res.WriteInt16(60002);//Port

            //sub_484420
            res.WriteFloat(1);//X Pos
            res.WriteFloat(2);//Y Pos
            res.WriteFloat(3);//Z Pos
            res.WriteByte(1);//Maybe view offset
            //

            Router.Send(client, (ushort) MsgPacketId.recv_channel_select_r, res);
        }
    }
}