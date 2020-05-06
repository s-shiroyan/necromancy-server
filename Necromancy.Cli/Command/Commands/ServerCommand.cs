using System;
using System.Collections.Generic;
using System.Threading;
using Necromancy.Cli.Argument;
using Necromancy.Server;
using Necromancy.Server.Setting;

namespace Necromancy.Cli.Command.Commands
{
    public class ServerCommand : ConsoleCommand, ISwitchConsumer
    {
        private NecServer _server;
        private readonly LogWriter _logWriter;
        private bool _service;
        
        public ServerCommand(LogWriter logWriter)
        {
            _service = false;
            _logWriter = logWriter;
            Switches = new List<ISwitchProperty>();
            Switches.Add(
                new SwitchProperty<bool>(
                    "--service",
                    "--service=true (true|false)",
                    "Run the server as a dedicated service",
                    bool.TryParse,
                    (result => _service = result)
                )
            );
        }

        public List<ISwitchProperty> Switches { get; }

        public override void Shutdown()
        {
            if (_server != null)
            {
                _server.Stop();
            }
        }

        public override CommandResultType Handle(ConsoleParameter parameter)
        {
            if (_server == null)
            {
                NecSetting setting = new NecSetting();
                _server = new NecServer(setting);
            }

            if (parameter.Arguments.Contains("start"))
            {
                _server.Start();
                if (_service)
                {
                    while (_server.Running)
                    {
                        Thread.Sleep(TimeSpan.FromMinutes(5));
                    }

                    return CommandResultType.Exit;
                }

                return CommandResultType.Completed;
            }

            if (parameter.Arguments.Contains("stop"))
            {
                _server.Stop();
                return CommandResultType.Completed;
            }

            return CommandResultType.Continue;
        }

        public override string Key => "server";


        public override string Description =>
            $"Wizardry Online Server. Ex.:{Environment.NewLine}server start{Environment.NewLine}server stop";
    }
}
