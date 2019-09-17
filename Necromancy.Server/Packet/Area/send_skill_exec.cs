using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System.Threading;

namespace Necromancy.Server.Packet.Area
{
    public class send_skill_exec : Handler
    {
        public send_skill_exec(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_skill_exec;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int targetID = packet.Data.ReadInt32();
            float X = packet.Data.ReadFloat();
            float Y = packet.Data.ReadFloat();
            float Z = packet.Data.ReadFloat();
            //float cooldownTime = packet.Data.ReadFloat();
            float stickTime = packet.Data.ReadFloat();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);//1 - not enough distance, 2 - unable to use skill: 2 error, 0 - success
            res.WriteFloat(1);//Recast time(server does a lookup here i think)
            res.WriteFloat(stickTime);//Stick time(player movement stopped for this long)
            Router.Send(client, (ushort)AreaPacketId.recv_skill_exec_r, res);
        }
    }
}