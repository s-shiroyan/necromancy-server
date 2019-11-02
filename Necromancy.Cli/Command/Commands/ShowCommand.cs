using System;

namespace Necromancy.Cli.Command.Commands
{
    public class ShowCommand : ConsoleCommand
    {
        protected override void Run()
        {
            Console.WriteLine("show");
        }

        public override string Key => "show";
        public override bool RequireArgs => true;
        public override string Description => $"Shows Copyright. Ex.:{Environment.NewLine}show w{Environment.NewLine}show c";
    }
}
