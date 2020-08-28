using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;

namespace Necromancy.Server.Packet.Msg
{
    public class send_chara_get_list : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_chara_get_list));    
        public send_chara_get_list(NecServer server) : base(server)  { }
        public override ushort Id => (ushort) MsgPacketId.send_chara_get_list;
        public override void Handle(NecClient client, NecPacket packet)
        {
            List<Character> characters = Database.SelectCharactersBySoulId(client.Soul.Id);
            if (characters == null || characters.Count <= 0)
            {
                Logger.Debug(client, "No characters found");
                return;
            }
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(characters.Count); //expected character count per soul
            Router.Send(client, (ushort) MsgPacketId.recv_chara_get_list_r, res, ServerType.Msg);
            SendNotifyData(client);
            SendNotifyDataComplete(client);
        }

        private void SendNotifyDataComplete(NecClient client)
        {
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteByte(0xFF);
            res2.WriteUInt32(0xFFFFFFFF); //prisonment settings??  .. Slot of character in prison and something else.
            res2.WriteInt32(0xFFF);
            res2.WriteUInt32(0b11111111); //Soul Premium Flag
            Router.Send(client, (ushort) MsgPacketId.recv_chara_notify_data_complete, res2, ServerType.Msg);
        }

        private void SendNotifyData(NecClient client)
        {
            List<Character> characters = Database.SelectCharactersBySoulId(client.Soul.Id);
            if (characters == null || characters.Count <= 0)
            {
                Logger.Debug(client, "No characters found");
                return;
            }

            foreach (Character character in characters)
            {
                //populate soul and character inventory from database.
                List<InventoryItem> inventoryItems = Server.Database.SelectInventoryItemsByCharacterId(character.Id); //to-do. leverage SQL query to grab items where state > 0
                foreach (InventoryItem inventoryItem in inventoryItems)
                {
                    Item item = Server.Items[inventoryItem.ItemId];
                    inventoryItem.Item = item;
                    if (inventoryItem.State > 0  & inventoryItem.State < 262145)
                    {
                        character.Inventory.Equip(inventoryItem);
                        inventoryItem.CurrentEquipmentSlotType = inventoryItem.Item.EquipmentSlotType;
                        inventoryItem.Item.LoadEquipType = (ItemEquipDisplayType)Enum.Parse(typeof(ItemEquipDisplayType), inventoryItem.Item.ItemType.ToString());
                    }
                }
                if (character.Inventory._equippedItems.Count > 20)
                {
                    Logger.Error($"Character {character.Name} has too many equipment entries"); 
                    continue;  // skip if more than 19 equipped items.  corrupt DB entries in itemSpawn
                }

                IBuffer res = BufferProvider.Provide();

                res.WriteByte(character.Slot); //character slot, 0 for left, 1 for middle, 2 for right
                res.WriteInt32(character.Id); //  Character ID
                res.WriteFixedString(character.Name, 91); // 0x5B | 91x 1 byte

                res.WriteInt32(0); // 0 = Alive | 1 = Dead
                res.WriteInt32(character.Level); //character level stat
                res.WriteInt32(1); //TODO (unknown)
                res.WriteUInt32(character.ClassId); //class stat 

                //Traits
                res.WriteUInt32(character.Raceid); 
                res.WriteUInt32(character.Sexid);
                res.WriteByte(character.HairId); 
                res.WriteByte(character.HairColorId); 
                res.WriteByte(character.FaceId); 

                LoadEquip.SlotSetup(res, character, 19);
                LoadEquip.EquipItems(res, character, 19);
                LoadEquip.EquipSlotBitMask(res, character, 19);
                LoadEquip.SlotUpgradeLevel(res, character, 19);

                res.WriteByte(19);  //Number of equipment to display
                res.WriteInt32(character.MapId); //map location ID
                Router.Send(client, (ushort) MsgPacketId.recv_chara_notify_data, res, ServerType.Msg);
            }
        }
    }
}
