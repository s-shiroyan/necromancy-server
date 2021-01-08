using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

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

            //notify other players of your new look
            RecvDataNotifyCharaData myCharacterData = new RecvDataNotifyCharaData(client.Character, client.Soul.Name);
            Router.Send(client.Map, myCharacterData, client);

            res.WriteInt32((int) ItemActionResultType.Ok);
            Router.Send(client, (ushort) AreaPacketId.recv_item_equip_r, res, ServerType.Area);

            short PhysAttack = 0;
            short MagAttack = 0;
            short PhysDef = 0;
            short MagDef = 0;

            foreach (InventoryItem inventoryItem2 in client.Character.Inventory._equippedItems.Values)
            {
                if ((int)inventoryItem2.CurrentEquipmentSlotType < 3)
                {
                    PhysAttack += (short)inventoryItem2.Item.Physical;
                    MagAttack += (short)inventoryItem2.Item.Magical;
                }
                else
                {
                    PhysDef += (short)inventoryItem2.Item.Physical;
                    MagDef += (short)inventoryItem2.Item.Magical;
                }
            }

            res = BufferProvider.Provide();
            res.WriteInt16((short)client.Character.Strength); //base Phys Attack
            res.WriteInt16((short)client.Character.intelligence); //base Mag attack
            res.WriteInt16((short)client.Character.dexterity); //base Phys Def
            res.WriteInt16((short)client.Character.piety); //base Mag Def

            res.WriteInt16(PhysAttack); //Equip Bonus Phys attack
            res.WriteInt16(MagAttack); //Equip Bonus Mag Attack
            res.WriteInt16(PhysDef); //Equip bonus Phys Def
            res.WriteInt16(MagDef); //Equip bonus Mag Def
            //Router.Send(client, (ushort)AreaPacketId.recv_chara_update_battle_base_param, res, ServerType.Area);
        }
    }
}
