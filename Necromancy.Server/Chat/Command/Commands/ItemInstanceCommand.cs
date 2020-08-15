using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Quick item test commands.
    /// </summary>
    public class ItemInstanceCommand : ServerChatCommand
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(ItemInstanceCommand));

        public ItemInstanceCommand(NecServer server) : base(server)
        {
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "itemi";
        public override string HelpText => "usage: `/itemi [itemId]`";

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

            if (!Server.Items.ContainsKey(itemId))
            {
                responses.Add(ChatResponse.CommandError(client, $"ItemId: '{itemId}' does not exist"));
                return;
            }

            Item item = Server.Items[itemId];
            Character character = client.Character;
            if (character == null)
            {
                responses.Add(ChatResponse.CommandError(client, "Character is null"));
                return;
            }

            // Create InventoryItem
            InventoryItem inventoryItem = new InventoryItem();
            inventoryItem.Item = item;
            inventoryItem.ItemId = item.Id;
            inventoryItem.Quantity = 1;
            inventoryItem.CurrentDurability = item.Durability;
            inventoryItem.CharacterId = character.Id;
            inventoryItem.CurrentEquipmentSlotType = EquipmentSlotType.NONE;
            inventoryItem.State = 0;

            client.Character.Inventory.AddInventoryItem(inventoryItem);
            if (!Server.Database.InsertInventoryItem(inventoryItem))
            {
                responses.Add(ChatResponse.CommandError(client, "Could not save InventoryItem to Database"));
                return;
            }

            RecvItemInstance recvItemInstance = new RecvItemInstance(inventoryItem, client);
            Router.Send(recvItemInstance, client);
            responses.Add(ChatResponse.CommandInfo(client, $"item {item.Id} in slot {inventoryItem.BagSlotIndex} in bag {inventoryItem.BagId}"));

        
        }
    }
}
