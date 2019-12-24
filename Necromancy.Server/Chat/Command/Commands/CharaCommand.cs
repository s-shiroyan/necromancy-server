using System.Collections.Generic;
using Necromancy.Server.Model;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Packet.Id;
using System.Threading;
using System;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Packet.Response;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Quick mob test commands.
    /// </summary>
    public class CharaCommand : ServerChatCommand
    {
        public CharaCommand(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (command[0] == null)
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid argument: {command[0]}"));
                return;
            }
            Character character2 = null;
            if (uint.TryParse(command[1], out uint x))
            {
                IInstance instance = Server.Instances.GetInstance(x);
                if (instance is Character character)
                {
                    character2 = character;
                }
                else
                {
                    responses.Add(ChatResponse.CommandError(client, $"Please provide a character instance id"));
                    return;
                }
            }

            if (!int.TryParse(command[2], out int y))
            {
                responses.Add(ChatResponse.CommandError(client, $"Please provide a value to test"));
                return;
            }


            switch (command[0])
            {
                case "hp":
                    IBuffer res = BufferProvider.Provide();
                    res.WriteInt32(y);
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_chara_update_hp, res, ServerType.Area);
                    break;

                case "dead":
                    SendBattleReportStartNotify(client, character2);
                    //recv_battle_report_noact_notify_dead = 0xCDC9,
                    IBuffer res2 = BufferProvider.Provide();
                    res2.WriteInt32(character2.InstanceId);
                    res2.WriteInt32(y); // death type? 1 = death, 2 = death and message, 3 = unconscious, beyond that = nothing
                    res2.WriteInt32(0);
                    res2.WriteInt32(0);
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_noact_notify_dead, res2, ServerType.Area);
                    SendBattleReportEndNotify(client, character2);
                    break;

                case "pose":
                    IBuffer res3 = BufferProvider.Provide();
                    //recv_battle_attack_pose_start_notify = 0x7CB2,
                    res3.WriteInt32(y);
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_attack_pose_start_notify, res3, ServerType.Area);
                    break;

                case "pose2":
                    IBuffer res4 = BufferProvider.Provide();
                    res4.WriteInt32(character2.InstanceId);//Character ID
                    res4.WriteInt32(y); //Character pose
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_chara_pose_notify, res4, ServerType.Area, client);

                    break;

                case "emotion":
                    //recv_emotion_notify_type = 0xF95B,
                    IBuffer res5 = BufferProvider.Provide();
                    res5.WriteInt32(character2.InstanceId);
                    res5.WriteInt32(y);
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_emotion_notify_type, res5, ServerType.Area);
                    break;

                case "deadstate":
                    //recv_charabody_notify_deadstate = 0xCC36, // Parent = 0xCB94 // Range ID = 03
                    IBuffer res6 = BufferProvider.Provide();
                    res6.WriteInt32(character2.InstanceId);
                    res6.WriteInt32(y);//4 here causes a cloud and the model to disappear, 5 causes a mist to happen and disappear
                    res6.WriteInt32(y);
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_charabody_notify_deadstate, res6, ServerType.Area);
                    break;

                case "start":
                    IBuffer res7 = BufferProvider.Provide();
                    res7.WriteInt32(character2.InstanceId);
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_start_notify, res7, ServerType.Area);
                    break;

                case "end":
                    IBuffer res8 = BufferProvider.Provide();
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_end_notify, res8, ServerType.Area);
                    break;

                case "gimmick":
                    //recv_data_notify_gimmick_data = 0xBFE9,
                    IBuffer res9 = BufferProvider.Provide();
                    res9.WriteInt32(69696);
                    res9.WriteFloat(client.Character.X + 100);
                    res9.WriteFloat(client.Character.Y);
                    res9.WriteFloat(client.Character.Z);
                    res9.WriteByte(client.Character.Heading);
                    res9.WriteInt32(y); //Gimmick number (from gimmick.csv)
                    res9.WriteInt32(0);
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_data_notify_gimmick_data, res9, ServerType.Area);
                    break;

                case "bodystate":
                    //recv_charabody_state_update_notify = 0x1A0F, 
                    IBuffer res10 = BufferProvider.Provide();
                    res10.WriteInt32(character2.InstanceId);
                    res10.WriteInt32(y);
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_charabody_state_update_notify, res10, ServerType.Area);
                    break;

                case "charastate":
                    //recv_chara_notify_stateflag = 0x23D3, 
                    IBuffer res11 = BufferProvider.Provide();
                    res11.WriteInt32(character2.InstanceId);
                    res11.WriteInt32(y);
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_chara_notify_stateflag, res11, ServerType.Area);
                    break;

                case "spirit":
                    //recv_charabody_notify_spirit = 0x36A6, 
                    IBuffer res12 = BufferProvider.Provide();
                    res12.WriteInt32(character2.InstanceId);
                    res12.WriteByte((byte)y);
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_charabody_notify_spirit, res12, ServerType.Area);
                    break;

                case "data":
                    SendDataGetSelfCharaData(Server.Clients.GetByCharacterInstanceId(character2.InstanceId));
                    break;

                case "abyss":
                    //recv_charabody_self_notify_abyss_stead_pos = 0x679B,
                    IBuffer res13 = BufferProvider.Provide();
                    res13.WriteFloat(character2.X);
                    res13.WriteFloat(character2.Y);
                    res13.WriteFloat(character2.Z);
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_charabody_self_notify_abyss_stead_pos, res13, ServerType.Area);
                    break;

                case "charadata":
                    RecvDataNotifyCharaData cData = new RecvDataNotifyCharaData(character2, character2.Name);
                    Router.Send(Server.Clients.GetAll(), cData.ToPacket());
                    break;

                case "charabodydata":
                    //recv_data_notify_charabody_data = 0x906A,
                    IBuffer res14 = BufferProvider.Provide();
                    res14.WriteInt32(character2.InstanceId + 10000); //Instance ID of dead body
                    res14.WriteInt32(character2.InstanceId); //Reference to actual player's instance ID
                    res14.WriteCString("soulname"); // Soul name 
                    res14.WriteCString($"{character2.Name}"); // Character name
                    res14.WriteFloat(character2.X + 200); // X
                    res14.WriteFloat(character2.Y); // Y
                    res14.WriteFloat(character2.Z); // Z
                    res14.WriteByte(character2.Heading); // Heading
                    res14.WriteInt32(0);

                    int numEntries = 19;
                    res14.WriteInt32(numEntries);//less than or equal to 19
                    for (int i = 0; i < numEntries; i++)
                    {
                        res14.WriteInt32(0);
                    }

                    numEntries = 19;
                    res14.WriteInt32(numEntries);
                    for (int i = 0; i < numEntries; i++)
                    {
                        res14.WriteInt32(0);
                        res14.WriteByte(0);
                        res14.WriteByte(0);
                        res14.WriteByte(0);
                        res14.WriteInt32(0);
                        res14.WriteByte(0);
                        res14.WriteByte(0);
                        res14.WriteByte(0);

                        res14.WriteByte(0);
                        res14.WriteByte(0);
                        res14.WriteByte(0);//bool
                        res14.WriteByte(0);
                        res14.WriteByte(0);
                        res14.WriteByte(0);
                        res14.WriteByte(0);
                        res14.WriteByte(0);
                    }

                    numEntries = 19;
                    res14.WriteInt32(numEntries);
                    for (int i = 0; i < numEntries; i++)
                    {
                        res14.WriteInt32(0);
                    }

                    res14.WriteInt32(0); //Race ID
                    res14.WriteInt32(0); //Gender ID
                    res14.WriteByte(0); //Hair style
                    res14.WriteByte(0); //Hair color
                    res14.WriteByte(0); //Face id
                    res14.WriteInt32(1);// 0 = bag, 1 for dead? (Can't enter soul form if this isn't 0 or 1 i think).
                    res14.WriteInt32(0);//4 = ash pile, not sure what this is.
                    res14.WriteInt32(0);
                    res14.WriteInt32(y); //death pose 0 = faced down, 1 = head chopped off, 2 = no arm, 3 = faced down, 4 = chopped in half, 5 = faced down, 6 = faced down, 7 and up "T-pose" the body (ONLY SEND 1 IF YOU ARE CALLING THIS FOR THE FIRST TIME)
                    res14.WriteByte(0);//crim status (changes icon on the end also), 0 = white, 1 = yellow, 2 = red, 3 = red with crim icon, 
                    res14.WriteByte(0);// (bool) Beginner protection
                    res14.WriteInt32(1);
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_data_notify_charabody_data, res14, ServerType.Area);
                    break;

                default:
                    Logger.Error($"There is no recv of type : {command[0]} ");
                    break;
            }
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "chara";
        public override string HelpText => "usage: `/chara [command] [instance id] [y]` - Fires a recv to the game of command type with [instance id] as target and [y] number as an argument.";

        private void SendBattleReportStartNotify(NecClient client, IInstance instance)
        {
            IBuffer res4 = BufferProvider.Provide();
            res4.WriteInt32(instance.InstanceId);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_start_notify, res4, ServerType.Area);
        }
        private void SendBattleReportEndNotify(NecClient client, IInstance instance)
        {
            IBuffer res4 = BufferProvider.Provide();
            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_end_notify, res4, ServerType.Area);
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
            res.WriteInt16(client.Character.Level); // current level
            res.WriteInt64(555555550); // current exp
            res.WriteInt64(777777712); // soul exp
            res.WriteInt64(33); // exp needed to level
            res.WriteInt64(44); // soul exp needed to level
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
            res.WriteInt16(24); // str
            res.WriteInt16(28); // vit
            res.WriteInt16(35); // dex
            res.WriteInt16(89); // agi
            res.WriteInt16(42); // int
            res.WriteInt16(52); // pie
            res.WriteInt16(90); // luk

            // mag atk atrb
            res.WriteInt16(5); // fire
            res.WriteInt16(52); // water
            res.WriteInt16(58); // wind
            res.WriteInt16(45); // earth
            res.WriteInt16(33); // light
            res.WriteInt16(12); // dark
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably

            // mag def atrb
            res.WriteInt16(5); // fire
            res.WriteInt16(52); // water
            res.WriteInt16(58); // wind
            res.WriteInt16(45); // earth
            res.WriteInt16(33); // light
            res.WriteInt16(12); // dark
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably

            //status change resistance
            res.WriteInt16(215); // fire
            res.WriteInt16(99); // water
            res.WriteInt16(88); // wind
            res.WriteInt16(455); // earth
            res.WriteInt16(333); // light
            res.WriteInt16(1222); // dark
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably

            // gold and alignment?
            res.WriteInt64(client.Character.AdventureBagGold); // gold
            res.WriteInt32(187); // changed nothing visably
            res.WriteInt32(600000); // lawful
            res.WriteInt32(5000); // neutral
            res.WriteInt32(4000); // chaos
            res.WriteInt32(Util.GetRandomNumber(90400101, 90400130)); // title from honor.csv

            //sub_484980
            res.WriteInt32(0); // changed nothing visably
            res.WriteInt32(0); // changed nothing visably
            res.WriteInt32(0); // changed nothing visably

            // characters stats
            res.WriteInt16(client.Character.Strength); // str
            res.WriteInt16(client.Character.vitality); // vit
            res.WriteInt16(client.Character.dexterity); // dex
            res.WriteInt16(client.Character.agility); // agi
            res.WriteInt16(client.Character.intelligence); // int
            res.WriteInt16(client.Character.piety); // pie
            res.WriteInt16(client.Character.luck); // luk

            // nothing
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably


            // nothing
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably

            // nothing
            res.WriteInt16(51); // changed nothing visably
            res.WriteInt16(25); // changed nothing visably
            res.WriteInt16(10); // changed nothing visably
            res.WriteInt16(25); // changed nothing visably
            res.WriteInt16(87); // changed nothing visably
            res.WriteInt16(122); // changed nothing visably
            res.WriteInt16(14); // changed nothing visably
            res.WriteInt16(73); // changed nothing visably
            res.WriteInt16(69); // changed nothing visably
            res.WriteInt16(73); // changed nothing visably
            res.WriteInt16(69); // changed nothing visably


            //sub_484B00 map ip and connection
            res.WriteInt32(client.Character.MapId); //MapSerialID
            res.WriteInt32(client.Character.MapId); //MapID
            res.WriteFixedString("127.0.0.1", 65); //IP
            res.WriteInt16(60002); //Port

            //sub_484420 // Map Spawn coord
            res.WriteFloat(client.Character.X); //X Pos
            res.WriteFloat(client.Character.Y); //Y Pos
            res.WriteFloat(client.Character.Z); //Z Pos
            res.WriteByte(15); //view offset

            //sub_read_int32 skill point
            res.WriteInt32(101); // skill point

            //sub_483420 character state like alive/dead/invis
            res.WriteInt32(0); //-254 GM

            //sub_494AC0
            res.WriteByte(78); // soul level
            res.WriteInt32(22); // current soul points
            res.WriteInt32(29); // changed nothing visably
            res.WriteInt32(12); // max soul points
            res.WriteByte(0); // 0 is white,1 yellow 2 red 3+ skull
            res.WriteByte(0); //Bool
            res.WriteByte(0); // changed nothing visably
            res.WriteByte(0); // changed nothing visably
            res.WriteByte(0); // changed nothing visably
            res.WriteByte(0); // changed nothing visably

            //sub_read_3-int16 unknown
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably

            //sub_4833D0
            res.WriteInt64(0); // changed nothing visably

            //sub_4833D0
            res.WriteInt64(0); // changed nothing visably

            //sub_4834A0
            res.WriteFixedString("", 97); //Shopname

            //sub_4834A0
            res.WriteFixedString("", 385); //Comment

            //sub_494890
            res.WriteByte(1); //Bool

            //sub_4834A0
            res.WriteFixedString("aaaa", 385); //Chatbox?

            //sub_494890
            res.WriteByte(1); //Bool

            //sub_483420
            int numEntries = 19;
            res.WriteInt32(numEntries); //has to be less than 19(defines how many int32s to read?)

            //Consolidated Frequently Used Code
            LoadEquip.SlotSetup(res, client.Character);


            //sub_483420
            res.WriteInt32(numEntries); //has to be less than 19

            //Consolidated Frequently Used Code
            LoadEquip.EquipItems(res, client.Character);

            //sub_483420
            res.WriteInt32(numEntries);

            LoadEquip.EquipSlotBitMask(res, client.Character);

            //sub_483420
            numEntries = 128;
            res.WriteInt32(numEntries); //has to be less than 128

            //sub_485A70
            for (int imac = 0; imac < numEntries; imac++) //status buffs / debuffs
            {
                res.WriteInt32(15100901); //[eax]:&L"i.dllext-ms-mf-pal-l2-1-0"
                res.WriteInt32(15100901);
                res.WriteInt32(15100901);
            }

            //Router.Send(client, (ushort)AreaPacketId.recv_data_get_self_chara_data_r, res, ServerType.Area);

            IBuffer res2 = BufferProvider.Provide();
            Router.Send(client, (ushort)AreaPacketId.recv_data_get_self_chara_data_request_r, res2, ServerType.Area);
        }
    }

}
