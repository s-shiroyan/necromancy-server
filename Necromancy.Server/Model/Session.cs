using System;
using Arrowgene.Services.Networking.Tcp;

namespace Necromancy.Server.Model
{
    /// <summary>
    /// Status of client data across sockets.
    /// </summary>
    public class Session
    {
        public string Key { get; }
        public DateTime Creation { get; }
        public Account Account { get; set; }
        public Soul Soul { get; set; }
        public Character Character { get; set; }
        public Channel Channel { get; set; }
        public Map Map { get; set; }
        public ITcpSocket authSocket { get; set; }
        public ITcpSocket msgSocket { get; set; }
        public ITcpSocket areaSocket { get; set; }

        public Session(string sessionKey, Account account)
        {
            Key = sessionKey;
            Creation = DateTime.Now;
            Account = account;
        }
    }
}