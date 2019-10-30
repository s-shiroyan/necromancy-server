using System.Collections.Generic;

namespace Necromancy.Cli.Command
{
    public abstract class ConsoleCommand : IConsoleCommand
    {
        public ConsoleCommand()
        {
        }

        public void Handle(string line)
        {
            Run();
        }

        protected abstract void Run();

        protected Dictionary<string, string> ArgumentMap;
        protected Dictionary<string, string> SwitchMap;

        public abstract string Key { get; }
    }
}
