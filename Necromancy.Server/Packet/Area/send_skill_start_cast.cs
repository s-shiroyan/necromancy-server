using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System.Threading;

namespace Necromancy.Server.Packet.Area
{
    public class send_skill_start_cast : Handler
    {
        bool skillExec = false;
        public send_skill_start_cast(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_skill_start_cast;

        public override void Handle(NecClient client, NecPacket packet)
        {
            
            SendSkillStartCast(client);
            //SendSkillStartCastSelf(client);
            SendSkillStartCastExR(client);
            if(skillExec)
                SendSkillExecR(client);
        }

        private void SendSkillStartCastSelf(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(1);
            res.WriteFloat(10);
            Router.Send(client, (ushort) AreaPacketId.recv_skill_start_cast_self, res);
        }
        private void SendSkillStartCast(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteFloat(10);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_skill_start_cast_r, res, client);
        }
        private void SendSkillStartCastExR(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0x3910);
            res.WriteFloat(10);

            res.WriteInt32(0x3910);
            res.WriteInt32(0x3910);
            res.WriteInt32(0x3910);
            res.WriteInt32(0x3910);

            res.WriteInt32(0x30D5);

            res.WriteInt32(0x3910);
            res.WriteInt32(0x3910);
            res.WriteInt32(0x3910);
            res.WriteInt32(0x3910);

            res.WriteInt32(0x30D5);

            Thread.Sleep(5000);
            skillExec = true;
        }

        private void SendSkillExecR(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);//1 - not enough distance, 2 - unable to use skill: 2 error, 0 - success
            res.WriteFloat(10);//Cooldown time
            res.WriteFloat(10);//Cast time?
            Router.Send(client, (ushort) AreaPacketId.recv_skill_exec_r, res);
            skillExec = false;
        }
    }
}