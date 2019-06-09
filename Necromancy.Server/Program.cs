using System;
using System.Net;
using Arrowgene.Services.Logging;
using Arrowgene.Services.Networking.Tcp.Server.AsyncEvent;

namespace Necromancy.Server
{
    internal class Program
    {
        private static void Main(string[] args) => new Program();

        private object _consoleLock;

        public Program()
        {
            _consoleLock = new object();
            LogProvider.GlobalLogWrite += LogProviderOnGlobalLogWrite;
            AsyncEventServer auth = new AsyncEventServer(IPAddress.Any, 60000, new AuthenticationServer());
            AsyncEventServer msg = new AsyncEventServer(IPAddress.Any, 60001, new MessageServer());
            AsyncEventServer area = new AsyncEventServer(IPAddress.Any, 60002, new AreaServer());
            auth.Start();
            msg.Start();
            area.Start();
            Console.WriteLine("Press any key to exit..");
            Console.ReadKey();
            auth.Stop();
            msg.Stop();
            area.Stop();
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
            if (tag is string)
            {
                switch (tag)
                {
                    case "IN":
                        consoleColor = ConsoleColor.Green;
                        break;
                    case "OUT":
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