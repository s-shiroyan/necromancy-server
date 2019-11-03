using Arrowgene.Services.Logging;
using Necromancy.Server.Chat.Command;
using Necromancy.Server.Chat.Middleware;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat
{
    public class ChatManager
    {
        private readonly MiddlewareStack _middlewareStack;
        private readonly NecLogger _logger;

        public ChatManager()
        {
            _logger = LogProvider.Logger<NecLogger>(this);
            _middlewareStack = new MiddlewareStack(Kernel);
            CommandHandler = new ChatCommandHandler();
            AddMiddleware(CommandHandler);
        }

        public ChatCommandHandler CommandHandler { get; }

        public void AddMiddleware(IChatMiddleware middleware)
        {
            _middlewareStack.Use(middleware);
        }

        public void Handle(NecClient client, ChatMessage message)
        {
            if (client == null)
            {
                _logger.Debug("Client is Null");
                return;
            }

            if (message == null)
            {
                _logger.Debug(client, "Chat Message is Null");
                return;
            }

            _middlewareStack.Start(client, message, new ChatResponse());
        }

        private void Kernel(NecClient client, ChatMessage request, ChatResponse response)
        {
        }
    }
}
