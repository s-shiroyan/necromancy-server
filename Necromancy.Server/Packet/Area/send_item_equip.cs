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
            byte bagId = packet.Data.ReadByte();
            short bagSlotIndex = packet.Data.ReadInt16();
            int equipBit = packet.Data.ReadInt32();
            Logger.Debug($"storageType:{storageType} bagId:{bagId} bagSlotIndex:{bagSlotIndex} equipBit:{equipBit}");

            IBuffer res = BufferProvider.Provide();
            InventoryItem inventoryItem = client.Character.Inventory.GetInventoryItem(bagId, bagSlotIndex);
            if (inventoryItem == null)
            {
                Logger.Error($"Item not found: bagId:{bagId} BagSlot:{bagSlotIndex}");
                res.WriteInt32((int) ItemActionResultType.ErrorGeneric);
                Router.Send(client, (ushort) AreaPacketId.recv_item_equip_r, res, ServerType.Area);
                return;
            }

            if (inventoryItem.CurrentEquipmentSlotType != EquipmentSlotType.NONE)
            {
                Logger.Error($"Already Equipped: {inventoryItem.Id}{inventoryItem.Item.Name}");
                res.WriteInt32((int) ItemActionResultType.ErrorUse);
                Router.Send(client, (ushort) AreaPacketId.recv_item_equip_r, res, ServerType.Area);
                return;
            }



            //two phased equipped item check for two H weapons
            if (inventoryItem.Item.EquipmentSlotType.HasFlag(EquipmentSlotType.HAND_L | EquipmentSlotType.HAND_R))
            {
                Logger.Debug($"two handed item detected {inventoryItem.Item.EquipmentSlotType}");
                InventoryItem equippedItem = client.Character.Inventory.CheckAlreadyEquipped(EquipmentSlotType.HAND_R);
                if (equippedItem != null && equippedItem.CurrentEquipmentSlotType != EquipmentSlotType.NONE)
                {
                    client.Character.Inventory.UnEquip(equippedItem);
                    equippedItem.CurrentEquipmentSlotType = EquipmentSlotType.NONE;
                    equippedItem.State = (int)EquipmentSlotType.NONE;
                    RecvItemUpdateEqMask recvItemUpdateEqMaskCurr = new RecvItemUpdateEqMask(equippedItem);
                    Router.Send(recvItemUpdateEqMaskCurr, client);
                }
                equippedItem = client.Character.Inventory.CheckAlreadyEquipped(EquipmentSlotType.HAND_L);
                if (equippedItem != null && equippedItem.CurrentEquipmentSlotType != EquipmentSlotType.NONE)
                {
                    client.Character.Inventory.UnEquip(equippedItem);
                    equippedItem.CurrentEquipmentSlotType = EquipmentSlotType.NONE;
                    equippedItem.State = (int)EquipmentSlotType.NONE;
                    RecvItemUpdateEqMask recvItemUpdateEqMaskCurr = new RecvItemUpdateEqMask(equippedItem);
                    Router.Send(recvItemUpdateEqMaskCurr, client);
                }
            }
            else  //everything besides 2h weapons
            {
                Logger.Debug($"equipment slot type {inventoryItem.Item.EquipmentSlotType}");
                //InventoryItem equippedItem = client.Character.Inventory.GetEquippedInventoryItem(inventoryItem.Item.EquipmentSlotType);

                InventoryItem equippedItem = client.Character.Inventory.CheckAlreadyEquipped(inventoryItem.Item.EquipmentSlotType);

                if (equippedItem != null && equippedItem.CurrentEquipmentSlotType != EquipmentSlotType.NONE)
                {
                    Logger.Debug($"equipment slot type already equipped item {equippedItem.Item.EquipmentSlotType}");
                    client.Character.Inventory.UnEquip(equippedItem);
                    equippedItem.CurrentEquipmentSlotType = EquipmentSlotType.NONE;
                    equippedItem.State = (int)EquipmentSlotType.NONE;
                    RecvItemUpdateEqMask recvItemUpdateEqMaskCurr = new RecvItemUpdateEqMask(equippedItem);
                    Router.Send(recvItemUpdateEqMaskCurr, client);
                }
            }
               

            //add the item to list of equipped items
            client.Character.Inventory.Equip(inventoryItem);
            
            inventoryItem.CurrentEquipmentSlotType = inventoryItem.Item.EquipmentSlotType;
            inventoryItem.State = (int)inventoryItem.Item.EquipmentSlotType; //Crude equipped flag. to be further developed.
            if (!Server.Database.UpdateInventoryItem(inventoryItem))
            {
                Logger.Error("Could not update InventoryItem in Database");
                return;
            }
            RecvItemUpdateEqMask recvItemUpdateEqMask = new RecvItemUpdateEqMask(inventoryItem);
            Router.Send(recvItemUpdateEqMask, client);

            res.WriteInt32((int) ItemActionResultType.Ok);
            Router.Send(client, (ushort) AreaPacketId.recv_item_equip_r, res, ServerType.Area);
        }
    }
}
