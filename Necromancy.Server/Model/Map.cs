using System;
using System.Collections.Generic;
using System.Numerics;
using Arrowgene.Services.Logging;
using Arrowgene.Services.Tasks;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Packet.Receive;
using Necromancy.Server.Packet.Response;

namespace Necromancy.Server.Model
{
    public class Map
    {
        public const int NewCharacterMapId = 1001902;   //2006000

        private readonly NecLogger _logger;
        private readonly NecServer _server;
        public int Id { get; set; }
        public float X { get; }
        public float Y { get; }
        public float Z { get; }
        public string Country { get; set; }
        public string Area { get; set; }
        public string Place { get; set; }
        public byte Orientation { get; }
        public string FullName => $"{Country}/{Area}/{Place}";
        public ClientLookup ClientLookup { get; }
        public Dictionary<int, NpcSpawn> NpcSpawns { get; }
        public Dictionary<int, MonsterSpawn> MonsterSpawns { get; }
        public Dictionary<int, DeadBody> DeadBodies { get; }
        public TaskManager MonsterTasks;

        public Map(MapSetting setting, NecServer server)
        {
            _server = server;
            _logger = LogProvider.Logger<NecLogger>(this);
            ClientLookup = new ClientLookup();
            NpcSpawns = new Dictionary<int, NpcSpawn>();
            MonsterSpawns = new Dictionary<int, MonsterSpawn>();
            Id = setting.Id;
            X = setting.X;
            Y = setting.Y;
            Z = setting.Z;
            Country = setting.Country;
            Area = setting.Area;
            Place = setting.Place;
            Orientation = (byte)(setting.Orientation/2);   // Client uses 180 degree orientation
            MonsterTasks = new TaskManager();

            //Assign Unique Instance ID to each NPC per map. Add to dictionary stored with the Map object
            List<NpcSpawn> npcSpawns = server.Database.SelectNpcSpawnsByMapId(setting.Id);
            foreach (NpcSpawn npcSpawn in npcSpawns)
            {
                server.Instances.AssignInstance(npcSpawn);
                NpcSpawns.Add((int)npcSpawn.InstanceId, npcSpawn);
            }

            //To-Do   | for each deadBody in Deadbodies {RecvDataNotifyCharabodyData} 

            List<MonsterSpawn> monsterSpawns = server.Database.SelectMonsterSpawnsByMapId(setting.Id);
            foreach (MonsterSpawn monsterSpawn in monsterSpawns)
            {
                server.Instances.AssignInstance(monsterSpawn);
                if (!_server.SettingRepository.ModelCommon.TryGetValue(monsterSpawn.ModelId, out ModelCommonSetting modelSetting))
                {
                    return;
                }
                if (!_server.SettingRepository.Monster.TryGetValue(monsterSpawn.MonsterId, out MonsterSetting monsterSetting))
                {
                    return;
                }
                monsterSpawn.ModelId = modelSetting.Id;
                monsterSpawn.Size = (short)(modelSetting.Height / 2);
                monsterSpawn.Radius = (short)modelSetting.Radius;
                monsterSpawn.MaxHp = 300;
                monsterSpawn.SetHP(300);
                monsterSpawn.AttackSkillId = monsterSetting.AttackSkillId;
                monsterSpawn.Level = (byte)monsterSetting.Level;
                monsterSpawn.CombatMode = monsterSetting.CombatMode;
                monsterSpawn.CatalogId = monsterSetting.CatalogId;
                monsterSpawn.TextureType = monsterSetting.TextureType;
                monsterSpawn.Map = this;
                MonsterSpawns.Add((int)monsterSpawn.InstanceId, monsterSpawn);

                List<MonsterCoord> coords = server.Database.SelectMonsterCoordsByMonsterId(monsterSpawn.Id);
                if (coords.Count > 0)
                {
                    monsterSpawn.defaultCoords = false;
                    monsterSpawn.monsterCoords.Clear();
                    foreach (MonsterCoord monsterCoord in coords)
                    {
                        //Console.WriteLine($"added coord {monsterCoord} to monster {monsterSpawn.InstanceId}");
                        monsterSpawn.monsterCoords.Add(monsterCoord);
                    }
                }
                else
                {

                    //home coordinate set to monster X,Y,Z from database
                    Vector3 homeVector3 = new Vector3(monsterSpawn.X, monsterSpawn.Y, monsterSpawn.Z);
                    MonsterCoord homeCoord = new MonsterCoord();
                    homeCoord.Id = monsterSpawn.Id;
                    homeCoord.MonsterId = (uint)monsterSpawn.MonsterId;
                    homeCoord.MapId = (uint)monsterSpawn.MapId;
                    homeCoord.CoordIdx = 0;
                    homeCoord.destination = homeVector3;
                    monsterSpawn.monsterCoords.Add(homeCoord);

                    //default path part 2
                    Vector3 defaultVector3 = new Vector3(monsterSpawn.X, monsterSpawn.Y + Util.GetRandomNumber(50, 150), monsterSpawn.Z);
                    MonsterCoord defaultCoord = new MonsterCoord();
                    defaultCoord.Id = monsterSpawn.Id;
                    defaultCoord.MonsterId = (uint)monsterSpawn.MonsterId;
                    defaultCoord.MapId = (uint)monsterSpawn.MapId;
                    defaultCoord.CoordIdx = 1; 
                    defaultCoord.destination = defaultVector3;

                    monsterSpawn.monsterCoords.Add(defaultCoord);

                    //default path part 3
                    Vector3 defaultVector32 = new Vector3(monsterSpawn.X + Util.GetRandomNumber(50, 150), monsterSpawn.Y + Util.GetRandomNumber(50, 150), monsterSpawn.Z);
                    MonsterCoord defaultCoord2 = new MonsterCoord();
                    defaultCoord2.Id = monsterSpawn.Id;
                    defaultCoord2.MonsterId = (uint)monsterSpawn.MonsterId;
                    defaultCoord2.MapId = (uint)monsterSpawn.MapId;
                    defaultCoord2.CoordIdx = 2; //64 is currently the Idx of monsterHome on send_map_get_info.cs
                    defaultCoord2.destination = defaultVector32;

                    monsterSpawn.monsterCoords.Add(defaultCoord2);

                }
                
            }
            // ToDo this should be a database lookup
            if (Id == 2002104)
            {
                Vector3 leftVec = new Vector3((float)-515.07556, -12006, (float)462.58215);
                Vector3 rightVec = new Vector3((float)-1230.5432, -12006, (float)462.58215);
                MapTransition mapTransition = new MapTransition(_server, this, 2002105, leftVec, rightVec, false);
            }
            else if (Id == 2002105)
            {
                Vector3 leftVec = new Vector3((float)-5821.617, (float)-5908.8086, (float)-0.22658157);
                Vector3 rightVec = new Vector3((float)-5820.522, (float)-6114.8306, (float)0.046382904);
                MapPosition returnPos = new MapPosition((float)-889.7094, (float)-11444.197, (float)462.58234);
                MapTransition mapTransition = new MapTransition(_server, this, 2002104, leftVec, rightVec, true, returnPos);

            }
        }





