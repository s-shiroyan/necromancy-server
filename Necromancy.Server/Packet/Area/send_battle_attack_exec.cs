using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class send_battle_attack_exec : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_battle_attack_exec));

        private readonly NecServer _server;

        public send_battle_attack_exec(NecServer server) : base(server)
        {
            _server = server;
        }

        public override ushort Id => (ushort) AreaPacketId.send_battle_attack_exec;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int Unknown1 = packet.Data.ReadInt32();
            uint instanceId = packet.Data.ReadUInt32();
            int Unknown2 = packet.Data.ReadInt32();

            client.Character.eventSelectReadyCode = (uint) instanceId;
            Logger.Debug($"Just attacked Target {client.Character.eventSelectReadyCode}");


            int damage = 0;
            int seed = Util.GetRandomNumber(0, 20);
            if (seed < 2)
                damage = Util.GetRandomNumber(1, 4); // Light hit
            else if (seed < 19)
                damage = Util.GetRandomNumber(16, 24); // Normal hit
            else
                damage = Util.GetRandomNumber(32, 48); // Critical hit

            //Testing some aoe damage stuff.......  Takes a long time to process.
            AttackObjectsInRange(client, damage);


            SendBattleAttackExecR(client);
        }

        //What do the other _r  recvs do? 
        private void SendBattleAttackExecR(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // If not zero next attack is allowed before first complete
            Router.Send(client, (ushort) AreaPacketId.recv_battle_attack_exec_r, res, ServerType.Area);
        }

        static double distance(double targetX, double targetY, double objectX, double objectY)
        {
            // Calculating distance 
            return Math.Sqrt(Math.Pow(objectX - targetX, 2) +
                             Math.Pow(objectY - targetY, 2) * 1.0);
        }

        private void AttackObjectsInRange(NecClient client, int damage)
        {
            float perHp = 100.0f;

            //Damage Players in range
            foreach (NecClient targetClient in client.Map.ClientLookup.GetAll())
            {
                if (targetClient == client) continue;

                double distanceToCharacter = distance(targetClient.Character.X, targetClient.Character.Y,
                    client.Character.X, client.Character.Y);
                Logger.Debug(
                    $"target Character name [{targetClient.Character.Name}] distanceToCharacter [{distanceToCharacter}] Radius { /*[{monsterSpawn.Radius}]*/"125"} {targetClient.Character.Name}");
                if (distanceToCharacter > /*targetClient.Character.Radius +*/ 125)
                {
                    continue;
                }

                if (targetClient.Character.Hp.depleted)
                {
                    continue;
                }

                targetClient.Character.Hp.Modify(-damage, client.Character.InstanceId);
                perHp = (float) targetClient.Character.Hp.current / targetClient.Character.Hp.max * 100;
                Logger.Debug(
                    $"CurrentHp [{targetClient.Character.Hp.current}] MaxHp[{targetClient.Character.Hp.max}] perHp[{perHp}]");
                RecvCharaUpdateHp cHpUpdate = new RecvCharaUpdateHp(targetClient.Character.Hp.current);
                _server.Router.Send(targetClient, cHpUpdate.ToPacket());

                //logic to turn characters to criminals on criminal actions.  possibly should move to character task.
                client.Character.criminalState += 1;
                if (client.Character.criminalState == 1 | client.Character.criminalState == 2 |
                    client.Character.criminalState == 3)
                {
                    IBuffer res40 = BufferProvider.Provide();
                    res40.WriteUInt32(client.Character.InstanceId);
                    res40.WriteByte(client.Character.criminalState);

                    Logger.Debug(
                        $"Setting crime level for Character {client.Character.Name} to {client.Character.criminalState}");
                    Router.Send(client, (ushort) AreaPacketId.recv_chara_update_notify_crime_lv, res40,
                        ServerType.Area);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_charabody_notify_crime_lv, res40,
                        ServerType.Area, client);
                }

                if (client.Character.criminalState > 255)
                {
                    client.Character.criminalState = 255;
                }

                DamageTheObject(client, targetClient.Character.InstanceId, damage, perHp);
            }

            //Damage Monsters in range
            foreach (MonsterSpawn monsterSpawn in client.Map.MonsterSpawns.Values)
            {
                double distanceToObject =
                    distance(monsterSpawn.X, monsterSpawn.Y, client.Character.X, client.Character.Y);
                Logger.Debug(
                    $"target Monster name [{monsterSpawn.Name}] distanceToObject [{distanceToObject}] Radius [{monsterSpawn.Radius}] {monsterSpawn.Name}");
                if (distanceToObject > monsterSpawn.Radius * 5
                ) //increased hitbox for monsters by a factor of 5.  Beetle radius is 40
                {
                    continue;
                }

                if (monsterSpawn.Hp.depleted)
                {
                    continue;
                }

                monsterSpawn.Hp.Modify(-damage, client.Character.InstanceId);
                perHp = (float) monsterSpawn.Hp.current / monsterSpawn.Hp.max * 100;
                Logger.Debug($"CurrentHp [{monsterSpawn.Hp.current}] MaxHp[{monsterSpawn.Hp.max}] perHp[{perHp}]");

                //just for fun. turn on inactive monsters
                if(monsterSpawn.Active == false) 
                { 
                    monsterSpawn.Active = true;
                    monsterSpawn.SpawnActive = true;
                    if (!monsterSpawn.TaskActive)
                    {
                        Tasks.MonsterTask monsterTask = new Tasks.MonsterTask(_server, monsterSpawn);
                        if (monsterSpawn.defaultCoords)
                            monsterTask.monsterHome = monsterSpawn.monsterCoords[0];
                        else
                            monsterTask.monsterHome = monsterSpawn.monsterCoords.Find(x => x.CoordIdx == 64);
                        monsterTask.Start();
                    }

                }

                DamageTheObject(client, monsterSpawn.InstanceId, damage, perHp);
            }

            //Damage NPCs in range
            foreach (NpcSpawn npcSpawn in client.Map.NpcSpawns.Values)
            {
                double distanceToObject = distance(npcSpawn.X, npcSpawn.Y, client.Character.X, client.Character.Y);
                Logger.Debug(
                    $"target NPC name [{npcSpawn.Name}] distanceToObject [{distanceToObject}] Radius [{npcSpawn.Radius}] {npcSpawn.Name}");
                if (distanceToObject > npcSpawn.Radius)
                {
                    continue;
                }

                DamageTheObject(client, npcSpawn.InstanceId, damage, perHp);
            }
        }

        private void DamageTheObject(NecClient client, uint instanceId, int damage, float perHp)
        {
            List<PacketResponse> brTargetList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(instanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionAttackExec
                brAttack = new RecvBattleReportActionAttackExec(
                    (int) instanceId); //should this be the instance ID of the attacker? we have it marked as skillId
            RecvBattleReportNotifyHitEffect brHit = new RecvBattleReportNotifyHitEffect(instanceId);
            RecvBattleReportPhyDamageHp brPhyHp = new RecvBattleReportPhyDamageHp(instanceId, damage);
            RecvBattleReportDamageHp brHp = new RecvBattleReportDamageHp(instanceId, damage);
            RecvObjectHpPerUpdateNotify oHpUpdate = new RecvObjectHpPerUpdateNotify(instanceId, perHp);
            RecvBattleReportNotifyKnockback brKnockBack = new RecvBattleReportNotifyKnockback(instanceId, 001, 001);

            brTargetList.Add(brStart);
            brTargetList.Add(brAttack);
            brTargetList.Add(brHit);
            //brTargetList.Add(brPhyHp);
            brTargetList.Add(brHp);
            brTargetList.Add(oHpUpdate);
            //brTargetList.Add(brKnockBack); //knockback doesn't look right here. need to make it better.
            brTargetList.Add(brEnd);
            Router.Send(client.Map, brTargetList);
        }

        //To be implemented for monsters
        private void SendBattleReportKnockBack(NecClient client, IInstance instance)
        {
            MonsterSpawn monster = (MonsterSpawn) instance;
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(monster.InstanceId);
            res.WriteFloat(0);
            res.WriteFloat(2); // delay in seconds
            Router.Send(client.Map, (ushort) AreaPacketId.recv_battle_report_noact_notify_knockback, res,
                ServerType.Area);
        }

        private void OldBattleLogic(NecClient client, uint instanceId)
        {
            int damage = 0;
            float perHp = 100.0f;
            int seed = Util.GetRandomNumber(0, 20);
            if (seed < 2)
                damage = Util.GetRandomNumber(1, 4); // Light hit
            else if (seed < 19)
                damage = Util.GetRandomNumber(16, 24); // Normal hit
            else
                damage = Util.GetRandomNumber(32, 48); // Critical hit

            //stops the logic gate below if nothing is targeted.  This was because Rev 1 of battle logic only worked on targeted objects.  probably will go away with progress on 'area of affect' based melee
            if (instanceId == 0)
                return;

            IInstance instance = Server.Instances.GetInstance(instanceId);

            switch (instance)
            {
                case NpcSpawn npcSpawn:
                    client.Map.NpcSpawns.TryGetValue(npcSpawn.InstanceId, out npcSpawn);
                {
                    double distanceToNPC = distance(npcSpawn.X, npcSpawn.Y, client.Character.X, client.Character.Y);
                    Logger.Debug(
                        $"NPC name [{npcSpawn.Name}] distanceToNPC [{distanceToNPC}] Radius [{npcSpawn.Radius}] {npcSpawn.Name}");
                    if (distanceToNPC > npcSpawn.Radius + 125)
                    {
                        //SendBattleReportEndNotify(client, instance);
                        return;
                    }

                    if (client.Character.criminalState < 1)
                    {
                        client.Character.criminalState = 1;
                        IBuffer res40 = BufferProvider.Provide();
                        res40.WriteUInt32(client.Character.InstanceId);
                        res40.WriteByte(client.Character.criminalState);

                        Logger.Debug(
                            $"Setting crime level for Character {client.Character.Name} to {client.Character.criminalState}");
                        Router.Send(client, (ushort) AreaPacketId.recv_chara_update_notify_crime_lv, res40,
                            ServerType.Area);
                        Router.Send(client.Map, (ushort) AreaPacketId.recv_charabody_notify_crime_lv, res40,
                            ServerType.Area, client);
                    }
                }
                    break;
                case MonsterSpawn monsterSpawn:
                    client.Map.MonsterSpawns.TryGetValue(monsterSpawn.InstanceId, out monsterSpawn);
                {
                    double distanceToMonster = distance(monsterSpawn.X, monsterSpawn.Y, client.Character.X,
                        client.Character.Y);
                    Logger.Debug(
                        $"monster name [{monsterSpawn.Name}] distanceToMonster [{distanceToMonster}] Radius [{monsterSpawn.Radius}] {monsterSpawn.Name}");
                    if (distanceToMonster > monsterSpawn.Radius + 125)
                    {
                        //SendBattleReportEndNotify(client, instance);
                        return;
                    }

                    if (monsterSpawn.GetAgroCharacter(client.Character.InstanceId))
                    {
                        monsterSpawn.UpdateHP(-damage);
                    }
                    else
                    {
                        monsterSpawn.UpdateHP(-damage, _server, true, client.Character.InstanceId);
                    }

                    if (client.Character.IsStealthed())
                    {
                        client.Character.ClearStateBit(CharacterState.StealthForm);
                        RecvCharaNotifyStateflag charState =
                            new RecvCharaNotifyStateflag(client.Character.InstanceId, (uint)client.Character.State);
                        _server.Router.Send(client.Map, charState);
                    }

                    perHp = (float) monsterSpawn.Hp.current / monsterSpawn.Hp.max * 100;
                    Logger.Debug($"CurrentHp [{monsterSpawn.Hp.current}] MaxHp[{monsterSpawn.Hp.max}] perHp[{perHp}]");
                }
                    break;
                case Character character:
                    NecClient targetClient = client.Map.ClientLookup.GetByCharacterInstanceId(instance.InstanceId);
                    double distanceToCharacter = distance(targetClient.Character.X, targetClient.Character.Y,
                        client.Character.X, client.Character.Y);
                    Logger.Debug(
                        $"target Character name [{targetClient.Character.Name}] distanceToCharacter [{distanceToCharacter}] Radius { /*[{monsterSpawn.Radius}]*/"125"} {targetClient.Character.Name}");
                    if (distanceToCharacter > /*targetClient.Character.Radius +*/ 125)
                    {
                        //SendBattleReportEndNotify(client, instance);
                        return;
                    }

                    targetClient.Character.Hp.Modify(-damage, character.InstanceId);
                    perHp = (float) targetClient.Character.Hp.current / targetClient.Character.Hp.max * 100;
                    Logger.Debug(
                        $"CurrentHp [{targetClient.Character.Hp.current}] MaxHp[{targetClient.Character.Hp.max}] perHp[{perHp}]");
                    RecvCharaUpdateHp cHpUpdate = new RecvCharaUpdateHp(targetClient.Character.Hp.current);
                    _server.Router.Send(targetClient, cHpUpdate.ToPacket());

                    //logic to turn characters to criminals on criminal actions.  possibly should move to character task.
                    client.Character.criminalState += 1;
                    if (client.Character.criminalState == 1 | client.Character.criminalState == 2 |
                        client.Character.criminalState == 3)
                    {
                        IBuffer res40 = BufferProvider.Provide();
                        res40.WriteUInt32(client.Character.InstanceId);
                        res40.WriteByte(client.Character.criminalState);

                        Logger.Debug(
                            $"Setting crime level for Character {client.Character.Name} to {client.Character.criminalState}");
                        Router.Send(client, (ushort) AreaPacketId.recv_chara_update_notify_crime_lv, res40,
                            ServerType.Area);
                        Router.Send(client.Map, (ushort) AreaPacketId.recv_charabody_notify_crime_lv, res40,
                            ServerType.Area, client);
                    }

                    if (client.Character.criminalState > 255)
                    {
                        client.Character.criminalState = 255;
                    }

                    break;

                default:
                    Logger.Error($"Instance with InstanceId: {instance.InstanceId} does not exist");
                    break;
            }

            List<PacketResponse> brTargetList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(client.Character.InstanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionAttackExec brAttack = new RecvBattleReportActionAttackExec((int) instance.InstanceId);
            RecvBattleReportNotifyHitEffect brHit = new RecvBattleReportNotifyHitEffect(instance.InstanceId);
            RecvBattleReportPhyDamageHp brPhyHp = new RecvBattleReportPhyDamageHp(instance.InstanceId, damage);
            RecvBattleReportDamageHp brHp = new RecvBattleReportDamageHp(instance.InstanceId, damage);
            RecvObjectHpPerUpdateNotify oHpUpdate = new RecvObjectHpPerUpdateNotify(instance.InstanceId, perHp);


            brTargetList.Add(brStart);
            brTargetList.Add(brAttack);
            brTargetList.Add(brHit);
            brTargetList.Add(brPhyHp);
            //brTargetList.Add(brHp);
            brTargetList.Add(oHpUpdate);
            brTargetList.Add(brEnd);
            Router.Send(client.Map, brTargetList);
        }
    }
}
