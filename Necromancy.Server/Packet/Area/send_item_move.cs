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
            if (inventoryItem == null)
            {
                res.WriteInt32((int)ItemActionResultType.ErrorGeneric); 
                Router.Send(client, (ushort) AreaPacketId.recv_item_move_r, res, ServerType.Area);
                return;
            }

            ItemActionResultType actionResult = client.Inventory.MoveInventoryItem(inventoryItem, toBagId, toSlot);
            if (actionResult != ItemActionResultType.Ok)
            {
                res.WriteInt32((int)actionResult); 
                Router.Send(client, (ushort) AreaPacketId.recv_item_move_r, res, ServerType.Area);
                return;
            }

            
            
            res.WriteInt32((int)actionResult); 
            Router.Send(client, (ushort) AreaPacketId.recv_item_move_r, res, ServerType.Area);


            //     InventoryItem fromInvItem = client.Character.GetInventoryItem(fromStoreType, fromBagId, fromSlot);
            //   InventoryItem toInvItem = client.Character.GetInventoryItem(toStoreType, toBagId, toSlot);
            //   if (toInvItem != null && (fromInvItem.StorageItem != toInvItem.StorageItem))
            //   {
            //       RecvNormalSystemMessage unlikeItems = new RecvNormalSystemMessage("You can only stack like items!");
            //       _server.Router.Send(unlikeItems, client);
            //       return;
            //   }

            //   if (toInvItem != null && (toInvItem.StorageCount >= 255))
            //   {
            //       RecvNormalSystemMessage unlikeItems =
            //           new RecvNormalSystemMessage("The move would place too many items in destination slot!");
            //       _server.Router.Send(unlikeItems, client);
            //       return;
            //   }

            //   if (fromInvItem.StorageCount > 1)
            //   {
            //       if (client.Character.currentEvent != null)
            //       {
            //           Logger.Error(
            //               $"Trying to start new event with another outstanding event active! Outstanding event type [{client.Character.currentEvent.EventType}]");
            //           client.Character.currentEvent = null;
            //       }

            //       MoveItem moveItem = _server.Instances.CreateInstance<MoveItem>();
            //       moveItem.toStoreType = toStoreType;
            //       moveItem.toBagId = toBagId;
            //       moveItem.toSlot = toSlot;
            //       moveItem.fromStoreType = fromStoreType;
            //       moveItem.fromBagId = moveItem.fromBagId;
            //       moveItem.fromSlot = fromSlot;
            //       moveItem.itemCount = (byte) itemCount;
            //       moveItem.item = fromInvItem.StorageItem;
            //       client.Character.currentEvent = moveItem;
            //       RecvEventStart eventStart = new RecvEventStart(0, 0);
            //       Router.Send(eventStart, client);
            //       RecvEventRequestInt getCount = new RecvEventRequestInt("Select number to move.", 1,
            //           fromInvItem.StorageCount, fromInvItem.StorageCount);
            //       Router.Send(getCount, client);
            //   }
            //   else
            //   {
            //       if (toInvItem == null)
            //       {
            //           fromInvItem.StorageType = toStoreType;
            //           fromInvItem.StorageId = toBagId;
            //           fromInvItem.StorageSlot = toSlot;
            //           client.Character.UpdateInventoryItem(fromInvItem);
            //           RecvItemUpdatePlace changePlace =
            //               new RecvItemUpdatePlace(fromInvItem.InstanceId, toStoreType, toBagId, toSlot);
            //           _server.Router.Send(changePlace, client);
            //           client.Character.UpdateInventoryItem(fromInvItem);
            //       }
            //       else
            //       {
            //           toInvItem.StorageCount += 1;
            //           RecvItemUpdateNum updateNum = new RecvItemUpdateNum(toInvItem.InstanceId, toInvItem.StorageCount);
            //           _server.Router.Send(updateNum, client);
            //           RecvItemRemove removeitem = new RecvItemRemove(fromInvItem.InstanceId);
            //           _server.Router.Send(removeitem, client);
            //           client.Character.UpdateInventoryItem(toInvItem);
            //           client.Character.RemoveInventoryItem(fromInvItem);
            //       }
            //   }

            //SendItemPlace(client);
            //SendItemPlaceChange(client);
        }

        private void SendItemPlace(NecClient client, byte toStoreType, byte toBagId, ushort toSlot)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(10200101); // item id
            res.WriteByte(toStoreType); // 0 = adventure bag. 1 = character equipment, 2 = royal bag
            res.WriteByte(
                toBagId); // position 2	cause crash if you change the 0	]	} im assumming these are x/y row, and page
            res.WriteUInt16(toSlot); // bag index 0 to 24
            Router.Send(client, (ushort) AreaPacketId.recv_item_update_place, res, ServerType.Area);
        }

        private void SendItemPlaceChange(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            //res.WriteInt64(invItem.StorageItem.InstanceId); // item id
            //res.WriteByte(fromStoreType);// 0 = adventure bag. 1 = character equipment, 2 = royal bag ??
            //res.WriteByte(fromBagId); // Position 2 ??
            //res.WriteInt16(fromSlot); // bag index 0 to 24
            //res.WriteInt64(invItem.StorageItem.InstanceId); // item id
            //res.WriteByte(toStoreType); // 0 = adventure bag. 1 = character equipment, 2 = royal bag ??
            //res.WriteByte(toBagId); // Position 2 ??
            //res.WriteInt16(toSlot); // bag index 0 to 24
            Router.Send(client, (ushort) AreaPacketId.recv_item_update_place_change, res, ServerType.Area);
        }
    }
}
