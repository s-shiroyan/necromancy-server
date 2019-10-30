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
    }
}
