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
        public const char CliSeparator = ' ';
        public const char CliValueSeparator = '=';

        private static void Main(string[] args)
        {
            Console.WriteLine("Program started");
            Program program = new Program();
            if (args.Length > 0)
            {
                program.RunArguments(args);
            }
            else
            {
                program.RunInteractive();
            }

            Console.WriteLine("Program ended");
        }

        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly BlockingCollection<string> _inputQueue;
        private readonly Thread _consoleThread;
        private readonly object _consoleLock;
        private readonly Dictionary<string, IConsoleCommand> _commands;
        private readonly ILogger _logger;

        private Program()
        {
            _logger = LogProvider.Logger(this);
            _commands = new Dictionary<string, IConsoleCommand>();
            _inputQueue = new BlockingCollection<string>();
            _cancellationTokenSource = new CancellationTokenSource();
            _consoleLock = new object();
            _consoleThread = new Thread(ReadConsoleThread);

            LogProvider.GlobalLogWrite += LogProviderOnGlobalLogWrite;
            Console.CancelKeyPress += ConsoleOnCancelKeyPress;
        }

        private void LoadCommands()
        {
            AddCommand(new ShowCommand());
            AddCommand(new UnpackCommand());
            AddCommand(new ServerCommand());
        }

        private void RunArguments(string[] arguments)
        {
            if (arguments.Length <= 0)
            {
                _logger.Error("Invalid input");
                return;
            }

            LoadCommands();
            ShowCopyright();
            _logger.Info("Argument Mode");
            _logger.Info("Press `e'-key to exit.");

            string line = string.Join(CliSeparator, arguments);
            ProcessLine(line);

            ConsoleKeyInfo keyInfo = Console.ReadKey();
            while (keyInfo.Key != ConsoleKey.E)
            {
                keyInfo = Console.ReadKey();
            }

            ShutdownCommands();
        }

        private void RunInteractive()
        {
            LoadCommands();
            ShowCopyright();

            _logger.Info("Interactive Mode");

            _consoleThread.IsBackground = true;
            _consoleThread.Name = "Console Thread";
            _consoleThread.Start();

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

                ProcessLineResultType result = ProcessLine(line);
                if (result == ProcessLineResultType.Exit)
                {
                    break;
                }

                if (result == ProcessLineResultType.Continue)
                {
                    continue;
                }

                if (result == ProcessLineResultType.Completed)
                {
                    continue;
                }
            }

            StopReadConsoleThread();
            ShutdownCommands();
        }

        private ProcessLineResultType ProcessLine(string line)
        {
            if (line == null)
            {
                // Ctrl+Z, Ctrl+C or error
                return ProcessLineResultType.Exit;
            }

            string[] arguments = line.Split(CliSeparator);
            if (arguments.Length <= 0)
            {
                _logger.Error($"Invalid input: '{line}'. Type 'help' for a list of available commands.");
                return ProcessLineResultType.Continue;
            }

            string key = arguments[0];

            if (key.Equals("exit", StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.Info("Exiting...");
                return ProcessLineResultType.Exit;
            }

            if (key.Equals("help", StringComparison.InvariantCultureIgnoreCase))
            {
                if (arguments.Length >= 2)
                {
                    string subKey = arguments[1].ToLowerInvariant();
                    bool found = false;
                    foreach (string cmdKey in _commands.Keys)
                    {
                        if (cmdKey.ToLowerInvariant() == subKey)
                        {
                            IConsoleCommand consoleCommandHelp = _commands[cmdKey];
                            _logger.Info(ShowHelp(consoleCommandHelp));
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        _logger.Error(
                            $"Command: 'help {subKey}' not available. Type 'help' for a list of available commands.");
                    }

                    return ProcessLineResultType.Continue;
                }

                ShowHelp();
                return ProcessLineResultType.Continue;
            }

            if (!_commands.ContainsKey(key))
            {
                _logger.Error($"Command: '{key}' not available. Type 'help' for a list of available commands.");
                return ProcessLineResultType.Continue;
            }

            int cmdLength = arguments.Length - 1;
            string[] newArguments = new string[cmdLength];
            if (cmdLength > 0)
            {
                Array.Copy(arguments, 1, newArguments, 0, cmdLength);
            }

            line = string.Join(CliSeparator, newArguments);

            IConsoleCommand consoleCommand = _commands[key];
            consoleCommand.Handle(line);
            return ProcessLineResultType.Completed;
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

        private void AddCommand(IConsoleCommand command)
        {
            _commands.Add(command.Key, command);
        }

        private void ShutdownCommands()
        {
            foreach (IConsoleCommand command in _commands.Values)
            {
                command.Shutdown();
            }
        }

        private void ShowHelp()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Available Commands:");
            sb.Append(Environment.NewLine);

            sb.Append("exit");
            sb.Append(Environment.NewLine);
            sb.Append("- Closes the program");

            sb.Append(Environment.NewLine);
            sb.Append("----------");
            sb.Append(Environment.NewLine);

            sb.Append("help");
            sb.Append(Environment.NewLine);
            sb.Append("- Displays this text");

            foreach (string key in _commands.Keys)
            {
                sb.Append(Environment.NewLine);
                sb.Append("----------");
                sb.Append(Environment.NewLine);

                IConsoleCommand command = _commands[key];
                sb.Append(ShowHelp(command));
            }

            _logger.Info(sb.ToString());
        }

        private string ShowHelp(IConsoleCommand command)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(command.Key);
            sb.Append(Environment.NewLine);
            sb.Append($"- {command.Description}");
            return sb.ToString();
        }

        private void ShowCopyright()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("Necromancy.Cli Copyright (C) 2019-2020 Necromancy Team");
            sb.Append(Environment.NewLine);
            sb.Append("This program comes with ABSOLUTELY NO WARRANTY; for details type `show w'.");
            sb.Append(Environment.NewLine);
            sb.Append("This is free software, and you are welcome to redistribute it");
            sb.Append(Environment.NewLine);
            sb.Append("under certain conditions; type `show c' for details.");
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            _logger.Info(sb.ToString());
        }
    }
}
