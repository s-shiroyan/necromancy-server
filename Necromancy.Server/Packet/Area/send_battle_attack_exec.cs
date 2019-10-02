using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_battle_attack_exec : Handler
    {
        public send_battle_attack_exec(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_battle_attack_exec;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int Unknown1 = packet.Data.ReadInt32();
            int TargetID = packet.Data.ReadInt32();
            int Unknown2 = packet.Data.ReadInt32();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(TargetID);

            Router.Send(client, (ushort) AreaPacketId.recv_battle_attack_exec_direct_r, res);
        }
    }
}