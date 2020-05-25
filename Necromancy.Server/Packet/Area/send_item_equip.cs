using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;

namespace Necromancy.Server.Packet.Area
{
    public class send_item_equip : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_item_equip));

        public send_item_equip(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_item_equip;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte storageType = packet.Data.ReadByte();
            byte bagId = packet.Data.ReadByte(); //Equip slot maybe?
            short bagSlotIndex = packet.Data.ReadInt16(); //Slot from backpack the item is in
            int equipBit = packet.Data.ReadInt32();
            Logger.Debug(
                $"storageType: [{storageType}] bagId: [{bagId}]  backpackSlot: [{bagSlotIndex}] equipBit: [{equipBit}]");

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_item_equip_r, res, ServerType.Area);

            InventoryItem inventoryItem = client.Inventory.GetInventoryItem(bagId, bagSlotIndex);
            if (inventoryItem == null)
            {
                return;
            }

            if (inventoryItem.CurrentEquipmentSlotType != EquipmentSlotType.NONE)
            {
                // already equipped
                return;
            }

            inventoryItem.CurrentEquipmentSlotType = inventoryItem.Item.EquipmentSlotType;
            RecvItemUpdateEqMask recvItemUpdateEqMask = new RecvItemUpdateEqMask(inventoryItem);
            Router.Send(recvItemUpdateEqMask, client);
        }
    }
}
