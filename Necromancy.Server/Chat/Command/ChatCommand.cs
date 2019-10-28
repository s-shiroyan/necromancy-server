using Necromancy.Server.Model;

namespace Necromancy.Server.Chat.Command
{
    public abstract class ChatCommand
    {
        public abstract void Execute(string[] command, NecClient client, ChatMessage message, ChatResponse response);
        public abstract AccountStateType AccountState { get; }
        public abstract string Key { get; }
    }
}
