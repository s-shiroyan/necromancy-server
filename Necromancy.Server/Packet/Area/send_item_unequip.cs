using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;

namespace Necromancy.Server.Packet.Area
{
    public class send_item_unequip : ClientHandler
    {
        public send_item_unequip(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_item_unequip;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int slotNum = packet.Data.ReadInt32();
            EquipmentSlotType type = Item.GetEquipmentSlotTypeBySlotNumber(slotNum);
            InventoryItem inventoryItem = client.Inventory.GetEquippedInventoryItem(type);
            InventoryItem equippedItem = client.Inventory.CheckAlreadyEquipped(inventoryItem.Item.EquipmentSlotType);

            IBuffer res = BufferProvider.Provide();
            if (inventoryItem == null)
            {
                res.WriteInt32((int) ItemActionResultType.ErrorGeneric);
                Router.Send(client, (ushort) AreaPacketId.recv_item_unequip_r, res, ServerType.Area);
                return;
            }

            if (inventoryItem.CurrentEquipmentSlotType == EquipmentSlotType.NONE)
            {
                res.WriteInt32((int) ItemActionResultType.ErrorGeneric);
                Router.Send(client, (ushort) AreaPacketId.recv_item_unequip_r, res, ServerType.Area);
                return;
            }

            inventoryItem.CurrentEquipmentSlotType = EquipmentSlotType.NONE;
            RecvItemUpdateEqMask eqMask = new RecvItemUpdateEqMask(inventoryItem);
            Router.Send(eqMask, client);
            if (equippedItem != null)
            {
                client.Inventory.UnEquip(equippedItem);
            }

            res.WriteInt32((int) ItemActionResultType.Ok);
            Router.Send(client, (ushort) AreaPacketId.recv_item_unequip_r, res, ServerType.Area);
        }
    }
}
