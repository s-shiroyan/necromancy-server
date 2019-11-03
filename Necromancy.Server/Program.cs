using System;
using Arrowgene.Services.Logging;
using Necromancy.Server.Data;
using Necromancy.Server.Logging;
using Necromancy.Server.Setting;

namespace Necromancy.Server
{
    internal class Program
    {
        private static void Main(string[] args) => new Program(args);

        private object _consoleLock;

        public Program(string[] args)
        {
            _consoleLock = new object();
            LogProvider.GlobalLogWrite += LogProviderOnGlobalLogWrite;
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


            NecSetting setting = new NecSetting();
            LogProvider.Configure<NecLogger>(setting);

            NecServer server = new NecServer(setting);
            Console.WriteLine("Press E-key to exit..");
            server.Start();
            bool readKey = true;
            while (readKey)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                switch (keyInfo.Key)
                {
                    case ConsoleKey.E:
                        Console.WriteLine("Exiting...");
                        readKey = false;
                        server.Stop();
                        break;
                }
            }

            Console.WriteLine("Ended");
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
    }
}
