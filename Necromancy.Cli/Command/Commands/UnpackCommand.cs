using System;
using Necromancy.Server.Data;

namespace Necromancy.Cli.Command.Commands
{
    public class UnpackCommand : ConsoleCommand
    {
        protected override void Run()
        {
            if (Arguments.Count == 2)
            {
                FpmfArchiveIO archiveIO = new FpmfArchiveIO();
                FpmfArchive archive = archiveIO.Open(Arguments[0]);
                archiveIO.Save(archive, Arguments[1]);
                return;
            }

            if (Arguments.Count == 1)
            {
                FpmfArchiveIO archiveIO = new FpmfArchiveIO();
                archiveIO.OpenWoItm(Arguments[0]);
                return;
            }
        }


        public override string Key => "unpack";
        public override bool RequireArgs => true;

        public override string Description =>
            $"Unpacks Data. Ex.:{Environment.NewLine}unpack \"C:/Games/Wizardry Online/data/settings.hed\" \"C:/output\"";
    }
}
