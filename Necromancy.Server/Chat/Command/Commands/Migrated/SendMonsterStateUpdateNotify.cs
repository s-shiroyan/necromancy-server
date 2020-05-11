using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    public class SendMonsterStateUpdateNotify : ServerChatCommand
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(SendMonsterStateUpdateNotify));

        public SendMonsterStateUpdateNotify(NecServer server) : base(server)
        {
        }

        int i = 0;

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            IInstance instance = Server.Instances.GetInstance((uint) client.Character.eventSelectReadyCode);

            switch (instance)
            {
                case MonsterSpawn monsterSpawn:
                    Logger.Debug($"MonsterInstanceId: {monsterSpawn.InstanceId} state is being set to {i}");

                    IBuffer res = BufferProvider.Provide();

                    res.WriteUInt32(monsterSpawn.InstanceId);
                    //Toggles state between Alive(attackable),  Dead(lootable), or Inactive(nothing). 
                    res.WriteInt32(i);
                    i++;

                    Router.Send(client, (ushort) AreaPacketId.recv_monster_state_update_notify, res, ServerType.Area);

                    Logger.Debug(
                        $"Setting Monster Hate for Monster ID {monsterSpawn.InstanceId} to act on character ID {2}");
                    IBuffer res2 = BufferProvider.Provide();
                    res2.WriteInt32(
                        8008135); //Unique instance of this monsters "Hate" attribute. not to be confused with the Monsters InstanceID
                    res2.WriteUInt32(client.Character.InstanceId);


                    Router.Send(client, (ushort) AreaPacketId.recv_monster_hate_on, res2, ServerType.Area);


                    break;
                default:
                    Logger.Error($"There is no monster with ID : {instance.InstanceId} ");
                    break;
            }
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "monsterstate";
    }
}
