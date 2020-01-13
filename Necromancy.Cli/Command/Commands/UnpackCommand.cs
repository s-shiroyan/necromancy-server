using System;
using Necromancy.Cli.Argument;
using Necromancy.Server.Data;

namespace Necromancy.Cli.Command.Commands
{
    public class UnpackCommand : ConsoleCommand
    {
        public override CommandResultType Handle(ConsoleParameter parameter)
        {
            FpmfArchiveIO archiveIO2 = new FpmfArchiveIO();
            archiveIO2.OpenWoItm("C:\\Games\\Wizardry Online\\data_un\\settings\\item.csv");
 
            return CommandResultType.Continue;
            if (parameter.Arguments.Count == 2)
            {
                FpmfArchiveIO archiveIO = new FpmfArchiveIO();
                FpmfArchive archive = archiveIO.Open(parameter.Arguments[0]);
                archiveIO.Save(archive, parameter.Arguments[1]);
                return CommandResultType.Completed;
            }

            if (parameter.Arguments.Count == 1)
            {
                FpmfArchiveIO archiveIO = new FpmfArchiveIO();
                FpmfArchive archive = archiveIO.Open(parameter.Arguments[0]);
                archiveIO.OpenWoItm(parameter.Arguments[0]);
                return CommandResultType.Completed;
            }

            return CommandResultType.Continue;
        }

        public override string Key => "unpack";

        public override string Description =>
            $"Unpacks Data. Ex.:{Environment.NewLine}unpack \"C:/Games/Wizardry Online/data/settings.hed\" \"C:/output\"";
    }
}
