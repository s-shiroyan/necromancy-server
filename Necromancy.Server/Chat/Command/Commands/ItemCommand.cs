using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Items;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Quick item test commands.
    /// </summary>
    public class ItemCommand : ServerChatCommand
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(ItemCommand));

        public ItemCommand(NecServer server) : base(server)
        {
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "itm";
        public override string HelpText => "usage: `/itm [itemId] (optional)u`";

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (command.Length < 1)
            {
                responses.Add(ChatResponse.CommandError(client, "To few arguments"));
                return;
            }
            
            if (!int.TryParse(command[0], out int itemId))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[0]}"));
                return;
            }
            
            if (client.Character == null)
            {
                responses.Add(ChatResponse.CommandError(client, "Character is null"));
                return;
            }

            bool IsIdentified = true;
            if (command.Length > 1 && command[1] == "u")
            {
                IsIdentified = false;
            }

            ItemService itemService = new ItemService(client.Character);
            SpawnedItem spawnedItem;

            try
            {
                PacketResponse pResp;
                if (IsIdentified) { 
                    spawnedItem = itemService.SpawnIdentifiedItem(itemId);
                    pResp = new RecvItemInstance(client, spawnedItem);                    
                } else
                {
                    spawnedItem = itemService.SpawnUnidentifiedItem(itemId);
                    pResp = new RecvItemInstanceUnidentified(client, spawnedItem);                    
                }
                Router.Send(pResp);
            } catch (ItemException e) { responses.Add(ChatResponse.CommandError(client, e.Message)); }            
        }        
    }
}
