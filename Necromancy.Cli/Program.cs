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
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using Arrowgene.Services.Logging;
using Necromancy.Server.Data;
using Necromancy.Server.Logging;

namespace Necromancy.Cli
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Program program = new Program(args);
            program.Run();
            Console.WriteLine("Program ended");
        }

        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly BlockingCollection<string> _inputQueue;
        private readonly Thread _consoleThread;
        private readonly object _consoleLock;
        private readonly ILogger _logger;

        private Program(string[] args)
        {
            _logger = LogProvider.Logger(this);
            _consoleThread = new Thread(ReadConsole);
          //  _consoleThread.IsBackground = true;
            _consoleThread.Name = "Console Thread";
            _inputQueue = new BlockingCollection<string>();
            _cancellationTokenSource = new CancellationTokenSource();
            _consoleLock = new object();

            LogProvider.GlobalLogWrite += LogProviderOnGlobalLogWrite;
            Console.CancelKeyPress += ConsoleOnCancelKeyPress;

            if (args.Length == 2)
            {
                FpmfArchiveIO archiveIO = new FpmfArchiveIO();
                FpmfArchive archive = archiveIO.Open(args[0]);
                archiveIO.Save(archive, args[1]);
                return;
            }

            if (args.Length == 1)
            {
                FpmfArchiveIO archiveIO = new FpmfArchiveIO();
                archiveIO.OpenWoItm(args[0]);
                return;
            }
        }


        // NecSetting setting = new NecSetting();
        /// LogProvider.Configure<NecLogger>(setting);

        //  NecServer server = new NecServer(setting);
        // Console.WriteLine("Press E-key to exit..");
        // server.Start();
        private void ReadConsole()
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
                    Console.WriteLine("ReadConsole - OperationCanceledException");
                }
            }
        }

        private void Run()
        {
            ShowCopyright();
            _consoleThread.Start();
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    Stop1();
                    string line;
                    try
                    {
                        line = _inputQueue.Take(_cancellationTokenSource.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        line = null;
                        Console.WriteLine("Run - OperationCanceledException");
                    }

                    if (line == null)
                    {
                        // Ctrl+Z or error
                        break;
                    }

                    if (line.StartsWith("e", true, CultureInfo.InvariantCulture))
                    {
                        Console.WriteLine("Exiting...");
                        //server.Stop();
                        break;
                    }
                }
            }
            Stop1();
       //     Stop();
        }

        private void Stop1()
        {
            Stream stream = new MemoryStream();
            stream.Write(new byte[1]);
            stream.Position = 0;
            StreamReader t = new StreamReader(stream);
            
            Console.SetIn(t);
            Thread.Sleep(500);
            stream.Write(new byte[1]);
            
            Thread.Sleep(500);
        }

        private void Stop()
        {
            if (_consoleThread != null
                && _consoleThread.IsAlive
                && Thread.CurrentThread != _consoleThread
            )
            {
                //Console.OpenStandardInput().Write(new byte[1]);
                _consoleThread.Interrupt();
                if (!_consoleThread.Join(TimeSpan.FromMilliseconds(500)))
                {
                    try
                    {
                        _consoleThread.Abort();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        private void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            _cancellationTokenSource.Cancel();
            //    TextReader textReader = new TextReader();
            //     Console.SetIn();
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
