using Arrowgene.Services.Buffers;
using Arrowgene.Services.Logging;
using Arrowgene.Services.Tasks;
using Necromancy.Server.Common;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.Skills;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;
using Necromancy.Server.Packet.Response;
using System;
using System.Threading;

namespace Necromancy.Server.Tasks
{
    public class CharacterTask : PeriodicTask
    {
        private readonly object LogoutLock = new object();
        private readonly NecLogger _logger;
        private NecServer _server;
        private NecClient _client;
        private int tickTime;
        private DateTime _logoutTime;
        private byte _logoutType;
        public CharacterTask(NecServer server, NecClient client)
        {
            _logger = LogProvider.Logger<NecLogger>(server);
            _server = server;
            _client = client;
            tickTime = 500;
            _logoutTime = DateTime.MinValue;
        }

        public override string Name { get; }
        public override TimeSpan TimeSpan { get; }
        protected override bool RunAtStart { get; }
        protected override void Execute()
        {
            while (_client.Character.characterActive)
            {
                if (_logoutTime != DateTime.MinValue)
                {
                    if (DateTime.Now >= _logoutTime)
                    {
                        LogOutRequest();
                    }
                }
                // ToDo Character task 
                Thread.Sleep(tickTime);
            }
            this.Stop();
        }
        public void Logout(DateTime logoutTime, byte logoutType)
        {
            lock (LogoutLock)
            {
                _logoutTime = logoutTime;
                _logoutType = logoutType;
            }
            _logger.Debug($"logoutTime [{logoutTime}] _logoutType [{_logoutType}]");
        }

        private void LogOutRequest()
        {
            _logoutTime = DateTime.MinValue;
            IBuffer res = BufferProvider.Provide();
            IBuffer res2 = BufferProvider.Provide();
            IBuffer res3 = BufferProvider.Provide();
            IBuffer res4 = BufferProvider.Provide();
            IBuffer res5 = BufferProvider.Provide();
            _logger.Debug($"_logoutType [{_logoutType}]");
            if (_logoutType == 0x00)
            {
                //res.WriteInt32(0);

                _server.Router.Send(_client, (ushort)AreaPacketId.recv_escape_start, res, ServerType.Area);
            }

            if (_logoutType == 0x01)
            {

                res.WriteInt32(0);
                _server.Router.Send(_client, (ushort)MsgPacketId.recv_chara_select_back_soul_select_r, res, ServerType.Msg);
                
                Thread.Sleep(4100);

                res = null;
                res = BufferProvider.Provide();
                res.WriteInt32(0);
                res.WriteByte(0);
                _server.Router.Send(_client, (ushort)MsgPacketId.recv_soul_authenticate_passwd_r, res, ServerType.Msg);
            }

            if (_logoutType == 0x02)
            {
                res.WriteInt32(0);
                _server.Router.Send(_client, (ushort)MsgPacketId.recv_chara_select_back_r, res, ServerType.Msg);
            }
        }
    }
}
