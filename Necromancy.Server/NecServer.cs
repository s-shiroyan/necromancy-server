using System;
using Arrowgene.Services.Buffers;
using Arrowgene.Services.Networking.Tcp.Server.AsyncEvent;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Area;
using Necromancy.Server.Packet.Auth;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Msg;
using Necromancy.Server.Setting;

namespace Necromancy.Server
{
    public class NecServer
    {
        public NecSetting Setting { get; }
        public PacketRouter Router { get; }

        private readonly NecQueueConsumer _authConsumer;
        private readonly NecQueueConsumer _msgConsumer;
        private readonly NecQueueConsumer _areaConsumer;
        private readonly AsyncEventServer _authServer;
        private readonly AsyncEventServer _msgServer;
        private readonly AsyncEventServer _areaServer;
        private readonly NecLogger _logger;

        public NecServer(NecSetting setting)
        {
            Setting = setting;
            Router = new PacketRouter();
            _authConsumer = new NecQueueConsumer(Setting);
            _authConsumer.SetIdentity("Auth");
            _msgConsumer = new NecQueueConsumer(Setting);
            _msgConsumer.SetIdentity("Msg");
            _areaConsumer = new NecQueueConsumer(Setting);
            _areaConsumer.SetIdentity("Area");

            _authServer = new AsyncEventServer(
                Setting.ListenIpAddress,
                Setting.AuthPort,
                _authConsumer
            );

            _msgServer = new AsyncEventServer(
                Setting.ListenIpAddress,
                Setting.MsgPort,
                _msgConsumer
            );

            _areaServer = new AsyncEventServer(
                Setting.ListenIpAddress,
                Setting.AreaPort,
                _areaConsumer
            );

            LoadHandler();
        }

        public void Start()
        {
            _authServer.Start();
            _msgServer.Start();
            _areaServer.Start();
        }

        public void Stop()
        {
            _authServer.Stop();
            _msgServer.Stop();
            _areaServer.Stop();
        }

        private void LoadHandler()
        {
            // Authentication Handler
            _authConsumer.AddHandler(new send_base_check_version_auth(this));
            _authConsumer.AddHandler(new send_base_authenticate(this));
            _authConsumer.AddHandler(new send_base_get_worldlist(this));
            _authConsumer.AddHandler(new send_base_select_world(this));

            // Message Handler
            _msgConsumer.AddHandler(new send_base_check_version_msg(this));
            _msgConsumer.AddHandler(new send_base_login(this));
            _msgConsumer.AddHandler(new send_soul_create(this));
            _msgConsumer.AddHandler(new send_soul_select(this));
            _msgConsumer.AddHandler(new send_soul_authenticate_passwd(this));
            _msgConsumer.AddHandler(new send_chara_get_list(this));
            _msgConsumer.AddHandler(new send_cash_get_url_common(this));
            _msgConsumer.AddHandler(new send_chara_get_createinfo(this));
            _msgConsumer.AddHandler(new send_soul_set_passwd(this));
            _msgConsumer.AddHandler(new send_chara_draw_bonuspoint(this));
            _msgConsumer.AddHandler(new send_chara_create(this));
            _msgConsumer.AddHandler(new send_channel_select(this));
            _msgConsumer.AddHandler(new send_chara_select(this));
            
            // Area Handler
            _areaConsumer.AddHandler(new send_base_check_version_area(this));
            _areaConsumer.AddHandler(new send_base_enter(this));
            _areaConsumer.AddHandler(new send_data_get_self_chara_data_request(this));
            _areaConsumer.AddHandler(new send_map_entry(this));
            _areaConsumer.AddHandler(new send_map_get_info(this));
            _areaConsumer.AddHandler(new send_map_enter(this));
            _areaConsumer.AddHandler(new send_shortcut_request_data(this));
            _areaConsumer.AddHandler(new send_skill_request_info(this));
            _areaConsumer.AddHandler(new send_sv_conf_option_request(this));
        }
    }
}