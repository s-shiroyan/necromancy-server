using System;

namespace Necromancy.Cli.Command.Commands
{
    public class UnpackCommand : ConsoleCommand
    {
        protected override void Run()
        {
            Console.WriteLine("Unpack");


            //   if (args.Length == 2)
            //   {
            //       FpmfArchiveIO archiveIO = new FpmfArchiveIO();
            //       FpmfArchive archive = archiveIO.Open(args[0]);
            //       archiveIO.Save(archive, args[1]);
            //       return;
            //   }

            //   if (args.Length == 1)
            //   {
            //       FpmfArchiveIO archiveIO = new FpmfArchiveIO();
            //       archiveIO.OpenWoItm(args[0]);
            //       return;
            //   }
        }

        public override string Key => "unpack";
    }
}
