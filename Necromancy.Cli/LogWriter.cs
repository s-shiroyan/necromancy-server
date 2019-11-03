using System;
using System.Collections.Generic;
using System.Globalization;
using Arrowgene.Services.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;

namespace Necromancy.Cli
{
    public class LogWriter
    {
        private readonly object _consoleLock;
        private Dictionary<ServerType, HashSet<uint>> _ignoredPackets;

        public LogWriter()
        {
            _ignoredPackets = new Dictionary<ServerType, HashSet<uint>>();
            _consoleLock = new object();
            LogProvider.GlobalLogWrite += LogProviderOnGlobalLogWrite;
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

            lock (_consoleLock)
            {
                Console.ForegroundColor = consoleColor;
                Console.WriteLine(text);
                Console.ResetColor();
            }
        }

        public string CreatePacketLog(NecLogPacket logPacket)
        {
            string idName = logPacket.PacketIdName;
            String log = $"{logPacket.ClientIdentity} Packet Log";


            switch (idName)
            {
                case "send_character_view_offset":
                    int viewOffset = int.Parse(logPacket.Hex, NumberStyles.HexNumber);
                    log += $"[{logPacket.TimeStamp:HH:mm:ss}][Typ:{logPacket.LogType}]";
                    log += $"[{logPacket.ServerType}]";
                    log +=
                        $"[Id:0x{logPacket.Id:X2}|{logPacket.Id}][Len(Data/Total):{logPacket.Data.Size}/{logPacket.Data.Size + logPacket.Header.Length}][Header:{logPacket.HeaderHex}]";
                    log += $"[{idName}]";
                    log += Environment.NewLine;
                    log += "HEX:";
                    log += logPacket.Hex;
                    log += "   View Offest:";
                    log += viewOffset;
                    log += Environment.NewLine;
                    log += "----------";
                    return log;

                case "send_movement_info":
                    log += $"[{logPacket.TimeStamp:HH:mm:ss}][Typ:{logPacket.LogType}]";
                    log += $"[{logPacket.ServerType}]";
                    log +=
                        $"[Id:0x{logPacket.Id:X2}|{logPacket.Id}][Len(Data/Total):{logPacket.Data.Size}/{logPacket.Data.Size + logPacket.Header.Length}][Header:{logPacket.HeaderHex}]";
                    log += $"[{idName}]";
                    log += Environment.NewLine;
                    log += "HEX:";
                    log += logPacket.Hex;
                    log += Environment.NewLine;
                    log += "----------";
                    return log;

                case "send_system_register_error_report":
                    log += Environment.NewLine;
                    log += "----------";
                    log += Environment.NewLine;
                    log += $"[{logPacket.TimeStamp:HH:mm:ss}][Typ:{logPacket.LogType}]";
                    log += $"[{logPacket.ServerType}]";
                    log += Environment.NewLine;
                    log +=
                        $"[Id:0x{logPacket.Id:X2}|{logPacket.Id}][Len(Data/Total):{logPacket.Data.Size}/{logPacket.Data.Size + logPacket.Header.Length}][Header:{logPacket.HeaderHex}]";

                    if (idName != null)
                    {
                        log += $"[{idName}]";
                    }

                    if (logPacket.Hex != null)
                    {
                        log += Environment.NewLine;
                        log += "ASCII:";
                        log += Environment.NewLine;
                        log += logPacket.Ascii;
                        log += Environment.NewLine;
                        log += "HEX:";
                        log += Environment.NewLine;
                        log += logPacket.Hex;
                    }

                    log += Environment.NewLine;
                    log += "----------";
                    return log;

                default:
                    if (logPacket.Data.Size <= 60)
                    {
                        log += Environment.NewLine;
                        log += "----------";
                        log += Environment.NewLine;
                        log += $"[{logPacket.TimeStamp:HH:mm:ss}][Typ:{logPacket.LogType}]";
                        log += $"[{logPacket.ServerType}]";
                        log += Environment.NewLine;
                        log +=
                            $"[Id:0x{logPacket.Id:X2}|{logPacket.Id}][Len(Data/Total):{logPacket.Data.Size}/{logPacket.Data.Size + logPacket.Header.Length}][Header:{logPacket.HeaderHex}]";

                        if (idName != null)
                        {
                            log += $"[{idName}]";
                        }

                        if (logPacket.Hex != null)
                        {
                            log += Environment.NewLine;
                            log += "ASCII:";
                            log += Environment.NewLine;
                            log += logPacket.Ascii;
                            log += Environment.NewLine;
                            log += "HEX:";
                            log += Environment.NewLine;
                            log += logPacket.Hex;
                        }

                        log += Environment.NewLine;
                        log += "----------";
                        return log;
                    }
                    else
                    {
                        log += Environment.NewLine;
                        log += "----------";
                        log += Environment.NewLine;
                        log += $"[{logPacket.TimeStamp:HH:mm:ss}][Typ:{logPacket.LogType}]";
                        log += $"[{logPacket.ServerType}]";
                        log += Environment.NewLine;
                        log +=
                            $"[Id:0x{logPacket.Id:X2}|{logPacket.Id}][Len(Data/Total):{logPacket.Data.Size}/{logPacket.Data.Size + logPacket.Header.Length}][Header:{logPacket.HeaderHex}]";
                        if (idName != null)
                        {
                            log += $"[{idName}]";
                        }

                        log += Environment.NewLine;
                        log += "NecLogPacket.cs . . . . . . .Large Packet: Skipping Console Output for performance";
                        return log;
                    }
            }
        }
    }
}
