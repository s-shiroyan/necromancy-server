using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;
using Necromancy.Server.Packet.Response;
using Necromancy.Server.Tasks;
using System.Numerics;

namespace Necromancy.Server.Packet.Area
{
    public class send_map_get_info : ClientHandler
    {
        private readonly NecServer _server;
        public send_map_get_info(NecServer server) : base(server)
        {
            _server = server;
        }

        public override ushort Id => (ushort) AreaPacketId.send_map_get_info;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(client.Map.Id);
            Router.Send(client, (ushort) AreaPacketId.recv_map_get_info_r, res, ServerType.Area);

            foreach (MonsterSpawn monsterSpawn in client.Map.MonsterSpawns.Values)
            {
                if (monsterSpawn.Active == false)
                {
                    RecvDataNotifyMonsterData monsterData = new RecvDataNotifyMonsterData(monsterSpawn);
                    Logger.Debug($"Monster Id {monsterSpawn.Id} with model {monsterSpawn.ModelId} is loading");
                    Router.Send(monsterData, client);
                }
            }
            foreach (NpcSpawn npcSpawn in client.Map.NpcSpawns.Values)
            {
                // This requires database changes to add the GGates to the Npc database!!!!!
                if (npcSpawn.Name == "GGate")
                {
                    GGateSpawn gGate = new GGateSpawn();
                    gGate.X = npcSpawn.X;
                    gGate.Y = npcSpawn.Y;
                    gGate.Z = npcSpawn.Z;
                    gGate.Heading = npcSpawn.Heading;
                    gGate.MapId = npcSpawn.MapId;
                    gGate.Name = npcSpawn.Name;
                    gGate.Title = npcSpawn.Title;

                    RecvDataNotifyGGateData gGateData = new RecvDataNotifyGGateData(gGate);
                    Router.Send(gGateData, client);
                }
                else
                {
                    RecvDataNotifyNpcData npcData = new RecvDataNotifyNpcData(npcSpawn);
                    Router.Send(npcData, client);
                }
            }

            foreach (Gimmick gimmickSpawn in client.Map.GimmickSpawns.Values)
            {
                RecvDataNotifyGimmickData gimmickData = new RecvDataNotifyGimmickData(gimmickSpawn);
                    Router.Send(gimmickData, client);
                GGateSpawn gGateSpawn = new GGateSpawn();
                Server.Instances.AssignInstance(gGateSpawn);
                gGateSpawn.X = gimmickSpawn.X;
                gGateSpawn.Y = gimmickSpawn.Y;
                gGateSpawn.Z = gimmickSpawn.Z+300;
                gGateSpawn.Heading = gimmickSpawn.Heading;
                gGateSpawn.Name = $"gGateSpawn to your current position. ID {gimmickSpawn.ModelId}";
                gGateSpawn.Title = $"type '/gimmick move {gimmickSpawn.InstanceId} to move this ";
                gGateSpawn.MapId = gimmickSpawn.MapId;
                gGateSpawn.ModelId = 1900001;
                gGateSpawn.Active = 0;
                gGateSpawn.SerialId = 1900001;

                RecvDataNotifyGGateData gGateData = new RecvDataNotifyGGateData(gGateSpawn);
                Router.Send(gGateData, client);
            }

            foreach (GGateSpawn gGateSpawn in client.Map.GGateSpawns.Values)
            {
                RecvDataNotifyGGateData gGateSpawnData = new RecvDataNotifyGGateData(gGateSpawn);
                Router.Send(gGateSpawnData, client);
            }

            foreach (DeadBody deadBody in client.Map.DeadBodies.Values)
            {
                RecvDataNotifyCharaBodyData deadBodyData = new RecvDataNotifyCharaBodyData(deadBody,client);
                Router.Send(deadBodyData, client);
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

                if (otherClient.Union != null)
                {
                    RecvDataNotifyUnionData otherUnionData = new RecvDataNotifyUnionData(otherClient.Character, otherClient.Union.Name);
                    Router.Send(otherUnionData, client);
                }
            }
            if (client.Map.Id == 2002104)
            {
                Vector3 mapLinkSrc = new Vector3((float)-871.1273, (float)-11966.361, (float)462.58215);
                byte orientation = 90;
                SendDataNotifyMaplink(client, 2002105, mapLinkSrc, orientation);
            }
            else if (client.Map.Id == 2002105)
            {
                Vector3 mapLinkDest = new Vector3((float)-5778.367, (float)-6010.0425, (float)-1.6068916);
                byte orientation = 45;
                SendDataNotifyMaplink(client, 2002104, mapLinkDest, orientation);
            }
            // ToDo this should be a database lookup
            RecvMapFragmentFlag mapFragments = new RecvMapFragmentFlag(client.Map.Id, 0xff);
            _server.Router.Send(mapFragments, client);

        }
        public void SendDataNotifyMaplink(NecClient client, int mapId, Vector3 destCoords, byte orientation)
        {
            IBuffer res1 = BufferProvider.Provide(); // it's the aura portal for map
            res1.WriteInt32(mapId); // Unique ID

            res1.WriteFloat(destCoords.X); //x
            res1.WriteFloat(destCoords.Y); //y
            res1.WriteFloat(destCoords.Z + 2); //z
            res1.WriteByte(orientation); // offset

            res1.WriteFloat(1000); // Height
            res1.WriteFloat(100); // Width

            res1.WriteInt32(1); // Aura color 0=blue 1=gold 2=white 3=red 4=purple 5=black  0 to 5, crash above 5
            Router.Send(client, (ushort)AreaPacketId.recv_data_notify_maplink, res1, ServerType.Area);
        }
    }


}
