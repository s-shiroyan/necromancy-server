using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Arrowgene.Logging;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Necromancy.Server.Discord.Services;
using Necromancy.Server.Setting;

namespace Necromancy.Server.Discord
{
    public class NecromancyBot
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(NecromancyBot));

        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IServiceCollection _collection;
        private readonly List<Assembly> _assemblies;
        private readonly NecSetting _setting;

        private IServiceProvider _service;
        private Task _task;
        private bool _ready;

        public NecromancyBot(NecSetting setting)
        {
            _ready = false;
            _setting = setting;            
            _cancellationTokenSource = new CancellationTokenSource();
            _assemblies = new List<Assembly>();
            _assemblies.Add(Assembly.GetAssembly(typeof(NecromancyBot)));            
            _collection = new ServiceCollection();
            _collection
                .AddSingleton<CommandService>()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandHandlingService>();
        }

        public void Start()
        {
            if (String.IsNullOrWhiteSpace(_setting.DiscordBotToken))
            {
                Logger.Info("No Discord Token");
                return;
            }

            _service = _collection.BuildServiceProvider();
            _task = new Task(Run, _cancellationTokenSource.Token);
            _task.Start(TaskScheduler.Default);
        }

        public void Stop()
        {
            if (_task == null)
            {
                Logger.Info("DiscordBot not running");
                return;
            }

            _cancellationTokenSource.Cancel();
            _task = null;
        }

        public void Send_ServerStatus(string text)
        {
            if (!_ready)
            {
                return;
            }

            Task.Factory.StartNew(() => SendAsync(_setting.DiscordBotChannel_ServerStatus, text));
        }

        public void Send(ulong textChannelId, string text)
        {
            if (!_ready)
            {
                return;
            }

            Task.Factory.StartNew(() => SendAsync(textChannelId, text));
        }

        public async void SendAsync(ulong textChannelId, string text)
        {
            if (!_ready)
            {
                return;
            }

            try
            {
                DiscordSocketClient client = _service.GetRequiredService<DiscordSocketClient>();
                SocketGuild guild = client.GetGuild(_setting.DiscordGuild);
                if (guild == null)
                {
                    return;
                }

                SocketTextChannel textChannel = guild.GetTextChannel(textChannelId);
                if (textChannel == null)
                {
                    return;
                }

                await textChannel.SendMessageAsync(text);
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
            }
        }

        private async void Run()
        {
            try
            {
                Logger.Info("DiscordBot loading");
                DiscordSocketClient client = _service.GetRequiredService<DiscordSocketClient>();
                client.Log += ClientOnLog;
                client.Ready += ClientOnReady;
                await client.LoginAsync(TokenType.Bot, _setting.DiscordBotToken);
                await client.StartAsync();
                CommandService commands = _service.GetRequiredService<CommandService>();
                commands.Log += ClientOnLog;
                foreach (var assembly in _assemblies)
                {
                    await commands.AddModulesAsync(assembly, _service);
                }

                _service.GetRequiredService<CommandHandlingService>();

                Logger.Info("DiscordBot loaded");
                await Task.Delay(-1, _cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
            }

            Logger.Info("DiscordBot stopped");
        }

        private Task ClientOnReady()
        {
            _ready = true;
            Logger.Info("DiscordBot started");
            Send_ServerStatus("Hello! I'm Online!");
            return Task.CompletedTask;
        }

        private Task ClientOnLog(LogMessage arg)
        {
            if (arg.Exception != null)
            {
                Logger.Exception(arg.Exception);
            }

            LogLevel level;
            switch (arg.Severity)
            {
                case LogSeverity.Critical:
                    level = LogLevel.Error;
                    break;
                case LogSeverity.Error:
                    level = LogLevel.Error;
                    break;
                case LogSeverity.Warning:
                    level = LogLevel.Info;
                    break;
                case LogSeverity.Info:
                    level = LogLevel.Info;
                    break;
                case LogSeverity.Debug:
                    level = LogLevel.Debug;
                    break;
                case LogSeverity.Verbose:
                    return Task.CompletedTask;
                default:
                    return Task.CompletedTask;
            }

            Logger.Write(level, null, $"[{arg.Source}] {arg.Message}");
            return Task.CompletedTask;
        }
    }
}
