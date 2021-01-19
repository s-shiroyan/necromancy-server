using Arrowgene.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Systems.Item;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Chat.Command.Commands
{
    class ItemGeneratorCommand : ServerChatCommand
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(ItemCommand));

        public ItemGeneratorCommand(NecServer server) : base(server)
        {
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "genitems";
        public override string HelpText => "usage: `/genitems [package]`";

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (command.Length < 1)
            {
                responses.Add(ChatResponse.CommandError(client, "To few arguments"));
                return;
            }

            int[] itemIds;
            ItemLocation[] locs;
            switch (command[0])
            {
                case "bags":
                    int size = 10;
                    itemIds = new int[size];
                    locs = new ItemLocation[size];
                    itemIds[0] = 50100501;
                    itemIds[1] = 50100502;
                    itemIds[2] = 50100503;
                    itemIds[3] = 50100504;
                    itemIds[4] = 50100505;
                    itemIds[5] = 50100506;
                    itemIds[6] = 50100507;
                    itemIds[7] = 50100511;
                    itemIds[8] = 50100512;
                    itemIds[9] = 50100513;

                    ItemService itemService = new ItemService(client.Character);
                    List<ItemInstance> items = itemService.SpawnItemInstances(ItemZoneType.AdventureBag, itemIds);
                    break;
                default:
                    responses.Add(ChatResponse.CommandError(client, $"Invalid Package: {command[0]}"));
                    return;
            }
            
        }
    }
}

