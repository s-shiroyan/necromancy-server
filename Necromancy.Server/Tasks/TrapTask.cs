using Arrowgene.Services.Buffers;
using Arrowgene.Services.Tasks;
using Necromancy.Server.Common;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Model;
using Necromancy.Server.Model.Skills;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;
using Necromancy.Server.Packet.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;


namespace Necromancy.Server.Tasks
{
    // Usage: create a monster and spawn it then use the following
    //MonsterThread monsterThread = new MonsterThread(Server, client, monsterSpawn);
    //Thread workerThread2 = new Thread(monsterThread.InstanceMethod);
    //workerThread2.Start();

    class TrapTask : PeriodicTask
    {
        protected NecServer Server { get; }
        private Dictionary<int,IInstance> TrapList;
        private Vector3 TrapPos;
        private DateTime expireTime;
        private Map _map;
        private int triggerRadius;
        private int detectRadius;
        private int detectHeight;
        private int tickTime;

        public TrapTask(NecServer server, Map map, Vector3 trapPos, int activeTimeMs)
        {
            Server = server;
            TrapList = new Dictionary<int,IInstance>();
            TrapPos = trapPos;
            TimeSpan activeTime = new TimeSpan(0, 0, 0, 0, activeTimeMs);
            expireTime = DateTime.Now + activeTime;
            _map = map;
            triggerRadius = 120;
            detectHeight = 25;
            detectRadius = 1000;
            tickTime = 400;
        }

        public override string Name { get; }
        public override TimeSpan TimeSpan { get; }
        protected override bool RunAtStart { get; }
        protected override void Execute()
        {
            while (expireTime > DateTime.Now)
            {
                List<MonsterSpawn> monsters = _map.GetMonstersRange(TrapPos, detectRadius);
                if (monsters.Count > 0)
                {
                    bool triggered = false;
                    foreach (MonsterSpawn monster in monsters)
                    {
                        Vector3 monsterPos = new Vector3(monster.X, monster.Y, monster.Z);
                        if (Vector3.Distance(monsterPos, TrapPos) <= triggerRadius && (monsterPos.Z - TrapPos.Z <= detectHeight))
                        {
                            RecvDataNotifyEoData eoTriggerData = new RecvDataNotifyEoData((int)TrapList[0].InstanceId, (int)monsters[0].InstanceId, 1430212, TrapPos);
                            Server.Router.Send(_map, eoTriggerData);
                            triggered = true;
                        }
                    }
                    if (!triggered)
                        tickTime = 50;
                }
                Thread.Sleep(tickTime);
            }

            this.Stop();
        }

        public void AddTrap(int trapNum, IInstance trap)
        {
            TrapList.Add(trapNum,trap);
        }
        public void UpdateTrapTime(int activeTimeMs)
        {
            TimeSpan activeTime = new TimeSpan(0, 0, 0, 0, activeTimeMs);
            expireTime = DateTime.Now + activeTime;
        }
        private double ConvertToRadians(double angle, bool adjust)
        {
            angle = angle * 2;
            if (adjust)
                angle = (angle <= 90 ? angle + 270 : angle - 90);
            //direction < 270 ? (direction + 90) / 2 : (direction - 270) / 2;
            return (Math.PI / 180) * angle;
        }
    }
}
