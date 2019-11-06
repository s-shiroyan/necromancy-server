using Arrowgene.Services.Logging;

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

            SendCmdExecRequest sendCmdExecRequest = new SendCmdExecRequest(command);

            packet.Data.Position = 49;
            int parameterCount = 11;
            for (int i = 0; i < parameterCount; i++)
            {
                string parameter = packet.Data.ReadCString();
                sendCmdExecRequest.Parameter.Add(parameter);
                packet.Data.Position += 769; // Skip 769 Unknown bytes
            }

            packet.Data.Position = 7739;
            int u = packet.Data.ReadInt32();
            int u1 = packet.Data.ReadInt32();

            return sendCmdExecRequest;
        }
    }
}
