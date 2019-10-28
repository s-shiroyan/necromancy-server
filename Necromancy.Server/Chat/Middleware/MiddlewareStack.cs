using System;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat.Middleware
{
    /// <summary>
    /// Implementation of a middleware
    /// </summary>
    public class MiddlewareStack
    {
        private MiddlewareDelegate _middlewareDelegate;

        public MiddlewareStack(MiddlewareDelegate kernel)
        {
            _middlewareDelegate = kernel;
        }

        public void Start(NecClient client, ChatMessage message, ChatResponse response)
        {
            _middlewareDelegate(client, message, response);
        }

        public MiddlewareStack Use(Func<MiddlewareDelegate, MiddlewareDelegate> middleware)
        {
            _middlewareDelegate = middleware(_middlewareDelegate);
            return this;
        }

        public MiddlewareStack Use(IChatMiddleware middleware)
        {
            return Use(next => (client, message, response) => middleware.Handle(client, message, response, next));
        }
    }
}
