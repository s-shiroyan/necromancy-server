using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_battle_release_attack_pose : ClientHandler
    {
        public send_battle_release_attack_pose(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_battle_release_attack_pose;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            
            Router.Send(client, (ushort) AreaPacketId.recv_battle_release_attack_pose_r, res);  

            SendBatttleAttackPoseEndNotify(client);          
        }

        private void SendBatttleAttackPoseEndNotify(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            
            res.WriteInt32(client.Character.Id);

            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_attack_pose_end_notify, res, client);

            client.Character.weaponEquipped = false;
        }
    }
}