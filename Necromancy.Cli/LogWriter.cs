using System;
using System.Collections.Generic;
using System.Text;
using Arrowgene.Services.Buffers;
using Arrowgene.Services.Logging;
using Necromancy.Cli.Argument;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;

namespace Necromancy.Cli
{
    public class LogWriter : ISwitchConsumer
    {
        private readonly object _consoleLock;
        private Dictionary<ServerType, HashSet<uint>> _ignoredPackets;
        private readonly ILogger _logger;

        public LogWriter()
        {
            _logger = LogProvider.Logger(this);
            _ignoredPackets = new Dictionary<ServerType, HashSet<uint>>();
            _consoleLock = new object();
            Switches = new List<ISwitchProperty>();
            Reset();
            LoadSwitches();
            LogProvider.GlobalLogWrite += LogProviderOnGlobalLogWrite;
        }

        /// <summary>
        /// --max-packet-size=64
        /// </summary>
        public int MaxPacketSize { get; set; }

        /// <summary>
        /// --no-data=true
        /// </summary>
        public bool NoData { get; set; }

        public void Reset()
        {
            MaxPacketSize = -1;
            NoData = false;
        }

        public List<ISwitchProperty> Switches { get; }

        public void AddIgnoredPacket(ServerType serverType, uint packetId)
        {
            HashSet<uint> ignored;
            if (_ignoredPackets.ContainsKey(serverType))
            {
                ignored = _ignoredPackets[serverType];
            }
            else
            {
                ignored = new HashSet<uint>();
                _ignoredPackets.Add(serverType, ignored);
            }

            if (ignored.Contains(packetId))
            {
                _logger.Error($"ServerType:{serverType} PacketId:{packetId} is already ignored");
                return;
            }

            ignored.Add(packetId);
        }

        private void LoadSwitches()
        {
            Switches.Add(
                new SwitchProperty<bool>(
                    "--no-data",
                    "--no-data=true (true|false)",
                    "Don't display packet data",
                    bool.TryParse,
                    (result => NoData = result)
                )
            );
            Switches.Add(
                new SwitchProperty<int>(
                    "--max-packet-size",
                    "--max-packet-size=64 (integer)",
                    "Don't display packet data",
                    int.TryParse,
                    (result => MaxPacketSize = result)
                )
            );
        }

        private void LogProviderOnGlobalLogWrite(object sender, LogWriteEventArgs logWriteEventArgs)
        {
            ConsoleColor consoleColor;
            string text;

            object tag = logWriteEventArgs.Log.Tag;
            if (tag is NecLogPacket logPacket)
            {
                switch (logPacket.LogType)
                {
                    case NecLogType.PacketIn:
                        consoleColor = ConsoleColor.Green;
                        break;
                    case NecLogType.PacketOut:
                        consoleColor = ConsoleColor.Blue;
                        break;
                    default:
                        consoleColor = ConsoleColor.Gray;
                        break;
                }

                text = CreatePacketLog(logPacket);
            }
            else
            {
                switch (logWriteEventArgs.Log.LogLevel)
                {
                    case LogLevel.Debug:
                        consoleColor = ConsoleColor.DarkCyan;
                        break;
                    case LogLevel.Info:
                        consoleColor = ConsoleColor.Cyan;
                        break;
                    case LogLevel.Error:
                        consoleColor = ConsoleColor.Red;
                        break;
                    default:
                        consoleColor = ConsoleColor.Gray;
                        break;
                }

                text = logWriteEventArgs.Log.ToString();
            }

            if (text == null)
            {
                return;
            }

            lock (_consoleLock)
            {
                Console.ForegroundColor = consoleColor;
                Console.WriteLine(text);
                Console.ResetColor();
            }
        }

        private string CreatePacketLog(NecLogPacket logPacket)
        {
            ServerType serverType = logPacket.ServerType;
            ushort packetId = logPacket.Id;

            if (_ignoredPackets.ContainsKey(serverType)
                && _ignoredPackets[serverType].Contains(packetId))
            {
                return null;
            }

            int dataSize = logPacket.Data.Size;

            StringBuilder sb = new StringBuilder();
            sb.Append($"{logPacket.ClientIdentity} Packet Log");
            sb.Append(Environment.NewLine);
            sb.Append("----------");
            sb.Append(Environment.NewLine);
            sb.Append($"[{logPacket.TimeStamp:HH:mm:ss}][Typ:{logPacket.LogType}]");
            sb.Append($"[{serverType}]");
            sb.Append(Environment.NewLine);
            sb.Append(
                $"[Id:0x{packetId:X2}|{packetId}][Len(Data/Total):{dataSize}/{dataSize + logPacket.Header.Length}][Header:{logPacket.HeaderHex}]");
            sb.Append($"[{logPacket.PacketIdName}]");
            sb.Append(Environment.NewLine);

            if (!NoData)
            {
                IBuffer data = logPacket.Data;
                int maxPacketSize = MaxPacketSize;
                if (maxPacketSize > 0 && dataSize > maxPacketSize)
                {
                    data = data.Clone(0, maxPacketSize);

                    sb.Append($"- Truncated Data showing {maxPacketSize} of {dataSize} bytes");
                    sb.Append(Environment.NewLine);
                }

                sb.Append("ASCII:");
                sb.Append(Environment.NewLine);
                sb.Append(data.ToAsciiString(true));
                sb.Append(Environment.NewLine);
                sb.Append("HEX:");
                sb.Append(Environment.NewLine);
                sb.Append(data.ToHexString('-'));
                sb.Append(Environment.NewLine);
            }

            sb.Append("----------");

            return sb.ToString();
        }
    }
}
