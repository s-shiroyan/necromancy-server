using System.Collections.Generic;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Chat.Command.Commands
{
    //opens an empty Treasure box window
    public class SendEventTreasureboxBegin : ServerChatCommand
    {
        public SendEventTreasureboxBegin(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(0); // 1 = cinematic
            res2.WriteByte(0);

            Router.Send(client, (ushort) AreaPacketId.recv_event_start, res2, ServerType.Area);

            //recv_event_tresurebox_begin = 0xBD7E,
            IBuffer res1 = BufferProvider.Provide();
            int numEntries = 0x10;
            res1.WriteInt32(numEntries);
            for (int i = 0; i < numEntries; i++)
            {
                res1.WriteInt32(10001 + i);
            }

            Router.Send(client, (ushort) AreaPacketId.recv_event_treasurebox_begin, res1, ServerType.Area);


            IBuffer res4 = BufferProvider.Provide();
            res4.WriteInt32(0); // 1 = Error reported by SV,  1 = sucess
            Router.Send(client, (ushort) AreaPacketId.recv_event_treasurebox_select_r, res4, ServerType.Area);
            
            int itemId = 200101;
            ItemGenerator(itemId, client, 1);
            ItemGenerator(itemId, client, 2);
            ItemGenerator(itemId, client, 3);
            /*   IBuffer res4 = BufferProvider.Provide();
               res4.WriteByte(3);
               Router.Send(client, (ushort)AreaPacketId.recv_event_end, res4); */
        }
        public long ItemGenerator(int itemId, NecClient client, int i)
        {
            Item item = Server.Items[itemId];
            InventoryItem inventoryItem = new InventoryItem();
            inventoryItem.Id = 50000 + i;
            inventoryItem.Item = item;
            inventoryItem.ItemId = item.Id;
            inventoryItem.Quantity = 1;
            inventoryItem.CurrentDurability = item.Durability;
            inventoryItem.CharacterId = client.Character.Id;
            inventoryItem.CurrentEquipmentSlotType = EquipmentSlotType.NONE;
            inventoryItem.State = 0;
            inventoryItem.StorageType = (int)BagType.TreasureBox;
            client.Character.Inventory.AddTreasureBoxItem(inventoryItem);

            RecvItemInstance recvItemInstance = new RecvItemInstance(inventoryItem, client);
            Router.Send(recvItemInstance, client);
            RecvItemInstanceUnidentified recvItemInstanceUnidentified = new RecvItemInstanceUnidentified(inventoryItem, client);
            Router.Send(recvItemInstanceUnidentified, client);
            return inventoryItem.Id;
        }


        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "tbox";
    }
}
