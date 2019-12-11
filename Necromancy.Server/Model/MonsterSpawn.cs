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
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public List<MonsterCoord> monsterCoords;

        public MonsterSpawn()
        {
            CurrentHp = 1000;
            MaxHp = 50000;
            RespawnTime = 60000;
            SpawnActive = false;
            Created = DateTime.Now;
            Updated = DateTime.Now;
            monsterCoords = new List<MonsterCoord>();
            Heading = 0;

            //To-Do   add at least 1 default monster coord for /mon spawns
            Vector3 defaultVector3 = new Vector3(X,Y,Z); 
            MonsterCoord defaultCoord = new MonsterCoord();
            defaultCoord.Id = Id;
            defaultCoord.MonsterId = (uint)MonsterId;
            defaultCoord.MapId = (uint)MapId;
            defaultCoord.destination = defaultVector3;

            monsterCoords.Add(defaultCoord);

            //To-Do Next.  make a default heading for _monster.Heading = (byte)GetHeading(_monster.monsterCoords[1].destination);

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
