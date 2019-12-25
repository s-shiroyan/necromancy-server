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
        protected NecServer _server { get; }
        private Dictionary<int,IInstance> TrapList;
        private Vector3 TrapPos;
        private DateTime expireTime;
        private Map _map;
        private int ownerInstanceId;
        private int triggerRadius;
        private int detectRadius;
        private int detectHeight;
        private int tickTime;
        private bool triggered;
        public TrapTask(NecServer server, Map map, Vector3 trapPos, int instanceId, int activeTimeMs)
        {
            _server = server;
            TrapList = new Dictionary<int,IInstance>();
            TrapPos = trapPos;
            TimeSpan activeTime = new TimeSpan(0, 0, 0, 0, activeTimeMs);
            expireTime = DateTime.Now + activeTime;
            _map = map;
            triggerRadius = 120;
            detectHeight = 25;
            detectRadius = 1000;
            tickTime = 400;
            triggered = false;
            ownerInstanceId = instanceId;
        }

        public override string Name { get; }
        public override TimeSpan TimeSpan { get; }
        protected override bool RunAtStart { get; }
        protected override void Execute()
        {
            while (expireTime > DateTime.Now && !triggered)
            {
                List<MonsterSpawn> monsters = _map.GetMonstersRange(TrapPos, detectRadius);
                if (monsters.Count > 0)
                {
                    foreach (MonsterSpawn monster in monsters)
                    {
                        Vector3 monsterPos = new Vector3(monster.X, monster.Y, monster.Z);
                        if ((Vector3.Distance(monsterPos, TrapPos) <= triggerRadius) && (monsterPos.Z - TrapPos.Z <= detectHeight))
                        {
                            triggered = true;
                            break;
                        }
                    }
                    if (!triggered)
                        tickTime = 50;
                    else
                    {
                        foreach (MonsterSpawn monster in monsters)
                        {
                            Vector3 monsterPos = new Vector3(monster.X, monster.Y, monster.Z);
                            if (Vector3.Distance(monsterPos, TrapPos) <= triggerRadius + 150)
                            {
                                int damage = Util.GetRandomNumber(70, 90);
                                RecvDataNotifyEoData eoTriggerData = new RecvDataNotifyEoData((int)TrapList[0].InstanceId, (int)monsters[0].InstanceId, 1430212, TrapPos, 2, 2);
                                _server.Router.Send(_map, eoTriggerData);
                                float perHp = (((float)monster.GetHP() / (float)monster.MaxHp) * 100);
                                List<PacketResponse> brList = new List<PacketResponse>();
                                RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify((int)TrapList[0].InstanceId);
                                RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
                                RecvBattleReportActionAttackExec brAttack = new RecvBattleReportActionAttackExec((int)monster.InstanceId);
                                RecvBattleReportNotifyHitEffect brHit = new RecvBattleReportNotifyHitEffect((int)monster.InstanceId);
                                RecvBattleReportPhyDamageHp brPhyHp = new RecvBattleReportPhyDamageHp((int)monster.InstanceId, damage);
                                RecvObjectHpPerUpdateNotify oHpUpdate = new RecvObjectHpPerUpdateNotify((int)monster.InstanceId, perHp);
                                RecvBattleReportDamageHp brHp = new RecvBattleReportDamageHp((int)monster.InstanceId, damage);

                                brList.Add(brStart);
                                brList.Add(brAttack);
                                brList.Add(brHit);
                                brList.Add(brPhyHp);
                                brList.Add(brHp);
                                brList.Add(oHpUpdate);
                                brList.Add(brEnd);
                                _server.Router.Send(_map, brList);
                                if (monster.GetAgroCharacter(ownerInstanceId))
                                {
                                    monster.UpdateHP(-damage);
                                }
                                else
                                {
                                    monster.UpdateHP(-damage, _server, true, ownerInstanceId);
                                }
                            }
                        }
                    }
                }
                Thread.Sleep(tickTime);
            }
            RecvDataNotifyEoData eoDestroyData = new RecvDataNotifyEoData((int)TrapList[0].InstanceId, (int)TrapList[0].InstanceId, 0, TrapPos, 0, 0);
            _server.Router.Send(_map, eoDestroyData);
            RecvEoNotifyDisappearSchedule eoDisappear = new RecvEoNotifyDisappearSchedule((int)TrapList[0].InstanceId, 0.0F);
            _server.Router.Send(_map, eoDisappear);

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
