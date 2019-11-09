using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Response;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Spawns a monster
    /// </summary>
    public class MonsterCommand : ServerChatCommand
    {
        public MonsterCommand(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (!int.TryParse(command[0], out int monsterId))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[0]}"));
                return;
            }

            if (!Server.SettingRepository.Monster.TryGetValue(monsterId, out MonsterSetting monsterSetting))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid MonsterId: {monsterId}"));
                return;
            }

            if (!int.TryParse(command[1], out int modelId))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[1]}"));
                return;
            }

            if (!Server.SettingRepository.ModelCommon.TryGetValue(modelId, out ModelCommonSetting modelSetting))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid ModelId: {modelId}"));
                return;
            }

            MonsterSpawn monsterSpawn = Server.Instances.CreateInstance<MonsterSpawn>();
            monsterSpawn.MonsterId = monsterSetting.Id;
            monsterSpawn.Name = monsterSetting.Name;
            monsterSpawn.Title = monsterSetting.Title;
            monsterSpawn.Level = (byte) monsterSetting.Level;

            monsterSpawn.ModelId = modelSetting.Id;
            monsterSpawn.Size = (short) modelSetting.Height;

            monsterSpawn.MapId = client.Character.MapId;
            monsterSpawn.X = client.Character.X;
            monsterSpawn.Y = client.Character.Y;
            monsterSpawn.Z = client.Character.Z;
            monsterSpawn.Heading = client.Character.Heading;

            if (!Server.Database.InsertMonsterSpawn(monsterSpawn))
            {
                responses.Add(ChatResponse.CommandError(client, "MonsterSpawn could not be saved to database"));
                return;
            }

            RecvDataNotifyMonsterData monsterData = new RecvDataNotifyMonsterData(monsterSpawn);
            Router.Send(client.Map, monsterData);

            IBuffer res5 = BufferProvider.Provide();
            res5.WriteInt32(11);
            res5.WriteInt32(monsterSpawn.InstanceId);
            Router.Send(client, (ushort) AreaPacketId.recv_monster_hate_on, res5, ServerType.Area);

            IBuffer res6 = BufferProvider.Provide();
            res6.WriteInt32(11);
            res6.WriteInt32(monsterSpawn.InstanceId);
            Router.Send(client, (ushort) AreaPacketId.recv_battle_report_notify_damage_hp, res6, ServerType.Area);


            IBuffer res12 = BufferProvider.Provide();
            res12.WriteInt32(0);

            res12.WriteInt32(monsterSpawn.InstanceId);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_monster_state_update_notify, res12, ServerType.Area);

            /*IBuffer res81 = BufferProvider.Provide();
            res81.WriteInt32(11);
    
            res81.WriteFloat(45);
            res81.WriteFloat(0);
            res81.WriteFloat(0);
            res81.WriteByte(0);
    
            res81.WriteFloat(0);
            res81.WriteFloat(0);
            res81.WriteInt32(3);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_data_notify_maplink, res81);
    
            /*
                        IBuffer res5 = BufferProvider.Provide();
                        res5.WriteInt32(8);
                        res5.WriteInt32(0);
                        res5.WriteFloat(1);
                        Router.Send(client, (ushort)AreaPacketId.recv_battle_report_action_monster_skill_start_cast, res5);
    
    
                        IBuffer res6 = BufferProvider.Provide();
                        res6.WriteInt32(0);
                        Router.Send(client, (ushort)AreaPacketId.recv_battle_report_action_monster_skill_exec, res6);
    
                        IBuffer res8 = BufferProvider.Provide();
                        res8.WriteInt32(client.Character.Id);
                        res8.WriteInt32(1);
                        Router.Send(client, (ushort)AreaPacketId.recv_battle_report_notify_damage_hp, res8);
    
                        IBuffer res4 = BufferProvider.Provide();
                        res4.WriteInt32(1);
                        Router.Send(client, (ushort)AreaPacketId.recv_battle_report_notify_hit_effect, res4);
    
                        IBuffer res10 = BufferProvider.Provide();
                        res10.WriteByte(1);
                        res10.WriteInt16(0);
                        Router.Send(client, (ushort)AreaPacketId.recv_chara_target_move_side_speed_per, res10);
    
                        IBuffer res9 = BufferProvider.Provide();
                        res9.WriteInt32(8);
    
                        res9.WriteInt32(8); // 1 = no reactive ?
                        Router.Send(client, (ushort)AreaPacketId.recv_monster_state_update_notify, res9); */


            //3100102 attack monster, where to put it ?
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "mon";
        public override string HelpText => "usage: `/mon [monsterId] [modelId]` - Spawns a Monster";
    }
}
