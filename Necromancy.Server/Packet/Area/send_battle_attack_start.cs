using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_battle_attack_start : ClientHandler
    {
        public send_battle_attack_start(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_battle_attack_start;

        

        public override void Handle(NecClient client, NecPacket packet)
        {

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(client.Character.InstanceId); //0 means success
            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_attack_start_r, res, ServerType.Area, client);

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(client.Character.InstanceId);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_attack_start, res2, ServerType.Area, client);


        }

    }
}
