using System;
using System.Collections.Generic;
using Arrowgene.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Commands to find out who's on a map.
    /// </summary>
    public class PlayersCommand : ServerChatCommand
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(PlayersCommand));

        public PlayersCommand(NecServer server) : base(server)
        {
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "players";
        public override string HelpText => "usage: `/players [map|world|{characterName}]`";

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (command.Length < 1)
            {
                responses.Add(ChatResponse.CommandError(client, "To few arguments"));
                return;
            }

            switch (command[0])
            {
                case "map":
                {
                    foreach (NecClient theirClient in client.Map.ClientLookup.GetAll())
                    {
                        responses.Add(ChatResponse.CommandInfo(client,
                            $"{theirClient.Character.Name} {theirClient.Soul.Name} is on Map {theirClient.Character.MapId} with InstanceID {theirClient.Character.InstanceId}"));
                    }

                    break;
                }
                case "world":
                {
                    foreach (NecClient theirClient in Server.Clients.GetAll())
                    {
                        if (theirClient.Map != null)
                            responses.Add(ChatResponse.CommandInfo(client,
                                $"{theirClient.Character.Name} {theirClient.Soul.Name} is on Map {theirClient.Character.MapId} with InstanceID {theirClient.Character.InstanceId}"));
                    }

                    break;
                }

                default:
                    foreach (NecClient otherClient in Server.Clients.GetAll())
                    {
                        Character character = otherClient.Character;
                        if (character == null)
                        {
                            continue;
                        }

                        if (character.Name.Equals(command[0], StringComparison.InvariantCultureIgnoreCase))
                        {
                            string mapName = "None";
                            Map map = client.Map;
                            if (map != null)
                            {
                                mapName = $"{map.Id} ({map.Place})";
                            }

                            responses.Add(ChatResponse.CommandInfo(client,
                                $"CharacterName: {character.Name} SoulId:{character.SoulId} Map:{mapName} InstanceId: {character.InstanceId}"));
                            return;
                        }
                    }

                    responses.Add(ChatResponse.CommandError(client, $"Character: '{command[0]}' not found"));
                    break;
            }
        }
    }
}
