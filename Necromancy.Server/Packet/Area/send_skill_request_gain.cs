using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_skill_request_gain : ClientHandler
    {
        public send_skill_request_gain(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_skill_request_gain;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int skillID = packet.Data.ReadInt32(),
                 skillLevel = packet.Data.ReadInt32();

            SendSkillTreeGain(client, skillID, skillLevel);
            //uint skillID = packet.Data.ReadUInt32();
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);//1 = failed to aquire skill, 0 = success? but no skill aquired 
            Router.Send(client, (ushort) AreaPacketId.recv_skill_request_gain_r, res, ServerType.Area);            
        }

        private void SendSkillTreeGain(NecClient client, int skillID, int skillLevel)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(skillID);
            res.WriteInt32(skillLevel);//Level?
            res.WriteByte(0); //Bool
            res.WriteByte(0); //Bool

            Router.Send(client, (ushort) AreaPacketId.recv_skill_tree_gain, res, ServerType.Area);
        }
    }
}