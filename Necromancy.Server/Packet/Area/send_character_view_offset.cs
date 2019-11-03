using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_character_view_offset : ClientHandler
    {
        public send_character_view_offset(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_character_view_offset;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte view = packet.Data.ReadByte();

            //(client.Character != null)
                client.Character.viewOffset = view;
            //Console.WriteLine($"View Offset = {view}");
        }
    }
}
