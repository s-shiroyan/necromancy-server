using System;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Logging
{
    public class NecLogPacket : NecPacket
    {
        public NecLogPacket(NecClient client, NecPacket packet, NecLogType logType, string identity)
            : base(packet.Id, packet.Data.Clone())
        {
            Identity = identity;
            Header = packet.Header;
            LogType = logType;
            TimeStamp = DateTime.Now;
            Client = client;
        }

        public NecLogPacket(NecClient client, NecPacket packet, NecLogType logType)
            : this(client, packet, logType, null)
        {
        }

        public NecClient Client { get; }
        public NecLogType LogType { get; }
        public DateTime TimeStamp { get; }
        public string Identity { get; }
        public string Hex => Data.ToHexString('-');
        public string Ascii => Data.ToAsciiString(true);
        public string HeaderHex => Util.ToHexString(Header, '-');

        public string ToLogText()
        {
            String log = $"{Client.Identity} Packet Log";
            log += Environment.NewLine;
            log += "----------";
            log += Environment.NewLine;
            log += $"[{TimeStamp:HH:mm:ss}][Typ:{LogType}]";
            if (Identity != null)
            {
                log += $"[{Identity}]";
            }

            log += Environment.NewLine;
            log += $"[Id:0x{Id:X2}|{Id}][Len(Data/Total):{Data.Size}/{Data.Size + Header.Length}][Header:{HeaderHex}]";
            string idName = GetIdName();
            if (idName != null)
            {
                log += $"[{idName}]";
            }

            log += Environment.NewLine;
            log += "ASCII:";
            log += Environment.NewLine;
            log += Ascii;
            log += Environment.NewLine;
            log += "HEX:";
            log += Environment.NewLine;
            log += Hex;
            log += Environment.NewLine;
            log += "----------";
            return log;
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