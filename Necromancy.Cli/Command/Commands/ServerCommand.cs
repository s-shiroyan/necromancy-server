using System;
using System.Collections.Generic;
using System.Threading;
using Arrowgene.Logging;
using Necromancy.Cli.Argument;
using Necromancy.Server;
using Necromancy.Server.Logging;
using Necromancy.Server.Setting;

namespace Necromancy.Cli.Command.Commands
{
    public class ServerCommand : ConsoleCommand, ISwitchConsumer
    {
        public static readonly ILogger Logger = LogProvider.Logger(typeof(ServerCommand));
        
        private const string SettingFile = "server_setting.json";
        private const string SecretFile = "server_secret.json";
        private NecServer _server;
        private bool _service;

        public ServerCommand()
        {
            _service = false;
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
                SettingProvider settingProvider = new SettingProvider();
                NecSetting setting = settingProvider.Load<NecSetting>(SettingFile);
                if (setting == null)
                {
                    Logger.Info($"No `{SettingFile}` file found, creating new");
                    setting = new NecSetting();
                    settingProvider.Save(setting, SettingFile);
                }

                SettingProvider secretsProvider = new SettingProvider(setting.SecretsFolder);
                NecSecret secret = secretsProvider.Load<NecSecret>(SecretFile);
                if (secret == null)
                {
                    Logger.Info($"No `{SecretFile}` file found, creating new");
                    secret = new NecSecret();
                    secretsProvider.Save(secret, SecretFile);
                }

                setting.DiscordBotToken = secret.DiscordBotToken;
                setting.DatabaseSettings.Password = secret.DatabasePassword;

                LogProvider.Configure<NecLogger>(setting);
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
