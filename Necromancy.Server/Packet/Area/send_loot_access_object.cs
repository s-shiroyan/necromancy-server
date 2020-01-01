using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_loot_access_object : ClientHandler
    {
        public send_loot_access_object(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_loot_access_object;
        private short i = 0;
        public override void Handle(NecClient client, NecPacket packet)
        {
            int objectID = packet.Data.ReadInt32();
            Console.WriteLine($"first Int 32 is : {objectID}");

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(objectID);

            Router.Send(client, (ushort) AreaPacketId.recv_loot_access_object_r, res2, ServerType.Area);

            ///////////////test forced item receive. to be expanded upon using a drop table and slot checking logic
            ///
            i++;
            IBuffer res = BufferProvider.Provide();

            res.WriteInt64(50100301); //Item Object ID 

            res.WriteCString("Simple Camp Cheater loot"); //Name

            res.WriteInt32(45); //Wep type

            res.WriteInt32(1);

            res.WriteByte(8); //Number of items

            res.WriteInt32(0); //Item status 0 = identified  

            res.WriteInt32(50100301); //Item icon 50100301 = camp
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteInt32(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0); // bool
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteByte(0); // 0 = adventure bag. 1 = character equipment
            res.WriteByte(0); // 0~2
            res.WriteInt16(i); // bag index

            res.WriteInt32(0); //bit mask. This indicates where to put items.   e.g. 01 head 010 arm 0100 feet etc (0 for not equipped)

            res.WriteInt64(i);

            res.WriteInt32(1);

            Router.Send(client, (ushort)AreaPacketId.recv_item_instance_unidentified, res, ServerType.Area);
        
    }
    }
}
