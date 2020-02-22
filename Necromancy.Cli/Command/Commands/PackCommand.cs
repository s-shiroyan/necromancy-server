using System;
using Necromancy.Cli.Argument;
using Necromancy.Server.Data;

namespace Necromancy.Cli.Command.Commands
{
    public class PackCommand : ConsoleCommand
    {
        public override CommandResultType Handle(ConsoleParameter parameter)
        {
            FpmfArchiveIO archiveIO2 = new FpmfArchiveIO();

            if (parameter.Arguments.Count == 3)
            {
                FpmfArchiveIO hedFile = new FpmfArchiveIO();
                hedFile.Pack(parameter.Arguments[0], parameter.Arguments[1], parameter.Arguments[2]);
                return CommandResultType.Completed;
            }

            return CommandResultType.Continue;
        }

        public override string Key => "pack";

        /* Please save a copy of your client's data directory before using.
         * Currently need to unpack the client files for the type you want to modify.
         * The current version saves a _header.bin and .key file for each archive type during unpack that are used for pack command, ignore them, but leave them. :)
         * Example:  unpack "C:\Wizardy Online\data\script.hed" "C:\UnpackedFiles"
         * Once changes are made use "pack source_dir target_dir type"
         * Example: pack "C:\UnpackedFiles" "C:\PackedFiles" script
         * When complete, delete .hed and type, script, directory in client, script.hed and script directory 
         * Copy .hed and directory from PackedFiles to client data directory. Example: "C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data"
         * Run the client
         */
        public override string Description =>
            $"Packs Data. Ex.:{Environment.NewLine}pack \"C:/input\" \"C:/output\" \"script\"";
    }
}
