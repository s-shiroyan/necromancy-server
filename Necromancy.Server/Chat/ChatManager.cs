using System.Collections.Generic;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat
{
    public class ChatManager
    {
        private readonly List<IChatMiddleware> _middleware;

            public ChatManager()
            {
                _middleware = new List<IChatMiddleware>();
            }

            public void AddMiddleware(IChatMiddleware middleware)
            {
                _middleware.Add(middleware);
            }

            public void RemoveMiddleware(IChatMiddleware middleware)
            {
                _middleware.Remove(middleware);
            }

            public void Handle(NecClient client, ChatMessage message)
            {
    
            }
    }
}
