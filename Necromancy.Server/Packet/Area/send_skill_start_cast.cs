using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_skill_start_cast : Handler
    {
        public send_skill_start_cast(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_skill_start_cast;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);//1 - not enough distance, 2 - unable to use skill: 2 error, 0 - success
            res.WriteFloat(10);//Cooldown time
            res.WriteFloat(10);//Cast time?
            Router.Send(client, (ushort) AreaPacketId.recv_skill_exec_r, res);
        }
    }
}