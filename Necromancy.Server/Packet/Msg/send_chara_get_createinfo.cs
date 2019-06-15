using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_chara_get_createinfo : Handler
    {
        public send_chara_get_createinfo(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_chara_get_createinfo;

        /// <summary>
        /// [<= 8 array][Type unknown]
        ///   [<= 8 array][Type unknown]
        ///   [<= 8 array][Type unknown]
        ///   [<= 5 array][Type unknown]
        ///   [<= 10 array][Type unknown]
        ///   [<= 140 array][Type unknown]
        ///   [<= 14 array][Type unknown]
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            // 4bytes cmp < 8
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //
            res.WriteByte(0);

            // 4bytes cmp < 8
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //
            res.WriteByte(0);

            // 4bytes cmp < 5
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //
           
            res.WriteByte(0);
            
            res.WriteByte(2);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);   
            
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
     
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
        
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            res.WriteByte(0);
            res.WriteByte(0);

            Router.Send(client, (ushort) MsgPacketId.recv_chara_get_createinfo_r, res);
        }
    }
}