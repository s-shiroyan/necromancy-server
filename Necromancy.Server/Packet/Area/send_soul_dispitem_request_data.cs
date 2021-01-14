using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using System.Collections.Generic;

namespace Necromancy.Server.Packet.Area
{
    public class send_soul_dispitem_request_data : ClientHandler
    {
        public send_soul_dispitem_request_data(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_soul_dispitem_request_data;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            Router.Send(client, (ushort)AreaPacketId.recv_soul_dispitem_request_data_r, res, ServerType.Area);

            //notify you of the soul item you got based on something above.
            IBuffer res19 = BufferProvider.Provide();
            res19.WriteInt32(Util.GetRandomNumber(62000001, 62000015)); //soul_dispitem.csv
            Router.Send(client, (ushort)AreaPacketId.recv_soul_dispitem_notify_data, res19, ServerType.Area);

            LoadInventory(client);
            LoadCloakRoom(client);
            //LoadBattleStats(client);
            LoadHonor(client);
        }

        public void LoadHonor(NecClient client)
        {
            RecvGetHonor recvGetHonor = new RecvGetHonor(10010101 /*novice monster hunter*/,client.Character.InstanceId, 1 /*alreadyKnown*/);
            Router.Send(recvGetHonor, client);
        }
        public void LoadBattleStats(NecClient client)
        {
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

            IBuffer res = BufferProvider.Provide();
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

        public void LoadInventory(NecClient client)
        {
            //populate soul and character inventory from database.
            List<InventoryItem> inventoryItems = Server.Database.SelectInventoryItemsByCharacterId(client.Character.Id);
            foreach (InventoryItem inventoryItem in inventoryItems)
            {
                if (inventoryItem.StorageType == 3) continue; //cloak room stuff gets handled below.  possibility to refine SQL query to skip, but this was easier.
                Item itemLookup = Server.Items[inventoryItem.ItemId];

                Item item = new Item();
                item.Durability = itemLookup.Durability;
                item.EquipmentSlotType = itemLookup.EquipmentSlotType ;
                item.Id = itemLookup.Id;
                item.ItemType = itemLookup.ItemType;
                item.LoadEquipType = itemLookup.LoadEquipType;
                item.Magical = itemLookup.Magical;
                item.Name = itemLookup.Name;
                item.Physical = itemLookup.Physical;

                inventoryItem.Item = item;

                RecvItemInstance recvItemInstance = new RecvItemInstance(inventoryItem, client);
                Router.Send(recvItemInstance, client);

                RecvItemInstanceUnidentified recvItemInstanceUnidentified = new RecvItemInstanceUnidentified(inventoryItem,client); //do some testing without the state bit set to inentifried
                Router.Send(recvItemInstanceUnidentified, client); //Commented out for testing identify NPC

                itemStats(inventoryItem, client);


                client.Character.Inventory.LoginLoadInventory(inventoryItem);

                if (inventoryItem.State > 0) 
                {
                    client.Character.Inventory.Equip(inventoryItem);
                    inventoryItem.CurrentEquipmentSlotType = inventoryItem.Item.EquipmentSlotType;
                }
            }

        }
        public void LoadCloakRoom(NecClient client)
        {
            //populate soul and character inventory from database.
            List<InventoryItem> inventoryItems = Server.Database.SelectInventoryItemsBySoulIdCloakRoom(client.Character.SoulId);
            foreach (InventoryItem inventoryItem in inventoryItems)
            {
                Item itemLookup = Server.Items[inventoryItem.ItemId];

                Item item = new Item();
                item.Durability = itemLookup.Durability;
                item.EquipmentSlotType = itemLookup.EquipmentSlotType;
                item.Id = itemLookup.Id;
                item.ItemType = itemLookup.ItemType;
                item.LoadEquipType = itemLookup.LoadEquipType;
                item.Magical = itemLookup.Magical;
                item.Name = itemLookup.Name;
                item.Physical = itemLookup.Physical;
                if (inventoryItem.Item == null) { continue; }
                RecvItemInstance recvItemInstance = new RecvItemInstance(inventoryItem, client);
                Router.Send(recvItemInstance, client);
                RecvItemInstanceUnidentified recvItemInstanceUnidentified = new RecvItemInstanceUnidentified(inventoryItem,client);
                Router.Send(recvItemInstanceUnidentified, client);

                itemStats(inventoryItem, client);
                client.Character.Inventory.LoginLoadInventory(inventoryItem);
            }

        }

        public void itemStats(InventoryItem inventoryItem, NecClient client)
        {
            Server.SettingRepository.ItemLibrary.TryGetValue(inventoryItem.Item.Id, out ItemLibrarySetting itemLibrarySetting);
            if (itemLibrarySetting == null) return;

            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteInt32(itemLibrarySetting.Durability); // MaxDura points
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_maxdur, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteInt32(itemLibrarySetting.Durability - 10); // Durability points
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_durability, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteInt32((int)itemLibrarySetting.Weight*100); // Weight points
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_weight, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteInt16((short)itemLibrarySetting.PhysicalAttack); // Defense and attack points
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_physics, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteInt16((short)itemLibrarySetting.MagicalAttack); // Magic def and attack Points
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_magic, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteInt32(itemLibrarySetting.SpecialPerformance); // for the moment i don't know what it change
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_enchantid, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteInt16((short)Util.GetRandomNumber(50, 100)); // Shwo GP on certain items
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_ac, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteInt32(1); // for the moment i don't know what it change
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_date_end_protect, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteByte((byte)itemLibrarySetting.Hardness); // Hardness
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_hardness, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteByte(1); //Level requirement
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_level, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteByte(0); //sp Level requirement
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_sp_level, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteInt32(0b000000); // State bitmask
                                      //Router.Send(client, (ushort)AreaPacketId.recv_item_update_state, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id); // id?
            res.WriteInt64(Util.GetRandomNumber(10, 10000)); // price
            res.WriteInt64(Util.GetRandomNumber(10, 100)); // identify
            res.WriteInt64(Util.GetRandomNumber(10, 1000)); // curse?
            res.WriteInt64(Util.GetRandomNumber(10, 500)); // repair?
            Router.Send(client, (ushort)AreaPacketId.recv_shop_notify_item_sell_price, res, ServerType.Area);

        }

    }
}
