using System.Collections.Generic;
using Arrowgene.Services.Logging;
using Necromancy.Server.Chat.Command;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Response;

namespace Necromancy.Server.Chat
{
    public class ChatManager
    {
        private readonly List<IChatHandler> _handler;
        private readonly NecLogger _logger;
        private readonly NecServer _server;

        public ChatManager(NecServer server)
        {
            _server = server;
            _logger = LogProvider.Logger<NecLogger>(this);
            _handler = new List<IChatHandler>();
            CommandHandler = new ChatCommandHandler();
            AddHandler(CommandHandler);
        }

        public ChatCommandHandler CommandHandler { get; }

        public void AddHandler(IChatHandler handler)
        {
            _handler.Add(handler);
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

            ChatResponse response = new ChatResponse();
            response.Message = message.Message;
            response.CharacterId = client.Character.Id;
            response.CharacterName = client.Character.Name;
            response.SoulName = client.Soul.Name;
            response.Deliver = true;
            response.ErrorType = ChatErrorType.Success;
            response.MessageType = message.MessageType;
            response.Recipients.Add(client);
            foreach (IChatHandler handler in _handler)
            {
                handler.Handle(client, message, response);
            }

            if (!response.Deliver)
            {
                RespondPostMessage(client, ChatErrorType.GenericUnknownStatement);
                return;
            }
            RespondPostMessage(client, ChatErrorType.Success);
            RecvChatNotifyMessage notifyMessage = new RecvChatNotifyMessage(response);
            notifyMessage.AddClients(response.Recipients);
            _server.Router.Send(notifyMessage);
        }

        private void RespondPostMessage(NecClient client, ChatErrorType chatErrorType)
        {
            RecvChatPostMessageR postMessageResponse = new RecvChatPostMessageR(chatErrorType);
            postMessageResponse.AddClients(client);
            _server.Router.Send(postMessageResponse);
        }
    }
}
