using System;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Logging
{
    public class NecLogPacket : NecPacket
    {
        public NecLogPacket(string clientIdentity, NecPacket packet, NecLogType logType, ServerType serverType)
            : base(packet.Id, packet.Data.Clone())
        {
            ServerType = serverType;
            Header = packet.Header;
            LogType = logType;
            TimeStamp = DateTime.Now;
            ClientIdentity = clientIdentity;
        }
        
        public string ClientIdentity { get; }
        public NecLogType LogType { get; }
        public DateTime TimeStamp { get; }
        public ServerType ServerType { get; }
        public string Hex => Data.ToHexString('-');
        public string Ascii => Data.ToAsciiString(true);
        public string HeaderHex => Util.ToHexString(Header, '-');

        public string ToLogText()
        {
            string idName = GetIdName();
            String log = $"{ClientIdentity} Packet Log";


            switch (idName)
            {
                case "send_character_view_offset":
                    int viewOffset = int.Parse(Hex, System.Globalization.NumberStyles.HexNumber);
                    log += $"[{TimeStamp:HH:mm:ss}][Typ:{LogType}]";
                    log += $"[{ServerType}]";
                    log += $"[Id:0x{Id:X2}|{Id}][Len(Data/Total):{Data.Size}/{Data.Size + Header.Length}][Header:{HeaderHex}]";
                    log += $"[{idName}]";
                    log += Environment.NewLine;
                    log += "HEX:";
                    log += Hex;
                    log += "   View Offest:";
                    log += viewOffset;
                    log += Environment.NewLine;
                    log += "----------";
                    return log;

                case "send_movement_info":
                    log += $"[{TimeStamp:HH:mm:ss}][Typ:{LogType}]";
                    log += $"[{ServerType}]";
                    log += $"[Id:0x{Id:X2}|{Id}][Len(Data/Total):{Data.Size}/{Data.Size + Header.Length}][Header:{HeaderHex}]";
                    log += $"[{idName}]";
                    log += Environment.NewLine;
                    log += "HEX:";
                    log += Hex;
                    log += Environment.NewLine;
                    log += "----------";
                    return log;

                default:
                    if (Data.Size <= 50)
                    {
                        log += Environment.NewLine;
                        log += "----------";
                        log += Environment.NewLine;
                        log += $"[{TimeStamp:HH:mm:ss}][Typ:{LogType}]";
                        log += $"[{ServerType}]";
                        log += Environment.NewLine;
                        log += $"[Id:0x{Id:X2}|{Id}][Len(Data/Total):{Data.Size}/{Data.Size + Header.Length}][Header:{HeaderHex}]";

                        if (idName != null)
                        {
                            log += $"[{idName}]";
                        }
                        if (Hex != null)
                        {
                            log += Environment.NewLine;
                            log += "ASCII:";
                            log += Environment.NewLine;
                            log += Ascii;
                            log += Environment.NewLine;
                            log += "HEX:";
                            log += Environment.NewLine;
                            log += Hex;
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
                        log += $"[{TimeStamp:HH:mm:ss}][Typ:{LogType}]";
                        log += $"[{ServerType}]";
                        log += Environment.NewLine;
                        log += $"[Id:0x{Id:X2}|{Id}][Len(Data/Total):{Data.Size}/{Data.Size + Header.Length}][Header:{HeaderHex}]";
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

        public string GetIdName()
        {
            if (Enum.IsDefined(typeof(AuthPacketId), Id))
            {
                AuthPacketId authPacketId = (AuthPacketId) Id;
                return authPacketId.ToString();
            }

            if (Enum.IsDefined(typeof(MsgPacketId), Id))
            {
                MsgPacketId msgPacketId = (MsgPacketId) Id;
                return msgPacketId.ToString();
            }

            if (Enum.IsDefined(typeof(AreaPacketId), Id))
            {
                AreaPacketId areaPacketId = (AreaPacketId) Id;
                return areaPacketId.ToString();
            }

            return null;
        }
    }
}
