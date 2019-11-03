namespace Necromancy.Server.Chat
{
    public class ChatMessage
    {
        public ChatMessage(ChatMessageType messageType, string recipient, string message)
        {
            MessageType = messageType;
            Recipient = recipient;
            Message = message;
        }

        public ChatMessageType MessageType { get; }
        public string Recipient { get; }
        public string Message { get; }
    }
}
