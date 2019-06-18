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

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            // 4bytes (004E90F6) cmp, 8 -> ja
            byte entries = 2;
            res.WriteByte(entries);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            // 004E913F (jb loop)
            for (int i = 0; i < entries; i++)
            {
                res.WriteByte((byte) i);
            }

            // 4bytes (004E9174) cmp, 8 -> ja
            entries = 2;
            res.WriteByte(entries);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            // 004E91BF (jb loop)
            for (int i = 0; i < entries; i++)
            {
                res.WriteByte((byte) i);
            }

            // 4bytes (004E91F4) cmp, 5 -> ja
            entries = 2;
            res.WriteByte(entries);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            // note: advance 5 * 0x20 | 5 * 32


            // 4bytes 004E921C | E8 BF76FFFF | call wizardryonline_no_encryption.4E08E0 
            for (int i = 0; i < entries; i++)
            {
                wo_4E08E0(res);
            }

            // end  call wizardryonline_no_encryption.4E08E0 

            // 4 byte cmp, 10 -> ja (0019F954) loop create class? | 004E92B3 loop read data?
            entries = 2;
            res.WriteByte(entries); //50
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            for (int i = 0; i < entries; i++)
            {
                // 004E9297
                wo_4E3700(res);
            }


            //Read 4 byte (004E92E8) cmp,140(0x8C) -> JA (320)
            entries = 2;
            res.WriteByte(entries); // 0 = no elf , 1 = elf
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            for (int i = 0; i < entries; i++)
            {
                wo_4E37F0(res);
            }

            //4bytes cmp, E ( < 14) -> JA
            entries = 2;
            res.WriteByte(entries);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            
            for (int i = 0; i < entries; i++)
            {
                wo_4E0970(res);
            }


            Router.Send(client, (ushort) MsgPacketId.recv_chara_get_createinfo_r, res);
        }

        private void wo_4E0970(IBuffer res)
        {
            //4bytes
              res.WriteByte(1);
              res.WriteByte(0);
              res.WriteByte(0);
              res.WriteByte(0); 
              
              //1bytes
              res.WriteByte(2); 
            
            
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
              
              
              //4bytes
              res.WriteByte(1);
              res.WriteByte(0);
              res.WriteByte(0);
              res.WriteByte(0); 
              
              //4bytes
              res.WriteByte(5);
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
              
              
                            
              //4bytes
              res.WriteByte(1);
              res.WriteByte(0);
              res.WriteByte(0);
              res.WriteByte(0); 
              
              //4bytes
              res.WriteByte(4);
              res.WriteByte(0);
              res.WriteByte(0);
              res.WriteByte(0); 
              
                            
              //4bytes
              res.WriteByte(1);
              res.WriteByte(0);
              res.WriteByte(0);
              res.WriteByte(0); 
              
              
              //1bytes
              res.WriteByte(2); 
        }
        

        private void wo_4E37F0(IBuffer res)
        {
            //4bytes cmp,14 -> JA
            res.WriteByte(0x14);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //4bytes
            res.WriteByte(3);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //4bytes
            res.WriteByte(4);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);


            for (int i = 0; i < 19; i++)
            {
                //4bytes
                res.WriteByte((byte) i);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
            }

            for (int i = 0; i < 19; i++)
            {
                wo_4948C0(res);
            }


            for (int i = 0; i < 19; i++)
            {
                //4bytes
                res.WriteByte(1);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
            }


            // Read Byte
            res.WriteByte(0);
        }

        private void wo_4948C0(IBuffer res)
        {
            // loop 19x
            // 004E3863 | E8 5810FBFF              | call wizardryonline_no_encryption.4948C0            |
            // start loop 19x
            // loop 2x
            //4bytes
            res.WriteByte(2);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //4bytes - 004948EE
            res.WriteByte(1);
            // Read Byte
            res.WriteByte(1);
            // Read Byte
            res.WriteByte(1);

            //4bytes
            res.WriteByte(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            // Read Byte
            res.WriteByte(1);
            // Read Byte
            res.WriteByte(0);
            // Read Byte
            res.WriteByte(0);
            // end loop

            // Read Byte
            res.WriteByte(2);
            // Read Byte
            res.WriteByte(1);
            // Read Byte cmp,1 -> sete (bool)
            res.WriteByte(1);
            // Read Byte
            res.WriteByte(2);
            // Read Byte
            res.WriteByte(3);
            // Read Byte
            res.WriteByte(4);
            // Read Byte
            res.WriteByte(5);
            // Read Byte
            res.WriteByte(6);
            // end     
        }

        private void wo_4E3700(IBuffer res)
        {
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
            res.WriteByte(1);


            for (int i = 0; i < 19; i++)
            {
                // 4bytes
                res.WriteByte(1);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
            }


            // 004E3797
            for (int i = 0; i < 19; i++)
            {
                //4bytes
                res.WriteByte((byte) i);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                //4bytes - 004948EE
                res.WriteByte(1);
                // Read Byte
                res.WriteByte(2);
                // Read Byte
                res.WriteByte(3);

                //4bytes
                res.WriteByte(1);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                // Read Byte
                res.WriteByte(4);
                // Read Byte
                res.WriteByte(5);
                // Read Byte
                res.WriteByte(6);
                // end loop

                // Read Byte
                res.WriteByte(3);
                // Read Byte
                res.WriteByte(2);

                // Read Byte cmp,1 -> sete (bool)
                res.WriteByte(1);
                // Read Byte
                res.WriteByte(4);
                // Read Byte
                res.WriteByte(3);
                // Read Byte
                res.WriteByte(2);
                // Read Byte
                res.WriteByte(1);
                // Read Byte
                res.WriteByte(1);
                // end     
            }

            // 004E37BD
            for (int i = 0; i < 19; i++)
            {
                //4bytes
                res.WriteByte(1);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
            }


            //Read 1 byte (004E37C7)
            res.WriteByte(1);
        }

        private void wo_4E08E0(IBuffer res)
        {
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
        }
    }
}