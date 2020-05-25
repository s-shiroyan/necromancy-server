using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;

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
                responses.Add(ChatResponse.CommandError(client, $"To few arguments"));
                return;
            }

            if (!int.TryParse(command[0], out int itemId))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[0]}"));
                return;
            }

            Item item = Server.Database.SelectItemById(itemId);
            if (item == null)
            {
                // Require Item to be in Database because we have a constraint.
                // Create Item
                // TODO use this code to initialize `nec_item` table

                if (!Server.SettingRepository.ItemNecromancy.TryGetValue(itemId, out ItemNecromancySetting necItem))
                {
                    responses.Add(ChatResponse.CommandError(client,
                        $"ItemId: {itemId} - not found in `SettingRepository.ItemNecromancy`"));
                    return;
                }

                if (!Server.SettingRepository.ItemInfo.TryGetValue(itemId, out ItemInfoSetting itemInfo))
                {
                    responses.Add(ChatResponse.CommandError(client,
                        $"ItemId: {itemId} - not found in `SettingRepository.ItemInfo`"));
                    return;
                }

                item = new Item();
                item.Id = itemInfo.Id;
                item.Name = itemInfo.Name;
                item.Durability = necItem.Durability;
                item.Physical = necItem.Physical;
                item.Magical = necItem.Magical;
                item.ItemType = Item.ItemTypeByItemId(itemInfo.Id);
                item.EquipmentSlotType = Item.EquipmentSlotTypeByItemType(item.ItemType);

                if (!Server.Database.InsertItem(item))
                {
                    responses.Add(ChatResponse.CommandError(client, "Could not save Item to Database"));
                    return;
                }
            }

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
            for (int i = 60; i < 80; i++)
            {
                inventoryItem.Item.ItemType = (ItemType) i;

                client.Inventory.AddInventoryItem(inventoryItem);
                if (!Server.Database.InsertInventoryItem(inventoryItem))
                {
                    responses.Add(ChatResponse.CommandError(client, "Could not save InventoryItem to Database"));
                    return;
                }

                RecvItemInstanceUnidentified(client, inventoryItem);
            }
        }

        private void RecvItemInstanceUnidentified(NecClient client, InventoryItem inventoryItem)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64((ulong) inventoryItem.Id);
            res.WriteCString(inventoryItem.Item.Name);
            res.WriteInt32((int) inventoryItem.Item.ItemType); // TODO Investigate, for weapons it is -1 sometimes, why
            res.WriteInt32((int) inventoryItem.CurrentEquipmentSlotType);
            res.WriteByte(inventoryItem.Quantity);
            res.WriteInt32(0); //Item status 0 = identified  
            res.WriteInt32(inventoryItem.Item.Id); //Item icon 50100301 = camp
            res.WriteByte(5);
            res.WriteByte(4);
            res.WriteByte(7);
            res.WriteInt32(8);
            res.WriteByte(1);
            res.WriteByte(2);
            res.WriteByte(9);
            res.WriteByte(4);
            res.WriteByte(5);
            res.WriteByte(0); // bool
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0); // 0 = adventure bag. 1 = character equipment
            res.WriteByte(0); // 0~2
            res.WriteInt16(inventoryItem.BagSlotIndex);
            res.WriteInt32(0); //bit mask. This indicates where to put items.   e.g. 01 head 010 arm 0100 feet etc (0 for not equipped)
            res.WriteInt64(69);
            res.WriteInt32(59);
            Router.Send(client, (ushort) AreaPacketId.recv_item_instance_unidentified, res, ServerType.Area);
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
