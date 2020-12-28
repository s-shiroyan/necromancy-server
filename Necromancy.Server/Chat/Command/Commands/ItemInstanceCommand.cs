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

            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64(10001);      //SPAWN ID
            res.WriteInt32(10001);         //BASE ID
            res.WriteByte(1);         //QUANTITY
            res.WriteUInt32(1); //STATUSES
            res.WriteFixedString("HELMET", 0x10); //UNKNOWN - ITEM TYPE?
            res.WriteByte(1);      // STORAGE ZONE
            res.WriteByte(0);            //BAG
            res.WriteInt16(0);    //SLOT
            res.WriteInt32(1);           //bit mask. This indicates where to put items. PREV EQUIP SLOT
            res.WriteInt32(99); //spirit eq mask??? PREV DURABILITY
            res.WriteInt32(0);  //new
            res.WriteByte(0);   //ENHANCEMENT LEVEL?
            res.WriteByte(0); //SPECIAL FORGE LEVEL?
            res.WriteCString("MAYBE LORE"); // unknown
            res.WriteInt16(0); //PHYSICAL
            res.WriteInt16(0); //MAGICAL
            res.WriteInt32(100); //MAX DURABILITY
            res.WriteByte(5); //HARDNESS
            res.WriteInt32(0); //UNKNOWN

            const int MAX_WHATEVER_SLOTS = 2;
            int numEntries = 2;
            res.WriteInt32(numEntries);                  //less than or equal to 2?
            for (int j = 0; j < numEntries; j++)
            {
                res.WriteInt32((byte)0);                 //unknown
            }

            int numOfGemSlots = 1;
            res.WriteInt32(numOfGemSlots); //number of gem slots
            for (int j = 0; j < numOfGemSlots; j++)
            {
                res.WriteByte(0); //IS FILLED
                res.WriteInt32(0); //GEM TYPE
                res.WriteInt32(0); // GEM BASE ID
                res.WriteInt32(0);                       //maybe gem item 2 id for diamon 2 gem combine 
            }

            res.WriteInt32(0);
            //res.WriteInt32((0b1 >> _inventoryItem.BagSlotIndex));
            res.WriteInt64(0);//new
            res.WriteInt16(0xff); //0 = green (in shop for sale)  0xFF = normal /*item.ShopStatus*/
            res.WriteInt32(1); //ENCHANT ID
            res.WriteInt16(1); //GP

            numEntries = 5; //new
            res.WriteInt32(numEntries); // less than or equal to 5
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(0);//new
                res.WriteByte(0);//new
                res.WriteByte(0);//new
                res.WriteInt16(0);//new
                res.WriteInt16(0);//new
            }

            res.WriteInt64(0);//new
            res.WriteInt16(0);//new
            res.WriteInt16(0);//new
            res.WriteInt16(0);//new
            res.WriteInt16(0);//new
            res.WriteInt16(0);//new
            res.WriteInt16(0);//new
            res.WriteInt16(0);//new
            res.WriteInt32(0);//new
            res.WriteInt32(0);//new
            res.WriteInt32(0);//new
            res.WriteInt32(0);//new
            res.WriteInt32(0);//new
            res.WriteInt32(0);//new
            res.WriteInt32(0);//new
            res.WriteInt32(0);//new
            res.WriteInt32(0);//new
            res.WriteInt32(0);//new

            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(0);//new
                res.WriteByte(0);//new
                res.WriteByte(0);//new
                res.WriteInt16(0);//new
                res.WriteInt16(0);//new
            }

            res.WriteInt16(-2);//new
            res.WriteInt16(-1);//new
            res.WriteByte(1);//new
            res.WriteByte(2);//new

            res.WriteInt64(-1);//new
            res.WriteInt16(0);//new
            res.WriteInt32(0);//new
            res.WriteInt32(0);//new
            res.WriteInt16(0);//new
            res.WriteInt16(0);//new

            res.WriteInt32(0);//new
            res.WriteInt32(0);//new
            res.WriteInt16(0);//new
            res.WriteInt16(0);//new

            numEntries = 5;
            for (int i = 0; i < 5; i++)
            {
                res.WriteInt16(0);//new
                res.WriteInt32(0);//new
                res.WriteInt32(0);//new
                res.WriteInt16(0);//new
                res.WriteInt16(0);//new
            }

            res.WriteInt16(69);//new

            Router.Send(client, (ushort)AreaPacketId.recv_item_instance, res, ServerType.Area);

            //if (command.Length < 1)
            //{
            //    responses.Add(ChatResponse.CommandError(client, "To few arguments"));
            //    return;
            //}

            //if (!int.TryParse(command[0], out int itemId))
            //{
            //    responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[0]}"));
            //    return;
            //}

            //if (!Server.Items.ContainsKey(itemId))
            //{
            //    responses.Add(ChatResponse.CommandError(client, $"ItemId: '{itemId}' does not exist"));
            //    return;
            //}

            //int[] DonkeyItems = new int[] {100101,100102,100103,100104,100105,100106,100107,100108,100109,100110,100111,100112,100113,100114,100115,100116,100181,100191,100192,100193,100201,100202,100203,100204,100205,100206,100207,100208,100209,100210,100211,100212,100213,100214,100301,100302,100303,100304,100305,100306,100307,100308,100401,100402,100403,100404,100405,100406,100407,100408};
            //int numItems = DonkeyItems.Count();

            //for (int i = 0; i < numItems*5; i++)
            //{
            //    Item item = Server.Items[100101];
            //    // Create InventoryItem
            //    InventoryItem inventoryItem = new InventoryItem();
            //    inventoryItem.Id = item.Id + i;
            //    inventoryItem.ItemId = item.Id;
            //    inventoryItem.Quantity = 1;
            //    inventoryItem.CurrentDurability = item.Durability;
            //    inventoryItem.CharacterId = client.Character.Id;
            //    inventoryItem.CurrentEquipmentSlotType = EquipmentSlotType.NONE;
            //    inventoryItem.State = 0;
            //    inventoryItem.StorageType = (int)BagType.AvatarInventory;
            //    client.Character.Inventory.AddAvatarItem(inventoryItem);
            //    inventoryItem.Item.ItemType = (ItemType)(i);
            //    inventoryItem.Item.Name = $"ItemType + {i}"; 

            //    RecvItemInstance recvItemInstance = new RecvItemInstance(inventoryItem, client);
            //Router.Send(recvItemInstance, client);
            //    RecvItemInstanceUnidentified recvItemInstanceUnidentified = new RecvItemInstanceUnidentified(inventoryItem, client);
            //    Router.Send(recvItemInstanceUnidentified, client);
            //    responses.Add(ChatResponse.CommandInfo(client, $"item {item.Id} in slot {inventoryItem.BagSlotIndex} in bag {inventoryItem.BagId}"));
            //}
        
        }
    }
}
