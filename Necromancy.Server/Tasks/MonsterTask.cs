using Arrowgene.Services.Buffers;
using Arrowgene.Services.Tasks;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
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

    class MonsterTask : PeriodicTask
    {
        protected NecServer _server { get; }
        public MonsterSpawn _monster { get; set; }
        public bool monsterFreeze { get; set; }
        public bool monsterActive { get; set; }
        public bool monsterMoving { get; set; }
        private bool casting;
        private bool monsterWaiting;
        private bool spawnMonster;
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
        private static int[] skillList = new[] { 200301411, 200301412, 200301413, 200301414, 200301415, 200301416, 200301417 };
        private static int[] effectList = new[] { 301411, 301412, 301413, 301414, 301415, 301416, 301417 };
        private int currentSkill;
        private int skillInstanceId;
        
        public MonsterCoord monsterHome;
        public MonsterTask(NecServer server, MonsterSpawn monster)
        {
            _monster = monster;
            _server = server;
            monsterFreeze = false;
            monsterActive = true;
            monsterMoving = false;
            casting = false;
            spawnMonster = true;
            monsterHome = null;
            _monster.CurrentCoordIndex = 1;
            pathingTick = 100;
            agroTick = 500;
            updateTime = pathingTick;
            waitTime = 2000;
            currentWait = 0;
            moveTime = updateTime;
            monsterWaiting = true;
            agroRange = 1000;
            agroCheckTime = -1;
            fovAngle = (float)Math.Cos(Math.PI / 2);
            RunAtStart = false;
            Name = monster.Name;
            CastState = 0;
            respawnTime = 10000;
            currentSkill = 0;
            skillInstanceId = 0;
            Map = _server.Maps.Get(_monster.MapId);
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
                    Thread.Sleep(respawnTime/2);
                    updateTime = pathingTick;
                    _monster.CurrentCoordIndex = 1;
                    moveTime = updateTime;
                    MonsterSpawn();
                    Thread.Sleep(2000);
                }
                //MonsterCoord nextCoord = _monster.monsterCoords.Find(x => x.CoordIdx == _monster.CurrentCoordIndex);
                //Vector3 monster = new Vector3(_monster.X, _monster.Y, _monster.Z);
                //float distance = GetDistance(nextCoord.destination, monster);
                if (_monster.GetAgro())
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
            if (distance > _monster.GetGotoDistance() && !monsterFreeze && !monsterWaiting && !_monster.GetAgro())
            {
                 MonsterMove(nextCoord);
            }
            else if (monsterMoving)
            {
                //Thread.Sleep(updateTime/2); //Allow for cases where the remaining distance is less than the gotoDistance
                _monster.MonsterStop(_server,1,0,.1F);
                monsterMoving = false;
                if (!_monster.GetAgro())
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
            if (MonsterAgroCheck())
                _monster.SetAgro(true);
            if (_monster.GetAgro())
            {
                monsterMoving = false;
                _monster.MonsterStop(_server, 1,0, 0.1F);
                _monster.MonsterHate(_server, true, (int)_monster.GetCurrentTarget().InstanceId);
                _monster.SendBattlePoseStartNotify(_server);
                updateTime = agroTick;
                if (_monster.Id == 4)
                    _monster.SetGotoDistance(1000);
                else
                    _monster.SetGotoDistance(200);
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
            Character currentTarget = _monster.GetCurrentTarget();
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
                foreach (int instanceId in _monster.GetAgroInstanceList())
                {
                    _monster.MonsterHate(_server, false, instanceId);
                }
                RecvObjectDisappearNotify objectDisappearData = new RecvObjectDisappearNotify(_monster.InstanceId);
                _server.Router.Send(Map, objectDisappearData);
                spawnMonster = true;
                Logger.Debug($"Too far from home");
                return true;
            }
            MonsterAgroAdjust();
            Vector3 character = new Vector3(currentTarget.X, currentTarget.Y, currentTarget.Z);
            Vector3 monsterPos = new Vector3(_monster.X, _monster.Y, _monster.Z);
            float distanceChar = GetDistance(character, monster);
            if (distanceChar <= _monster.GetGotoDistance())
            {
                if (monsterMoving)
                {
                    Thread.Sleep(updateTime/2);
                    monsterMoving = false;
                    _monster.MonsterStop(_server, 1,0, 0.1F);
                    Thread.Sleep(500);
                }
                if (!monsterWaiting)
                {
                    List<PacketResponse> brList = new List<PacketResponse>();
                    RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify((int)_monster.InstanceId);
                    RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
                    switch (CastState)
                    {
                        case 0:
                            orientMonster();
                            //Logger.Debug($"MonsterId [{_monster.MonsterId}]");
                            skillInstanceId = (int)_server.Instances.CreateInstance<Skill>().InstanceId;
                            //SendBattleReportStartNotify();
                            if (_monster.MonsterId == 100104)
                            {
                                int attackId = (_monster.AttackSkillId * 100) + Util.GetRandomNumber(1, 3);
                                //Logger.Debug($"attackId [{attackId}]");
                                MonsterAttackQueue(attackId);
                                waitTime = 5000;
                                CastState = 0;
                            }
                            else if (_monster.MonsterId == 40101 || _monster.MonsterId == 40110)
                            {
                                int attackId = (_monster.AttackSkillId * 100) + (Util.GetRandomNumber(0, 100) < 51 ? 1 : 2);
                                //Logger.Debug($"_monster.AttackSkillId [{_monster.AttackSkillId}]  attackId[{attackId}]");
                                MonsterAttackQueue(attackId);
                                waitTime = 5000;
                                CastState = 0;
                            }
                            else //if (_monster.Id == 4)
                            {
                                Logger.Debug($"attackId [200301411]");
                                //_monster.MonsterStop(Server,8, 231, 2.0F);
                                StartMonsterCastQueue(200301411, skillInstanceId);
                                waitTime = 2000;
                                CastState = 1;
                            }
                            //SendBattleReportEndNotify();
                            monsterWaiting = true;
                            currentWait = 0;
                           break;
                        case 1:
                            //int skillInstanceID = (int)Server.Instances.CreateInstance<Skill>().InstanceId;

                            MonsterCastQueue(200301411);
                            monsterWaiting = true;
                            waitTime = 1000;
                            currentWait = 0;
                            CastState = 3;
                            break;
                        case 2:
                            monsterWaiting = true;
                            waitTime = 1000;
                            currentWait = 0;
                            CastState = 3;
                            break;
                        case 3:
                            //int effectObjectInstanceId = (int)Server.Instances.CreateInstance<Skill>().InstanceId;
                            SendDataNotifyEoData(skillInstanceId, 301411);
                            SendEoNotifyDisappearSchedule(skillInstanceId);
                            MonsterCastMove(skillInstanceId, 3000, 2, 2);
                            monsterWaiting = true;
                            waitTime = 5000;
                            currentWait = 0;
                            CastState = 0;
                            if (currentSkill < skillList.Length - 1)
                                currentSkill++;
                            else
                                currentSkill = 0;

                            break;
                        case 4:
                            //     Spell Onject Movement has to be after the Effect object is created
                            //        public void MonsterCastMove(int instanceId, int castVelocity, byte pose, byte animation)
                            //MonsterCastMove(skillInstanceId, 2000, 13, 0);
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
            Character currentTarget = _monster.GetCurrentTarget();

            IBuffer res = BufferProvider.Provide();
            res = BufferProvider.Provide();
            res.WriteInt32(skillId);  // From skill_base.csv
            res.WriteInt32(currentTarget.InstanceId);       //  ????????????????????
            res.WriteFloat(2.0F);                           //  ????????????????????
            _server.Router.Send(Map, (ushort)AreaPacketId.recv_battle_report_action_monster_skill_start_cast, res, ServerType.Area);


        }
        private void StartMonsterCastQueue(int skillId, int instanceId)
        {
            casting = true;
            Character currentTarget = _monster.GetCurrentTarget();
            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify((int)_monster.InstanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionMonsterSkillStartCast brStartCast = new RecvBattleReportActionMonsterSkillStartCast((int)currentTarget.InstanceId, skillId);
            brList.Add(brStart);
            brList.Add(brStartCast);
            brList.Add(brEnd);
            _server.Router.Send(Map, brList);

        }
        private void MonsterAttack()
        {
            IBuffer res = BufferProvider.Provide();
            res = BufferProvider.Provide();
            res.WriteInt32(10010401); //From monster attack      bare knuckles 10100202
            //res.WriteInt32(1410201); //From monster attack
            _server.Router.Send(Map, (ushort)AreaPacketId.recv_battle_report_action_attack_exec, res, ServerType.Area);
        }
        private void MonsterAttackQueue(int skillId)
        {
            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify((int)_monster.InstanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionAttackExec brAttack = new RecvBattleReportActionAttackExec(skillId);
            brList.Add(brStart);
            brList.Add(brAttack);
            brList.Add(brEnd);
            _server.Router.Send(Map, brList);

        }
        private void MonsterCast(int skillId)
        {
            casting = true;

            IBuffer res = BufferProvider.Provide();
            res = BufferProvider.Provide();
            res.WriteInt32(skillId);  // From skill_base.csv
            _server.Router.Send(Map, (ushort)AreaPacketId.recv_battle_report_action_monster_skill_exec, res, ServerType.Area);
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
            _server.Router.Send(Map, brList);

        }

        private void SendDataNotifyEoData(int instanceId, int effectId)
        {
            Character currentTarget = _monster.GetCurrentTarget();
            IBuffer res6 = BufferProvider.Provide();
            res6.WriteInt32(instanceId);
            res6.WriteFloat(2.0F);
            //_server.Router.Send(Map, (ushort)AreaPacketId.recv_eo_base_notify_sphere, res6, ServerType.Area);

            IBuffer res2 = BufferProvider.Provide();
            //Logger.Debug($"Skill instance {instanceId} was just cast");
            res2.WriteInt32(instanceId); // Unique Instance ID of Skill Cast
            res2.WriteFloat(currentTarget.X);//Effect Object X
            res2.WriteFloat(currentTarget.Y);//Effect Object y
            res2.WriteFloat(currentTarget.Z);//Effect Object z    (+100 just so i can see it better for now)

            //orientation related  (Note,  i believe at least 1 of these values must be above 0 for "arrows" to render"
            res2.WriteFloat(1);//Rotation Along X Axis if above 0
            res2.WriteFloat(1);//Rotation Along Y Axis if above 0
            res2.WriteFloat(1);//Rotation Along Z Axis if above 0

            res2.WriteInt32(effectId);// effect id
            res2.WriteInt32(currentTarget.InstanceId); //must be set to int32 contents. int myTargetID = packet.Data.ReadInt32();
            res2.WriteInt32(1);//unknown
            res2.WriteInt32(1);
            _server.Router.Send(Map, (ushort)AreaPacketId.recv_data_notify_eo_data, res2, ServerType.Area);
        }


        private void SendDataNotifyEoData2(int instanceId, int effectId)
        {
            Character currentTarget = _monster.GetCurrentTarget();
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
            _server.Router.Send(Map, (ushort)AreaPacketId.recv_data_notify_eo_data2, res2, ServerType.Area);
        }
        private void SendDataNotifyItemObjectDataTask(int instanceId, int effectId)
        {
            Character currentTarget = _monster.GetCurrentTarget();
            int objectInstanceId = (int)_server.Instances.CreateInstance<Skill>().InstanceId;
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
            res.WriteInt32(instanceId);

            res.WriteFloat(_monster.X);
            res.WriteFloat(_monster.Y);
            res.WriteFloat(_monster.Z);

            res.WriteFloat(currentTarget.X);
            res.WriteFloat(currentTarget.Y);
            res.WriteFloat(currentTarget.Z);
            res.WriteByte(_monster.Heading);

            res.WriteInt32(effectId);
            res.WriteInt32(1);
            res.WriteInt32(1);

            res.WriteInt32(1);
            res.WriteInt32(1);

            _server.Router.Send(Map, (ushort)AreaPacketId.recv_data_notify_itemobject_data, res, ServerType.Area);

        }
        public void MonsterCastMove(int instanceId, int castVelocity, byte pose, byte animation)
        {
            Character currentTarget = _monster.GetCurrentTarget();
            Vector3 destPos = new Vector3(currentTarget.X, currentTarget.Y, currentTarget.Z);
            Vector3 monsterPos = new Vector3(_monster.X, _monster.Y, _monster.Z);
            Vector3 moveTo = Vector3.Subtract(destPos, monsterPos);
            float distance = Vector3.Distance(monsterPos, destPos);
            float travelTime = distance / castVelocity;

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(instanceId);//Monster ID
            res.WriteFloat(_monster.X);
            res.WriteFloat(_monster.Y);
            res.WriteFloat(_monster.Z+75);
            res.WriteFloat(moveTo.X);       //X per tick
            res.WriteFloat(moveTo.Y);       //Y Per tick
            res.WriteFloat(moveTo.Z);              //verticalMovementSpeedMultiplier

            res.WriteFloat((float)1 / travelTime);              //movementMultiplier
            res.WriteFloat((float)travelTime);              //Seconds to move

            res.WriteByte(pose); //MOVEMENT ANIM
            res.WriteByte(animation);//JUMP & FALLING ANIM
            _server.Router.Send(Map, (ushort)AreaPacketId.recv_0x8D92, res, ServerType.Area);    //recv_0xE8B9  recv_0x1FC1 

        }
        private void SendEoNotifyDisappearSchedule(int instanceId)
        {

            IBuffer res = BufferProvider.Provide();
            res = BufferProvider.Provide();
                    
            res.WriteInt32(instanceId);
            res.WriteFloat(6.0F);
            _server.Router.Send(Map, (ushort)AreaPacketId.recv_eo_notify_disappear_schedule, res, ServerType.Area);

        }

        private void SendBattleAttackPose()
        {
            IBuffer res = BufferProvider.Provide();
            res = BufferProvider.Provide();
            res.WriteInt32(1410101);
            _server.Router.Send(Map, (ushort)AreaPacketId.recv_battle_attack_pose_r, res, ServerType.Area);
        }
        private void SendBattleAttackStart()
        {
            Character currentTarget = _monster.GetCurrentTarget();
            IBuffer res = BufferProvider.Provide();
            res = BufferProvider.Provide();
            res.WriteInt32(currentTarget.InstanceId); 
            _server.Router.Send(Map, (ushort)AreaPacketId.recv_battle_attack_start, res, ServerType.Area);
        }
        private void MonsterStateUpdateNotify()
        {
            IBuffer res = BufferProvider.Provide();
            res = BufferProvider.Provide();
            res.WriteInt32(_monster.InstanceId); //From monster attack
            res.WriteInt32(11300000); //From monster attack
            _server.Router.Send(Map, (ushort)AreaPacketId.recv_monster_state_update_notify, res, ServerType.Area);
        }
        public void MonsterSpawn()
        {
            _monster.SetAgro(false);
            monsterMoving = false;
            casting = false;
            monsterWaiting = false;
            _monster.SetGotoDistance(10);
            //monsterVelocity = 200;
            MonsterCoord spawnCoords = _monster.monsterCoords.Find(x => x.CoordIdx == 0);
            _monster.X = spawnCoords.destination.X; 
            _monster.Y = spawnCoords.destination.Y; 
            _monster.Z = spawnCoords.destination.Z; 
            _monster.Heading = (byte)GetHeading(_monster.monsterCoords.Find(x => x.CoordIdx == 1).destination);
            _monster.SetHP(100);
            respawnTime = _monster.RespawnTime;
            RecvDataNotifyMonsterData monsterData = new RecvDataNotifyMonsterData(_monster);
            _server.Router.Send(Map, monsterData);
            spawnMonster = false;
            _monster.MonsterVisible = true;
            _monster.ClearAgroList();
            //MonsterBattlePose(false);
        }
        public bool MonsterCheck()
        {
            Logger.Debug($"Monster HP [{_monster.GetHP()}]");
            if (_monster.GetHP() <= 0)
            {
                foreach (int instanceId in _monster.GetAgroInstanceList())
                {
                    _monster.MonsterHate(_server, false, instanceId);
                }
                //Death Animation
                List<PacketResponse> brList = new List<PacketResponse>();
                RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify((int)_monster.InstanceId);
                RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
                RecvBattleReportNoactDead brDead = new RecvBattleReportNoactDead((int)_monster.InstanceId);
                brList.Add(brStart);
                brList.Add(brDead);
                brList.Add(brEnd);
                _server.Router.Send(Map, brList);

                //Make the monster a lootable state
                IBuffer res10 = BufferProvider.Provide();
                res10.WriteInt32(_monster.InstanceId);
                res10.WriteInt32(2);//Toggles state between Alive(attackable),  Dead(lootable), or Inactive(nothing). 
                _server.Router.Send(Map, (ushort)AreaPacketId.recv_monster_state_update_notify, res10, ServerType.Area);

                //  Let a separate loot manager handle the monster body click?
                Thread.Sleep(_monster.RespawnTime);
                //decompose the body
                IBuffer res7 = BufferProvider.Provide();
                res7.WriteInt32(_monster.InstanceId);
                res7.WriteInt32(5);//4 here causes a cloud and the model to disappear, 5 causes a mist to happen and disappear
                res7.WriteInt32(1);
                _server.Router.Send(Map, (ushort)AreaPacketId.recv_charabody_notify_deadstate, res7, ServerType.Area);
                Thread.Sleep(2000);
                RecvObjectDisappearNotify objectDisappearData = new RecvObjectDisappearNotify(_monster.InstanceId);
                _server.Router.Send(Map, objectDisappearData);
                _monster.MonsterVisible = false;

                spawnMonster = true;
                return true;
            }
            return false;
        }
        private void MonsterMove(MonsterCoord monsterCoord)
        {

            //ShowVectorInfo(_monster.X, _monster.Y, monsterCoord.destination.X, monsterCoord.destination.Y);
            //ShowMonsterInfo();
            if (!monsterMoving)
            {
                monsterMoving = true;
                orientMonster();
                _monster.MonsterMove(_server, _monster.MonsterWalkVelocity, (byte)2, (byte)0);
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
            Character currentTarget = _monster.GetCurrentTarget();
            Vector3 charPos = new Vector3(currentTarget.X, currentTarget.Y, currentTarget.Z);
            Vector3 monsterPos = new Vector3(_monster.X, _monster.Y, _monster.Z);
            Vector3 moveTo = Vector3.Subtract(charPos, monsterPos);

            float distance = Vector3.Distance(monsterPos, charPos);
            //Logger.Debug($"distance [{distance}]");
            ShowVectorInfo(_monster.X, _monster.Y, _monster.Z, currentTarget.X, currentTarget.Y, currentTarget.Z);
            if (distance <= _monster.GetGotoDistance())
            {
                if (monsterMoving)
                {
                    monsterMoving = false;
                    _monster.MonsterStop(_server, 1,0, 1.0F);
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
            _monster.MonsterMove(_server, (byte)3, (byte)0, tick, travelTime);

            _monster.X = _monster.X + tick.xTick;
            _monster.Y = _monster.Y + tick.yTick;
            _monster.Z = _monster.Z + tick.zTick;
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
                        _monster.SetCurrentTarget(client.Character);
                        _monster.SetAgro(true);
                        _monster.AddAgroList((int)client.Character.InstanceId, 0);
                    }
                }
            }

            return _monster.GetAgro();
        }

        private void MonsterAgroAdjust()
        {
            Character currentTarget = _monster.GetCurrentTarget();
            if (agroCheckTime != -1 && agroCheckTime < 3000)
            {
                agroCheckTime += updateTime;
                return;
            }
            int currentInstance = _monster.GetAgroHigh();
            if (currentTarget.InstanceId != currentInstance)
            {
                currentTarget = Map.ClientLookup.GetByCharacterInstanceId((uint)currentInstance).Character;
            }
            agroCheckTime = 0;
        }
        private bool checkFOV(NecClient client)
        {
            Vector3 target = new Vector3(client.Character.X, client.Character.Y, client.Character.Z);
            Vector3 source = new Vector3(_monster.X, _monster.Y, _monster.Z);
            Vector3 targetVector = Vector3.Normalize(source - target);
            double sourceRadian = ConvertToRadians(_monster.Heading,Map.Id != 2006000);
            Vector3 sourceVector = new Vector3((float)Math.Cos(sourceRadian), (float)Math.Sin(sourceRadian), 0);
            sourceVector = Vector3.Normalize(sourceVector);
            float dotProduct = Vector3.Dot(sourceVector, targetVector);
            //Logger.Debug($"sourceVector.X[{sourceVector.X}] sourceVector.Y[{sourceVector.Y}]");
            if (dotProduct > fovAngle)
                Logger.Debug($"Monster {_monster.Name} sees you!!");
            //else
            Logger.Debug($"Monster {_monster.Name} is oblivious dotProduct [{dotProduct}] fovAngle [{fovAngle}]");
            return dotProduct > fovAngle;
        }
        private double ConvertToRadians(double angle, bool adjust)
        {
            angle = angle * 2;
            if (adjust)
                angle = (angle <= 90 ? angle + 270 : angle - 90);
            //direction < 270 ? (direction + 90) / 2 : (direction - 270) / 2;
            return (Math.PI / 180) * angle;
        }

        private void orientMonster()
        {
            if (_monster.GetAgro())
                AdjustHeading();

            _monster.MonsterOrient(_server);
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
            Character currentTarget = _monster.GetCurrentTarget();
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
            Character currentTarget = _monster.GetCurrentTarget();
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
            _server.Router.Send(Map, (ushort)AreaPacketId.recv_battle_report_start_notify, res, ServerType.Area);

        }
        private void SendBattleReportEndNotify()
        {
            IBuffer res = BufferProvider.Provide();
            _server.Router.Send(Map, (ushort)AreaPacketId.recv_battle_report_end_notify, res, ServerType.Area);
        }
    }
}
