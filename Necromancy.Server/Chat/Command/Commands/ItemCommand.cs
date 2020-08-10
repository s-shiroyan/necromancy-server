using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

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
        public override string HelpText => "usage: `/itm [itemId]`";

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

            RecvItemInstanceUnidentified recvItemInstanceUnidentified = new RecvItemInstanceUnidentified(inventoryItem);
            Router.Send(recvItemInstanceUnidentified, client);
            responses.Add(ChatResponse.CommandInfo(client, $"item {item.Id} in slot {inventoryItem.BagSlotIndex} in bag {inventoryItem.BagId}"));

        }

        public void RecvItemInstance(NecClient client, InventoryItem inventoryItem)
        {
            // TODO find the purpose of this call, is it just an update?
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64((ulong) inventoryItem.Id); //ItemID
            // res.WriteInt32(inventoryItem.Id);
            // res.WriteInt32(iid);
            res.WriteInt32(inventoryItem.Item.Id); // 0 does not display icon
            res.WriteByte(1); //Number of "items"
            res.WriteInt32(0); //--Item status, in multiples of numbers, 8 = blessed/cursed/both 
            res.WriteFixedString(inventoryItem.Item.Name, 0x10);
            res.WriteByte(0); // 0 = adventure bag. 1 = character equipment
            res.WriteByte(0); // 0~2 // maybe.. more bag index?
            res.WriteInt16(2); //-- bag slot index
            res.WriteInt32(0); //-- EquipmentSlotId (when equipped 0= not equipped)  //Slot spots? 10200101 here caused certain spots to have an item, -1 for all slots(avatar included)  /13
            res.WriteInt32(inventoryItem.Item.Id); //Percentage stat, 9 max i think       /12
            res.WriteByte(0); //1
            res.WriteByte(0); // Dest slot
            res.WriteCString(inventoryItem.Item.Name); // no max length
            res.WriteInt16(short.MaxValue); //10
            res.WriteInt16(short.MaxValue); //9
            res.WriteInt32(inventoryItem.Item.Id); //Divides max % by this number     //8
            res.WriteByte(0); //7
            res.WriteInt32(inventoryItem.Item.Id); //6

            int numEntries = 1;
            res.WriteInt32(numEntries); // less than or equal to 2
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(0 + i);
            }

            numEntries = 1;
            res.WriteInt32(numEntries); // less than or equal to 3
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteByte(1); //bool
                res.WriteInt32(1);
                res.WriteInt32(1);
                res.WriteInt32(1);
            }

            res.WriteInt32(inventoryItem.Item.Id); //4

            res.WriteInt32(inventoryItem.Item.Id); //5
            res.WriteInt16(0xFF); // 0 = green | 0xFF = normal

            res.WriteInt32(inventoryItem.Item.Id); //Guard protection toggle, 1 = on, everything else is off //3
            res.WriteInt16(0); //2

            Router.Send(client, (ushort) AreaPacketId.recv_item_instance, res, ServerType.Area);
        }

        public void UpdateState(NecClient client, InventoryItem invItem, uint state)
        {
            IBuffer res = BufferProvider.Provide();
            res = BufferProvider.Provide();
            res.WriteUInt64((ulong) invItem
                .Id); //client.Character.EquipId[x]  put stuff unidentified and get the status equipped  , 0 put stuff identified
            res.WriteUInt32(state);
            Router.Send(client, (ushort) AreaPacketId.recv_item_update_state, res, ServerType.Area);
        }

        public void ConfigureItem(NecClient client, InventoryItem inventoryItem)
        {
            //   IBuffer res = BufferProvider.Provide();
            //   res = BufferProvider.Provide();
            //   res.WriteUInt64((ulong)inventoryItem.Id);
            //   res.WriteByte(item.level);
            //   Router.Send(client, (ushort) AreaPacketId.recv_item_update_level, res, ServerType.Area);
            //   
            //   res = BufferProvider.Provide();
            //   res.WriteUInt64((ulong)inventoryItem.Id);
            //   res.WriteInt32(item.weight);
            //   Router.Send(client, (ushort) AreaPacketId.recv_item_update_weight, res, ServerType.Area);
            //   
            //   res = BufferProvider.Provide();
            //   res.WriteUInt64((ulong)inventoryItem.Id);
            //   res.WriteInt16(inventoryItem.Item.Physical); // Defense and attack points
            //   Router.Send(client, (ushort) AreaPacketId.recv_item_update_physics, res, ServerType.Area);

            //   res = BufferProvider.Provide();
            //   res.WriteUInt64((ulong)inventoryItem.Id);
            //   res.WriteInt32(item.enchatId);
            //   Router.Send(client, (ushort) AreaPacketId.recv_item_update_enchantid, res, ServerType.Area);

            //   res = BufferProvider.Provide();
            //   res.WriteUInt64((ulong)inventoryItem.Id);
            //   res.WriteByte(item.hardness);
            //   Router.Send(client, (ushort) AreaPacketId.recv_item_update_hardness, res, ServerType.Area);
            //   
            //   res = BufferProvider.Provide();
            //   res.WriteUInt64((ulong)inventoryItem.Id);
            //   res.WriteInt32(item.maxDurability);
            //   Router.Send(client, (ushort) AreaPacketId.recv_item_update_maxdur, res, ServerType.Area);
            //   
            //   res = BufferProvider.Provide();
            //   res.WriteUInt64((ulong)inventoryItem.Id);
            //   res.WriteInt32(item.durability);
            //   Router.Send(client, (ushort) AreaPacketId.recv_item_update_durability, res, ServerType.Area);

            //   res = BufferProvider.Provide();
            //   res.WriteUInt64((ulong)inventoryItem.Id);
            //   res.WriteInt16(item.magic);
            //   Router.Send(client, (ushort) AreaPacketId.recv_item_update_magic, res, ServerType.Area);

            //   res = BufferProvider.Provide();
            //   res.WriteUInt64((ulong)inventoryItem.Id);
            //   res.WriteInt16(item.ac);
            //   Router.Send(client, (ushort) AreaPacketId.recv_item_update_ac, res, ServerType.Area);

            //   res = BufferProvider.Provide();
            //   res.WriteUInt64((ulong)inventoryItem.Id);
            //   res.WriteInt16((short) 10000); // Shwo GP on certain items
            //   Router.Send(client, (ushort)AreaPacketId.recv_item_update_ac, res, ServerType.Area);

            //   res = BufferProvider.Provide();
            //   res.WriteUInt64((ulong)inventoryItem.Id);
            //   res.WriteByte(0);
            //   Router.Send(client, (ushort)AreaPacketId.recv_item_update_sp_level, res, ServerType.Area);
        }
    }
}
