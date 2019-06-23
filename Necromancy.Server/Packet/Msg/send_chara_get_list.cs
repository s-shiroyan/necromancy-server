using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_chara_get_list : Handler
    {
        public send_chara_get_list(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_chara_get_list;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(1);
            Router.Send(client, (ushort) MsgPacketId.recv_chara_get_list_r, res);


            SendNotifyData(client);
            SendNotifyDataComplete(client);
        }

        private void SendNotifyDataComplete(NecClient client)
        {
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteByte(1);
            res2.WriteInt32(1);
            res2.WriteInt32(1);
            res2.WriteInt32(1);
            Router.Send(client, (ushort) MsgPacketId.recv_chara_notify_data_complete, res2);
        }

        private void SendNotifyData(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(1);
            res.WriteInt32(1);
            res.WriteFixedString("Test", 91); // 0x5B | 91x 1 byte

            res.WriteInt32(1);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            //
            res.WriteInt32(1);
            res.WriteInt32(0);
            res.WriteByte(1);
            res.WriteByte(0);
            //

            res.WriteByte(1);
            //

            res.WriteInt32(1); //19x 4 byte
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            //

            //19x
            for (int i = 0; i < 19; i++)
            {
                res.WriteInt32(i);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0); // bool 1 | 0
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte((byte) i);
            }


            res.WriteInt32(1); //19x 4 byte
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(1);

            res.WriteInt32(2); //19x 4 byte
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(2);


            res.WriteByte(1);

            res.WriteInt32(0);

            Router.Send(client, (ushort) MsgPacketId.recv_chara_notify_data, res);
        }
    }
}