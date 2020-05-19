using System.Collections.Generic;
using Arrowgene.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Commands to find out who's on a ma.
    /// </summary>
    public class PlayersCommand : ServerChatCommand
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(PlayersCommand));

        public PlayersCommand(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (command[0] == null)
            {
                responses.Add(ChatResponse.CommandError(client,
                    $"Hi There!  Type /players world or /players map to see who's here.  You can also type a Character.Name"));
                return;
            }


            switch (command[0])
            {
                case "map": //tells you all the people on the Map you're on
                    foreach (NecClient theirClient in client.Map.ClientLookup.GetAll())
                    {
                        //if(theirClient.Map.Id != -1 && theirClient.Character.InstanceId != 0)
                            responses.Add(ChatResponse.CommandError(client,
                                $"{theirClient.Character.Name} {theirClient.Soul.Name} is on Map {theirClient.Character.MapId} with InstanceID {theirClient.Character.InstanceId}"));
                    }

                    break;

                case "world": //tells you all the people in the world
                    foreach (NecClient theirClient in Server.Clients.GetAll())
                    {
                        if (theirClient.Map != null)
                        {
                            responses.Add(ChatResponse.CommandError(client,
                            $"{theirClient.Character.Name} {theirClient.Soul.Name} is on Map {theirClient.Character.MapId} with InstanceID {theirClient.Character.InstanceId}"));
                        }
                    }

                    break;


                default: //you don't know what you're doing do you?
                    bool soulFound = false;

                    foreach (Character theirCharacter in Server.Characters.GetAll())
                    {
                        Logger.Debug($"Comparing {theirCharacter.Name} to {command[0]}");
                        if (theirCharacter.Name == command[0])
                        {
                            responses.Add(ChatResponse.CommandError(client,
                                $"{theirCharacter.Name} {theirCharacter.SoulName} is on Map {theirCharacter.MapId} with InstanceID {theirCharacter.InstanceId}"));
                            soulFound = true;
                        }
                    }

                    if (soulFound == false)
                    {
                        Logger.Error($"There is no command switch or player name matching : {command[0]} ");
                        responses.Add(
                            ChatResponse.CommandError(client, $"{command[0]} is not a valid players command."));
                    }

                    break;
            }
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "players";
        public override string HelpText => "usage: `/players [argument]` - returns a list of connected clients";
    }
}
