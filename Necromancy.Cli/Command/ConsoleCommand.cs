using System.Collections.Generic;
using Arrowgene.Services.Logging;

namespace Necromancy.Cli.Command
{
    public abstract class ConsoleCommand : IConsoleCommand
    {
        /// <summary>
        /// arg1 arg2
        /// arg1=value1 arg2
        /// arg1 -switch1
        /// arg1=value1 -switch1=value2
        /// </summary>
        protected ConsoleCommand()
        {
            Logger = LogProvider.Logger(this);
            Arguments = new List<string>();
            Switches = new List<string>();
            ArgumentMap = new Dictionary<string, string>();
            SwitchMap = new Dictionary<string, string>();
        }

        protected readonly ILogger Logger;
        protected List<string> Arguments { get; }
        protected List<string> Switches { get; }
        protected Dictionary<string, string> SwitchMap { get; }
        protected Dictionary<string, string> ArgumentMap { get; }

        public abstract string Key { get; }
        public abstract bool RequireArgs { get; }
        public abstract string Description { get; }
        protected abstract void Run();

        public virtual void Shutdown()
        {
        }

        public void Handle(string line)
        {
            Clear();
            if (!string.IsNullOrWhiteSpace(line))
            {
                Parse(line);
            }
            else if (RequireArgs)
            {
                Logger.Error($"Arguments are required, type `help {Key}' for details");
                return;
            }

            Run();
        }

        protected void Clear()
        {
            Arguments.Clear();
            Switches.Clear();
            ArgumentMap.Clear();
            SwitchMap.Clear();
        }

        /// <summary>
        /// Parses the input text into arguments and switches.
        /// </summary>
        protected void Parse(string line)
        {
            string[] parts = line.Split(Program.CliSeparator);
            foreach (string part in parts)
            {
                int count = CountChar(part, Program.CliValueSeparator);
                if (count == 1)
                {
                    string[] keyValue = part.Split(Program.CliValueSeparator);
                    if (keyValue.Length == 2)
                    {
                        string key = keyValue[0];
                        string value = keyValue[1];
                        if (key.StartsWith('-'))
                        {
                            key = key.Substring(1);
                            if (key.Length <= 0 || SwitchMap.ContainsKey(key))
                            {
                                Logger.Error($"Invalid switch key: '{key}' is empty or duplicated.");
                                continue;
                            }

                            SwitchMap.Add(key, value);
                            continue;
                        }

                        if (key.Length <= 0 || ArgumentMap.ContainsKey(key))
                        {
                            Logger.Error($"Invalid argument key: '{key}' is empty or duplicated.");
                            continue;
                        }

                        ArgumentMap.Add(key, value);
                        continue;
                    }
                }

                if (part.StartsWith('-'))
                {
                    string switchStr = part.Substring(1);
                    if (switchStr.Length <= 0 || Switches.Contains(switchStr))
                    {
                        Logger.Error($"Invalid switch: '{switchStr}' is empty or duplicated.");
                        continue;
                    }

                    Switches.Add(switchStr);
                    continue;
                }

                if (part.Length <= 0 || Switches.Contains(part))
                {
                    Logger.Error($"Invalid argument: '{part}' is empty or duplicated.");
                    continue;
                }

                Arguments.Add(part);
            }
        }

        private int CountChar(string str, char chr)
        {
            int count = 0;
            foreach (char c in str)
            {
                if (c == chr)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
