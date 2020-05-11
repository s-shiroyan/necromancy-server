using Necromancy.Cli.Argument;

namespace Necromancy.Cli.Command
{
    public abstract class ConsoleCommand : IConsoleCommand
    {
        public abstract string Key { get; }
        public abstract string Description { get; }
        public abstract CommandResultType Handle(ConsoleParameter parameter);

        public virtual void Shutdown()
        {
        }
    }
}
