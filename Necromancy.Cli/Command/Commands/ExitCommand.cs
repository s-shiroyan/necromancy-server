using Arrowgene.Logging;
using Necromancy.Cli.Argument;

namespace Necromancy.Cli.Command.Commands
{
    public class ExitCommand : ConsoleCommand
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(ExitCommand));
        
        public override CommandResultType Handle(ConsoleParameter parameter)
        {
            Logger.Info("Exiting...");
            return CommandResultType.Exit;
        }

        public override string Key => "exit";
        public override string Description => "Closes the program";
    }
}
