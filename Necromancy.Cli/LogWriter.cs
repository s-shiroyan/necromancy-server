using System;
using System.Collections.Generic;
using System.Text;
using Arrowgene.Services.Buffers;
using Arrowgene.Services.Logging;
using Necromancy.Cli.Argument;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Cli
{
    public class LogWriter : ISwitchConsumer
    {
        private readonly object _consoleLock;

        private Dictionary<ServerType, HashSet<ushort>> _serverTypeBlacklist;
        private Dictionary<ServerType, HashSet<ushort>> _serverTypeWhitelist;
        private HashSet<ushort> _packetIdWhitelist;
        private HashSet<ushort> _packetIdBlacklist;

        private readonly ILogger _logger;

        public LogWriter()
        {
            _logger = LogProvider.Logger(this);
            _serverTypeBlacklist = new Dictionary<ServerType, HashSet<ushort>>();
            _serverTypeWhitelist = new Dictionary<ServerType, HashSet<ushort>>();
            _packetIdWhitelist = new HashSet<ushort>();
            _packetIdBlacklist = new HashSet<ushort>();
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


        public void WhitelistPacket(ushort packetId)
        {
            if (_packetIdWhitelist.Contains(packetId))
            {
                _logger.Error($"PacketId:{packetId} is already whitelisted");
                return;
            }

            _packetIdWhitelist.Add(packetId);
        }

        public void BlacklistPacket(ushort packetId)
        {
            if (_packetIdBlacklist.Contains(packetId))
            {
                _logger.Error($"PacketId:{packetId} is already blacklisted");
                return;
            }

            _packetIdBlacklist.Add(packetId);
        }

        public void WhitelistAuthPacket(AuthPacketId authPacketId)
        {
            if (!AddToServerTypeList(_serverTypeWhitelist, ServerType.Auth, (ushort) authPacketId))
            {
                _logger.Error($"WhitelistAuthPacket: PacketId:{authPacketId} is already added");
            }
        }

        public void WhitelistMsgPacket(MsgPacketId msgPacketId)
        {
            if (!AddToServerTypeList(_serverTypeWhitelist, ServerType.Msg, (ushort) msgPacketId))
            {
                _logger.Error($"WhitelistMsgPacket: PacketId:{msgPacketId} is already added");
            }
        }

        public void WhitelistAreaPacket(AreaPacketId areaPacketId)
        {
            if (!AddToServerTypeList(_serverTypeWhitelist, ServerType.Area, (ushort) areaPacketId))
            {
                _logger.Error($"WhitelistAreaPacket: PacketId:{areaPacketId} is already added");
            }
        }

        public void BlacklistAuthPacket(AuthPacketId authPacketId)
        {
            if (!AddToServerTypeList(_serverTypeBlacklist, ServerType.Auth, (ushort) authPacketId))
            {
                _logger.Error($"BlacklistAuthPacket: PacketId:{authPacketId} is already added");
            }
        }

        public void BlacklistMsgPacket(MsgPacketId msgPacketId)
        {
            if (!AddToServerTypeList(_serverTypeBlacklist, ServerType.Msg, (ushort) msgPacketId))
            {
                _logger.Error($"BlacklistMsgPacket: PacketId:{msgPacketId} is already added");
            }
        }

        public void BlacklistAreaPacket(AreaPacketId areaPacketId)
        {
            if (!AddToServerTypeList(_serverTypeBlacklist, ServerType.Area, (ushort) areaPacketId))
            {
                _logger.Error($"BlacklistAreaPacket: PacketId:{areaPacketId} is already added");
            }
        }

        private bool AddToServerTypeList(Dictionary<ServerType, HashSet<ushort>> dictionary, ServerType serverType,
            ushort packetId)
        {
            HashSet<ushort> hashSet;
            if (dictionary.ContainsKey(serverType))
            {
                hashSet = dictionary[serverType];
            }
            else
            {
                hashSet = new HashSet<ushort>();
                dictionary.Add(serverType, hashSet);
            }

            if (hashSet.Contains(packetId))
            {
                return false;
            }

            return hashSet.Add(packetId);
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
            Switches.Add(
                new SwitchProperty<List<Tuple<ServerType?, ushort>>>(
                    "--b-list",
                    "--b-list=1:1000,2000,3:4000 (ServerType:PacketId | PacketId)",
                    "A blacklist that drops all packet logs specified",
                    TryParsePacketIdList,
                    results =>
                    {
                        PacketIdListAssigner(results, _serverTypeBlacklist, AddToServerTypeList, BlacklistPacket);
                    }
                )
            );
            Switches.Add(
                new SwitchProperty<List<Tuple<ServerType?, ushort>>>(
                    "--w-list",
                    "--w-list=1:1000,2000,3:4000 (ServerType:PacketId | PacketId)",
                    "A whitelist that only logs packets specified",
                    TryParsePacketIdList,
                    results =>
                    {
                        PacketIdListAssigner(results, _serverTypeWhitelist, AddToServerTypeList, WhitelistPacket);
                    }
                )
            );
        }

        /// <summary>
        /// parses strings like "1:1000,2000,3:4000" into ServerType and PacketId
        /// </summary>
        private bool TryParsePacketIdList(string value, out List<Tuple<ServerType?, ushort>> result)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = null;
                return false;
            }

            string[] values = value.Split(",");

            result = new List<Tuple<ServerType?, ushort>>();
            foreach (string entry in values)
            {
                if (entry.Contains(":"))
                {
                    string[] keyValue = entry.Split(":");
                    if (keyValue.Length != 2)
                    {
                        return false;
                    }

                    if (!byte.TryParse(keyValue[0], out byte serverTypeValue))
                    {
                        return false;
                    }

                    if (!Enum.IsDefined(typeof(ServerType), serverTypeValue))
                    {
                        return false;
                    }

                    ServerType serverType = (ServerType) serverTypeValue;
                    if (!ushort.TryParse(keyValue[1], out ushort val))
                    {
                        return false;
                    }

                    result.Add(new Tuple<ServerType?, ushort>(serverType, val));
                }
                else
                {
                    if (!ushort.TryParse(entry, out ushort val))
                    {
                        return false;
                    }

                    result.Add(new Tuple<ServerType?, ushort>(null, val));
                }
            }

            return true;
        }

        private void PacketIdListAssigner(List<Tuple<ServerType?, ushort>> results,
            Dictionary<ServerType, HashSet<ushort>> dictionary,
            Func<Dictionary<ServerType, HashSet<ushort>>, ServerType, ushort, bool> addToServerTypeList,
            Action<ushort> addToPacketList)
        {
            foreach (Tuple<ServerType?, ushort> entry in results)
            {
                if (entry.Item1.HasValue)
                {
                    addToServerTypeList(dictionary, entry.Item1.Value, entry.Item2);
                }
                else
                {
                    addToPacketList(entry.Item2);
                }
            }
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

            if (_serverTypeBlacklist.ContainsKey(serverType)
                && _serverTypeBlacklist[serverType].Contains(packetId))
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
