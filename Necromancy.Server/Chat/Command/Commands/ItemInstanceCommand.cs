using System;
using System.Collections.Generic;
using System.Linq;
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

            int[] DonkeyItems = new int[] {100101,100102,100103,100104,100105,100106,100107,100108,100109,100110,100111,100112,100113,100114,100115,100116,100181,100191,100192,100193,100201,100202,100203,100204,100205,100206,100207,100208,100209,100210,100211,100212,100213,100214,100301,100302,100303,100304,100305,100306,100307,100308,100401,100402,100403,100404,100405,100406,100407,100408};
            int numItems = DonkeyItems.Count();

            for (int i = 0; i < numItems*5; i++)
            {
                Item item = Server.Items[100101];
                // Create InventoryItem
                InventoryItem inventoryItem = new InventoryItem();
                inventoryItem.Id = item.Id + i;
                inventoryItem.ItemId = item.Id;
                inventoryItem.Quantity = 1;
                inventoryItem.CurrentDurability = item.Durability;
                inventoryItem.CharacterId = client.Character.Id;
                inventoryItem.CurrentEquipmentSlotType = EquipmentSlotType.NONE;
                inventoryItem.State = 0;
                inventoryItem.StorageType = (int)BagType.AvatarInventory;
                client.Character.Inventory.AddAvatarItem(inventoryItem);
                inventoryItem.Item.ItemType = (ItemType)(i);
                inventoryItem.Item.Name = $"ItemType + {i}"; 

                RecvItemInstance recvItemInstance = new RecvItemInstance(inventoryItem, client);
                Router.Send(recvItemInstance, client);
                RecvItemInstanceUnidentified recvItemInstanceUnidentified = new RecvItemInstanceUnidentified(inventoryItem, client);
                Router.Send(recvItemInstanceUnidentified, client);
                responses.Add(ChatResponse.CommandInfo(client, $"item {item.Id} in slot {inventoryItem.BagSlotIndex} in bag {inventoryItem.BagId}"));
            }
        
        }
    }
}
