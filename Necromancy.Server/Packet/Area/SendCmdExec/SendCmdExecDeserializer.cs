using System;
using Arrowgene.Services.Logging;
using Necromancy.Server.Chat;

namespace Necromancy.Server.Packet.Area.SendCmdExec
{
    public class SendCmdExecDeserializer : IPacketDeserializer<SendCmdExecRequest>
    {
        private ILogger _logger;

        public SendCmdExecDeserializer()
        {
            _logger = LogProvider.Logger(this);
        }

        public SendCmdExecRequest Deserialize(NecPacket packet)
        {
            string command = packet.Data.ReadCString();
            return new SendCmdExecRequest(command);
        }
    }
}
