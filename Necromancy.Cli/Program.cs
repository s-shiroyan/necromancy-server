/*
 * This file is part of Necromancy.Cli
 *
 * Necromancy.Cli is a server implementation for the game "Wizardry Online".
 * Copyright (C) 2019-2020 Necromancy Team
 *
 * Github: https://github.com/necromancyonline/necromancy-server
 *
 * Necromancy.Cli is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Necromancy.Cli is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Necromancy.Cli. If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using Arrowgene.Services.Logging;
using Necromancy.Cli.Command;
using Necromancy.Cli.Command.Commands;
using Necromancy.Server.Logging;

namespace Necromancy.Cli
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Program started");
            Program program = new Program(args);
            program.Run();
            Console.WriteLine("Program ended");
        }

        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly BlockingCollection<string> _inputQueue;
        private readonly Thread _consoleThread;
        private readonly object _consoleLock;
        private readonly Dictionary<string, IConsoleCommand> _commands;
        private readonly ILogger _logger;

        private Program(string[] args)
        {
            _logger = LogProvider.Logger(this);
            _commands = new Dictionary<string, IConsoleCommand>();
            _inputQueue = new BlockingCollection<string>();
            _cancellationTokenSource = new CancellationTokenSource();
            _consoleLock = new object();
            _consoleThread = new Thread(ReadConsoleThread);

            LogProvider.GlobalLogWrite += LogProviderOnGlobalLogWrite;
            Console.CancelKeyPress += ConsoleOnCancelKeyPress;

            _consoleThread.IsBackground = true;
            _consoleThread.Name = "Console Thread";
            _consoleThread.Start();
        }

        private void LoadCommands()
        {
            _commands.Add("show", new ShowCommand());
            _commands.Add("unpack", new UnpackCommand());
        }

        private void Run()
        {
            LoadCommands();
            ShowCopyright();
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    string line;
                    try
                    {
                        line = _inputQueue.Take(_cancellationTokenSource.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        line = null;
                    }

                    if (line == null)
                    {
                        // Ctrl+Z, Ctrl+C or error
                        break;
                    }



                    string[] arguments = line.Split(" ");
                    string key = arguments[0];
                    
                    
                    if (line.StartsWith("e", true, CultureInfo.InvariantCulture))
                    {
                        _logger.Info("Exiting...");
                        break;
                    }
                    
                    if (!_commands.ContainsKey(key))
                    {
                        continue;
                    }

                    IConsoleCommand consoleCommand = _commands[key];
                    consoleCommand.Handle(line);
                }
            }
            StopReadConsoleThread();
        }

        private void ReadConsoleThread()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                string line = Console.ReadLine();
                try
                {
                    _inputQueue.Add(line, _cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    // Ignored
                }
            }
        }

        private void StopReadConsoleThread()
        {
            if (_consoleThread != null
                && _consoleThread.IsAlive
                && Thread.CurrentThread != _consoleThread
            )
            {
                try
                {
                    _consoleThread.Interrupt();
                }
                catch (Exception)
                {
                    // Ignored
                }

                if (!_consoleThread.Join(TimeSpan.FromMilliseconds(500)))
                {
                    try
                    {
                        _consoleThread.Abort();
                    }
                    catch (Exception)
                    {
                        // Ignored
                    }
                }
            }
        }

        private void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }

        private void LogProviderOnGlobalLogWrite(object sender, LogWriteEventArgs logWriteEventArgs)
        {
            ConsoleColor consoleColor = ConsoleColor.Gray;
            switch (logWriteEventArgs.Log.LogLevel)
            {
                case LogLevel.Debug:
                    consoleColor = ConsoleColor.DarkCyan;
                    break;
                case LogLevel.Info:
                    consoleColor = ConsoleColor.Cyan;
                    break;
                case LogLevel.Error:
                    consoleColor = ConsoleColor.Red;
                    break;
            }

            object tag = logWriteEventArgs.Log.Tag;
            if (tag is NecLogType)
            {
                switch (tag)
                {
                    case NecLogType.In:
                        consoleColor = ConsoleColor.Green;
                        break;
                    case NecLogType.Out:
                        consoleColor = ConsoleColor.Blue;
                        break;
                }
            }

            lock (_consoleLock)
            {
                Console.ForegroundColor = consoleColor;
                Console.WriteLine(logWriteEventArgs.Log);
                Console.ResetColor();
            }
        }

        private void ShowCopyright()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Necromancy.Cli Copyright (C) 2019-2020 Necromancy Team");
            sb.Append(Environment.NewLine);
            sb.Append("This program comes with ABSOLUTELY NO WARRANTY; for details type `show w'.");
            sb.Append(Environment.NewLine);
            sb.Append("This is free software, and you are welcome to redistribute it");
            sb.Append(Environment.NewLine);
            sb.Append("under certain conditions; type `show c' for details.");
            sb.Append(Environment.NewLine);
            Console.WriteLine(sb);
        }
    }
}
