namespace Necromancy.Server.Chat
{
    public interface IChatMiddleware
    {
        void HandleMessage(ChatMessage message);
    }
}
