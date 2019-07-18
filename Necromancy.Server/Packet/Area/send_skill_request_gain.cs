using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_skill_request_gain : Handler
    {
        public send_skill_request_gain(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_skill_request_gain;

        public override void Handle(NecClient client, NecPacket packet)
        {
            //uint skillID = packet.Data.ReadUInt32();
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);//1 = failed to aquire skill, 0 = success? but no skill aquired 
            Router.Send(client, (ushort) AreaPacketId.recv_skill_request_gain_r, res);            
        }
    }
}