using System.Collections.Generic;
using Arrowgene.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat.Command.Commands
{
    public class SendMapCoord : ServerChatCommand
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(SendMapCoord));

        public SendMapCoord(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            Logger.Debug(
                $"MapId [{client.Character.MapId}] X[{client.Character.X}] Y[{client.Character.Y}] Z[{client.Character.Z}] Direction[{client.Character.Heading}]");
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "MapCoord";
    }
}
