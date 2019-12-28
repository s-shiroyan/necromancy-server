using Arrowgene.Services.Logging;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Logging;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Receive;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Tasks;
using System.Numerics;
using System.Collections.Generic;

namespace Necromancy.Server.Model.Skills
{
    public class TrapStack : IInstance
    {
        public uint InstanceId { get; set; }

        private NecClient _client;
        private readonly NecLogger _logger;
        private readonly NecServer _server;
        private uint _ownerInstanceId;
        public int _trapRadius { get; }
        public TrapTask _trapTask;
        private Map _map;
        private Vector3 _trapPos;
        public TrapStack(NecServer server, NecClient client, Vector3 trapPos, int trapRadius)
        {
            _server = server;
            _client = client;
            _map = _client.Map;
            _ownerInstanceId = client.Character.InstanceId;
            _trapPos = trapPos;
            _logger = LogProvider.Logger<NecLogger>(this);
            _trapRadius = trapRadius;
        }

        public void StartCast(SkillBaseSetting skillBase)
        {
            _logger.Debug($"Trap StartCast skillBase.Id [{skillBase.Id}] skillBase.CastingTime [{skillBase.CastingTime}]");
            RecvSkillStartCastSelf startCast = new RecvSkillStartCastSelf(skillBase.Id, skillBase.CastingTime);
            _server.Router.Send(startCast, _client) ;
            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(_client.Character.InstanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionSkillStartCast brStartCast = new RecvBattleReportActionSkillStartCast(skillBase.Id);

            brList.Add(brStart);
            brList.Add(brStartCast);
            brList.Add(brEnd);
            _server.Router.Send(_client.Map, brList);

        }

        public void SkillExec(Trap trap, bool isBaseTrap)
        {
            Vector3 trgCoord = new Vector3(_client.Character.X, _client.Character.Y, _client.Character.Z);
            if (!int.TryParse($"{trap._skillId}".Substring(1, 6) + 1, out int effectId))
            {
                _logger.Error($"Creating effectId from skillid [{trap._skillId}]");
            }

            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(_client.Character.InstanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionSkillExec brExec = new RecvBattleReportActionSkillExec(trap._skillId);

            brList.Add(brStart);
            brList.Add(brExec);
            brList.Add(brEnd);
            _server.Router.Send(_client.Map, brList);
            _logger.Debug($"SpearTrap effectId [{effectId}]");
            RecvDataNotifyEoData eoData = new RecvDataNotifyEoData(trap.InstanceId, _client.Character.InstanceId, effectId, trgCoord, 2, 2);
            _server.Router.Send(_map, eoData);

            if (isBaseTrap)
            {
                _trapTask = new TrapTask(_server, _map, _trapPos, _ownerInstanceId, trap, this.InstanceId);
                _trapTask.AddTrap(trap);
                _map.AddTrap(this.InstanceId, this);
                _trapTask.Start();
            }
            else
            {
                _trapTask.AddTrap(trap);
            }
        }
    }
}
