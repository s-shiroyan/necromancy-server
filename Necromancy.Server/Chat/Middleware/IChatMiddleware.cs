using Necromancy.Server.Model;

namespace Necromancy.Server.Chat.Middleware
{
    public interface IChatMiddleware
    {
        void Handle(NecClient client, ChatMessage message, ChatResponse response, MiddlewareDelegate next);
    }
}
