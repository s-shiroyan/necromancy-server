using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;
using System.Collections.Generic;

namespace Necromancy.Server.Packet.Area
{
    public class send_data_get_self_chara_data_request : ClientHandler
    {
        private static readonly NecLogger Logger =
            LogProvider.Logger<NecLogger>(typeof(send_data_get_self_chara_data_request));
        public send_data_get_self_chara_data_request(NecServer server) : base(server) { }
        public override ushort Id => (ushort) AreaPacketId.send_data_get_self_chara_data_request;
        public override void Handle(NecClient client, NecPacket packet)
        {
            LoadItems(client);
            SendDataGetSelfCharaData(client);
        }

        private void SendDataGetSelfCharaData(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();

            //sub_4953B0 - characteristics
            //Traits
            res.WriteUInt32(client.Character.RaceId); //race
            res.WriteUInt32(client.Character.SexId);
            res.WriteByte(client.Character.HairId); //hair
            res.WriteByte(client.Character.HairColorId); //color
            res.WriteByte(client.Character.FaceId); //face

            //sub_484720 - combat/leveling info
            Logger.Debug($"Character ID Loading : {client.Character.Id}");
            res.WriteUInt32(client.Character.InstanceId); // InstanceId
            res.WriteUInt32(client.Character.ClassId); // class
            res.WriteInt16(client.Character.Level); // current level 
            res.WriteInt64(91978348); // current exp
            res.WriteInt64(50000000); // soul exp
            res.WriteInt64(96978348); // exp needed to level
            res.WriteInt64(1100); // soul exp needed to level
            res.WriteInt32(client.Character.Hp.current); // current hp
            res.WriteInt32(client.Character.Mp.current); // current mp
            res.WriteInt32(client.Character.Od.current); // current od
            res.WriteInt32(client.Character.Hp.max); // max hp
            res.WriteInt32(client.Character.Mp.max); // maxmp
            res.WriteInt32(client.Character.Od.max); // max od
            res.WriteInt32(500); // current guard points
            res.WriteInt32(600); // max guard points
            res.WriteInt32(1238); // value/100 = current weight
            res.WriteInt32(1895); // value/100 = max weight
            res.WriteByte(200); // condition

            // total stat level includes bonus'?
            res.WriteUInt16(client.Character.Strength); // str
            res.WriteUInt16(client.Character.Vitality); // vit
            res.WriteInt16((short) (client.Character.Dexterity + 3)); // dex
            res.WriteUInt16(client.Character.Agility); // agi
            res.WriteUInt16(client.Character.Intelligence); // int
            res.WriteUInt16(client.Character.Piety); // pie
            res.WriteInt16((short) (client.Character.Luck + 4)); // luk

            // mag atk atrb
            res.WriteInt16(5); // fire
            res.WriteInt16(52); // water
            res.WriteInt16(58); // wind
            res.WriteInt16(45); // earth
            res.WriteInt16(33); // light
            res.WriteInt16(12); // dark
            res.WriteInt16(0);
            res.WriteInt16(0);
            res.WriteInt16(0);

            // mag def atrb
            res.WriteInt16(5); // fire
            res.WriteInt16(52); // water
            res.WriteInt16(58); // wind
            res.WriteInt16(45); // earth
            res.WriteInt16(33); // light
            res.WriteInt16(12); // dark
            res.WriteInt16(0);
            res.WriteInt16(0);
            res.WriteInt16(0);

            //status change resistance
            res.WriteInt16(11); // Poison
            res.WriteInt16(12); // Paralyze
            res.WriteInt16(13); // Stone
            res.WriteInt16(14); // Faint
            res.WriteInt16(15); // Blind
            res.WriteInt16(16); // Sleep
            res.WriteInt16(17); // Silent
            res.WriteInt16(18); // Charm
            res.WriteInt16(19); // confus
            res.WriteInt16(20); // fear
            res.WriteInt16(21); //possibly EXP Boost Gauge. trying to find it

            // gold and alignment?
            res.WriteInt64(client.Character.AdventureBagGold); // gold
            res.WriteUInt32(client.Character.AlignmentId); // AlignmentId
            res.WriteInt32(6000); // lawful
            res.WriteInt32(5000); // neutral
            res.WriteInt32(6100); // chaos
            res.WriteInt32(Util.GetRandomNumber(90400101, 90400130)); // title from honor.csv

            //sub_484980
            res.WriteInt32(10000); // ac eval calculation?
            res.WriteInt32(20000); // ac eval calculation?
            res.WriteInt32(30000); // ac eval calculation?

            // characters stats
            res.WriteUInt16(client.Character.Strength); // str
            res.WriteUInt16(client.Character.Vitality); // vit
            res.WriteInt16((short) (client.Character.Dexterity)); // dex
            res.WriteUInt16(client.Character.Agility); // agi
            res.WriteUInt16(client.Character.Intelligence); // int
            res.WriteUInt16(client.Character.Piety); // pie
            res.WriteInt16((short) (client.Character.Luck)); // luk

            // nothing
            res.WriteInt16(1);
            res.WriteInt16(2);
            res.WriteInt16(3);
            res.WriteInt16(4);
            res.WriteInt16(5);
            res.WriteInt16(6);
            res.WriteInt16(7);
            res.WriteInt16(8);
            res.WriteInt16(9);


            // nothing
            res.WriteInt16(1);
            res.WriteInt16(2);
            res.WriteInt16(3);
            res.WriteInt16(4);
            res.WriteInt16(5);
            res.WriteInt16(6);
            res.WriteInt16(7);
            res.WriteInt16(8);
            res.WriteInt16(9);

            // nothing
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
            res.WriteByte((byte)client.Character.BeginnerProtection); //Beginner protection (bool)
            res.WriteByte(50); //Level cap
            res.WriteByte(1);
            res.WriteByte(2);
            res.WriteByte(3);

            //sub_read_3-int16 unknown
            res.WriteInt16(50); // HP Consumption Rate?
            res.WriteInt16(50); // MP Consumption Rate?
            res.WriteInt16(5); // OD Consumption Rate (if greater than currentOD, Can not sprint)

            //sub_4833D0
            res.WriteInt64(1234);

            //sub_4833D0
            res.WriteInt64(5678);

            //sub_4834A0
            res.WriteFixedString($"{client.Soul.Name} Shop", 97); //Shopname

            //sub_4834A0
            res.WriteFixedString($"{client.Soul.Name} Comment", 385); //Comment

            //sub_494890
            res.WriteByte(0); //Bool for showing/hiding character comment.

            //sub_4834A0
            res.WriteFixedString($"{client.Soul.Name} chatbox?", 385); //Chatbox?

            //sub_494890
            res.WriteByte(1); //Bool

            //sub_483420
            int numOfEquippedItems = 0;
            res.WriteInt32(numOfEquippedItems); //has to be less than 19(max equipment slots)

            for (int i = 0; i < numOfEquippedItems; i++)
            {
                res.WriteInt32(0); //TODO figure out what the fuck this is for Item display type? something with spagetti load equipment
            }

            //sub_483420
            res.WriteInt32(numOfEquippedItems); //has to be less than 19
            
            for (int i = 0; i < numOfEquippedItems; i++)
            { //todo textures
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

            //sub_483420
            res.WriteInt32(numOfEquippedItems);

            for (int i = 0; i < numOfEquippedItems; i++)
            {                
                res.WriteInt32(0); //bitmask per equipment slot
            }

            //sub_483420
            int numEntries = 1;
            res.WriteInt32(numEntries); //has to be less than 128

            //sub_485A70
            for (int k = 0; k < numEntries; k++) //status buffs / debuffs
            {
                res.WriteInt32(2); //status
                res.WriteInt32(9999998); //time start?
                res.WriteInt32(9999999); //time end?
            }

            Router.Send(client, (ushort) AreaPacketId.recv_data_get_self_chara_data_r, res, ServerType.Area);
        }       

        private void LoadItems(NecClient client)
        {
            ItemService itemService = new ItemService(client.Character);
            List<SpawnedItem> ownedItems = itemService.GetOwnedItems();
            foreach (SpawnedItem ownedItem in ownedItems)
            {
                PacketResponse pResp;
                if (ownedItem.IsIdentified) pResp = new RecvItemInstance(client, ownedItem);
                else pResp = new RecvItemInstanceUnidentified(client, ownedItem);
                Router.Send(pResp);
            }
        }

    }
}
