using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_chara_pose_ladder : Handler
    {
        public send_chara_pose_ladder(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_chara_pose_ladder;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            //res.WriteByte(1);
            //res.WriteByte(1);
            res.WriteInt32(0);  //  Error

            Router.Send(client, (ushort) AreaPacketId.recv_chara_pose_ladder_r, res);
        }
    }
}