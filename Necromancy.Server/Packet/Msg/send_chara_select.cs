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
            /*IBuffer res = BufferProvider.Provide();
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

            Router.Send(client, (ushort) MsgPacketId.recv_chara_select_r, res);*/

  
            IBuffer res2 = BufferProvider.Provide();

            res2.WriteInt32(0);
            res2.WriteInt32(0);

            //sub_494c50
            res2.WriteInt32(1);
            res2.WriteInt32(2);
            res2.WriteInt32(3);
            res2.WriteInt16(4);
            res2.WriteByte(1);

            //sub_494B90 - dor loop
          for(int i =0; i < 0x80; i++)  {
                res2.WriteInt32(i);
                res2.WriteFixedString($"Channel {i}", 97);
                res2.WriteByte(1);//bool 1 | 0
                res2.WriteInt16(0);
                res2.WriteInt16(0);
                res2.WriteByte(0);
                res2.WriteByte(0);
                //
            }


            res2.WriteByte(1);

            Router.Send(client, (ushort)MsgPacketId.recv_chara_select_channel_r, res2);
        }
    }
}