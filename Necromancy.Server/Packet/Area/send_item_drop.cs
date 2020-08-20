using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_item_drop : ClientHandler
    {
        public send_item_drop(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_item_drop;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte storageType = packet.Data.ReadByte();
            byte bagId = packet.Data.ReadByte();
            short bagSlotIndex = packet.Data.ReadInt16();
            byte stackSize = packet.Data.ReadByte();


            InventoryItem inventoryItem = client.Character.Inventory.GetInventoryItem(storageType, bagId, bagSlotIndex);
            if (inventoryItem == null)
            {
                return;
            }

            if (!client.Character.Inventory.RemoveInventoryItem(inventoryItem))
            {
                return;
            }

            if (!Server.Database.DeleteInventoryItem(inventoryItem.Id))
            {
                return;
            }

            IBuffer res = BufferProvider.Provide();
            res.SetPositionStart();
            res.WriteUInt64((ulong) inventoryItem.Id);
            Router.Send(client, (ushort) AreaPacketId.recv_item_remove, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_item_drop_r, res, ServerType.Area);
        }
    }
}
