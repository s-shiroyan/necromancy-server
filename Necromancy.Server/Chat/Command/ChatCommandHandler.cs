using System;
using System.Collections.Generic;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat.Command
{
    public class ChatCommandHandler : ChatHandler
    {
        private readonly Dictionary<string, ChatCommand> _commands;

        public ChatCommandHandler()
        {
            _commands = new Dictionary<string, ChatCommand>();
        }

        public void AddCommand(ChatCommand command)
        {
            _commands.Add(command.Key, command);
        }

        public void HandleCommand(NecClient client, string command)
        {
            if (client == null)
            {
                return;
            }

            ChatMessage message = new ChatMessage(ChatMessageType.ChatCommand, client.Character.Name, $"/{command}");
            Handle(client, message, new ChatResponse());
        }

        public override void Handle(NecClient client, ChatMessage message, ChatResponse response)
        {
            if (message.Message == null
                || message.Message.Length <= 1
                || message.Message[0] != '/')
            {
                return;
            }

            string commandMessage = message.Message.Substring(1);
            string[] command = commandMessage.Split(' ');
            if (command.Length <= 0)
            {
                return;
            }

            if (!_commands.ContainsKey(command[0]))
            {
                return;
            }

            ChatCommand chatCommand = _commands[command[0]];
            if (client.Account.State < chatCommand.AccountState)
            {
                Logger.Debug(client,
                    $"Not entitled to execute command '{chatCommand.Key}' (State < Required: {client.Account.State} < {chatCommand.AccountState})");
                return;
            }

            int cmdLength = command.Length - 1;
            string[] subCommand;
            if (cmdLength > 0)
            {
                subCommand = new string[cmdLength];
                Array.Copy(command, 1, subCommand, 0, cmdLength);
            }
            else
            {
                subCommand = new string[0];
            }

            chatCommand.Execute(subCommand, client, message, response);
        }
    }
}
