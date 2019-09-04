using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_battle_attack_pose : Handler
    {
        public send_battle_attack_pose(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_battle_attack_pose;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            Router.Send(client.Map, (ushort) AreaPacketId.recv_battle_attack_pose_r, res);

            SendBattleAttackPoseStartNotify(client);    
        }

        private void SendBattleAttackPoseStartNotify(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            
            res.WriteInt32(client.Character.Id);

            Router.Send(client.Map, (ushort) AreaPacketId.recv_battle_attack_pose_start_notify, res, client);

          

        }
    }
}