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
    public class send_item_unequip : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_item_unequip));
        public send_item_unequip(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_item_unequip;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int slotNum = packet.Data.ReadInt32();
            EquipmentSlotType type = Item.GetEquipmentSlotTypeBySlotNumber(slotNum);
            InventoryItem inventoryItem = client.Character.Inventory.GetEquippedInventoryItem(type);
            InventoryItem equippedItem = client.Character.Inventory.CheckAlreadyEquipped(inventoryItem.Item.EquipmentSlotType);

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
            inventoryItem.State = 0; //Crude unequipped flag. to be further developed.
            if (!Server.Database.UpdateInventoryItem(inventoryItem))
            {
                Logger.Error("Could not update InventoryItem in Database");
                return;
            }
            RecvItemUpdateEqMask eqMask = new RecvItemUpdateEqMask(inventoryItem);
            Router.Send(eqMask, client);
            if (equippedItem != null)
            {
                client.Character.Inventory.UnEquip(equippedItem);
            }

            res.WriteInt32((int) ItemActionResultType.Ok);
            Router.Send(client, (ushort) AreaPacketId.recv_item_unequip_r, res, ServerType.Area);

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
            Router.Send(client, (ushort)AreaPacketId.recv_chara_update_battle_base_param, res, ServerType.Area);
        }
    }
}
