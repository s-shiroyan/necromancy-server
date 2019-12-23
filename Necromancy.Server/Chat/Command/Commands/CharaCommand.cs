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
                case "state":
                    IBuffer res = BufferProvider.Provide();
                    Console.Write("state");
                    //Router.Send(client.Map, (ushort)AreaPacketId.recv_monster_state_update_notify, res, ServerType.Area);
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
