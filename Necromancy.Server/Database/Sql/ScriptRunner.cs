using System;
using System.IO;
using System.Text;
using Arrowgene.Logging;

namespace Necromancy.Server.Database.Sql
{
    public class ScriptRunner
    {
        private const string Delimiter = ";";
        private const bool FullLineDelimiter = false;

        private static readonly ILogger Logger = LogProvider.Logger(typeof(ScriptRunner));

        private readonly IDatabase _database;

        /**
         * Default constructor
         */
        public ScriptRunner(IDatabase database)
        {
            _database = database;
        }

        public void Run(string path)
        {
            int index = 0;
            try
            {
                string[] file = File.ReadAllLines(path);
                StringBuilder command = null;
                for (; index < file.Length; index++)
                {
                    string line = file[index];
                    if (command == null)
                    {
                        command = new StringBuilder();
                    }

                    string trimmedLine = line.Trim();

                    if (trimmedLine.Length < 1)
                    {
                        // Do nothing
                    }
                    else if (trimmedLine.StartsWith("//") || trimmedLine.StartsWith("--"))
                    {
                        // Print comment
                    }
                    else if (!FullLineDelimiter && trimmedLine.EndsWith(Delimiter)
                             || FullLineDelimiter && trimmedLine == Delimiter)
                    {
                        command.Append(
                            line.Substring(0, line.LastIndexOf(Delimiter, StringComparison.InvariantCulture)));
                        command.Append(" ");
                        _database.Execute(command.ToString());
                        command = null;
                    }
                    else
                    {
                        command.Append(line);
                        command.Append("\n");
                    }
                }

                if (command != null)
                {
                    string cmd = command.ToString();
                    if (string.IsNullOrWhiteSpace(cmd))
                    {
                        //do nothing;
                    }
                    else
                    {
                        _database.Execute(cmd);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Error($"Sql error at Line: {index}");
                Logger.Exception(exception);
            }
        }
    }
}
