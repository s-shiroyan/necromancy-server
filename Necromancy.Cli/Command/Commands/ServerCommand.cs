using System;
using Necromancy.Server;
using Necromancy.Server.Setting;

namespace Necromancy.Cli.Command.Commands
{
    public class ServerCommand : ConsoleCommand
    {
        private NecServer _server;

        public ServerCommand()
        {
        }

        public override void Shutdown()
        {
            _server.Stop();
        }

        protected override void Run()
        {
            if (_server == null)
            {
                NecSetting setting = new NecSetting();
                _server = new NecServer(setting);
            }

            if (Arguments.Contains("start"))
            {
                _server.Start();
            }

            if (Arguments.Contains("stop"))
            {
                _server.Stop();
            }
        }

        public override string Key => "server";
        public override bool RequireArgs => true;

        public override string Description =>
            $"Wizardry Online Server. Ex.:{Environment.NewLine}server start{Environment.NewLine}server stop";
    }
}
