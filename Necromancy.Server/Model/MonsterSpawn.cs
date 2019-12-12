using System;
using System.Collections.Generic;
using System.Numerics;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model
{
    public class MonsterSpawn : IInstance
    {
        public uint InstanceId { get; set; }
        public int Id { get; set; }
        public int MonsterId { get; set; }
        public int ModelId { get; set; }
        public byte Level { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int MapId { get; set; }
        public bool Active { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public byte Heading { get; set; }
        public short Size { get; set; }
        public short Radius { get; set; }
        public int CurrentHp { get; set; }
        public int MaxHp { get; set; }
        public int RespawnTime { get; set; }
        public bool SpawnActive { get; set; }
        public bool TaskActive { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public List<MonsterCoord> monsterCoords;
        public bool defaultCoords { get; set; }
        public Dictionary<int, int> MonsterAgro { get; set; }


        public MonsterSpawn()
        {
            CurrentHp = 1000;
            MaxHp = 50000;
            RespawnTime = 6000;//60000
            SpawnActive = false;
            TaskActive = false;
            defaultCoords = true;
            Created = DateTime.Now;
            Updated = DateTime.Now;
            monsterCoords = new List<MonsterCoord>();
            MonsterAgro = new Dictionary<int, int>();

        }
    }

    public class MonsterCoord 
    {
        public int Id;
        public uint MonsterId { get; set; }
        public uint MapId { get; set; }
        public int CoordIdx { get; set; }
        public Vector3 destination { get; set; }

    }
}
