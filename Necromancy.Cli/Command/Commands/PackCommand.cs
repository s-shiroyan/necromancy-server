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
            } else if (parameter.Arguments.Count == 4)
            {
                FpmfArchiveIO hedFile = new FpmfArchiveIO();
                hedFile.Pack(parameter.Arguments[0], parameter.Arguments[1], parameter.Arguments[2], parameter.Arguments[3]);
                return CommandResultType.Completed;
            }


            return CommandResultType.Continue;
        }

        public override string Key => "pack";

        /* Please save a copy of your client's data directory before using.
         * Currently need to unpack the client files for the type you want to modify.
         * Example:  unpack "C:\Wizardy Online\data\script.hed" "C:\UnpackedFiles"
         * Once changes are made use "pack source_dir target_dir type"
         * Example: pack "C:\UnpackedFiles" "C:\PackedFiles" script
         * If target .hed is not in the root of the out directory and typethe relative path after "pack source_dir target_dir type path"
         * Example: pack "C:\UnpackedFiles" "C:\PackedFiles" chara 00\01
         * When complete, delete .hed and type, script, directory in client, script.hed and script directory 
         * Copy .hed and directory from PackedFiles to client data directory. Example: "C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data"
         * Run the client
         */
        public override string Description =>
            $"Packs Data. Ex.:{Environment.NewLine}pack \"C:/input\" \"C:/output\" \"script\"";
    }
}
