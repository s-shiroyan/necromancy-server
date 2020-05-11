using System;
using Arrowgene.Logging;
using Necromancy.Server.Chat;
using Necromancy.Server.Logging;

namespace Necromancy.Server.Packet.Area.SendChatPostMessage
{
    public class SendChatPostMessageDeserializer : IPacketDeserializer<ChatMessage>
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(SendChatPostMessageDeserializer));

        public SendChatPostMessageDeserializer()
        {
        }

        public ChatMessage Deserialize(NecPacket packet)
        {
            int messageTypeValue = packet.Data.ReadInt32();
            if (!Enum.IsDefined(typeof(ChatMessageType), messageTypeValue))
            {
                Logger.Error($"ChatMessageType: {messageTypeValue} not defined");
                return null;
            }

            ChatMessageType messageType = (ChatMessageType) messageTypeValue;
            string recipient = packet.Data.ReadCString();
            string message = packet.Data.ReadCString();

            return new ChatMessage(messageType, recipient, message);
        }
    }
}
