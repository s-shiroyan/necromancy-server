using System;

namespace Necromancy.Cli.Command.Commands
{
    public class ServerCommand : ConsoleCommand
    {
        protected override void Run()
        {
            Console.WriteLine("Server");



            // NecSetting setting = new NecSetting();
            /// LogProvider.Configure<NecLogger>(setting);

            //  NecServer server = new NecServer(setting);
            // Console.WriteLine("Press E-key to exit..");
            // server.Start();
        }

        public override string Key => "server";
    }
}
