using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System.Threading;

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
            int skillID = packet.Data.ReadInt32();
            
            //SendSkillStartCastSelf(client, skillID);
            //SendSkillStartCast(client, skillID);
            SendSkillStartCastExR(client, skillID);
        }

        private void SendSkillStartCastSelf(NecClient client, int skillID)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(skillID);
            res.WriteFloat(3);
            Router.Send(client, (ushort) AreaPacketId.recv_skill_start_cast_self, res);
        }

        private void SendSkillStartCast(NecClient client, int skillID)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(skillID);
            res.WriteFloat(3);
            Router.Send(client, (ushort) AreaPacketId.recv_skill_start_cast_r, res);
        }

        private void SendSkillStartCastExR(NecClient client, int skillID)
        {
            //5000001,45,125,120,125,1,45,1,1,0 ,,, #, decoy,
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);//Error if not 0
            res.WriteFloat(1);//Cast time

            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);

            res.WriteInt32(0);

            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);

            res.WriteInt32(0);

            Router.Send(client, (ushort)AreaPacketId.recv_skill_start_cast_ex_r, res);
        }
    }
}