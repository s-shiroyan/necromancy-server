using Arrowgene.Services.Buffers;
using Arrowgene.Services.Tasks;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Response;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

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

        private NecClient _client;
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

        private int moveTime;
        private int updateTime;
        private int waitTime;
        private int pathingTick;
        private int agroTick;
        private int monsterVelocity;
        private int respawnTime;
        public List<int> playerHate { get; set; }
        public MonsterCoord monsterHome;
        public MonsterTask(NecServer server, NecClient client, MonsterSpawn monster)
        {
            _client = client;
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
            playerHate = new List<int>();
            monsterHome = null;
            pathingTick = 100;
            agroTick = 1000;
            updateTime = pathingTick;
            waitTime = 2000;
            moveTime = updateTime;
            monsterAgro = false;
            monsterWaiting = true;
            agroRange = 1000;
            monsterVelocity = 250;
            RunAtStart = false;
            Name = monster.Name;
            respawnTime = 10000;
        }

        public override string Name { get; }
        public override TimeSpan TimeSpan { get; }
        protected override bool RunAtStart { get; }
        protected override void Execute()
        {
            int currentDest = 1;
            float fovAngle = (float)Math.Cos(Math.PI / 2);
            int currentWait = 0;
            while (monsterActive && _monster.SpawnActive)
            {
                if (spawnMonster)
                {
                    Thread.Sleep(respawnTime);
                    currentDest = 1;
                    MonsterSpawn();
                    Thread.Sleep(3000);
                }
                MonsterCoord nextCoord = _monster.monsterCoords.Find(x => x.CoordIdx == currentDest);
                Vector3 monster = new Vector3(_monster.X, _monster.Y, _monster.Z);
                Vector3 character = new Vector3(_client.Character.X, _client.Character.Y, _client.Character.Z);
                float distanceChar = GetDistance(character, monster);
                float distance = GetDistance(nextCoord.destination, monster);
                if (distance > gotoDistance && !monsterFreeze && !monsterWaiting && !monsterAgro)
                {
                    MonsterMove(nextCoord);
                }
                else if (monsterAgro)
                {
                    if (MonsterCheck())
                    {
                        continue;
                    }
                    float homeDistance = GetDistance(monsterHome.destination, monster);
                    if (homeDistance >= (agroRange * 2))
                    {
                        RecvObjectDisappearNotify objectDisappearData = new RecvObjectDisappearNotify(_monster.InstanceId);
                        Router.Send(_client.Map, objectDisappearData);
                        spawnMonster = true;
                        continue;
                    }
                    if (!casting && CheckHeading() == false && monsterAgro)
                        orientMonster();
                    MonsterAgroMove();
                }
                else
                {
                    if (monsterMoving)
                    {
                        Thread.Sleep(updateTime); //Allow for cases where the remaining distance is less than the gotoDistance
                        MonsterStop();
                        if (!monsterAgro)
                        {
                            monsterWaiting = true;
                            currentWait = 0;
                            //                        Thread.Sleep(2000);
                            if (currentDest < _monster.monsterCoords.Count - 2)
                                currentDest++;
                            else
                                currentDest = 0;
                            _monster.Heading = (byte)GetHeading(_monster.monsterCoords.Find(x => x.CoordIdx == currentDest).destination);
                        }
                    }
                    /*                    if (!battlePose)
                                            MonsterBattlePose(true);
                                        if (!hateOn)
                                        {
                                            MonsterHate((int)_client.Character.InstanceId, true);
                                        } 
                                        if (!casting)
                                        {
                                            //MonsterTarget();
                                            //SendBattleReportStartNotify();
                                            //MonsterCast();
                                            //SendBattleReportEndNotify();
                                        }
                                        */
                }
                if (distanceChar <= agroRange && !monsterAgro) 
                {
                    monsterAgro = chechFOV(fovAngle);
                    if (monsterAgro)
                    {
                        MonsterStop();
                        MonsterHate(true);
                        moveTime = agroTick;
                        gotoDistance = 800;
                        monsterVelocity = 500;
                        //SendBattleReportStartNotify();
                        //MonsterTarget();
                        //MonsterCast();
                        //SendBattleReportEndNotify();

                        orientMonster();
                        MonsterAgroMove();
                    }
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
        }

        private void MonsterTarget()
        {
            IBuffer res = BufferProvider.Provide();
            //res.WriteInt32(_monster.InstanceId);
            res.WriteInt32(_client.Character.InstanceId);
            Router.Send(_client, (ushort)AreaPacketId.recv_0xB586, res, ServerType.Area);



            //int numEntries = 0x5;
            //res.WriteInt32(numEntries); //less than or equal to 5
            //res.WriteInt32(_client.Character.InstanceId);
            //for (int i = 0; i < numEntries-1; i++)
                //res.WriteInt32(0);
            //Router.Send(_client, (ushort)AreaPacketId.recv_0x10DA, res, ServerType.Area);

        }
        private void MonsterCast()
        {
            casting = true;
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_client.Character.InstanceId);
            res.WriteInt32(14101);
            res.WriteFloat(4.0F);
            Router.Send(_client, (ushort)AreaPacketId.recv_battle_report_action_monster_skill_start_cast, res, ServerType.Area);
        }
        public void MonsterHate(bool hateOn)
        {
            if (this.hateOn != hateOn)
            {
                IBuffer res = BufferProvider.Provide();
                res.WriteInt32(_monster.InstanceId);
                res.WriteInt32(_client.Character.InstanceId);
                if (hateOn)
                {
                    Router.Send(_client, (ushort)AreaPacketId.recv_monster_hate_on, res, ServerType.Area);
                }
                else
                {
                    Router.Send(_client, (ushort)AreaPacketId.recv_monster_hate_off, res, ServerType.Area);
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
            monsterVelocity = 250;
            MonsterCoord spawnCoords = _monster.monsterCoords.Find(x => x.CoordIdx == 0);
            _monster.X = spawnCoords.destination.X;
            _monster.Y = spawnCoords.destination.Y;
            _monster.Z = spawnCoords.destination.Z;
            _monster.Heading = (byte)GetHeading(_monster.monsterCoords.Find(x => x.CoordIdx == 1).destination);
            _monster.CurrentHp = 100;
            respawnTime = _monster.RespawnTime;
            RecvDataNotifyMonsterData monsterData = new RecvDataNotifyMonsterData(_monster);
            Router.Send(monsterData, _client);
            spawnMonster = false;
        }
        public bool MonsterCheck()
        {
            if (_monster.CurrentHp <= 0)
            {
                MonsterHate(false);
                //Death Animation
                IBuffer res5 = BufferProvider.Provide();
                res5.WriteInt32(_monster.InstanceId);
                res5.WriteInt32(1); //Death int
                res5.WriteInt32(0);
                res5.WriteInt32(0);
                Router.Send(_client.Map, (ushort)AreaPacketId.recv_battle_report_noact_notify_dead, res5, ServerType.Area);

                //Make the monster a lootable state
                IBuffer res10 = BufferProvider.Provide();
                res10.WriteInt32(_monster.InstanceId);
                res10.WriteInt32(2);//Toggles state between Alive(attackable),  Dead(lootable), or Inactive(nothing). 
                Router.Send(_client, (ushort)AreaPacketId.recv_monster_state_update_notify, res10, ServerType.Area);

                //  Let a separate loot manager handle the monster body click?
                Thread.Sleep(_monster.RespawnTime);
                //decompose the body
                IBuffer res7 = BufferProvider.Provide();
                res7.WriteInt32(_monster.InstanceId);
                res7.WriteInt32(5);//4 here causes a cloud and the model to disappear, 5 causes a mist to happen and disappear
                res7.WriteInt32(1);
                Router.Send(_client.Map, (ushort)AreaPacketId.recv_charabody_notify_deadstate, res7, ServerType.Area);
                Thread.Sleep(2000);
                RecvObjectDisappearNotify objectDisappearData = new RecvObjectDisappearNotify(_monster.InstanceId);
                Router.Send(objectDisappearData, _client);

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
                Router.Send(_client, (ushort)AreaPacketId.recv_battle_attack_pose_start_notify, res, ServerType.Area);
                battlePose = true;
            }
            else
            {
                Router.Send(_client, (ushort)AreaPacketId.recv_battle_attack_pose_end_notify, res, ServerType.Area);
                battlePose = false;
            }
        }
        private void MonsterMove(MonsterCoord monsterCoord)
        {
            Vector2 destPos = new Vector2(monsterCoord.destination.X, monsterCoord.destination.Y);
            Vector2 monsterPos = new Vector2(_monster.X, _monster.Y);
            Vector2 moveTo = Vector2.Subtract(destPos, monsterPos);
            float distance = Vector2.Distance(monsterPos, destPos);
            float travelTime = distance / monsterVelocity;

            //ShowVectorInfo(_monster.X, _monster.Y, monsterCoord.destination.X, monsterCoord.destination.Y);
            //ShowMonsterInfo();
            if (!monsterMoving)
            {
                orientMonster();
                //Logger.Debug($"distance [{distance}] travelTime[{travelTime}] moveTo.X [{moveTo.X}] moveTo.Y [{moveTo.Y}]");
                IBuffer res = BufferProvider.Provide();
                res.WriteInt32(_monster.InstanceId);//Monster ID
                res.WriteFloat(_monster.X);
                res.WriteFloat(_monster.Y);
                res.WriteFloat(_monster.Z);
                res.WriteFloat(moveTo.X);       //X per tick
                res.WriteFloat(moveTo.Y);       //Y Per tick
                res.WriteFloat((float)1);              //verticalMovementSpeedMultiplier

                res.WriteFloat((float)1/travelTime);              //movementMultiplier
                res.WriteFloat((float)travelTime);              //Seconds to move

                res.WriteByte(2); //MOVEMENT ANIM
                res.WriteByte(0);//JUMP & FALLING ANIM
                Router.Send(_client, (ushort)AreaPacketId.recv_0x8D92, res, ServerType.Area);
                monsterMoving = true;
            }
            //            Logger.Debug($"distance [{distance}]");
            //            ShowMonsterInfo();
            int tickDivisor = 1000 / updateTime;
            if (distance >= monsterVelocity / tickDivisor)
            {
                _monster.X = _monster.X + (moveTo.X / travelTime)/ tickDivisor;
                _monster.Y = _monster.Y + (moveTo.Y / travelTime)/ tickDivisor;
                moveTime = updateTime;
            } else
            {
                _monster.X = destPos.X;
                _monster.Y = destPos.Y;
                moveTime = (int)(travelTime * 1000);
                //Logger.Debug($"moveTime [{moveTime}]");
            }
        }
        private void MonsterAgroMove()
        {
            Vector3 charPos = new Vector3(_client.Character.X, _client.Character.Y, _client.Character.Z);
            Vector3 monsterPos = new Vector3(_monster.X, _monster.Y, _monster.Z);
            Vector3 moveTo = Vector3.Subtract(charPos, monsterPos);
            float distance = Vector3.Distance(monsterPos, charPos);
            //Logger.Debug($"distance [{distance}]");
            if (distance <= gotoDistance)
            {
                if (monsterMoving)
                {
                    MonsterStop();
                }
                return;
            }
            ShowVectorInfo(_monster.X, _monster.Y, _client.Character.X, _client.Character.Y);
            //Logger.Debug($"moving [{moving}]");
            //Vector2 moveTo = GetVector(monsterSpawn.X, monsterSpawn.Y, client.Character.X, client.Character.Y);
            //ShowMonsterInfo(monsterSpawn);
            if (!monsterMoving)
                monsterMoving = true;


            float travelTime = distance / monsterVelocity;
            float xTick = moveTo.X / travelTime;
            float yTick = moveTo.Y / travelTime;
            //Logger.Debug($"distance [{distance}] travelTime[{travelTime}] xTick [{xTick}] yTick [{yTick}] moveTo.X [{moveTo.X}] moveTo.Y [{moveTo.Y}]");
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(_monster.InstanceId);//Monster ID
            res2.WriteFloat(_monster.X);
            res2.WriteFloat(_monster.Y);
            res2.WriteFloat(_monster.Z);
            res2.WriteFloat(xTick);       //X per tick
            res2.WriteFloat(yTick);       //Y Per tick
            res2.WriteFloat((float)1);              //verticalMovementSpeedMultiplier

            res2.WriteFloat((float)1);              //movementMultiplier
            res2.WriteFloat((float)1);              //Seconds to move

            res2.WriteByte(3); //MOVEMENT ANIM
            res2.WriteByte(0);//JUMP & FALLING ANIM
            Router.Send(_client, (ushort)AreaPacketId.recv_0x8D92, res2, ServerType.Area);
            _monster.X = _monster.X + xTick;
            _monster.Y = _monster.Y + yTick;
        }

        private void MonsterStop()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_monster.InstanceId);//Monster ID
            res.WriteFloat(_monster.X);
            res.WriteFloat(_monster.Y);
            res.WriteFloat(_monster.Z);
            res.WriteFloat(0);       //X per tick
            res.WriteFloat(0);       //Y Per tick
            res.WriteFloat((float)1);              //verticalMovementSpeedMultiplier

            res.WriteFloat((float)1);              //movementMultiplier
            res.WriteFloat((float)1);              //Seconds to move

            res.WriteByte(0); //MOVEMENT ANIM
            res.WriteByte(0);//JUMP & FALLING ANIM
            Router.Send(_client, (ushort)AreaPacketId.recv_0x8D92, res, ServerType.Area);
            monsterMoving = false;
            Thread.Sleep(100);
        }

        private bool chechFOV(float fovAngle)
        {
            Vector2 target = new Vector2(_client.Character.X, _client.Character.Y);
            Vector2 source = new Vector2(_monster.X, _monster.Y);
            Vector2 targetVector = Vector2.Normalize(source - target);
            double sourceRadian = ConvertToRadians(_monster.Heading);
            Vector2 sourceVector = new Vector2((float)Math.Cos(sourceRadian), (float)Math.Sin(sourceRadian));
            sourceVector = Vector2.Normalize(sourceVector);
            float dotProduct = Vector2.Dot(sourceVector, targetVector);
            //Logger.Debug($"sourceVector.X[{sourceVector.X}] sourceVector.Y[{sourceVector.Y}]");
            if (dotProduct > fovAngle)
                Logger.Debug($"Monster sees you!!");
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

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(_monster.InstanceId);

            res.WriteFloat(_monster.X);
            res.WriteFloat(_monster.Y);
            res.WriteFloat(_monster.Z);
            res.WriteByte(_monster.Heading);
            res.WriteByte(1);
            Router.Send(_client, (ushort)AreaPacketId.recv_0x6B6A, res, ServerType.Area);
        }
        private float GetDistance(Vector3 target, Vector3 source)
        {
            return Vector3.Distance(target, source);
        }

        private void ShowVectorInfo(double targetX, double targetY, double objectX, double objectY)
        {
            Vector2 target = new Vector2((float)targetX, (float)targetY);
            Vector2 source = new Vector2((float)objectX, (float)objectY);
            Vector2 moveTo = Vector2.Subtract(target, source);
            float distance = Vector2.Distance(target, source);
            double dx = objectX - targetX;
            double dy = objectY - targetY;
            Logger.Debug($"dx [{dx}]    dy[{dy}]   distance [{distance}] moveTo.X [{moveTo.X}]  moveTo.Y [{moveTo.Y}]");
        }

        private void ShowMonsterInfo()
        {
            Logger.Debug($"monster [{_monster.Name}]    X[{_monster.X}]   Y [{_monster.Y}] monster.Z [{_monster.Z}]  Heading [{_monster.Heading}]");
        }

        private double GetHeading() // Will return heading for x2/y2 object to look at x1/y1 object
        {
            double dx = _monster.X - _client.Character.X;
            double dy = _monster.Y - _client.Character.Y;
            double direction = (Math.Atan2(dy, dx) / System.Math.PI) * 180f; ;
            if (direction < 0) direction += 360f;
            direction = direction < 270 ? (direction + 90) / 2 : (direction - 270) / 2;
            return direction;
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
            double dx = _monster.X - _client.Character.X;
            double dy = _monster.Y - _client.Character.Y;
            double direction = (Math.Atan2(dy, dx) / System.Math.PI) * 180f; ;
            if (direction < 0) direction += 360f;
            direction = direction < 270 ? (direction + 90) / 2 : (direction - 270) / 2;
            //Logger.Debug($"direction after [{direction}]");
            _monster.Heading = (byte)direction;
        }
        private bool CheckHeading() // Will return heading for x2/y2 object to look at x1/y1 object
        {
            double dx = _monster.X - _client.Character.X;
            double dy = _monster.Y - _client.Character.Y;
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
            Router.Send(_client, (ushort)AreaPacketId.recv_battle_report_start_notify, res, ServerType.Area);
        }
        private void SendBattleReportEndNotify()
        {
            IBuffer res = BufferProvider.Provide();
            Router.Send(_client, (ushort)AreaPacketId.recv_battle_report_end_notify, res, ServerType.Area);
        }
    }
}
