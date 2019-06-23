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
            res.WriteByte(2); //character slot, 0 for left, 1 for middle, 2 for right
            res.WriteInt32(1); //num of characters? 0 = none, 1 = 1 character, 2 does nothing missing data maybe?
            res.WriteFixedString("Test", 91); // Name

            res.WriteInt32(0); // 0 = Alive | 1 = Dead
            res.WriteInt32(2); // Level
            res.WriteInt32(1); //changed nothing visibly 
            res.WriteInt32(0); //class stat 
            //
            res.WriteInt32(0);//race flag
            res.WriteInt32(1);//gender flag
            res.WriteByte(2);//changing this byte makes hair and face change?
            res.WriteByte(0);//not sure
            //

            res.WriteByte(0);//changed nothing visibly
            //

            res.WriteInt32(0); //19x 4 byte //changed nothing visibly
            res.WriteInt32(0);//changed nothing visibly
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
                res.WriteInt32(0);
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


            res.WriteInt32(0); //19x 4 byte
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

            res.WriteInt32(0); //19x 4 byte
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


            res.WriteByte(0);//changed nothing visibly

            res.WriteInt32(0);//changed nothing visibly

            Router.Send(client, (ushort) MsgPacketId.recv_chara_notify_data, res);
        }
    }
}