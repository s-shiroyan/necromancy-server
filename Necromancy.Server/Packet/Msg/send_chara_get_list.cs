using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;

namespace Necromancy.Server.Packet.Msg
{
    public class send_chara_get_list : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_chara_get_list));
        private readonly SettingRepository _settingRepository;

        public send_chara_get_list(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_chara_get_list;


        public override void Handle(NecClient client, NecPacket packet)
        {
            List<Character> characters = Database.SelectCharactersBySoulId(client.Soul.Id);
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
            res2.WriteByte(0); //bool Result
            res2.WriteUInt32(0); //Wanted_dead_CharaId - for when a char got sent to prison.  Slot of character in prison
            res2.WriteInt64(0b11111111); //Soul Premium Flags
            res2.WriteByte(5/*client.Soul.CrimeLevel*/); //Crime Level
            res2.WriteUInt32(1); //Soul State
            Router.Send(client, (ushort) MsgPacketId.recv_chara_notify_data_complete, res2, ServerType.Msg);
        }

        private void SendNotifyData(NecClient client)
        {
            List<Character> characters = Database.SelectCharactersBySoulId(client.Soul.Id);

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
                        inventoryItem.Item.LoadEquipType = (LoadEquipType)Enum.Parse(typeof(LoadEquipType), inventoryItem.Item.ItemType.ToString());
                    }
                }
                if (character.Inventory._equippedItems.Count > 25)
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
                res.WriteInt32(0); //todo (unknown)
                res.WriteUInt32(character.ClassId); //class stat 

                //Consolidated Frequently Used Code
                LoadEquip.BasicTraits(res, character);
                LoadEquip.SlotSetup(res, character, 0x19);
                LoadEquip.EquipItems(res, character, 0x19);
                LoadEquip.EquipSlotBitMask(res, character, 0x19);
                LoadEquip.SlotUpgradeLevel(res, character, 0x19);

                for (int i = 0; i < 0x19; i++)
                    res.WriteByte(1);


                res.WriteByte((byte)character.Inventory._equippedItems.Count);

                res.WriteInt32(character.MapId); //Map your character is on
                res.WriteInt32(0);//??? probably map area related

                res.WriteByte(0);
                res.WriteByte(0); //Character Name change in Progress.  (0 no : 1 Yes ).  Red indicator on top right

                res.WriteFixedString(character.Name, 91); // 0x5B | 91x 1 byte
                /*res.WriteByte(19);  //Number of equipment to display
                res.WriteInt32(character.MapId); //map location ID*/
                Router.Send(client, (ushort) MsgPacketId.recv_chara_notify_data, res, ServerType.Msg);
            }
        }
    }
}
