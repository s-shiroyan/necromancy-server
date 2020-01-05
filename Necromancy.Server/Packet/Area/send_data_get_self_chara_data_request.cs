using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_data_get_self_chara_data_request : ClientHandler
    {
        public send_data_get_self_chara_data_request(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_data_get_self_chara_data_request;

        public override void Handle(NecClient client, NecPacket packet)
        {
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

            //sub_484720 - combat/leveling info
            Logger.Debug($"Character ID Loading : {client.Character.Id}");
            res.WriteInt32(client.Character.InstanceId); // InstanceId
            res.WriteInt32(client.Character.ClassId); // class
            res.WriteInt16(50); // current level //+50 Temporary client.Character.Level
            res.WriteInt64(555555550); // current exp
            res.WriteInt64(777777712); // soul exp
            res.WriteInt64(330000); // exp needed to level
            res.WriteInt64(440000); // soul exp needed to level
            res.WriteInt32(client.Character.currentHp); // current hp
            res.WriteInt32(client.Character.currentMp); // current mp
            res.WriteInt32(client.Character.currentOd); // current od
            res.WriteInt32(client.Character.maxHp); // max hp
            res.WriteInt32(client.Character.maxMp); // maxmp
            res.WriteInt32(client.Character.maxOd); // max od
            res.WriteInt32(client.Character.AdventureBagGold); // current gp
            res.WriteInt32(2000000); // map gp
            res.WriteInt32(1238); // value/100 = current weight
            res.WriteInt32(1895); // value/100 = max weight
            res.WriteByte(200); // condition

            // total stat level includes bonus'?
            res.WriteInt16(client.Character.Strength); // str
            res.WriteInt16(client.Character.vitality); // vit
            res.WriteInt16(client.Character.dexterity); // dex
            res.WriteInt16(client.Character.agility); // agi
            res.WriteInt16(client.Character.intelligence); // int
            res.WriteInt16(client.Character.piety); // pie
            res.WriteInt16(client.Character.luck); // luk

            // mag atk atrb
            res.WriteInt16(5); // fire
            res.WriteInt16(52); // water
            res.WriteInt16(58); // wind
            res.WriteInt16(45); // earth
            res.WriteInt16(33); // light
            res.WriteInt16(12); // dark
            res.WriteInt16(455); // changed nothing visably
            res.WriteInt16(466); // changed nothing visably
            res.WriteInt16(477); // changed nothing visably

            // mag def atrb
            res.WriteInt16(5); // fire
            res.WriteInt16(52); // water
            res.WriteInt16(58); // wind
            res.WriteInt16(45); // earth
            res.WriteInt16(33); // light
            res.WriteInt16(12); // dark
            res.WriteInt16(555); // changed nothing visably
            res.WriteInt16(566); // changed nothing visably
            res.WriteInt16(577); // changed nothing visably

            //status change resistance
            res.WriteInt16(2111); // Poison
            res.WriteInt16(2112); // Paralyze
            res.WriteInt16(2113); // Stone
            res.WriteInt16(2114); // Faint
            res.WriteInt16(2115); // Blind
            res.WriteInt16(2116); // Sleep
            res.WriteInt16(2117); // Silent
            res.WriteInt16(2118); // Charm
            res.WriteInt16(2119); // confus
            res.WriteInt16(2120); // fear
            res.WriteInt16(2121); // changed nothing visably (possibly EXP Boost Gauge. trying to find it

            // gold and alignment?
            res.WriteInt64(client.Character.AdventureBagGold); // gold
            res.WriteInt32(client.Character.Alignmentid); // AlignmentId
            res.WriteInt32(6000); // lawful
            res.WriteInt32(5000); // neutral
            res.WriteInt32(400000); // chaos
            res.WriteInt32(Util.GetRandomNumber(90400101, 90400130)); // title from honor.csv

            //sub_484980
            res.WriteInt32(110); // changed nothing visably
            res.WriteInt32(220); // changed nothing visably
            res.WriteInt32(330); // changed nothing visably

            // characters stats
            res.WriteInt16(client.Character.Strength); // str
            res.WriteInt16(client.Character.vitality); // vit
            res.WriteInt16(client.Character.dexterity); // dex
            res.WriteInt16(client.Character.agility); // agi
            res.WriteInt16(client.Character.intelligence); // int
            res.WriteInt16(client.Character.piety); // pie
            res.WriteInt16(client.Character.luck); // luk

            // nothing
            res.WriteInt16(10); // changed nothing visably
            res.WriteInt16(20); // changed nothing visably
            res.WriteInt16(30); // changed nothing visably
            res.WriteInt16(40); // changed nothing visably
            res.WriteInt16(50); // changed nothing visably
            res.WriteInt16(60); // changed nothing visably
            res.WriteInt16(70); // changed nothing visably
            res.WriteInt16(80); // changed nothing visably
            res.WriteInt16(90); // changed nothing visably


            // nothing
            res.WriteInt16(11); // changed nothing visably
            res.WriteInt16(22); // changed nothing visably
            res.WriteInt16(33); // changed nothing visably
            res.WriteInt16(44); // changed nothing visably
            res.WriteInt16(55); // changed nothing visably
            res.WriteInt16(66); // changed nothing visably
            res.WriteInt16(77); // changed nothing visably
            res.WriteInt16(88); // changed nothing visably
            res.WriteInt16(99); // changed nothing visably

            // nothing
            res.WriteInt16(111); // changed nothing visably
            res.WriteInt16(112); // changed nothing visably
            res.WriteInt16(113); // changed nothing visably
            res.WriteInt16(114); // changed nothing visably
            res.WriteInt16(115); // changed nothing visably
            res.WriteInt16(116); // changed nothing visably
            res.WriteInt16(117); // changed nothing visably
            res.WriteInt16(118); // changed nothing visably
            res.WriteInt16(119); // changed nothing visably
            res.WriteInt16(120); // changed nothing visably
            res.WriteInt16(121); // changed nothing visably


            //sub_484B00 map ip and connection
            res.WriteInt32(client.Character.MapId); //MapSerialID
            res.WriteInt32(client.Character.MapId); //MapID
            res.WriteFixedString("127.0.0.1", 65); //IP
            res.WriteInt16(60002); //Port

            //sub_484420 // Map Spawn coord
            res.WriteFloat(client.Character.X); //X Pos
            res.WriteFloat(client.Character.Y); //Y Pos
            res.WriteFloat(client.Character.Z); //Z Pos
            res.WriteByte(client.Character.Heading); //view offset

            //sub_read_int32 skill point
            res.WriteInt32(101); // skill point

            //sub_483420 character state like alive/dead/invis
            res.WriteInt32(0); //-254 GM

            //sub_494AC0
            res.WriteByte(78); // soul level
            res.WriteInt32(2200000); // current soul points
            res.WriteInt32(2900000); // changed nothing visably
            res.WriteInt32(12000000); // max soul points
            res.WriteByte(0); // 0 is white,1 yellow 2 red 3+ skull
            res.WriteByte(1); //Bool
            res.WriteByte(10); // changed nothing visably
            res.WriteByte(11); // changed nothing visably
            res.WriteByte(12); // changed nothing visably
            res.WriteByte(13); // changed nothing visably

            //sub_read_3-int16 unknown
            res.WriteInt16(50); // HP Consumption Rate?
            res.WriteInt16(50); // MP Consumption Rate?
            res.WriteInt16(50); // OD Consumption Rate (if greater than currentOD, Can not sprint)

            //sub_4833D0
            res.WriteInt64(1111111111111111); // changed nothing visably

            //sub_4833D0
            res.WriteInt64(2222222222222222); // changed nothing visably

            //sub_4834A0
            res.WriteFixedString($"{client.Soul.Name} Shop", 97); //Shopname

            //sub_4834A0
            res.WriteFixedString($"{client.Soul.Name} Comment", 385); //Comment

            //sub_494890
            res.WriteByte(0); //Bool 0 off 1 on

            //sub_4834A0
            res.WriteFixedString($"{client.Soul.Name} chatbox?", 385); //Chatbox?

            //sub_494890
            res.WriteByte(1); //Bool

            //sub_483420
            int numEntries = 19;
            res.WriteInt32(numEntries); //has to be less than 19(defines how many int32s to read?)

            //Consolidated Frequently Used Code
            LoadEquip.SlotSetup(res, client.Character ,numEntries);


            //sub_483420
            res.WriteInt32(numEntries); //has to be less than 19

            //Consolidated Frequently Used Code
            LoadEquip.EquipItems(res, client.Character, numEntries);

            //sub_483420
            res.WriteInt32(numEntries);

            LoadEquip.EquipSlotBitMask(res, numEntries);

            //sub_483420
            numEntries = 128;
            res.WriteInt32(numEntries); //has to be less than 128

            //sub_485A70
            for (int k = 0; k < numEntries; k++) //status buffs / debuffs
            {
                res.WriteInt32(15100901); //set to k
                res.WriteInt32(15100901);
                res.WriteInt32(15100901);
            }

            Router.Send(client, (ushort) AreaPacketId.recv_data_get_self_chara_data_r, res, ServerType.Area);
        }
    }
}
