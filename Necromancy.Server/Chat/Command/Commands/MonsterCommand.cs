using System.Collections.Generic;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Response;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Spawns a monster
    /// </summary>
    public class MonsterCommand : ServerChatCommand
    {
        //protected NecServer server { get; }
        public MonsterCommand(NecServer server) : base(server)
        {
            //this.server = server;
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            MonsterSpawn monsterSpawn = Server.Instances.CreateInstance<MonsterSpawn>();
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

            Logger.Debug($"modelSetting.Radius [{modelSetting.Radius}]");
            monsterSpawn.MonsterId = monsterSetting.Id;
            monsterSpawn.Name = monsterSetting.Name;
            monsterSpawn.Title = monsterSetting.Title;
            monsterSpawn.Level = (byte) monsterSetting.Level;

            monsterSpawn.ModelId = modelSetting.Id;
            monsterSpawn.Size = (short) (modelSetting.Height / 2);
            monsterSpawn.Radius = (short) modelSetting.Radius;

            monsterSpawn.MapId = client.Character.MapId;

            monsterSpawn.X = client.Character.X;
            monsterSpawn.Y = client.Character.Y;
            monsterSpawn.Z = client.Character.Z;
            monsterSpawn.Heading = client.Character.Heading;

            monsterSpawn.Hp.setMax(100);
            monsterSpawn.Hp.setCurrent(100);

            if (!Server.Database.InsertMonsterSpawn(monsterSpawn))
            {
                responses.Add(ChatResponse.CommandError(client, "MonsterSpawn could not be saved to database"));
                return;
            }

            RecvDataNotifyMonsterData monsterData = new RecvDataNotifyMonsterData(monsterSpawn);
            Router.Send(client.Map, monsterData);
        }


        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "mon";
        public override string HelpText => "usage: `/mon [monsterId] [modelId]` - Spawns a Monster";
    }
}
