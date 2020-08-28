using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;
using System.Collections.Generic;

namespace Necromancy.Server.Packet.Area
{
    public class send_data_get_self_chara_data_request : ClientHandler
    {
        private static readonly NecLogger Logger =
            LogProvider.Logger<NecLogger>(typeof(send_data_get_self_chara_data_request));

        public send_data_get_self_chara_data_request(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_data_get_self_chara_data_request;

        public override void Handle(NecClient client, NecPacket packet)
        {
            LoadInventory(client);

            SendDataGetSelfCharaData(client);

            IBuffer res2 = BufferProvider.Provide();
            Router.Send(client, (ushort) AreaPacketId.recv_data_get_self_chara_data_request_r, res2, ServerType.Area);
        }

        private void SendDataGetSelfCharaData(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();

            //sub_4953B0 - characteristics
            //Consolidated Frequently Used Code
            LoadEquip.BasicTraits(res, client.Character);

            //sub_48C1F0
            /*for (int k = 0; k < 0xA/2; k++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        res.WriteInt64(2); // InstanceId
                    }
                }
            }

            for (int k = 0; k < 0x14/2; k++) //Equipment related.  20 slots.
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        res.WriteInt64(2); // InstanceId
                    }
                }
            }

            for (int k = 0; k < 0x1E/2; k++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        res.WriteInt64(2); // InstanceId
                    }
                }
            }

            for (int k = 0; k < 0x28/2; k++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        res.WriteInt64(2); // InstanceId
                    }
                }
            }

            for (int k = 0; k < 0x32/2; k++) //this one breaks that cmp 14 loop.   
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        res.WriteInt64(0); // InstanceId
                    }
                }
            }*/

            for (int i = 0; i < 100; i++)
                res.WriteInt64(0);


            //sub_484720 - combat/leveling info
            Logger.Debug($"Character ID Loading : {client.Character.Id}");
            res.WriteUInt32(client.Character.InstanceId); // InstanceId
            res.WriteUInt32(client.Character.ClassId); // class
            res.WriteInt16(client.Character.Level); // current level 
            res.WriteInt64(11); // current exp

                res.WriteInt64(12); // soul exp

                res.WriteInt64(13); // exp needed to level

                res.WriteInt64(14); // soul exp needed to level

                res.WriteInt32(client.Character.Hp.current); // current hp
                res.WriteInt32(client.Character.Mp.current); // current mp
                res.WriteInt32(client.Character.Od.current); // current od
                res.WriteInt32(client.Character.Hp.max); // max hp
                res.WriteInt32(client.Character.Mp.max); // maxmp

                res.WriteInt32(client.Character.Od.max); // max od
                res.WriteInt32(500); // current guard points
                res.WriteInt32(600); // max guard points
                res.WriteInt32(10); // value/100 = current weight
                res.WriteInt32(12); // value/100 = max weight

                res.WriteByte(200); // condition

                // total stat level includes bonus'?   // 7 loop int 16 broken out
                res.WriteUInt16(client.Character.Strength); // str
                res.WriteUInt16(client.Character.vitality); // vit
                res.WriteInt16((short) (client.Character.dexterity + 3)); // dex
                res.WriteUInt16(client.Character.agility); // agi
                res.WriteUInt16(client.Character.intelligence); // int
                res.WriteUInt16(client.Character.piety); // pie
                res.WriteInt16((short) (client.Character.luck + 4)); // luk

                // mag atk atrb  /9 loop int 16 broken out
                res.WriteInt16(5); // fire
                res.WriteInt16(52); // water
                res.WriteInt16(58); // wind
                res.WriteInt16(45); // earth
                res.WriteInt16(33); // light
                res.WriteInt16(12); // dark
                res.WriteInt16(0);
                res.WriteInt16(0);
                res.WriteInt16(0);

                // mag def atrb /9 loop int 16 broken out
                res.WriteInt16(5); // fire
                res.WriteInt16(52); // water
                res.WriteInt16(58); // wind
                res.WriteInt16(45); // earth
                res.WriteInt16(33); // light
                res.WriteInt16(12); // dark
                res.WriteInt16(0);
                res.WriteInt16(0);
                res.WriteInt16(0);

                //status change resistance //0xBoy loop int 16 broken out
                res.WriteInt16(11); // Poison
                res.WriteInt16(12); // Paralyze
                res.WriteInt16(13); // Stone
                res.WriteInt16(14); // Faint
                res.WriteInt16(15); // Blind
                res.WriteInt16(16); // Sleep
                res.WriteInt16(17); // Silent
                res.WriteInt16(18); // Charm
                res.WriteInt16(18); // new1
                res.WriteInt16(18); // new2
                res.WriteInt16(18); // new3



                // gold and alignment?
                res.WriteInt64(client.Character.AdventureBagGold); // gold

                res.WriteUInt32(client.Character.Alignmentid); // AlignmentId

                res.WriteInt32(6000); // lawful
                res.WriteInt32(5000); // neutral
                res.WriteInt32(6100); // chaos

                res.WriteInt32(0); // title from honor.csv

            //sub_484980
            res.WriteInt32(10000); // ac eval calculation?
            res.WriteInt32(20000); // ac eval calculation?
            res.WriteInt32(30000); // ac eval calculation?

            // characters stats /7 int 16 broken up
            res.WriteUInt16(client.Character.Strength); // str
            res.WriteUInt16(client.Character.vitality); // vit
            res.WriteInt16((short) (client.Character.dexterity)); // dex
            res.WriteUInt16(client.Character.agility); // agi
            res.WriteUInt16(client.Character.intelligence); // int
            res.WriteUInt16(client.Character.piety); // pie
            res.WriteInt16((short) (client.Character.luck)); // luk

            // nothing //9 int 16 broken up
            res.WriteInt16(1);
            res.WriteInt16(2);
            res.WriteInt16(3);
            res.WriteInt16(4);
            res.WriteInt16(5);
            res.WriteInt16(6);
            res.WriteInt16(7);
            res.WriteInt16(8);
            res.WriteInt16(9);


            // nothing //9 int 16 broken up
            res.WriteInt16(1);
            res.WriteInt16(2);
            res.WriteInt16(3);
            res.WriteInt16(4);
            res.WriteInt16(5);
            res.WriteInt16(6);
            res.WriteInt16(7);
            res.WriteInt16(8);
            res.WriteInt16(9);

            // nothing //0xB int 16 broken up
            res.WriteInt16(1);
            res.WriteInt16(2);
            res.WriteInt16(3);
            res.WriteInt16(4);
            res.WriteInt16(5);
            res.WriteInt16(6);
            res.WriteInt16(7);
            res.WriteInt16(8);
            res.WriteInt16(9);
            res.WriteInt16(10);
            res.WriteInt16(11);


            //sub_484B00 map ip and connection
            res.WriteInt32(client.Character.MapId); //MapSerialID
            res.WriteInt32(client.Character.MapId); //MapID
            res.WriteFixedString(Settings.DataAreaIpAddress, 65); //IP
            res.WriteUInt16(Settings.AreaPort); //Port

            //sub_484420 // Map Spawn coord
            res.WriteFloat(client.Character.X); //X Pos
            res.WriteFloat(client.Character.Y); //Y Pos
            res.WriteFloat(client.Character.Z); //Z Pos
            res.WriteByte(client.Character.Heading); //view offset

            //sub_read_int32 skill point
            res.WriteInt32(101); // skill point

            //sub_483420 character state like alive/dead/invis
            res.WriteInt32((int)client.Character.State); //-254 GM

            //sub_494AC0
            res.WriteByte(client.Soul.Level); // soul level
            res.WriteInt32(22); // current soul points
            res.WriteInt32(790); // soul point bar value (percenage of current/max)
            res.WriteInt32(120); // max soul points
                res.WriteByte(client.Character.criminalState); // 0 is white,1 yellow 2 red 3+ skull
                res.WriteByte((byte)client.Character.beginnerProtection); //Beginner protection (bool)
                res.WriteByte(50); //Level cap
                res.WriteByte(1);
                res.WriteByte(2);
                res.WriteByte(3);

                res.WriteByte(3); //unk 4  this is new in sunset


            //sub_read_3-int16 unknown
            res.WriteInt16(50); // HP Consumption Rate?
            res.WriteInt16(50); // MP Consumption Rate?
            res.WriteInt16(5); // OD Consumption Rate (if greater than currentOD, Can not sprint)

            //sub_4833D0
            res.WriteInt64(1);

            //sub_4833D0
            res.WriteInt64(1);

            //sub_4834A0
            res.WriteFixedString($"{client.Soul.Name} Shop", 97); //Shopname

            //sub_4834A0
            res.WriteFixedString($"{client.Soul.Name} Comment", 385); //Comment

            //sub_494890
            res.WriteByte(1); //Bool for showing/hiding character comment.

            //sub_4834A0
            res.WriteFixedString($"{client.Soul.Name} chatbox?", 385); //Chatbox?

            //sub_494890
            res.WriteByte(1); //Bool

            res.WriteInt32(2); //this is new in sunset

            res.WriteByte(1); //this is new in sunset
            //--Good so far-------------------------

            int numEntries = 0x14;
            res.WriteInt32(numEntries); //has to be below or equal to 0x14

            for (int i = 0; i < numEntries; i++)
                res.WriteInt32(0);

            res.WriteInt32(numEntries); //less than or equal to 0x14

            for(int i = 0; i < numEntries; i++)
            { 
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0); //bool
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
            }

            res.WriteInt32(numEntries); //less than or equal to 0x14

            for (int i = 0; i < numEntries; i++)
                res.WriteInt32(0);

            res.WriteInt32(0x80); //less than or equal to 0x80

            for (int i = 0; i < 0x80; i++)
            {
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
            }

            //The stuff below here should replace the 4 int32 above, but im missing something

            /*
            //Consolidated Frequently Used Code
            LoadEquip.SlotSetup(res, client.Character, numEntries);


            //sub_483420
            res.WriteInt32(numEntries); //has to be less than 19

            //Consolidated Frequently Used Code
            LoadEquip.EquipItems(res, client.Character, numEntries);

            //sub_483420
            res.WriteInt32(numEntries);

            LoadEquip.EquipSlotBitMask(res, client.Character, numEntries);

            //sub_483420
            numEntries = 1;
            res.WriteInt32(numEntries); //has to be less than 128

            //sub_485A70
            for (int k = 0; k < numEntries; k++) //status buffs / debuffs
            {
                res.WriteInt32(2); //status
                res.WriteInt32(9999998); //time start?
                res.WriteInt32(9999999); //time end?
            }
            */
            Router.Send(client, (ushort) AreaPacketId.recv_data_get_self_chara_data_r, res, ServerType.Area);
        }


        public void LoadInventory(NecClient client)
        {
            //populate soul and character inventory from database.
            List<InventoryItem> inventoryItems = Server.Database.SelectInventoryItemsByCharacterIdEquipped(client.Character.Id);
            foreach (InventoryItem inventoryItem in inventoryItems)
            {
                Item item = Server.Items[inventoryItem.ItemId];
                inventoryItem.Item = item;
                if (inventoryItem.State > 0 & inventoryItem.State < 262145) //this is redundant. could be removed for  better performance. 
                {
                    client.Character.Inventory.Equip(inventoryItem);
                    inventoryItem.CurrentEquipmentSlotType = inventoryItem.Item.EquipmentSlotType;
                }

            }

        }

    }
}
