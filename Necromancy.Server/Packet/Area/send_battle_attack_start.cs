using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_battle_attack_start : Handler
    {
        public send_battle_attack_start(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_battle_attack_start;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteByte(0);//Bool

            //Router.Send(client, (ushort) AreaPacketId.recv_battle_attack_start, res);

            SendNPCStateUpdateNotify(client);
        }

        private void SendNPCStateUpdateNotify(NecClient client)
        {
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(1);
            res2.WriteInt32(1);
            Router.Send(client, (ushort)AreaPacketId.recv_npc_state_update_notify, res2);
        }
    }
}