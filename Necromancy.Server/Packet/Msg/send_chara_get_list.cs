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
            res2.WriteInt32(2);
            res2.WriteInt32(3);
            res2.WriteInt32(4);
            Router.Send(client, (ushort) MsgPacketId.recv_chara_notify_data_complete, res2);
        }

        private void SendNotifyData(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(1);//character slot, 0 for left, 1 for middle, 2 for right
            res.WriteInt32(1);    //  Character ID
            res.WriteFixedString("Test1", 91); // 0x5B | 91x 1 byte

            res.WriteInt32(0); // 0 = Alive | 1 = Dead
            res.WriteInt32(1);//character level stat
            res.WriteInt32(1);//todo (unknown)
            res.WriteInt32(0);//class stat 
            //
            res.WriteInt32(0);//race flag
            res.WriteInt32(1);//gender flag
            res.WriteByte(2);//changing this byte makes hair and face change?
            res.WriteByte(0);//hair color? i think
            res.WriteByte(1);//changed nothing visibly
            //

            //I think these int32s correspond to each part of the characters item slots
            res.WriteInt32(-1); 
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            //

            //19x
            {
                //-----------------------------------------1
                res.WriteInt32(0);//weapon ID goes here, 15000101 buckler worked, 10310503 1h sword, 
                res.WriteByte(0);//ID type specifier, 0 for wep, 1 for armor, 2 for accessory?
                res.WriteByte(0);//didnt change anything?
                res.WriteByte(0);//didnt change anything?

                res.WriteInt32(0);//didnt change anything?
                res.WriteByte(0);//didnt change anything?
                res.WriteByte(0);//didnt change anything?
                res.WriteByte(0);//didnt change anything?

                res.WriteByte(0);//didnt change anything?
                res.WriteByte(0);//didnt change anything?
                res.WriteByte(0); // bool 1 | 0 //didnt change anything?
                res.WriteByte(0);//didnt change anything?
                res.WriteByte(0);//didnt change anything?
                res.WriteByte(0);//didnt change anything?
                res.WriteByte(0);//didnt change anything?

                res.WriteByte(0);//didnt change anything?

                for (int i = 0; i < 18; i++)
                {
                    res.WriteInt32(0);//weapon ID goes here, 15000101 buckler worked, 10310503 1h sword, 
                    res.WriteByte(0);//ID type specifier, 0 for wep, 1 for armor, 2 for accessory?
                    res.WriteByte(0);//didnt change anything?
                    res.WriteByte(0);//didnt change anything?

                    res.WriteInt32(0);//didnt change anything?
                    res.WriteByte(0);//didnt change anything?
                    res.WriteByte(0);//didnt change anything?
                    res.WriteByte(0);//didnt change anything?

                    res.WriteByte(0);//didnt change anything?
                    res.WriteByte(0);//didnt change anything?
                    res.WriteByte(0); // bool 1 | 0 //didnt change anything?
                    res.WriteByte(0);//didnt change anything?
                    res.WriteByte(0);//didnt change anything?
                    res.WriteByte(0);//didnt change anything?
                    res.WriteByte(0);//didnt change anything?

                    res.WriteByte(0);//didnt change anything?
                }
            }


            //19x 4 byte/ wep type i think also? 10 makes the wep go underneath the character
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);

            //19x 4 byte //item quality(+#) or aura? 10 = +7, 19 = +6,(maybe just wep aura)
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);
            res.WriteInt32(-1);


            res.WriteByte(1);//changed nothing visibly

            res.WriteInt32(1001001);//map location ID

            Router.Send(client, (ushort) MsgPacketId.recv_chara_notify_data, res);//SOE
            //Router.Send(client,0xA535, res);//JP
        }
    }
}