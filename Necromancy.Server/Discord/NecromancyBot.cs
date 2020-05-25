using System;
using System.Collections.Concurrent;
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

        private readonly IServiceCollection _collection;
        private readonly List<Assembly> _assemblies;
        private readonly NecSetting _setting;
        private readonly BlockingCollection<DiscordEvent> _events;

        private CancellationTokenSource _cancellationTokenSource;
        private IServiceProvider _service;
        private Task _task;
        private bool _ready;

        public NecromancyBot(NecSetting setting)
        {
            _ready = false;
            _setting = setting;
            _events = new BlockingCollection<DiscordEvent>();
            _cancellationTokenSource = null;
            _assemblies = new List<Assembly>();
            _collection = new ServiceCollection();
            _collection
                .AddSingleton<CommandService>()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<TextHandlingService>();
        }

        public void AddSingleton<T>(T singleton)
        {
            _collection.AddSingleton(typeof(T), singleton);
            _assemblies.Add(Assembly.GetAssembly(typeof(T)));
        }

        public void Start()
        {
            if (String.IsNullOrWhiteSpace(_setting.DiscordBotToken))
            {
                Logger.Info("No Discord Token");
                return;
            }

            _ready = false;
            _cancellationTokenSource = new CancellationTokenSource();
            _service = _collection.BuildServiceProvider();
            _task = new Task(Run, _cancellationTokenSource.Token);
            _task.Start(TaskScheduler.Default);
        }

        public void Stop()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
            }

            _ready = false;
            _task = null;
        }

        public void EnqueueEvent(DiscordEvent discordEvent)
        {
            _events.Add(discordEvent);
        }

        public bool Send(DiscordEvent discordEvent)
        {
            Task<bool> sendTask = SendAsync(discordEvent);
            return sendTask.Result;
        }

        public async Task<bool> SendAsync(DiscordEvent discordEvent)
        {
            if (!_ready)
            {
                return false;
            }

            if (discordEvent == null)
            {
                Logger.Error("DiscordEvent is null");
                return false;
            }

            try
            {
                DiscordSocketClient client = _service.GetRequiredService<DiscordSocketClient>();
                SocketGuild guild = client.GetGuild(_setting.DiscordGuild);
                if (guild == null)
                {
                    return false;
                }

                SocketTextChannel textChannel = guild.GetTextChannel(discordEvent.TextChannelId);
                if (textChannel == null)
                {
                    return false;
                }

                await textChannel.SendMessageAsync(discordEvent.Text);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
            }

            return false;
        }

        /// <summary>
        /// send a message to the #server-status channel
        /// </summary>
        public void EnqueueEvent_ServerStatus(string text)
        {
            DiscordEvent discordEvent = new DiscordEvent();
            discordEvent.TextChannelId = _setting.DiscordBotChannel_ServerStatus;
            discordEvent.Text = text;
            EnqueueEvent(discordEvent);
        }

        /// <summary>
        /// send a message to the #server-status channel
        /// </summary>
        public void Send_ServerStatus(string text)
        {
            DiscordEvent discordEvent = new DiscordEvent();
            discordEvent.TextChannelId = _setting.DiscordBotChannel_ServerStatus;
            discordEvent.Text = text;
            Send(discordEvent);
        }

        private async void Run()
        {
            try
            {
                Logger.Info("DiscordBot loading...");
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

                // required to start the services
                _service.GetRequiredService<CommandHandlingService>();
                _service.GetRequiredService<TextHandlingService>();
                //
                
                Logger.Info("DiscordBot getting ready...");
                while (!_ready)
                {
                    await Task.Delay(1000, _cancellationTokenSource.Token);
                }

                Logger.Info("DiscordBot ready!");
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    DiscordEvent discordEvent;
                    try
                    {
                        discordEvent = _events.Take(_cancellationTokenSource.Token);
                    }
                    catch (OperationCanceledException ex)
                    {
                        Logger.Exception(ex);
                        return;
                    }

                    bool success = await SendAsync(discordEvent);
                    if (!success)
                    {
                        Logger.Error($"Failed to deliver discord message: '{discordEvent.Text}'");
                    }
                }
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

            Logger.Write(level, $"[{arg.Source}] {arg.Message}", arg);
            return Task.CompletedTask;
        }
    }
}
