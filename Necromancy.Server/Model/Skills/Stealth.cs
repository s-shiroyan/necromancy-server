using Arrowgene.Services.Buffers;
using Arrowgene.Services.Logging;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.Skills;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;
using System;
using System.Collections.Generic;
using Necromancy.Server.Packet.Response;
using System.Threading.Tasks;

namespace Necromancy.Server.Model.Skills
{
   class Stealth : IInstance
    {
        public uint InstanceId { get; set; }

        private NecClient _client;
        private readonly NecLogger _logger;
        private readonly NecServer _server;
        private int _skillid;

        public Stealth(NecServer server, NecClient client, int skillId)
        {
            _server = server;
            _client = client;
            _skillid = skillId;
            _logger = LogProvider.Logger<NecLogger>(this);
        }

        public void StartCast()
        {
            RecvSkillStartCastSelf startCast = new RecvSkillStartCastSelf(_skillid, 0.0F);
            _server.Router.Send(_client.Map, startCast);
            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(_client.Character.InstanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionSkillStartCast brStartCast = new RecvBattleReportActionSkillStartCast(_skillid);

            brList.Add(brStart);
            brList.Add(brStartCast);
            brList.Add(brEnd);
            _server.Router.Send(_client.Map, brList);

        }

        public void SkillExec()
        {
            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(_client.Character.InstanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionSkillExec brExec = new RecvBattleReportActionSkillExec(_client.Character.skillStartCast);
            brList.Add(brStart);
            brList.Add(brExec);
            brList.Add(brEnd);
            _server.Router.Send(_client.Map, brList);

            IBuffer res11 = BufferProvider.Provide();
            res11.WriteInt32(_client.Character.InstanceId);
            res11.WriteInt32(2 ^ 8); //2^16 is normal state.  0001 0000 0000 0000 0000
            _server.Router.Send(_client.Map, (ushort)AreaPacketId.recv_chara_notify_stateflag, res11, ServerType.Area);

            Task.Delay(TimeSpan.FromSeconds(30)).ContinueWith
            (t1 =>
                {
                    IBuffer res12 = BufferProvider.Provide();
                    res12.WriteInt32(_client.Character.InstanceId);
                    res12.WriteInt32(2 ^ 16); //2^16 is normal state.  0001 0000 0000 0000 0000
                    _server.Router.Send(_client.Map, (ushort)AreaPacketId.recv_chara_notify_stateflag, res12, ServerType.Area);
                }
            );

            //IBuffer res12 = BufferProvider.Provide();
            //res12.WriteByte(1);
            //_server.Router.Send(_client, (ushort)AreaPacketId.recv_cloak_notify_open, res12, ServerType.Area);

        }

    }
}
