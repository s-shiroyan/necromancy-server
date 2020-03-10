using System;
using System.IO;
using Necromancy.Cli.Argument;
using Necromancy.Server.Data;

namespace Necromancy.Cli.Command.Commands
{
    public class UnpackCommand : ConsoleCommand
    {
        public override CommandResultType Handle(ConsoleParameter parameter)
        {
            FpmfArchiveIO archiveIO2 = new FpmfArchiveIO();
//            archiveIO2.OpenWoItm("C:\\Games\\Wizardry Online\\data_un\\settings\\item.csv");
 
//            return CommandResultType.Continue;
            if (parameter.Arguments.Count == 2)
            {
                FpmfArchiveIO archiveIO = new FpmfArchiveIO();
                string[] hedFiles;
                FileAttributes attr = File.GetAttributes(parameter.Arguments[0]);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    hedFiles = Directory.GetFiles(parameter.Arguments[0], "*.hed", SearchOption.AllDirectories);
                }
                else
                {
                    hedFiles = new string[1];
                    hedFiles[0] = parameter.Arguments[0];
                }
                for (int i = 0; i < hedFiles.Length; i++)
                {
                    FpmfArchive archive = archiveIO.Open(hedFiles[i], parameter.Arguments[1]);
                    archiveIO.Save(archive, parameter.Arguments[1]);
                }
                return CommandResultType.Completed;
            } else if (parameter.Arguments.Count == 1)
            {
                FpmfArchiveIO archiveIO = new FpmfArchiveIO();
                FpmfArchive archive = archiveIO.Open(parameter.Arguments[0]);
                archiveIO.OpenWoItm(parameter.Arguments[0]);
                return CommandResultType.Completed;
            } else if (parameter.Arguments.Count == 3)
            {
                FpmfArchiveIO archiveIO = new FpmfArchiveIO();
                string[] hedFiles;
                FileAttributes attr = File.GetAttributes(parameter.Arguments[1]);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    hedFiles = Directory.GetFiles(parameter.Arguments[1], "*.hed", SearchOption.AllDirectories);
                }
                else
                {
                    hedFiles = new string[1];
                    hedFiles[0] = parameter.Arguments[1];
                }
                for (int i = 0; i < hedFiles.Length; i++)
                {
                    if (parameter.Arguments[0] == "header")
                    {
                        archiveIO.Header(hedFiles[i], parameter.Arguments[2]);
                    }

                }
                return CommandResultType.Completed;

            }

            return CommandResultType.Continue;
        }

        public override string Key => "unpack";

        public override string Description =>
            $"Unpacks Data. Ex.:{Environment.NewLine}unpack \"C:/Games/Wizardry Online/data/settings.hed\" \"C:/output\"";
    }
}
