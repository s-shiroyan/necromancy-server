using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_base_login : Handler
    {
        bool toggle = false;
        public send_base_login(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_base_login;

        public override void Handle(NecClient client, NecPacket packet)
        {
            if (!toggle)
            {
                IBuffer res = BufferProvider.Provide();
                res.WriteInt32(0);  //  Error

                res.WriteByte(1);
                res.WriteFixedString("Soul 1", 49);
                res.WriteByte(1); // Soul Level
                res.WriteByte(0); // bool   (important bool, if use value 1 - can't join in msg server character list)

                res.WriteByte(0);
                res.WriteFixedString("Soul 2", 49);
                res.WriteByte(0); // Soul Level
                res.WriteByte(0); //bool

                /*//JP-start
               res.WriteByte(0);
               res.WriteFixedString("Soul 3", 49);
               res.WriteByte(0); // Soul Level
               res.WriteByte(0); //bool

               res.WriteByte(0);
               res.WriteFixedString("Soul 4", 49);
               res.WriteByte(0); // Soul Level
               res.WriteByte(0); //bool

               res.WriteByte(0);
               res.WriteFixedString("Soul 5", 49);
               res.WriteByte(0); // Soul Level
               res.WriteByte(0); //bool

               res.WriteByte(0);
               res.WriteFixedString("Soul 6", 49);
               res.WriteByte(0); // Soul Level
               res.WriteByte(0); //bool

               res.WriteByte(0);
               res.WriteFixedString("Soul 7", 49);
               res.WriteByte(0); // Soul Level
               res.WriteByte(0); //bool

               res.WriteByte(0);
               res.WriteFixedString("Soul 8", 49);
               res.WriteByte(0); // Soul Level
               res.WriteByte(0); //bool

               res.WriteInt32(0);
                *///JP-end

                res.WriteByte(0); //bool
                res.WriteByte(0);

                Router.Send(client, (ushort)MsgPacketId.recv_base_login_r, res);
                toggle = true;
            }
            else
            {
                IBuffer res1 = BufferProvider.Provide();
                res1.WriteInt32(0);
                Router.Send(client, (ushort)MsgPacketId.recv_chara_select_back_soul_select_r, res1);
            }
        }
    }
}