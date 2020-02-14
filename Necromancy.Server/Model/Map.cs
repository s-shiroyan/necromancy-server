using System;
using System.Collections.Generic;
using System.Numerics;
using Arrowgene.Services.Logging;
using Arrowgene.Services.Tasks;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Model.Skills;
using Necromancy.Server.Packet.Receive;
using Necromancy.Server.Packet.Response;
using Necromancy.Server.Tasks;

namespace Necromancy.Server.Model
{
    public class Map
    {
        private readonly object TrapLock = new object();

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
        public Dictionary<uint, NpcSpawn> NpcSpawns { get; }
       // public Dictionary<int, TrapTransition> Trap { get; }
        public Dictionary<uint, MonsterSpawn> MonsterSpawns { get; }
        public Dictionary<uint, TrapStack> Traps { get; }
        public Dictionary<uint, DeadBody> DeadBodies { get; }

        public Map(MapSetting setting, NecServer server)
        {
            _server = server;
            _logger = LogProvider.Logger<NecLogger>(this);
            ClientLookup = new ClientLookup();
            NpcSpawns = new Dictionary<uint, NpcSpawn>();
            MonsterSpawns = new Dictionary<uint, MonsterSpawn>();
            Id = setting.Id;
            X = setting.X;
            Y = setting.Y;
            Z = setting.Z;
            Country = setting.Country;
            Area = setting.Area;
            Place = setting.Place;
            Orientation = (byte)(setting.Orientation/2);   // Client uses 180 degree orientation
            Traps = new Dictionary<uint, TrapStack>();

            //Assign Unique Instance ID to each NPC per map. Add to dictionary stored with the Map object
            List<NpcSpawn> npcSpawns = server.Database.SelectNpcSpawnsByMapId(setting.Id);
            foreach (NpcSpawn npcSpawn in npcSpawns)
            {
                server.Instances.AssignInstance(npcSpawn);
                NpcSpawns.Add(npcSpawn.InstanceId, npcSpawn);
            }

            //To-Do   | for each deadBody in Deadbodies {RecvDataNotifyCharabodyData} 

            List<MonsterSpawn> monsterSpawns = server.Database.SelectMonsterSpawnsByMapId(setting.Id);
            foreach (MonsterSpawn monsterSpawn in monsterSpawns)
            {
                server.Instances.AssignInstance(monsterSpawn);
                if (!_server.SettingRepository.ModelCommon.TryGetValue(monsterSpawn.ModelId, out ModelCommonSetting modelSetting))
                {
                    _logger.Error($"Error getting ModelCommonSetting for ModelId {monsterSpawn.ModelId}");
                    continue;
                }
                if (!_server.SettingRepository.Monster.TryGetValue(monsterSpawn.MonsterId, out MonsterSetting monsterSetting))
                {
                    _logger.Error($"Error getting MonsterSetting for MonsterId {monsterSpawn.MonsterId}");
                    continue;
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
                MonsterSpawns.Add(monsterSpawn.InstanceId, monsterSpawn);

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
            if (client.Union != null)
            {
                RecvDataNotifyUnionData myUnionData = new RecvDataNotifyUnionData(client.Character, client.Union.Name);
                _server.Router.Send(this, myUnionData, client);
            }

            foreach (MonsterSpawn monsterSpawn in this.MonsterSpawns.Values)
            {
                if (!monsterSpawn.Active)
                {
                    monsterSpawn.SpawnActive = true;
                    if (!monsterSpawn.TaskActive)
                    {
                        MonsterTask monsterTask = new MonsterTask(_server, monsterSpawn);
                        if (monsterSpawn.defaultCoords)
                            monsterTask.monsterHome = monsterSpawn.monsterCoords[0];
                        else
                            monsterTask.monsterHome = monsterSpawn.monsterCoords.Find(x => x.CoordIdx == 64);
                        monsterTask.Start();
                    }
                    else
                    {
                        if (monsterSpawn.MonsterVisible)
                        {
                            _logger.Debug($"MonsterTask already running for [{monsterSpawn.Name}]");
                            RecvDataNotifyMonsterData monsterData = new RecvDataNotifyMonsterData(monsterSpawn);
                            _server.Router.Send(monsterData, client);
                            if (!monsterSpawn.GetAgro())
                            {
                                monsterSpawn.MonsterMove(_server, client, monsterSpawn.MonsterWalkVelocity, (byte)2, (byte)0);
                            }
                        }
                    }
                }
            }
        }

        public void Leave(NecClient client)
        {
            _logger.Info(client, $"Leaving Map: {Id}:{FullName}", client);
            ClientLookup.Remove(client);
            if (!_server.Database.UpdateCharacter(client.Character))
            {
                _logger.Error("Could not update the database with last known player position");
            }
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

        public bool MonsterInRange(Vector3 position, int range)
        {
            foreach (MonsterSpawn monster in MonsterSpawns.Values)
            {
                Vector3 monsterPos = new Vector3(monster.X, monster.Y, monster.Z);
                if (Vector3.Distance(position, monsterPos) <= range)
                {
                    return true;
                }
            }
            return false;
        }

        public MonsterSpawn GetMonsterByInstanceId(uint instanceId)
        {
            foreach (MonsterSpawn monster in MonsterSpawns.Values)
            {
                if (monster.InstanceId == instanceId)
                {
                    return monster;
                }
            }
            return null;
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

        public List<TrapStack> GetTraps()
        {
            List<TrapStack> traps = new List<TrapStack>();
            lock (TrapLock)
            {
                foreach (TrapStack trap in Traps.Values)
                {
                    traps.Add(trap);
                }
            }
            return traps;
        }
        public List<TrapStack> GetTrapsCharacter(uint characterInstanceId)
        {
            List<TrapStack> traps = new List<TrapStack>();
            lock (TrapLock)
            {
                foreach (TrapStack trap in Traps.Values)
                {
                    if (trap._trapTask.ownerInstanceId == characterInstanceId)
                    {
                        traps.Add(trap);
                    }
                }
            }
            return traps;
        }
        public bool GetTrapsCharacterRange(uint characterInstanceId, int range, Vector3 position)
        {
            bool inRange = false;
            lock (TrapLock)
            {
                foreach (TrapStack trap in Traps.Values)
                {
                    if (trap._trapTask.ownerInstanceId == characterInstanceId)
                    {
                        double distance = Vector3.Distance(trap._trapTask.TrapPos, position);
                        if (distance < range)
                            return true;
                    }
                }
            }
            return inRange;
        }
        public TrapStack GetTrapCharacterRange(uint characterInstanceId, int range, Vector3 position)
        {
            lock (TrapLock)
            {
                foreach (TrapStack trap in Traps.Values)
                {
                    if (trap._trapTask.ownerInstanceId == characterInstanceId)
                    {
                        double distance = Vector3.Distance(trap._trapTask.TrapPos, position);
                        if (distance < range)
                            return trap;
                    }
                }
            }
            return null;
        }
        public void AddTrap(uint instanceId, TrapStack trap)
        {
            lock (TrapLock)
            {
                Traps.Add(instanceId, trap);
            }
        }

        public void RemoveTrap(uint instanceId)
        {
            lock (TrapLock)
            {
                Traps.Remove(instanceId);
            }
        }

        public MonsterSpawn MonsterInRange(uint instanceId)
        {
            foreach (MonsterSpawn monster in MonsterSpawns.Values)
            {
                if (monster.InstanceId == instanceId)
                {
                    return monster;
                }
            }
            return null;
        }
    }
}
