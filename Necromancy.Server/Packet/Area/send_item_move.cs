using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_item_move : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_item_move));

        public send_item_move(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_item_move;


        public override void Handle(NecClient client, NecPacket packet)
        {
            // [0 = adventure bag. 1 = character equipment], [then unknown byte], [then slot], [then unknown]
            byte toStoreType = packet.Data.ReadByte();
            byte toBagId = packet.Data.ReadByte();
            short fromSlot = packet.Data.ReadInt16();
            byte fromStoreType = packet.Data.ReadByte();
            byte fromBagId = packet.Data.ReadByte();
            short toSlot = packet.Data.ReadInt16();
            int itemCount = packet.Data.ReadByte(); //last byte is stack count?
            
            Logger.Debug($"fromStoreType byte [{fromStoreType}]");
            Logger.Debug($"fromBagId byte [{fromBagId}]");
            Logger.Debug($"fromSlot byte [{fromSlot}]");
            Logger.Debug($"toStoreType byte [{toStoreType}]");
            Logger.Debug($"toBagId byte [{toBagId}]");
            Logger.Debug($"toSlot [{toSlot}]");
            Logger.Debug($"itemCount [{itemCount}]");
            
            IBuffer res = BufferProvider.Provide();
            InventoryItem inventoryItem = client.Inventory.GetInventoryItem(fromBagId, fromSlot);
            InventoryItem inventoryItemTo = client.Inventory.GetInventoryItem(toBagId, toSlot);
            if (inventoryItem == null)
            {
                res.WriteInt32((int)ItemActionResultType.ErrorGeneric); 
                Router.Send(client, (ushort) AreaPacketId.recv_item_move_r, res, ServerType.Area);
                return;
            }
            if (inventoryItemTo == null)
            {
                ItemActionResultType actionResult = client.Inventory.MoveInventoryItem(inventoryItem, toBagId, toSlot);
                if (actionResult != ItemActionResultType.Ok)
                {
                    res.WriteInt32((int)actionResult);
                    Router.Send(client, (ushort)AreaPacketId.recv_item_move_r, res, ServerType.Area);
                    return;
                }

                res.WriteInt32((int)actionResult);
                Router.Send(client, (ushort)AreaPacketId.recv_item_move_r, res, ServerType.Area);

                SendItemPlace(client, inventoryItem.Id, toStoreType, toBagId, toSlot);
            }
            else
            {
                ItemActionResultType actionResult = client.Inventory.SwapInventoryItem(inventoryItem, toBagId, toSlot, inventoryItemTo, fromBagId, fromSlot);
                if (actionResult != ItemActionResultType.Ok)
                {
                    res.WriteInt32((int)actionResult);
                    Router.Send(client, (ushort)AreaPacketId.recv_item_move_r, res, ServerType.Area);
                    return;
                }

                res.WriteInt32((int)actionResult);
                Router.Send(client, (ushort)AreaPacketId.recv_item_move_r, res, ServerType.Area);
                SendItemPlaceChange(client, inventoryItem.Id, toStoreType, toBagId, toSlot, inventoryItemTo.Id, fromStoreType, fromBagId, fromSlot);
            }
        }

        private void SendItemPlace(NecClient client,long itemId, byte toStoreType, byte toBagId, short toSlot)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(itemId); // item id
            res.WriteByte(toStoreType); // 0 = adventure bag. 1 = character equipment, 2 = royal bag
            res.WriteByte(toBagId); // position 2	cause crash if you change the 0	]	} im assumming these are x/y row, and page
            res.WriteInt16(toSlot); // bag index 0 to 24
            Router.Send(client, (ushort) AreaPacketId.recv_item_update_place, res, ServerType.Area);
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
