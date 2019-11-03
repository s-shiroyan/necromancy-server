using Arrowgene.Services.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat.Middleware
{
    public abstract class ChatMiddleware : IChatMiddleware
    {
        protected ChatMiddleware()
        {
            Logger = LogProvider.Logger<NecLogger>(this);
        }

        protected NecLogger Logger { get; }

        public abstract void Handle(NecClient client, ChatMessage message, ChatResponse response,
            MiddlewareDelegate next);
    }
}
