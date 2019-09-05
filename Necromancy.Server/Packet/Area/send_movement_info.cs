using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_movement_info : Handler
    {
        public send_movement_info(NecServer server) : base(server)
        {

        }

        public override ushort Id => (ushort)AreaPacketId.send_movement_info;

        int x = 0;

        public override void Handle(NecClient client, NecPacket packet)
        {

            byte a = 0;
            int b = 0;
            byte c = 0;
            byte d = 0;
            int e = 0;
            byte f = 0;
            int g = 0;
            byte h = 0;
            byte i = 0;
            byte j = 0;
            int k = 0;
            int l = 0;
            byte m = 0;
            



            if (client.Character != null)
            {
                client.Character.X = packet.Data.ReadFloat();
                client.Character.Y = packet.Data.ReadFloat();
                client.Character.Z = packet.Data.ReadFloat();
                client.Character.a = packet.Data.ReadByte();
                client.Character.b = packet.Data.ReadByte();
                client.Character.b2 = packet.Data.ReadByte();
                client.Character.c = packet.Data.ReadByte();
                d = packet.Data.ReadByte();
                e = packet.Data.ReadInt16();
                f = packet.Data.ReadByte();
                g = packet.Data.ReadInt32();
                h = packet.Data.ReadByte();
                i = packet.Data.ReadByte();
                j = packet.Data.ReadByte();
                k = packet.Data.ReadInt32();
                l = packet.Data.ReadByte();
                client.Character.movementAnim = packet.Data.ReadByte();
                client.Character.animJump = packet.Data.ReadByte();
                
            }

           
            {
                //for (byte xd = 0; xd < 255; xd++)
                
                    IBuffer res2 = BufferProvider.Provide();

                    res2.WriteInt32(client.Character.Id);//Character ID
                    res2.WriteFloat(client.Character.X);
                    res2.WriteFloat(client.Character.Y);
                    res2.WriteFloat(client.Character.Z);

                    res2.WriteByte(0);
                    res2.WriteByte(0);
                    res2.WriteByte(0);
                    res2.WriteInt16(0);
                    res2.WriteByte(0);
                    res2.WriteByte(client.Character.movementAnim); //MOVEMENT ANIM
                    res2.WriteByte(client.Character.animJump);




                    Router.Send(client.Map, (ushort)AreaPacketId.recv_0xE8B9, res2, client);

                    //System.Threading.Thread.Sleep(1000);
                
                IBuffer res = BufferProvider.Provide();

                res.WriteInt32(client.Character.Id);//Character ID
                res.WriteFloat(client.Character.X);
                res.WriteFloat(client.Character.Y);
                res.WriteFloat(client.Character.Z);
                res.WriteByte(client.Character.viewOffset);//View offset
                res.WriteByte(client.Character.viewOffset);//Character state?

                //Router.Send(client.Map, (ushort)AreaPacketId.recv_0x6B6A, res, client); 

                Router.Send(client.Map, (ushort)AreaPacketId.recv_object_point_move_notify, res, client);


            }
        }
    }
}