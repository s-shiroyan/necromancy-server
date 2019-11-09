using System.Collections.Generic;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Changes the map
    /// </summary>
    public class SendMapChangeForce : ServerChatCommand
    {
        public SendMapChangeForce(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (!int.TryParse(command[0], out int mapId))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[0]}"));
                return;
            }

            if (!Server.Maps.TryGet(mapId, out Map map))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid MapId: {mapId}"));
                return;
            }

            map.EnterForce(client);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "map";
    }
}
