using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Response;

namespace Necromancy.Server.Packet.Area
{
    public class send_map_get_info : ClientHandler
    {
        public send_map_get_info(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_map_get_info;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(client.Map.Id);
            Router.Send(client, (ushort) AreaPacketId.recv_map_get_info_r, res, ServerType.Area);

            foreach (NpcSpawn npcSpawn in client.Map.NpcSpawns.Values)
            {
                // This requires database changes to add the GGates to the Npc database!!!!!
                if (npcSpawn.Name == "GGate")
                {
                    SendDataNotifyGGateStoneData(client, npcSpawn);
                }
                else
                {
                    RecvDataNotifyNpcData npcData = new RecvDataNotifyNpcData(npcSpawn);
                    Router.Send(npcData, client);
                }
            }

            List<MonsterSpawn> monsterSpawns = Database.SelectMonsterSpawnsByMapId(client.Map.Id);
            foreach (MonsterSpawn monsterSpawn in monsterSpawns)
            {
                Server.Instances.AssignInstance(monsterSpawn);
                RecvDataNotifyMonsterData monsterData = new RecvDataNotifyMonsterData(monsterSpawn);
                Router.Send(monsterData, client);
            }

            foreach (NecClient otherClient in client.Map.ClientLookup.GetAll())
            {
                if (otherClient == client)
                {
                    // skip myself
                    continue;
                }

                RecvDataNotifyCharaData otherCharacterData =
                    new RecvDataNotifyCharaData(otherClient.Character, otherClient.Soul.Name);
                Router.Send(otherCharacterData, client);
            }
        }

        private void SendDataNotifyGGateStoneData(NecClient client, NpcSpawn npcSpawn)
        {
            if (client.Character.MapId == 2002104 || client.Character.MapId == 2002105 || client.Character.MapId == 2002106)
            {
                Logger.Debug($"npcSpawnWarp.InstanceId: {npcSpawn.InstanceId}  |   npcSpawnWarp.NpcId: {npcSpawn.NpcId}");
                IBuffer res = BufferProvider.Provide();
                //                res.WriteInt32(GGateModelIds[GGateChoice]);// Unique Object ID.  Crash if already in use (dont use your character ID)
                //                res.WriteInt32(GGateChoice);// Serial ID for Interaction? from npc.csv????
                res.WriteInt32(npcSpawn.InstanceId);// Unique Object ID.  Crash if already in use (dont use your character ID)
                res.WriteInt32(93101);// Serial ID for Interaction? from npc.csv????
                res.WriteByte(1);// 0 = Text, 1 = F to examine  , 2 or above nothing
                res.WriteCString($"");//"0x5B" //Name
                res.WriteCString($"");//"0x5B" //Title
                res.WriteFloat((float)npcSpawn.X);//X
                res.WriteFloat((float)npcSpawn.Y);//Y
                res.WriteFloat((float)npcSpawn.Z);//Z
                res.WriteByte(0);//
                res.WriteInt32(npcSpawn.ModelId);// Optional Model ID. Warp Statues. Gaurds, Pedistals, Etc., to see models refer to the model_common.csv

                res.WriteInt16(npcSpawn.Size);//  size of the object

                res.WriteInt32(npcSpawn.Active ? 1 : 0);// 0 = collision, 1 = no collision  (active/Inactive?)

                res.WriteInt32(0);//0= no effect color appear, //Red = 0bxx1x   | Gold = obxxx1   |blue = 0bx1xx


                Router.Send(client, (ushort)AreaPacketId.recv_data_notify_ggate_stone_data, res, ServerType.Area);
            }
        }
    }
}
