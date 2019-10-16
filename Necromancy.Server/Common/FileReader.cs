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
            string newPath = Path.GetFullPath(Path.Combine(Util.ExecutingDirectory(), @"..\..\..\")) + "Database/TempNPCData.csv";
            System.Console.WriteLine($"Contents of CSV file {newPath} = ");
            string[] lines = System.IO.File.ReadAllLines(newPath/*,shiftJis*/);
            int matchCount = 0;
            int lastMatchRow = 0;
            for (int i = 0; i < lines.Length; i++) 
            {
                if (lines[i].StartsWith($"{client.Character.MapId}") || (lines[i].StartsWith($"#{client.Character.MapId}")))
                {
                    matchCount++;
                    lastMatchRow = i+1; 
                }
            }
            Console.WriteLine($"MatchCount : {matchCount}");
            Console.WriteLine($"lastMatchRow : {lastMatchRow}");
            string[][] jaggedArray = new string[matchCount][];
            int rowStop = lastMatchRow - matchCount; 
            Console.WriteLine($"lastMatchRow : {rowStop}");
            for (int i = rowStop; i < lastMatchRow; i++)
            {
                if (lines[i - rowStop].StartsWith("#"))
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
                Console.WriteLine($"Processed file row {i}");

            }
            //For debugging
           /* foreach (string[] ary1 in jaggedArray)
            {
                foreach (string ary2 in ary1)
                {
                    Console.WriteLine(ary2);
                }
            }
            */
            return jaggedArray;


        }
    }

}

