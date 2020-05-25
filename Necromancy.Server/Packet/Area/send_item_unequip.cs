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
            /*
            ERR_UNEQUIP 1
            ERR_UNEQUIP - 203
            ERR_UNEQUIP - 201
            ERR_UNEQUIP - 208
            ERR_UNEQUIP GENERIC
            */
            int slotNum = packet.Data.ReadInt32();
            EquipmentSlotType type = Item.GetEquipmentSlotTypeBySlotNumber(slotNum);
            InventoryItem inventoryItem = client.Inventory.GetEquippedInventoryItem(type);

            if (inventoryItem != null)
            {
                if (inventoryItem.CurrentEquipmentSlotType == EquipmentSlotType.NONE)
                {
                    inventoryItem.CurrentEquipmentSlotType = inventoryItem.Item.EquipmentSlotType;
                }
                else
                {
                    inventoryItem.CurrentEquipmentSlotType = EquipmentSlotType.NONE;
                }

                IBuffer res = BufferProvider.Provide();
                res.WriteInt32(0); //error check. 0 to work
                RecvItemUpdateEqMask eqMask = new RecvItemUpdateEqMask(inventoryItem);
                Router.Send(eqMask, client);
                Router.Send(client, (ushort) AreaPacketId.recv_item_unequip_r, res, ServerType.Area);
            }
            else
            {
                IBuffer res = BufferProvider.Provide();
                res.WriteInt32(203); //error check. 0 to work
                Router.Send(client, (ushort) AreaPacketId.recv_item_unequip_r, res, ServerType.Area);
            }
        }
    }
}
