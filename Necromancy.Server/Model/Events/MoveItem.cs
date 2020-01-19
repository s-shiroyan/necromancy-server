using Arrowgene.Services.Buffers;
using Arrowgene.Services.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;
namespace Necromancy.Server.Model
{
    public class MoveItem : Event
    {
        private readonly NecLogger _logger;
        public byte toStoreType; // [0 = adventure bag. 1 = character equipment], [then unknown byte], [then slot], [then unknown]
        public byte toBagId { get; set; }
        public short toSlot { get; set; }
        public short fromSlot { get; set; }
        public byte fromStoreType { get; set; }
        public byte fromBagId { get; set; }
        public byte itemCount { get; set; }
        public Item item { get; set; }
        public MoveItem()
        {
            EventType = (ushort)AreaPacketId.recv_event_request_int;
            _logger = LogProvider.Logger<NecLogger>(this);
        }

        public void Move(NecServer server, NecClient client)
        {
            InventoryItem invItem = client.Character.GetInventoryItem(fromStoreType, fromBagId, fromSlot);
            if (invItem.StorageCount == itemCount)
            {
                IBuffer res = BufferProvider.Provide();
                client.Character.UpdateInventoryItem(invItem);
                _logger.Debug($"invItem.StorageItem.InstanceId [{invItem.StorageItem.InstanceId}]");
                res = null;
                res = BufferProvider.Provide();
                res.WriteInt64(invItem.StorageItem.InstanceId); // item id
                res.WriteByte(toStoreType); // 0 = adventure bag. 1 = character equipment, 2 = royal bag ??
                res.WriteByte(toBagId); // Position 2 ??
                res.WriteInt16(toSlot); // bag index 0 to 24
                server.Router.Send(client, (ushort)AreaPacketId.recv_item_update_place, res, ServerType.Area);
                invItem.StorageType = toStoreType;
                invItem.StorageId = toBagId;
                invItem.StorageSlot = toSlot;
                client.Character.UpdateInventoryItem(invItem);
            }
            else
            {
                byte remainCount = (byte)(invItem.StorageCount - itemCount); // huh??? byte - byte is cast as int????
                RecvItemUpdateNum updateNum = new RecvItemUpdateNum(invItem.InstanceId, remainCount);
                server.Router.Send(updateNum, client);
                InventoryItem newInvItem = client.Character.GetNextInventoryItem(server);
                newInvItem.StorageCount = (byte)itemCount;
                newInvItem.StorageType = toStoreType;
                newInvItem.StorageId = toBagId;
                newInvItem.StorageSlot = toSlot;
                newInvItem.StorageItem = item;
                RecvItemInstanceUnidentified itemUnidentified = new RecvItemInstanceUnidentified(newInvItem);
                server.Router.Send(itemUnidentified, client);
                invItem.StorageCount = remainCount;
                client.Character.UpdateInventoryItem(invItem);
            }
        }
    }
}
