using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_battle_attack_next : ClientHandler
    {
        public send_battle_attack_next(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_battle_attack_next;

        

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //0 means success
            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_attack_next_r, res, client);   
        }
    }
}
