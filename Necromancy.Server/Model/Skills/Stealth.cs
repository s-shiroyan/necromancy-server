using Arrowgene.Services.Buffers;
using Arrowgene.Services.Logging;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Data.Setting;
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
        private SkillBaseSetting _skillSetting;

        public Stealth(NecServer server, NecClient client, int skillId)
        {
            _server = server;
            _client = client;
            _skillid = skillId;
            _logger = LogProvider.Logger<NecLogger>(this);
            if (!_server.SettingRepository.SkillBase.TryGetValue(skillId, out _skillSetting))
            {
                _logger.Error($"Could not get SkillBaseSetting for skillId : {skillId}");
            }

        }

        public void StartCast()
        {
            _logger.Debug($"CastingTime : {_skillSetting.CastingTime}");
            RecvSkillStartCastSelf startCast = new RecvSkillStartCastSelf(_skillid, _skillSetting.CastingTime);
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

            int state = _client.Character.GetState();
            int newState = 0;
            if (_client.Character.IsStealthed())
            {
                newState = _client.Character.ClearStateBit(0x8);
                res11.WriteInt32(newState);
            }
            else
            {
                newState = _client.Character.AddStateBit(0x8);
                res11.WriteInt32(newState); 
            }
            RecvCharaNotifyStateflag stateFlag = new RecvCharaNotifyStateflag(_client.Character.InstanceId, newState);
            _server.Router.Send(_client.Map, stateFlag);

            //0bxxxxxxx1 - 1 Soul Form / 0 Normal  | (Soul form is Glowing with No armor) 
            //0bxxxxxx1x - 1 Battle Pose / 0 Normal
            //0bxxxxx1xx - 1 Block Pose / 0 Normal | (for coming out of stealth while blocking)
            //0bxxxx1xxx - 1 transparent / 0 solid  | (stealth in party partial visibility)
            //0bxxx1xxxx -
            //0bxx1xxxxx - 1 invisible / 0 visible  | (Stealth to enemies)
            //0bx1xxxxxx - 1 blinking  / 0 solid    | (10  second invulnerability blinking)
            //0b1xxxxxxx - 

            //Task.Delay(TimeSpan.FromSeconds(30)).ContinueWith
            //(t1 =>
            //{
            //IBuffer res12 = BufferProvider.Provide();
            //res12.WriteInt32(_client.Character.InstanceId);
            //res12.WriteInt32(2 ^ 16); //2^16 is normal state.  0001 0000 0000 0000 0000
            //_server.Router.Send(_client.Map, (ushort)AreaPacketId.recv_chara_notify_stateflag, res12, ServerType.Area);
            //}
            //);

            //IBuffer res12 = BufferProvider.Provide();
            //res12.WriteByte(1);
            //_server.Router.Send(_client, (ushort)AreaPacketId.recv_cloak_notify_open, res12, ServerType.Area);

        }

    }
}
