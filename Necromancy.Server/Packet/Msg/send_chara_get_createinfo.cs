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
            res.WriteInt32(0); //4

            // 4bytes cmp, 8 -> ja
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0); //8

            res.WriteByte(0);

            // 4bytes cmp, 8 -> ja
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteByte(0);

            // 4bytes cmp, 5 -> ja
            res.WriteByte(1); //15
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            // 4bytes
            res.WriteByte(1);
            res.WriteByte(0); //20
            res.WriteByte(0);
            res.WriteByte(0);

            // 4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            // 4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0); // 30
            res.WriteByte(0);

            // 4bytes
            res.WriteByte(1); //32 (point edx here)
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            // 2byte 7x loop
            // 2 bytes
            res.WriteByte(1);
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(2);
            res.WriteByte(0);

            //2 byte
            res.WriteByte(3); //40
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(1);
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(1);
            res.WriteByte(0); //45

            // 2 byte
            res.WriteByte(2);
            res.WriteByte(0);

            // 2 byte
            res.WriteByte(3); //48
            res.WriteByte(0);
            //end loop

            // 4 byte cmp, 10 -> ja
            res.WriteByte(1); //50
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            // 4 byte
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //1 byte cmp,1 -> sete (bool)
            res.WriteByte(3);
            
            // 19x4byte loop
            // 4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // 4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // 4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // 4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // 4bytes
            res.WriteByte(2);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // 4bytes
            res.WriteByte(2);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // 4bytes
            res.WriteByte(2);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // 4bytes
            res.WriteByte(2);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // 4bytes
            res.WriteByte(3);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // 4bytes
            res.WriteByte(3);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // 4bytes
            res.WriteByte(3);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // 4bytes
            res.WriteByte(3);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // 4bytes
            res.WriteByte(4);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // 4bytes
            res.WriteByte(4);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // 4bytes
            res.WriteByte(4);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // 4bytes
            res.WriteByte(4);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // 4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // 4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // 4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            // end loop
            
            
            // loop 19x
            
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end
            
            // end loop
            
            
            // loop 19x

            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(2);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(2);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(2);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(2);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(2);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(2);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(2);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(2);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(2);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(3);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(3);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(3);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(3);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(3);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(3);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //end loop
            
            
            //Read 1 byte
            res.WriteByte(0);
 
            //Read 4 byte  cmp,140 -> JA
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes cmp,14 -> JA
            res.WriteByte(2);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            
            // loop 19x
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(2);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            
            //end loop

            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            // loop 19x
            // 004E3863 | E8 5810FBFF              | call wizardryonline_no_encryption.4948C0            |
            // start loop 19x
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end     
            // start
            // loop 2x
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            //4bytes - 004948EE
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            
            //4bytes
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop
            
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end
            
            // end loop 19X
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
                 // loop 19x
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //end loop
            
            
            
            // Read Byte
            res.WriteByte(0);
            
            //4bytes cmp, E ( < 14) -> JA
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            
            
            
                      
            //4bytes
        //   res.WriteByte(0xEE);
        //   res.WriteByte(0);
        //   res.WriteByte(0);
        //   res.WriteByte(0); 
        //   
        //   //4bytes
        //   res.WriteByte(0xDD);
        //   res.WriteByte(0);
        //   res.WriteByte(0);
        //   res.WriteByte(0);


            Router.Send(client, (ushort) MsgPacketId.recv_chara_get_createinfo_r, res);
        }
    }
}