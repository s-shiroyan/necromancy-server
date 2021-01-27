using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_battle_guard_start : ClientHandler
    {
        public send_battle_guard_start(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_battle_guard_start;


        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteUInt32(client.Character.InstanceId); //Character ID

            Router.Send(client.Map, (ushort) AreaPacketId.recv_dbg_battle_guard_start_notify, res, ServerType.Area,
                client);
            client.Character.AddStateBit(CharacterState.BlockPose);

        }
    }
}