        public void EnterForce(NecClient client, MapPosition mapPosition = null)
        {
            Enter(client, mapPosition);
            _server.Router.Send(new RecvMapChangeForce(this, mapPosition), client);
        }

        public void EnterSyncOk(NecClient client)
        {
            _server.Router.Send(new RecvMapChangeSyncOk(), client);
        }

        public void Enter(NecClient client, MapPosition mapPosition = null)
        {
            if (client.Map != null)
            {
                client.Map.Leave(client);
            }

            _logger.Info(client, $"Entering Map: {Id}:{FullName}", client);
            ClientLookup.Add(client);
            client.Map = this;
            client.Character.MapId = Id;
            RecvDataNotifyCharaData myCharacterData = new RecvDataNotifyCharaData(client.Character, client.Soul.Name);
            _server.Router.Send(this, myCharacterData, client);
        }

        public void Leave(NecClient client)
        {
            _logger.Info(client, $"Leaving Map: {Id}:{FullName}", client);
            ClientLookup.Remove(client);
            client.Map = null;

            RecvObjectDisappearNotify objectDisappearData = new RecvObjectDisappearNotify(client.Character.InstanceId);
            _server.Router.Send(this, objectDisappearData, client);
            if (ClientLookup.GetAll().Count == 0)
            {
                foreach (MonsterSpawn monsterSpawn in this.MonsterSpawns.Values)
                {
                    monsterSpawn.SpawnActive = false;
                }
            }
        }

        public List<MonsterSpawn> GetMonstersRange(Vector3 position, int range)
        {
            List<MonsterSpawn> monsters = new List<MonsterSpawn>();

            foreach (MonsterSpawn monster in MonsterSpawns.Values)
            {
                Vector3 monsterPos = new Vector3(monster.X, monster.Y, monster.Z);
                if (Vector3.Distance(position, monsterPos) <= range)
                {
                    monsters.Add(monster);
                }
            }
            return monsters;
        }

        public List<Character> GetCharactersRange(Vector3 position, int range)
        {
            List<Character> characters = new List<Character>();

           foreach (NecClient client in ClientLookup.GetAll())
            {
                Character character = client.Character;
                Vector3 characterPos = new Vector3(character.X, character.Y, character.Z);
                if (Vector3.Distance(position, characterPos) <= range)
                {
                    characters.Add(character);
                }
            }
            return characters;
        }
    }

    public class MapPosition
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public byte Heading { get; set; }

        public MapPosition(float Xpos = 0, float Ypos = 0, float Zpos = 0, byte heading = 0)
        {
            X = Xpos;
            Y = Ypos;
            Z = Zpos;
            Heading = heading;
        }
    }
}
