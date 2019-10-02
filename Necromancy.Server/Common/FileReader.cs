
using System;
using System.IO;

using Necromancy.Server.Model;



namespace Necromancy.Server.Common
{
    class FileReader
    {
        public static string[][] GameFileReader(NecClient client)
        {
            //var shiftJis = Encoding.GetEncoding(932); // CSV files are in this encording.  To-Do. Figure out encoding loading. 
            string fileName = @"C:\Github\necromancy-server\Necromancy.Server\Database/TempNPCData.csv";
            string newPath = Path.GetFullPath(Path.Combine(Util.ExecutingDirectory(), @"..\..\..\")) + "Database/TempNPCData.csv";
            string fileName2 = @"..\" + Directory.GetCurrentDirectory();
            System.Console.WriteLine($"Contents of CSV file {fileName} = ");
            System.Console.WriteLine($"Contents of CSV file {newPath} = ");
            string[] lines = System.IO.File.ReadAllLines(fileName/*,shiftJis*/);
            int matchCount = 0;
            int lastMatchRow = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith($"{client.Character.MapId}") || (lines[i].StartsWith($"#{client.Character.MapId}")))
                {
                    matchCount++;
                    lastMatchRow = i;
                }
            }
            Console.WriteLine($"MatchCount : {matchCount}");
            Console.WriteLine($"lastMatchRow : {lastMatchRow}");
            string[][] jaggedArray = new string[matchCount][];
            int rowStop = lastMatchRow - matchCount;
            for (int i = rowStop; i < lastMatchRow; i++)
            {
                if (lines[i].StartsWith("#"))
                {
                    //# is commented out to not load.  populating space with empty string
                    string[] npcRowStringArray = new string[] { "0", "0", "0", "0", "0", "0", "0", "0", "0" };
                    jaggedArray[i - rowStop] = npcRowStringArray;
                }
                else
                {
                    string[] npcRowStringArray = lines[i].Split(',');
                    jaggedArray[i - rowStop] = npcRowStringArray;
                }

            }
            //For debugging
            foreach (string[] ary1 in jaggedArray)
            {
                foreach (string ary2 in ary1)
                {
                    //Console.WriteLine(ary2);
                }
            }

            return jaggedArray;


        }
    }

}


