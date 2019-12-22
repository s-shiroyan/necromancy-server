using Arrowgene.Services.Buffers;
using Arrowgene.Services.Tasks;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;
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

    class MonsterTask : PeriodicTask
    {
        protected NecServer Server { get; }
        protected PacketRouter Router { get; }
        public MonsterSpawn _monster { get; set; }
        public bool monsterFreeze { get; set; }
        public bool monsterActive { get; set; }
        public bool monsterMoving { get; set; }
        public bool hateOn { get; set; }
        private bool casting;
        private bool battlePose;
        private bool monsterAgro;
        private bool monsterWaiting;
        private bool spawnMonster;
        public int gotoDistance { get; set; }
        public int agroRange { get; set; }

        //private int currentDest;
        private int moveTime;
        private int updateTime;
        private int waitTime;
        private int pathingTick;
        private int agroTick;
        //private int monsterVelocity;
        private int respawnTime;
        private int agroCheckTime;
        private int currentWait;
        private float fovAngle;
        private Map Map;
        private int CastState;
        private static int[] skillList = new[] { 301411, 301412, 301413, 301414, 301415, 301416, 301417, 301418, 301419, 301420 };
        private int currentSkill;
        private int skillInstanceId;
        private Character currentTarget;

        public MonsterCoord monsterHome;
        public MonsterTask(NecServer server, MonsterSpawn monster)
        {
            _monster = monster;
            Server = server;
            Router = Server.Router;
            monsterFreeze = false;
            monsterActive = true;
            monsterMoving = false;
            hateOn = false;
            battlePose = false;
            casting = false;
            spawnMonster = true;
            monsterHome = null;
            _monster.CurrentCoordIndex = 1;
            pathingTick = 100;
            agroTick = 500;
            updateTime = pathingTick;
            waitTime = 2000;
            currentWait = Util.GetRandomNumber(0,500);
            currentWait = 0;
            moveTime = updateTime;
            monsterAgro = false;
            monsterWaiting = true;
            agroRange = 1000;
            agroCheckTime = -1;
            fovAngle = (float)Math.Cos(Math.PI / 2);
            RunAtStart = false;
            Name = monster.Name;
            CastState = 0;
            respawnTime = 10000;
            currentTarget = null;
            currentSkill = 0;
            skillInstanceId = 0;
            Map = Server.Maps.Get(_monster.MapId);
        }

        public override string Name { get; }
        public override TimeSpan TimeSpan { get; }
        protected override bool RunAtStart { get; }
        protected override void Execute()
        {
            _monster.TaskActive = true;
            while (monsterActive && _monster.SpawnActive)
            {
                if (spawnMonster)
                {
                    Thread.Sleep(respawnTime/5);
                    updateTime = pathingTick;
                    _monster.CurrentCoordIndex = 1;
                    moveTime = updateTime;
                    MonsterSpawn();
                    //SendBattleReportStartNotify();
                    //MonsterBattlePose(false);
                    //SendBattleReportEndNotify();
                    Thread.Sleep(2000);
                }
                MonsterCoord nextCoord = _monster.monsterCoords.Find(x => x.CoordIdx == _monster.CurrentCoordIndex);
                Vector3 monster = new Vector3(_monster.X, _monster.Y, _monster.Z);
                float distance = GetDistance(nextCoord.destination, monster);
                if (monsterAgro)
                {
                    if (MonsterAgro())
                        continue;
                }
                else
                { 
                    MonsterPath();
                }
                Thread.Sleep(moveTime);
                if (monsterWaiting)
                {
                    currentWait += updateTime;
                    if (currentWait >= waitTime)
                    {
                        monsterWaiting = false;
                        currentWait = 0;
                    }
                }
            }
            this.Stop();
            _monster.TaskActive = false;
        }

        private void MonsterPath()
        {
            MonsterCoord nextCoord = _monster.monsterCoords.Find(x => x.CoordIdx == _monster.CurrentCoordIndex);
            Vector3 monster = new Vector3(_monster.X, _monster.Y, _monster.Z);
            float distance = GetDistance(nextCoord.destination, monster);
            if (distance > gotoDistance && !monsterFreeze && !monsterWaiting && !monsterAgro)
            {
                 MonsterMove(nextCoord);
            }
            else if (monsterMoving)
            {
                //Thread.Sleep(updateTime/2); //Allow for cases where the remaining distance is less than the gotoDistance
                _monster.MonsterStop(Server,1,0,1.0F);
                monsterMoving = false;
                if (!monsterAgro)
                {
                    monsterWaiting = true;
                    currentWait = 0;
                    //                        Thread.Sleep(2000);
                    if (_monster.CurrentCoordIndex < _monster.monsterCoords.Count - (_monster.defaultCoords ? 1 : 2))
                        _monster.CurrentCoordIndex++;
                    else
                        _monster.CurrentCoordIndex = 0;

                    _monster.Heading = (byte)GetHeading(_monster.monsterCoords.Find(x => x.CoordIdx == _monster.CurrentCoordIndex).destination);
                }
            }
            monsterAgro = MonsterAgroCheck();
            if (monsterAgro)
            {
                monsterMoving = false;
                _monster.MonsterStop(Server, 1,0, 0.1F);
                MonsterHate(true, (int)currentTarget.InstanceId);
                SendBattlePoseStartNotify();
                updateTime = agroTick;
                if (_monster.Id == 4)
                    gotoDistance = 1000;
                else
                    gotoDistance = 300; 
                //monsterVelocity = 500;
                moveTime = agroTick;
                agroCheckTime = 0;

                orientMonster();
                MonsterAgroMove();

            }
        }
        private bool MonsterAgro()
        {
            Vector3 monster = new Vector3(_monster.X, _monster.Y, _monster.Z);
            if (currentTarget == null)
            {
                Logger.Error($"No character target set for agroed monster");
                return true;
            }
            if (MonsterCheck())
            {
                Logger.Debug($"MonsterCheck returned true");
                return true;
            }
            float homeDistance = GetDistance(monsterHome.destination, monster);
            if (homeDistance >= (agroRange * 2))
            {
                RecvObjectDisappearNotify objectDisappearData = new RecvObjectDisappearNotify(_monster.InstanceId);
                Router.Send(Map, objectDisappearData);
                spawnMonster = true;
                Logger.Debug($"Too far from home");
                return true;
            }
            MonsterAgroAdjust();
            Vector3 character = new Vector3(currentTarget.X, currentTarget.Y, currentTarget.Z);
            Vector3 monsterPos = new Vector3(_monster.X, _monster.Y, _monster.Z);
            float distanceChar = GetDistance(character, monster);
            if (distanceChar <= gotoDistance)
            {
                if (monsterMoving)
                {
                    Thread.Sleep(updateTime/2);
                    monsterMoving = false;
                    _monster.MonsterStop(Server, 1,0, 0.1F);
                    Thread.Sleep(500);
                }
                if (!monsterWaiting)
                {
                    switch (CastState)
                    {
                        case 0:
                            orientMonster();
                            skillInstanceId = (int)Server.Instances.CreateInstance<Skill>().InstanceId;
                            //SendBattleReportStartNotify();
                            if (_monster.Id != 4)
                            {
                                MonsterAttackQueue(1);
                                //waitTime = Util.GetRandomNumber(3500,5000);
                                waitTime = 5000;
                                CastState = 0;
                            }
                            else if (_monster.Id == 4)
                            {
                                StartMonsterCast(200301411, skillInstanceId);
                                CastState = 1;
                                waitTime = 2000;
                            }
                            //Thread.Sleep(Util.GetRandomNumber(0,150)); // End Notifies can't fire at the same time.
                            //SendBattleReportEndNotify();
                            monsterWaiting = true;
                            currentWait = 0;
                            break;
                        case 1:
                            //int skillInstanceID = (int)Server.Instances.CreateInstance<Skill>().InstanceId;
                            SendBattleReportStartNotify();
                            MonsterCast(200301411);
                            SendBattleReportEndNotify();
                            monsterWaiting = true;
                            waitTime = 1500;
                            currentWait = 0;
                            CastState = 3;
                            break;
                        case 2:
                            //     Spell Onject Movement     <---------------------------FIX ME PLEASE!!!!!!!!!!!!
                            monsterWaiting = true;
                            waitTime = 1000;
                            currentWait = 0;
                            CastState = 3;
                            break;
                        case 3:
                            //int effectObjectInstanceId = (int)Server.Instances.CreateInstance<Skill>().InstanceId;
                            SendDataNotifyEoData(skillInstanceId, 301411);
                            SendEoNotifyDisappearSchedule(skillInstanceId);
                            //MonsterCastMove(skillInstanceId, 1000, 0, 0);
                            monsterWaiting = false;
                            waitTime = 1000;
                            currentWait = 0;
                            CastState = 4;
                            break;
                        case 4:
                            //     Spell Onject Movement has to be after the Effect object is created
                            //        public void MonsterCastMove(int instanceId, int castVelocity, byte pose, byte animation)
                            MonsterCastMove(skillInstanceId, 2000, 3, 0);
                            monsterWaiting = true;
                            waitTime = 5000;
                            currentWait = 0;
                            CastState = 0;
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                if (!casting && CheckHeading() == false)
                    orientMonster();
                MonsterAgroMove();
            }
            return false;
        }
        private void StartMonsterCast(int skillId, int instanceId)
        {
            casting = true;

            IBuffer res = BufferProvider.Provide();
            res = BufferProvider.Provide();
            res.WriteInt32(skillId);  // From skill_base.csv
            res.WriteInt32(instanceId);       //  ????????????????????
            res.WriteFloat(2.0F);                           //  ????????????????????
            Router.Send(Map, (ushort)AreaPacketId.recv_battle_report_action_monster_skill_start_cast, res, ServerType.Area);


        }
        private void StartMonsterCastQueue(int skillId, int instanceId)
        {
            casting = true;
            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify((int)_monster.InstanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionMonsterSkillStartCast brStartCast = new RecvBattleReportActionMonsterSkillStartCast((int)currentTarget.InstanceId, skillId);
            brList.Add(brStart);
            brList.Add(brStartCast);
            brList.Add(brEnd);
            Router.Send(Map, brList);

        }
        private void MonsterCast(int skillId)
        {
            casting = true;

            IBuffer res = BufferProvider.Provide();
            res = BufferProvider.Provide();
            res.WriteInt32(skillId);  // From skill_base.csv
            Router.Send(Map, (ushort)AreaPacketId.recv_battle_report_action_monster_skill_exec, res, ServerType.Area);
            //makes effect have a sphere of collision???

        }

        private void MonsterCastQueue(int skillId)
        {
            casting = true;
            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify((int)_monster.InstanceId);
            RecvBattleReportActionMonsterSkillExec brExec = new RecvBattleReportActionMonsterSkillExec(skillId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            brList.Add(brStart);
            brList.Add(brExec);
            brList.Add(brEnd);
            Router.Send(Map, brList);

        }
        private void MonsterAttackQueue(int skillId)
        {
            float perHp = (int)((currentTarget.currentHp / currentTarget.maxHp) * 100);

            int thisAttackId = _monster.SkillAttackId * 100 + Util.GetRandomNumber(1, 3); // most monsters have 1 to 3 melee attacks. ToDo Monster_attack.csv reader
            Logger.Debug($"Monster {_monster.InstanceId} is attacking with melee skill {thisAttackId}");
            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify((int)_monster.InstanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionAttackExec brAttack = new RecvBattleReportActionAttackExec(thisAttackId);
            RecvBattleReportNotifyHitEffect brHit = new RecvBattleReportNotifyHitEffect((int)currentTarget.InstanceId);
            RecvBattleReportDamageHp brHp = new RecvBattleReportDamageHp((int)currentTarget.InstanceId, Util.GetRandomNumber(8,43));
            RecvObjectHpPerUpdateNotify oHpUpdate = new RecvObjectHpPerUpdateNotify((int)currentTarget.InstanceId, perHp);
            brList.Add(brStart);
            brList.Add(brAttack);
            brList.Add(brHit);
            brList.Add(brHp);
            brList.Add(oHpUpdate);
            brList.Add(brEnd);
            Router.Send(Map, brList);

        }
        private void SendDataNotifyEoData(int instanceId, int effectId)
        {
            IBuffer res6 = BufferProvider.Provide();
            res6.WriteInt32(effectId);
            res6.WriteFloat(2.0F);
            //Router.Send(Map, (ushort)AreaPacketId.recv_eo_base_notify_sphere, res6, ServerType.Area);

            IBuffer res2 = BufferProvider.Provide();
            Logger.Debug($"Skill instance {instanceId} was just cast");
            res2.WriteInt32(instanceId); // Unique Instance ID of Skill Cast
            res2.WriteFloat(_monster.X);//Effect Object X
            res2.WriteFloat(_monster.Y);//Effect Object y
            res2.WriteFloat(_monster.Z +100);//Effect Object z    (+100 just so i can see it better for now)

            //orientation related  (Note,  i believe at least 1 of these values must be above 0 for "arrows" to render"
            res2.WriteFloat(1);//Rotation Along X Axis if above 0
            res2.WriteFloat(1);//Rotation Along Y Axis if above 0
            res2.WriteFloat(1);//Rotation Along Z Axis if above 0

            res2.WriteInt32(effectId);// effect id
            res2.WriteInt32(currentTarget.InstanceId); //must be set to int32 contents. int myTargetID = packet.Data.ReadInt32();
            res2.WriteInt32(1);//unknown

            res2.WriteInt32(1);
            Router.Send(Map, (ushort)AreaPacketId.recv_data_notify_eo_data, res2, ServerType.Area);
        }


        private void SendDataNotifyEoData2(int instanceId, int effectId)
        {
            IBuffer res2 = BufferProvider.Provide();
            Logger.Debug($"Skill instance {instanceId} was just cast");
            res2.WriteInt32(instanceId); // Unique Instance ID of Skill Cast
            res2.WriteInt32(effectId);
            res2.WriteFloat(currentTarget.X);//Effect Object X
            res2.WriteFloat(currentTarget.Y);//Effect Object y
            res2.WriteFloat(currentTarget.Z);//Effect Object z

            //orientation related
            res2.WriteFloat(0);//Rotation Along X Axis if above 0
            res2.WriteFloat(0);//Rotation Along Y Axis if above 0
            res2.WriteFloat(0);//Rotation Along Z Axis if above 0

            res2.WriteInt32(effectId);// effect id
            res2.WriteInt32(0); //must be set to int32 contents. int myTargetID = packet.Data.ReadInt32();
            res2.WriteInt32(0);//unknown

            res2.WriteInt32(0);
            res2.WriteInt32(0);
            res2.WriteInt32(0);
            res2.WriteInt32(0);
            Router.Send(Map, (ushort)AreaPacketId.recv_data_notify_eo_data2, res2, ServerType.Area);
        }
        private void SendDataNotifyItemObjectData(int instanceId, int effectId)
        {
            int objectInstanceId = (int)Server.Instances.CreateInstance<Skill>().InstanceId;
            Vector3 destPos = new Vector3(currentTarget.X, currentTarget.Y, currentTarget.Z);
            Vector3 monsterPos = new Vector3(_monster.X, _monster.Y, _monster.Z);
            Vector3 moveTo = Vector3.Subtract(destPos, monsterPos);
            float distance = Vector3.Distance(monsterPos, destPos);
            float travelTime = distance / 300;
            float xTick = moveTo.X / travelTime;
            float yTick = moveTo.Y / travelTime;
            float zTick = moveTo.Z / travelTime;

            IBuffer res = BufferProvider.Provide();
            res = BufferProvider.Provide();
            res.WriteInt32(effectId);

            res.WriteFloat(_monster.X);
            res.WriteFloat(_monster.Y);
            res.WriteFloat(_monster.Z);

            res.WriteFloat(currentTarget.X);
            res.WriteFloat(currentTarget.Y);
            res.WriteFloat(currentTarget.Z);
            res.WriteByte(_monster.Heading);

            res.WriteInt32(effectId);
            res.WriteInt32(effectId);
            res.WriteInt32(effectId);

            res.WriteInt32(1);
            res.WriteInt32(effectId);

            Router.Send(Map, (ushort)AreaPacketId.recv_data_notify_itemobject_data, res, ServerType.Area);

        }
        private void SendEoNotifyDisappearSchedule(int instanceId)
        {

            IBuffer res = BufferProvider.Provide();
            res = BufferProvider.Provide();
                    
            res.WriteInt32(instanceId);
            res.WriteFloat(6.0F);
            Router.Send(Map, (ushort)AreaPacketId.recv_eo_notify_disappear_schedule, res, ServerType.Area);

        }

        private void SendBattleAttackPose()
        {
            IBuffer res = BufferProvider.Provide();
            res = BufferProvider.Provide();
            res.WriteInt32(1410101);
            Router.Send(Map, (ushort)AreaPacketId.recv_battle_attack_pose_r, res, ServerType.Area);
        }
        private void SendBattleAttackStart()
        {
            IBuffer res = BufferProvider.Provide();
            res = BufferProvider.Provide();
            res.WriteInt32(currentTarget.InstanceId); 
            Router.Send(Map, (ushort)AreaPacketId.recv_battle_attack_start, res, ServerType.Area);
        }
        private void MonsterAttack()
        {
            int thisAttackId = _monster.SkillAttackId * 100 + Util.GetRandomNumber(1, 3); // most monsters have 1 to 3 melee attacks. ToDo Monster_attack.csv reader
            Logger.Debug($"Monster {_monster.InstanceId} is attacking with melee skill {thisAttackId}");
            IBuffer res = BufferProvider.Provide();
            res = BufferProvider.Provide();
            res.WriteInt32(thisAttackId); //From monster_attack.csv     
            Router.Send(Map, (ushort)AreaPacketId.recv_battle_report_action_attack_exec, res, ServerType.Area);
        }

        private void MonsterStateUpdateNotify()
        {
            IBuffer res = BufferProvider.Provide();
            res = BufferProvider.Provide();
            res.WriteInt32(_monster.InstanceId); //From monster attack
            res.WriteInt32(11300000); //From monster attack
            Router.Send(Map, (ushort)AreaPacketId.recv_monster_state_update_notify, res, ServerType.Area);
        }
        public void MonsterHate(bool hateOn, int instanceId)
        {
            if (this.hateOn != hateOn)
            {
                IBuffer res = BufferProvider.Provide();
                res.WriteInt32(_monster.InstanceId);
                res.WriteInt32(instanceId);
                if (hateOn)
                {
                    Router.Send(Map, (ushort)AreaPacketId.recv_monster_hate_on, res, ServerType.Area);
                }
                else
                {
                    Router.Send(Map, (ushort)AreaPacketId.recv_monster_hate_off, res, ServerType.Area);
                }
                this.hateOn = hateOn;
            }
        }
        public void MonsterSpawn()
        {
            monsterAgro = false;
            monsterMoving = false;
            casting = false;
            monsterWaiting = false;
            gotoDistance = 10;
            //monsterVelocity = 200;
            MonsterCoord spawnCoords = _monster.monsterCoords.Find(x => x.CoordIdx == 0);
            _monster.X = spawnCoords.destination.X; 
            _monster.Y = spawnCoords.destination.Y; 
            _monster.Z = spawnCoords.destination.Z; 
            _monster.Heading = (byte)GetHeading(_monster.monsterCoords.Find(x => x.CoordIdx == 1).destination); 
            _monster.CurrentHp = 100;
            respawnTime = _monster.RespawnTime;
            RecvDataNotifyMonsterData monsterData = new RecvDataNotifyMonsterData(_monster);
            Router.Send(Map, monsterData);
            spawnMonster = false;
            _monster.MonsterVisible = true;
            _monster.MonsterAgro.Clear();
            //MonsterBattlePose(false);
        }
        public bool MonsterCheck()
        {
            //Logger.Debug($"Monster HP [{_monster.CurrentHp}]");
            if (_monster.CurrentHp <= 0)
            {
                //SendBattleReportStartNotify();
                //Death Animation
                List<PacketResponse> brList = new List<PacketResponse>();
                RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify((int)_monster.InstanceId);
                RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
                RecvBattleReportNoactDead brDead = new RecvBattleReportNoactDead((int)_monster.InstanceId);
                brList.Add(brStart);
                brList.Add(brDead);
                brList.Add(brEnd);
                Router.Send(Map, brList);
                //IBuffer res5 = BufferProvider.Provide();
                //res5.WriteInt32(_monster.InstanceId);
                //res5.WriteInt32(1); //Death int
                //res5.WriteInt32(0);
                //res5.WriteInt32(0);
                //Router.Send(Map, (ushort)AreaPacketId.recv_battle_report_noact_notify_dead, res5, ServerType.Area);

                //SendBattleReportEndNotify();

                //Make the monster a lootable state
                IBuffer res10 = BufferProvider.Provide();
                res10.WriteInt32(_monster.InstanceId);
                res10.WriteInt32(2);//Toggles state between Alive(attackable),  Dead(lootable), or Inactive(nothing). 
                Router.Send(Map, (ushort)AreaPacketId.recv_monster_state_update_notify, res10, ServerType.Area);

                //  Let a separate loot manager handle the monster body click?
                Thread.Sleep(_monster.RespawnTime);
                //decompose the body
                IBuffer res7 = BufferProvider.Provide();
                res7.WriteInt32(_monster.InstanceId);
                res7.WriteInt32(5);//4 here causes a cloud and the model to disappear, 5 causes a mist to happen and disappear
                res7.WriteInt32(1);
                Router.Send(Map, (ushort)AreaPacketId.recv_charabody_notify_deadstate, res7, ServerType.Area);
                Thread.Sleep(2000);
                RecvObjectDisappearNotify objectDisappearData = new RecvObjectDisappearNotify(_monster.InstanceId);
                Router.Send(Map, objectDisappearData);
                _monster.MonsterVisible = false;

                spawnMonster = true;
                return true;
            }
            return false;
        }
        public void MonsterBattlePose(bool poseOn)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_monster.InstanceId);
            if (poseOn)
            {
                Router.Send(Map, (ushort)AreaPacketId.recv_battle_attack_pose_start_notify, res, ServerType.Area);
                battlePose = true;
            }
            else
            {
                Router.Send(Map, (ushort)AreaPacketId.recv_battle_attack_pose_end_notify, res, ServerType.Area);
                battlePose = false;
            }
        }
        private void MonsterMove(MonsterCoord monsterCoord)
        {

            //ShowVectorInfo(_monster.X, _monster.Y, monsterCoord.destination.X, monsterCoord.destination.Y);
            //ShowMonsterInfo();
            if (!monsterMoving)
            {
                monsterMoving = true;
                orientMonster();
                _monster.MonsterMove(Server,_monster.MonsterWalkVelocity, (byte)2, (byte)0);
            }
            else
            {
                Vector3 destPos = new Vector3(monsterCoord.destination.X, monsterCoord.destination.Y, monsterCoord.destination.Z);
                Vector3 monsterPos = new Vector3(_monster.X, _monster.Y, _monster.Z);
                Vector3 moveTo = Vector3.Subtract(destPos, monsterPos);
                float distance = Vector3.Distance(monsterPos, destPos);
                float travelTime = distance / _monster.MonsterWalkVelocity;
                float xTick = moveTo.X / travelTime;
                float yTick = moveTo.Y / travelTime;
                float zTick = moveTo.Z / travelTime;
                int tickDivisor = 1000 / updateTime;

                if (distance >= _monster.MonsterWalkVelocity / tickDivisor)
                {
                    _monster.X = _monster.X + (moveTo.X / travelTime) / tickDivisor;
                    _monster.Y = _monster.Y + (moveTo.Y / travelTime) / tickDivisor;
                    //_monster.Z = _monster.Z + (moveTo.Z / travelTime) / tickDivisor;
                }
                else
                {
                    _monster.X = destPos.X;
                    _monster.Y = destPos.Y;
                    _monster.Z = destPos.Z;
                }
                //Logger.Debug($"distance [{distance}] travelTime[{travelTime}] moveTo.X [{moveTo.X}] moveTo.Y [{moveTo.Y}] moveTo.Z [{moveTo.Z}]");
            }
            //            Logger.Debug($"distance [{distance}]");
            //            ShowMonsterInfo();
        }
        private void MonsterAgroMove()
        {
            Vector3 charPos = new Vector3(currentTarget.X, currentTarget.Y, currentTarget.Z);
            Vector3 monsterPos = new Vector3(_monster.X, _monster.Y, _monster.Z);
            Vector3 moveTo = Vector3.Subtract(charPos, monsterPos);

            float distance = Vector3.Distance(monsterPos, charPos);
            //Logger.Debug($"distance [{distance}]");
            ShowVectorInfo(_monster.X, _monster.Y, _monster.Z, currentTarget.X, currentTarget.Y, currentTarget.Z);
            if (distance <= gotoDistance)
            {
                if (monsterMoving)
                {
                    monsterMoving = false;
                    _monster.MonsterStop(Server, 1,0, 1.0F);
                }
                return;
            }
            //Logger.Debug($"moving [{moving}]");
            //Vector2 moveTo = GetVector(monsterSpawn.X, monsterSpawn.Y, client.Character.X, client.Character.Y);
            //ShowMonsterInfo(monsterSpawn);
            if (!monsterMoving)
                monsterMoving = true;


            int tickDivisor = 1000 / moveTime;
            float travelTime = (float)moveTime / 1000;
            MonsterTick tick = new MonsterTick();
            tick.xTick = (moveTo.X * travelTime);
            tick.yTick = (moveTo.Y * travelTime);
            tick.zTick = (moveTo.Z * travelTime);
            //Logger.Debug($"distance [{distance}] monsterVelocity [{_monster.MonsterRunVelocity}]  travelTime[{travelTime}] xTick [{tick.xTick}] yTick [{tick.yTick}] moveTo.X [{moveTo.X}] moveTo.Y [{moveTo.Y}] moveTo.Z [{moveTo.Z}]");
            _monster.MonsterMove(Server, (byte)3, (byte)0, tick, travelTime);

            _monster.X = _monster.X + tick.xTick;
            _monster.Y = _monster.Y + tick.yTick;
            _monster.Z = _monster.Z + tick.zTick;
        }

        public void MonsterCastMove(int instanceId, int castVelocity, byte pose, byte animation)
        {
            Vector3 destPos = new Vector3(currentTarget.X, currentTarget.Y, currentTarget.Z);
            Vector3 monsterPos = new Vector3(_monster.X, _monster.Y, _monster.Z);
            Vector3 moveTo = Vector3.Subtract(destPos, monsterPos);
            float distance = Vector3.Distance(monsterPos, destPos);
            float travelTime = distance / castVelocity;

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(instanceId);//Monster ID
            res.WriteFloat(_monster.X);
            res.WriteFloat(_monster.Y);
            res.WriteFloat(_monster.Z);
            res.WriteFloat(moveTo.X);       //X per tick
            res.WriteFloat(moveTo.Y);       //Y Per tick
            res.WriteFloat((float)1);              //verticalMovementSpeedMultiplier

            res.WriteFloat((float)1 / travelTime);              //movementMultiplier
            res.WriteFloat((float)travelTime);              //Seconds to move

            res.WriteByte(pose); //MOVEMENT ANIM
            res.WriteByte(animation);//JUMP & FALLING ANIM
            Server.Router.Send(Map, (ushort)AreaPacketId.recv_0x8D92, res, ServerType.Area);    //recv_0xE8B9  recv_0x1FC1 

        }

        private bool MonsterAgroCheck()
        {
           List<NecClient> mapsClients = Map.ClientLookup.GetAll();

            Vector3 monster = new Vector3(_monster.X, _monster.Y, _monster.Z);
            foreach (NecClient client in mapsClients)
            {
                Vector3 character = new Vector3(client.Character.X, client.Character.Y, client.Character.Z);
                float distanceChar = GetDistance(character, monster);
                if (distanceChar <= agroRange)
                {
                    if (checkFOV(client))
                    {
                        currentTarget = client.Character;
                        monsterAgro = true;
                        _monster.MonsterAgro.Add((int)client.Character.InstanceId, 0);
                    }
                }
            }

            return monsterAgro;
        }

        private void MonsterAgroAdjust()
        {
            if (agroCheckTime != -1 && agroCheckTime < 3000)
            {
                agroCheckTime += updateTime;
                return;
            }
            int currentInstance = _monster.MonsterAgro.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            if (currentTarget.InstanceId != currentInstance)
            {
                currentTarget = Map.ClientLookup.GetByCharacterInstanceId((uint)currentInstance).Character;
            }
            agroCheckTime = 0;
        }
        private bool checkFOV(NecClient client)
        {
            Vector2 target = new Vector2(client.Character.X, client.Character.Y);
            Vector2 source = new Vector2(_monster.X, _monster.Y);
            Vector2 targetVector = Vector2.Normalize(source - target);
            double sourceRadian = ConvertToRadians(_monster.Heading);
            Vector2 sourceVector = new Vector2((float)Math.Cos(sourceRadian), (float)Math.Sin(sourceRadian));
            sourceVector = Vector2.Normalize(sourceVector);
            float dotProduct = Vector2.Dot(sourceVector, targetVector);
            //Logger.Debug($"sourceVector.X[{sourceVector.X}] sourceVector.Y[{sourceVector.Y}]");
            if (dotProduct > fovAngle)
                Logger.Debug($"Monster {_monster.Name} sees you!!");
            //else
            //Logger.Debug($"Monster is oblivious dotProduct [{dotProduct}] fovAngle [{fovAngle}]");
            return dotProduct > fovAngle;
        }
        private double ConvertToRadians(double angle)
        {
            angle = angle * 2;
            angle = (angle <= 90 ? angle + 270 : angle - 90);
            //direction < 270 ? (direction + 90) / 2 : (direction - 270) / 2;
            return (Math.PI / 180) * angle;
        }

        private void orientMonster()
        {
            if (monsterAgro)
                AdjustHeading();

            _monster.MonsterOrient(Server);
        }
        private float GetDistance(Vector3 target, Vector3 source)
        {
            return Vector3.Distance(target, source);
        }

        private void ShowVectorInfo(double targetX, double targetY, double targetZ, double objectX, double objectY, double objectZ)
        {
            Vector3 target = new Vector3((float)targetX, (float)targetY, (float)targetZ);
            Vector3 source = new Vector3((float)objectX, (float)objectY, (float)objectZ);
            Vector3 moveTo = Vector3.Subtract(target, source);
            float distance = Vector3.Distance(target, source);
            double dx = objectX - targetX;
            double dy = objectY - targetY;
            double dz = objectZ - targetZ;
            Logger.Debug($"dx [{dx}]   dy[{dy}]  dz[{dz}]   distance [{distance}] moveTo.X [{moveTo.X}]  moveTo.Y [{moveTo.Y}]  moveTo.Z [{moveTo.Z}]");
        }

        private void ShowMonsterInfo()
        {
            Logger.Debug($"monster [{_monster.Name}]    X[{_monster.X}]   Y [{_monster.Y}] monster.Z [{_monster.Z}]  Heading [{_monster.Heading}]");
        }

        private double GetHeading(Vector3 destination) // Will return heading for x2/y2 object to look at x1/y1 object
        {
            double dx = _monster.X - destination.X;
            double dy = _monster.Y - destination.Y;
            double direction = (Math.Atan2(dy, dx) / System.Math.PI) * 180f; ;
            if (direction < 0) direction += 360f;
            direction = direction < 270 ? (direction + 90) / 2 : (direction - 270) / 2;
            return direction;
        }
        private void AdjustHeading()
        {
            double dx = _monster.X - currentTarget.X;
            double dy = _monster.Y - currentTarget.Y;
            double direction = (Math.Atan2(dy, dx) / System.Math.PI) * 180f; ;
            if (direction < 0) direction += 360f;
            direction = direction < 270 ? (direction + 90) / 2 : (direction - 270) / 2;
            //Logger.Debug($"direction after [{direction}]");
            _monster.Heading = (byte)direction;
        }
        private bool CheckHeading() // Will return heading for x2/y2 object to look at x1/y1 object
        {
            double dx = _monster.X - currentTarget.X;
            double dy = _monster.Y - currentTarget.Y;
            double direction = (Math.Atan2(dy, dx) / System.Math.PI) * 180f; ;
            if (direction < 0) direction += 360f;
            direction = direction < 270 ? (direction + 90) / 2 : (direction - 270) / 2;
            //Logger.Debug($"direction after [{direction}]");
            return _monster.Heading == (byte)direction;
        }
        private void SendBattleReportStartNotify()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_monster.InstanceId);
            Router.Send(Map, (ushort)AreaPacketId.recv_battle_report_start_notify, res, ServerType.Area);
        }
        private void SendBattleReportEndNotify()
        {
            IBuffer res = BufferProvider.Provide();
            Router.Send(Map, (ushort)AreaPacketId.recv_battle_report_end_notify, res, ServerType.Area);
        }
        private void SendBattlePoseStartNotify()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_monster.InstanceId);
            Router.Send(Map, (ushort)AreaPacketId.recv_battle_attack_pose_start_notify, res, ServerType.Area);
        }
        private void SendBattlePoseEndNotify()
        {
            IBuffer res = BufferProvider.Provide();
            Router.Send(Map, (ushort)AreaPacketId.recv_battle_attack_pose_end_notify, res, ServerType.Area);
        }
    }
}
