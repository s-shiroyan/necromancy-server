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
                a = packet.Data.ReadByte();
                b = packet.Data.ReadInt16();
                c = packet.Data.ReadByte();
                d = packet.Data.ReadByte();
                e = packet.Data.ReadInt16();
                f = packet.Data.ReadByte();
                g = packet.Data.ReadInt32();
                h = packet.Data.ReadByte();
                i = packet.Data.ReadByte();
                j = packet.Data.ReadByte();
                k = packet.Data.ReadInt32();
                l = packet.Data.ReadInt16();
                m = packet.Data.ReadByte();
                
            }



            {
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