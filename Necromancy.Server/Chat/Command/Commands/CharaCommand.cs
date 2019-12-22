using System.Collections.Generic;
using Necromancy.Server.Model;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Packet.Id;
using System.Threading;
using System;
using Necromancy.Server.Common.Instance;

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
                    //recv_chara_update_hp = 0xD133,
                    res.WriteInt32(y);
                    Router.Send(client, (ushort)AreaPacketId.recv_chara_update_hp, res, ServerType.Area);
                    break;

                case "dead":
                    //recv_battle_report_noact_notify_dead = 0xCDC9,
                    IBuffer res2 = BufferProvider.Provide();
                    res2.WriteInt32(character2.InstanceId);
                    res2.WriteInt32(y);
                    res2.WriteInt32(y);
                    res2.WriteInt32(y);
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_noact_notify_dead, res2, ServerType.Area);
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

                case "eo":
                    //recv_data_notify_eo_data = 0x8075, // Parent = 0x8066 // Range ID = 02
                    IBuffer res10 = BufferProvider.Provide();
                    res10.WriteInt32(123456);// Unique Instance ID of Skill Cast
                    res10.WriteFloat(character2.X + 100);//Effect Object X
                    res10.WriteFloat(character2.Y);//Effect Object Y
                    res10.WriteFloat(character2.Z + 100);//Effect Object Z
                    res10.WriteFloat(100);//Rotation Along X Axis if above 0
                    res10.WriteFloat(100);//Rotation Along Y Axis if above 0
                    res10.WriteFloat(100);//Rotation Along Z Axis if above 0
                    res10.WriteInt32(500008);//Effect id
                    res10.WriteInt32(1);//must be set to int32 contents. int myTargetID = packet.Data.ReadInt32();
                    res10.WriteInt32(0);//unknown
                    res10.WriteInt32(0);//unknown
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_data_notify_eo_data, res10, ServerType.Area);

                    IBuffer res11 = BufferProvider.Provide();
                    res11.WriteInt32(123456);
                    res11.WriteFloat(500);
                    //Router.Send(client.Map, (ushort)AreaPacketId.recv_eo_base_notify_sphere, res11, ServerType.Area);
                    break;

                case "eos":
                    //recv_eo_update_state = 0x28FD, // Parent = 0x28E7 // Range ID = 01
                    IBuffer res12 = BufferProvider.Provide();
                    res12.WriteInt32(123456);
                    res12.WriteInt32(y);
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_eo_update_state, res12, ServerType.Area);
                    break;

                case "sync":
                    IBuffer res13 = BufferProvider.Provide();
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_map_change_sync_ok, res13, ServerType.Area);
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
    }

}
