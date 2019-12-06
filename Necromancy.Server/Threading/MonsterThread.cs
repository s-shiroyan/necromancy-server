using Arrowgene.Services.Buffers;
using Arrowgene.Services.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Response;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;

namespace Necromancy.Server.Threading
{
    // Usage: create a monster and spawn it then use the following
    //MonsterThread monsterThread = new MonsterThread(Server, client, monsterSpawn);
    //Thread workerThread2 = new Thread(monsterThread.InstanceMethod);
    //workerThread2.Start();

    class MonsterThread
    {
        protected readonly NecLogger Logger;
        protected NecServer Server { get; }
        protected PacketRouter Router { get; }

        private NecClient _client;
        public MonsterSpawn _monster { get; set; }
        public bool freeze { get; set; }
        public bool active { get; set; }
        public bool moving { get; set; }
        public bool hateOn { get; set; }
        private bool casting;
        private bool battlePose;
        private bool monsterAgro;
        private bool monsterWaiting;
        public int gotoDistance { get; set; }
        public int agroRange { get; set; }

        private int moveTime;
        private int updateTime;
        private int waitTime;
        private int pathingTick;
        private int agroTick;
        private int monsterVelocity;
        public List<MonsterCoord> monsterCoords { get; set; }
        public List<int> playerHate { get; set; }
        public MonsterCoord monsterHome;
        public MonsterThread(NecServer server, NecClient client, MonsterSpawn monster)
        {
            _client = client;
            _monster = monster;
            Server = server;
            Router = Server.Router;
            Logger = LogProvider.Logger<NecLogger>(this);
            freeze = false;
            active = true;
            moving = false;
            hateOn = false;
            battlePose = false;
            casting = false;
            monsterCoords = new List<MonsterCoord>();
            playerHate = new List<int>();
            monsterHome = null;
            pathingTick = 100;
            agroTick = 1000;
            updateTime = pathingTick;
            waitTime = 2000;
            moveTime = updateTime;
            monsterAgro = false;
            monsterWaiting = false;
            agroRange = 1000;
            monsterVelocity = 250;
        }
        public void InstanceMethod()
        {
            Thread.Sleep(3000);
            int currentDest = 1;
            MonsterCoord currentCoord = monsterCoords[0];
            float fovAngle = (float)Math.Cos(Math.PI / 2);
            int currentWait = 0;
            while (active)
            {
                MonsterCoord nextCoord = monsterCoords[currentDest];
                Vector3 monster = new Vector3(_monster.X, _monster.Y, _monster.Z);
                Vector3 character = new Vector3(_client.Character.X, _client.Character.Y, _client.Character.Z);
                float distanceChar = GetDistance(character, monster);
                float distance = GetDistance(nextCoord.destination, monster);
                float homeDistance = GetDistance(monsterHome.destination, monster);
                //if (monsterAgro)
                //Logger.Debug($"distanceChar [{distanceChar}]");
                if (distance > gotoDistance && !freeze && !monsterWaiting && !monsterAgro)
                {
                        MoveMonster(nextCoord);
                }
                else if (monsterAgro)
                {
                    if (homeDistance >= (agroRange * 2))
                    {
                        RecvObjectDisappearNotify objectDisappearData = new RecvObjectDisappearNotify(_monster.InstanceId);
                        Router.Send(_client.Map, objectDisappearData);
                        active = false;
                        continue;
                    }
                    if (!casting && CheckHeading() == false && monsterAgro)
                        orientMonster();
                    AgroMoveMonster();
                }
                else
                {
                    if (moving)
                    {
                        Thread.Sleep(updateTime); //Allow for cases where the remaining distance is less than the gotoDistance
                        StopMonster();
                        if (!monsterAgro)
                        {
                            monsterWaiting = true;
                            currentWait = 0;
                            //                        Thread.Sleep(2000);
                            if (currentDest < monsterCoords.Count - 1)
                                currentDest++;
                            else
                                currentDest = 0;
                            _monster.Heading = monsterCoords[currentDest].Heading;
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
                        StopMonster();
                        MonsterHate(true);
                        moveTime = agroTick;
                        gotoDistance = 800;
                        monsterVelocity = 500;
                        orientMonster();
                        AgroMoveMonster();
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
        }

        private void MonsterTarget()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_monster.InstanceId);
            res.WriteInt32(_client.Character.InstanceId);
            Router.Send(_client, (ushort)AreaPacketId.recv_0x692A, res, ServerType.Area);
        }
        private void MonsterCast()
        {
            casting = true;
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_monster.InstanceId);
            res.WriteInt32(14101);
            res.WriteFloat(4);
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
        private void MoveMonster(MonsterCoord monsterCoord)
        {
            Vector2 destPos = new Vector2(monsterCoord.destination.X, monsterCoord.destination.Y);
            Vector2 monsterPos = new Vector2(_monster.X, _monster.Y);
            Vector2 moveTo = Vector2.Subtract(destPos, monsterPos);
            float distance = Vector2.Distance(monsterPos, destPos);
            float travelTime = distance / monsterVelocity;

            //ShowVectorInfo(_monster.X, _monster.Y, monsterCoord.destination.X, monsterCoord.destination.Y);
            //ShowMonsterInfo(monsterSpawn);
            if (!moving)
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
                moving = true;
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
        private void AgroMoveMonster()
        {
            Vector3 charPos = new Vector3(_client.Character.X, _client.Character.Y, _client.Character.Z);
            Vector3 monsterPos = new Vector3(_monster.X, _monster.Y, _monster.Z);
            Vector3 moveTo = Vector3.Subtract(charPos, monsterPos);
            float distance = Vector3.Distance(monsterPos, charPos);
            //Logger.Debug($"distance [{distance}]");
            if (distance <= gotoDistance)
            {
                if (moving)
                {
                    StopMonster();
                }
                return;
            }
            ShowVectorInfo(_monster.X, _monster.Y, _client.Character.X, _client.Character.Y);
            //Logger.Debug($"moving [{moving}]");
            //Vector2 moveTo = GetVector(monsterSpawn.X, monsterSpawn.Y, client.Character.X, client.Character.Y);
            //ShowMonsterInfo(monsterSpawn);
            if (!moving)
                moving = true;


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

        private void StopMonster()
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
            moving = false;
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
        private double GetHeading(Vector3 monster, Vector3 destination) // Will return heading for x2/y2 object to look at x1/y1 object
        {
            double dx = monster.X - destination.X;
            double dy = monster.Y - destination.Y;
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
