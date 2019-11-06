using System.Collections.Generic;
using Necromancy.Server.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat.Command.Commands
{
    //runs a command from FileReader.CS for testing output
    public class ReadFile : ServerChatCommand
    {
        public ReadFile(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            FileReader.GameFileReader(client);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "ReadFile";
    }
}
