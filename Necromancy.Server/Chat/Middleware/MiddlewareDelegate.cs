using Necromancy.Server.Model;

namespace Necromancy.Server.Chat.Middleware
{
    public delegate void MiddlewareDelegate(NecClient client, ChatMessage message, ChatResponse response);
}
