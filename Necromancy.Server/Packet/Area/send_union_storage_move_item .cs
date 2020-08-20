using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_union_storage_move_item : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_union_storage_move_item));

        public send_union_storage_move_item(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_union_storage_move_item;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_union_storage_move_item_r, res, ServerType.Area);

            byte fromStoreType = packet.Data.ReadByte();
            byte fromBagId = packet.Data.ReadByte();
            short fromSlot = packet.Data.ReadInt16();
            byte toStoreType = packet.Data.ReadByte();
            byte toBagId = packet.Data.ReadByte();
            short toSlot = packet.Data.ReadInt16();
            byte itemCount = packet.Data.ReadByte();

            Logger.Debug($"fromStoreType byte [{fromStoreType}]");
            Logger.Debug($"fromBagId byte [{fromBagId}]");
            Logger.Debug($"fromSlot byte [{fromSlot}]");
            Logger.Debug($"toStoreType byte [{toStoreType}]");
            Logger.Debug($"toBagId byte [{toBagId}]");
            Logger.Debug($"toSlot [{toSlot}]");
            Logger.Debug($"itemCount [{itemCount}]");

            res = BufferProvider.Provide();
            InventoryItem inventoryItem = client.Character.Inventory.GetInventoryItem(fromStoreType, fromBagId, fromSlot);
            InventoryItem inventoryItem2 = client.Character.Inventory.GetInventoryItem(toStoreType, toBagId, toSlot);
            if (inventoryItem == null)
            {
                res.WriteInt32((int)ItemActionResultType.ErrorGeneric);
                Router.Send(client, (ushort)AreaPacketId.recv_item_move_r, res, ServerType.Area);
                return;
            }
            if (inventoryItem2 == null)
            {
                ItemActionResultType actionResult = client.Character.Inventory.MoveInventoryItem(inventoryItem, toStoreType, toBagId, toSlot);
                if (actionResult != ItemActionResultType.Ok)
                {
                    res.WriteInt32((int)actionResult);
                    Router.Send(client, (ushort)AreaPacketId.recv_item_move_r, res, ServerType.Area);
                    return;
                }

                res.WriteInt32((int)actionResult);
                Router.Send(client, (ushort)AreaPacketId.recv_item_move_r, res, ServerType.Area);

                SendItemPlace(client, inventoryItem.Id, toStoreType, toBagId, toSlot);
                if (!Server.Database.UpdateInventoryItem(inventoryItem))
                {
                    Logger.Error("Could not update InventoryItem in Database");
                    return;
                }
            }
            else
            {
                ItemActionResultType actionResult = client.Character.Inventory.SwapInventoryItem(inventoryItem, toStoreType, toBagId, toSlot, inventoryItem2, fromStoreType, fromBagId, fromSlot);
                if (actionResult != ItemActionResultType.Ok)
                {
                    res.WriteInt32((int)actionResult);
                    Router.Send(client, (ushort)AreaPacketId.recv_item_move_r, res, ServerType.Area);
                    return;
                }

                res.WriteInt32((int)actionResult);
                Router.Send(client, (ushort)AreaPacketId.recv_item_move_r, res, ServerType.Area);
                SendItemPlaceChange(client, inventoryItem.Id, toStoreType, toBagId, toSlot, inventoryItem2.Id, fromStoreType, fromBagId, fromSlot);
                if (!Server.Database.UpdateInventoryItem(inventoryItem))
                {
                    Logger.Error("Could not update InventoryItem in Database");
                    return;
                }
                if (!Server.Database.UpdateInventoryItem(inventoryItem2))
                {
                    Logger.Error("Could not update InventoryItem number 2 in Database");
                    return;
                }
            }
        }

        private void SendItemPlace(NecClient client, long itemId, byte toStoreType, byte toBagId, short toSlot)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(itemId); // item id
            res.WriteByte(toStoreType); // 0 = adventure bag. 1 = character equipment, 2 = royal bag
            res.WriteByte(toBagId); // position 2	cause crash if you change the 0	]	} im assumming these are x/y row, and page
            res.WriteInt16(toSlot); // bag index 0 to 24
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_place, res, ServerType.Area);
        }
        private void SendItemPlaceChange(NecClient client, long toItemId, byte toStoreType, byte toBagId, short toSlot, long fromItemId, byte fromStoreType, byte fromBagId, short fromSlot)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(fromItemId); // item id
            res.WriteByte(fromStoreType);// 0 = adventure bag. 1 = character equipment, 2 = royal bag ??
            res.WriteByte(fromBagId); // Position 2 ??
            res.WriteInt16(fromSlot); // bag index 0 to 24
            res.WriteInt64(toItemId); // item id
            res.WriteByte(toStoreType); // 0 = adventure bag. 1 = character equipment, 2 = royal bag ??
            res.WriteByte(toBagId); // Position 2 ??
            res.WriteInt16(toSlot); // bag index 0 to 24
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_place_change, res, ServerType.Area);
        }

    }
}