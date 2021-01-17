using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

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
            ItemEquipSlots equipSlot = (ItemEquipSlots) packet.Data.ReadInt32(); 
            
            ItemService itemService = new ItemService(client.Character);
            ItemInstance unequippedItem;
            int error = 0;

            try
            {
                unequippedItem = itemService.Unequip(equipSlot);
                RecvItemUpdateEqMask recvItemUpdateEqMask = new RecvItemUpdateEqMask(client, unequippedItem);
                Router.Send(recvItemUpdateEqMask);
            }
            catch (ItemException e)
            {
                error = (int)e.ExceptionType;
                //TODO logger
            }

            RecvItemUnequip recvItemUnequip = new RecvItemUnequip(client, error);
            Router.Send(recvItemUnequip);

            //short PhysAttack = 0;
            //short MagAttack = 0;
            //short PhysDef = 0;
            //short MagDef = 0;

            //foreach (InventoryItem inventoryItem2 in client.Character.Inventory._equippedItems.Values)
            //{
            //    if ((int)inventoryItem2.CurrentEquipmentSlotType < 3)
            //    {
            //        PhysAttack += (short)inventoryItem2.Item.Physical;
            //        MagAttack += (short)inventoryItem2.Item.Magical;
            //    }
            //    else
            //    {
            //        PhysDef += (short)inventoryItem2.Item.Physical;
            //        MagDef += (short)inventoryItem2.Item.Magical;
            //    }
            //}

            //res = BufferProvider.Provide();
            //res.WriteInt16((short)client.Character.Strength); //base Phys Attack
            //res.WriteInt16((short)client.Character.intelligence); //base Mag attack
            //res.WriteInt16((short)client.Character.dexterity); //base Phys Def
            //res.WriteInt16((short)client.Character.piety); //base Mag Def

            //res.WriteInt16(PhysAttack); //Equip Bonus Phys attack
            //res.WriteInt16(MagAttack); //Equip Bonus Mag Attack
            //res.WriteInt16(PhysDef); //Equip bonus Phys Def
            //res.WriteInt16(MagDef); //Equip bonus Mag Def
            //Router.Send(client, (ushort)AreaPacketId.recv_chara_update_battle_base_param, res, ServerType.Area);
        }
    }
}
