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
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteUInt32(0xFFFFFFFF);
            Router.Send(client, (ushort) MsgPacketId.recv_chara_get_list_r, res, ServerType.Msg);
            SendNotifyData(client);
            SendNotifyDataComplete(client);
        }

        private void SendNotifyDataComplete(NecClient client)
        {
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteByte(0xFF);
            res2.WriteUInt32(0xFFFFFFFF);
            res2.WriteInt32(0xFFF);
            res2.WriteUInt32(0xFFFFFFFF);
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
                List<InventoryItem> EquippedItems = new List<InventoryItem>();
                foreach (InventoryItem inventoryItem in inventoryItems)
                {
                    Item item = Server.Items[inventoryItem.ItemId];
                    inventoryItem.Item = item;
                    if (inventoryItem.State > 0)
                    {
                        inventoryItem.CurrentEquipmentSlotType = inventoryItem.Item.EquipmentSlotType;
                        inventoryItem.Item.LoadEquipType = (LoadEquipType)Enum.Parse(typeof(LoadEquipType), inventoryItem.Item.ItemType.ToString());
                        EquippedItems.Add(inventoryItem);
                    }
                }


                IBuffer res = BufferProvider.Provide();

                res.WriteByte(character.Slot); //character slot, 0 for left, 1 for middle, 2 for right
                res.WriteInt32(character.Id); //  Character ID
                res.WriteFixedString(character.Name, 91); // 0x5B | 91x 1 byte

                res.WriteInt32(0); // 0 = Alive | 1 = Dead
                res.WriteInt32(character.Level); //character level stat
                res.WriteInt32(2); //todo (unknown)
                res.WriteUInt32(character.ClassId); //class stat 

                if (EquippedItems.Count == 110)
                {
                    //Consolidated Frequently Used Code
                    LoadEquip.BasicTraits(res, character);
                    LoadEquip.SlotSetup(res, character, 19);
                    LoadEquip.EquipItems(res, character, 19);
                    LoadEquip.EquipSlotBitMask(res, character, 19);
                }
                else
                {
                    LoadEquip.BasicTraits(res, character);

                    //sub_483660 
                    foreach (InventoryItem inventoryItem in EquippedItems) 
                    {
                        res.WriteInt32((int)inventoryItem.Item.LoadEquipType);

                    }

                    //sub_4948C0
                    foreach (InventoryItem inventoryItem in EquippedItems)
                    {
                        res.WriteInt32(inventoryItem.Item.Id); //Sets your Item ID per Iteration
                        res.WriteByte(0); // 
                        res.WriteByte(0); // (theory bag)
                        res.WriteByte(0); // (theory Slot)

                        res.WriteInt32(inventoryItem.Item.Id); //testing (Theory, Icon related)
                        res.WriteByte(0); //
                        res.WriteByte(0); // (theory bag)
                        res.WriteByte(0); // (theory Slot)

                        res.WriteByte(0); // Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
                        res.WriteByte(00); //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
                        res.WriteByte(0); // testing (Theory Torso Tex)
                        res.WriteByte(0); // testing (Theory Pants Tex)
                        res.WriteByte(0); // testing (Theory Hands Tex)
                        res.WriteByte(0); // testing (Theory Feet Tex)
                        res.WriteByte(0); //Alternate texture for item model 

                        res.WriteByte(0); // separate in assembly
                    }

                    //sub_483420 
                    foreach (InventoryItem inventoryItem in EquippedItems)
                    {
                        res.WriteInt32((int)inventoryItem.Item.EquipmentSlotType); //bitmask per equipment slot
                    }
                }


                //19x 4 byte //item quality(+#) or aura? 10 = +7, 19 = +6,(maybe just wep aura)
                res.WriteInt32(10); //Right Hand    //1 for weapon
                res.WriteInt32(10); //Left Hand     //2 for Shield
                res.WriteInt32(10); //Torso         //16 for torso
                res.WriteInt32(10); //Head          //08 for head
                res.WriteInt32(10); //Legs          //32 for legs
                res.WriteInt32(10); //Arms          //64 for Arms
                res.WriteInt32(10); //Feet          //128 for feet
                res.WriteInt32(5); //???Cape
                res.WriteInt32(5); //???Ring
                res.WriteInt32(5); //???Earring
                res.WriteInt32(5); //???Necklace
                res.WriteInt32(5); //???Belt
                res.WriteInt32(0); //Avatar Torso
                res.WriteInt32(0); //Avatar Feet
                res.WriteInt32(0); //Avatar Arms
                res.WriteInt32(0); //Avatar Legs
                res.WriteInt32(0); //Avatar Head  
                res.WriteInt32(0); //???Talk Ring
                res.WriteInt32(00); //???Quiver    
                res.WriteByte(19); //Number of equipment to display
                res.WriteInt32(character.MapId); //map location ID
                Router.Send(client, (ushort) MsgPacketId.recv_chara_notify_data, res, ServerType.Msg);
            }
        }
    }
}
