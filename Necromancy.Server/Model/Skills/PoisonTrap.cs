using Arrowgene.Services.Logging;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Logging;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Receive;
using System.Numerics;
using System.Collections.Generic;

namespace Necromancy.Server.Model.Skills
{
   class PoisonTrap : IInstance
    {
        public uint InstanceId { get; set; }

        private NecClient _client;
        private readonly NecLogger _logger;
        private readonly NecServer _server;
        private int _skillId;

        public PoisonTrap(NecServer server, NecClient client, int skillId)
        {
            _server = server;
            _client = client;
            _skillId = skillId;
            _logger = LogProvider.Logger<NecLogger>(this);
        }

        public void StartCast()
        {
            _logger.Debug($"PoisonTrap StartCast");
            RecvSkillStartCastSelf startCast = new RecvSkillStartCastSelf(_skillId, 2.0F);
            _server.Router.Send(_client.Map, startCast);
            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify((int)_client.Character.InstanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionSkillStartCast brStartCast = new RecvBattleReportActionSkillStartCast(_skillId);

            brList.Add(brStart);
            brList.Add(brStartCast);
            brList.Add(brEnd);
            _server.Router.Send(_client.Map, brList);

        }

        public void SkillExec()
        {
            Vector3 trgCoord = new Vector3(_client.Character.X, _client.Character.Y, _client.Character.Z);
            if (!int.TryParse($"{_skillId}".Substring(1, 6)+1, out int effectId))
            {
                _logger.Error($"Creating effectId from skillid [{_skillId}]");
            }

            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify((int)_client.Character.InstanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionSkillExec brExec = new RecvBattleReportActionSkillExec(_skillId);

            brList.Add(brStart);
            brList.Add(brExec);
            brList.Add(brEnd);
            _server.Router.Send(_client.Map, brList);
            _logger.Debug($"SpearTrap effectId [{effectId}]");
            RecvDataNotifyEoData eoData = new RecvDataNotifyEoData((int)InstanceId, (int)_client.Character.InstanceId, effectId, trgCoord, 2, 2);
            _server.Router.Send(_client.Map, eoData);
            RecvEoNotifyDisappearSchedule eoDisappear = new RecvEoNotifyDisappearSchedule((int)InstanceId, 30.0F);
            _server.Router.Send(_client.Map, eoDisappear);

        }

    }
}
