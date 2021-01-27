using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Chat;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_event_treasurebox_select : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_item_move));
        public send_event_treasurebox_select(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_event_treasurebox_select;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte fromStoreType = packet.Data.ReadByte();
            byte fromBagId = packet.Data.ReadByte();
            short fromSlot = packet.Data.ReadInt16();
            Logger.Debug($"fromStoreType byte [{fromStoreType}]");
            Logger.Debug($"fromBagId byte [{fromBagId}]");
            Logger.Debug($"fromSlot byte [{fromSlot}]");

            IBuffer res = BufferProvider.Provide();

            InventoryItem inventoryItem = client.Character.Inventory.GetInventoryItem(fromStoreType, fromBagId, fromSlot);
            client.Character.Inventory.RemoveInventoryItem(inventoryItem);
            if (inventoryItem == null)
            {
                Logger.Error("could not retrieve item from inventory");
                return;
            }
            inventoryItem.StorageType = (int)BagType.AdventureBag;
            client.Character.Inventory.AddInventoryItem(inventoryItem);

            SendItemPlace(client, inventoryItem.Id, inventoryItem.StorageType, inventoryItem.BagId, inventoryItem.BagSlotIndex);
            if (!Server.Database.InsertInventoryItem(inventoryItem))
            {
                Logger.Error("Could not save InventoryItem to Database");
                return;
            }

            res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_event_treasurebox_select_r, res, ServerType.Area);


            res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort)AreaPacketId.recv_situation_start, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(itemId); // item id
            res.WriteByte(toStoreType); // 0 = adventure bag. 1 = character equipment, 2 = royal bag
            res.WriteByte(toBagId); // position 2	cause crash if you change the 0	]	} im assumming these are x/y row, and page
            res.WriteInt16(toSlot); // bag index 0 to 24
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_place, res, ServerType.Area);

            res = BufferProvider.Provide();
            Router.Send(client, (ushort)AreaPacketId.recv_situation_end, res, ServerType.Area);

        }
    }
}
