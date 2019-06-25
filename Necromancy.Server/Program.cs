using System;
using Arrowgene.Services.Logging;
using Necromancy.Server.Data;
using Necromancy.Server.Logging;
using Necromancy.Server.Setting;

namespace Necromancy.Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                new Program();
            }
            else if (args.Length == 2)
            {
                FpmfArchive archive = new FpmfArchive();
                archive.Decrypt(args[0], args[1]);
            }
        }

        private object _consoleLock;

        public Program()
        {
            _consoleLock = new object();
            NecSetting setting = new NecSetting();

            LogProvider.Configure<NecLogger>(setting);
            LogProvider.GlobalLogWrite += LogProviderOnGlobalLogWrite;

            NecServer server = new NecServer(setting);
            server.Start();
            Console.WriteLine("Press any key to exit..");
            Console.ReadKey();
            server.Stop();
        }

        private void LogProviderOnGlobalLogWrite(object sender, LogWriteEventArgs logWriteEventArgs)
        {
            ConsoleColor consoleColor = ConsoleColor.Gray;
            switch (logWriteEventArgs.Log.LogLevel)
            {
                case LogLevel.Debug:
                    consoleColor = ConsoleColor.Yellow;
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
    }
}