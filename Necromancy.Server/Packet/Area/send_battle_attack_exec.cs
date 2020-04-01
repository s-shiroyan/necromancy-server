using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Common.Instance;
using System;
using System.Collections.Generic;
using Necromancy.Server.Packet.Receive;
using Necromancy.Server.Packet.Response;

namespace Necromancy.Server.Packet.Area
{
    public class send_battle_attack_exec : ClientHandler
    {
        private readonly NecServer _server;
        public send_battle_attack_exec(NecServer server) : base(server)
        {
            _server = server;
        }

        public override ushort Id => (ushort)AreaPacketId.send_battle_attack_exec;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int Unknown1 = packet.Data.ReadInt32();
            uint instanceId = packet.Data.ReadUInt32();
            int Unknown2 = packet.Data.ReadInt32();

            client.Character.eventSelectReadyCode = (uint)instanceId;
            Logger.Debug($"Just attacked Target {client.Character.eventSelectReadyCode}");




            int damage = 0;
            float perHp = 100.0f;
            int seed = Util.GetRandomNumber(0, 20);
            if (seed < 2)
                damage = Util.GetRandomNumber(1, 4);    // Light hit
            else if (seed < 19)
                damage = Util.GetRandomNumber(16, 24);  // Normal hit
            else
                damage = Util.GetRandomNumber(32, 48);  // Critical hit

            //Testing some aoe damage stuff.......  Takes a long time to process.
            AreaOfEffectCalculation(client, damage);

            //stops the logic gate below if nothing is targeted.  This was because Rev 1 of battle logic only worked on targeted objects.  probably will go away with progress on 'area of affect' based melee
            if (instanceId == 0)
                return;

            IInstance instance = Server.Instances.GetInstance(instanceId);
            SendBattleAttackExecR(client);
//            SendBattleReportStartNotify(client, instance);

            switch (instance)
            {
                case NpcSpawn npcSpawn:
                    client.Map.NpcSpawns.TryGetValue(npcSpawn.InstanceId, out npcSpawn);
                    {
                        double distanceToNPC = distance(npcSpawn.X, npcSpawn.Y, client.Character.X, client.Character.Y);
                        Logger.Debug($"NPC name [{npcSpawn.Name}] distanceToNPC [{distanceToNPC}] Radius [{npcSpawn.Radius}] {npcSpawn.Name}");
                        if (distanceToNPC > npcSpawn.Radius + 125)
                        {
                            //SendBattleReportEndNotify(client, instance);
                            return;
                        }
                        if (client.Character.criminalState < 1 ) 
                        { 
                            client.Character.criminalState = 1;
                            IBuffer res40 = BufferProvider.Provide();
                            res40.WriteInt32(client.Character.InstanceId);
                            res40.WriteByte(client.Character.criminalState);

                            Logger.Debug($"Setting crime level for Character {client.Character.Name} to {client.Character.criminalState}");
                            Router.Send(client, (ushort)AreaPacketId.recv_chara_update_notify_crime_lv, res40, ServerType.Area);
                            Router.Send(client.Map, (ushort)AreaPacketId.recv_charabody_notify_crime_lv, res40, ServerType.Area, client);
                        }

                    }
                    break;
                case MonsterSpawn monsterSpawn:
                    client.Map.MonsterSpawns.TryGetValue(monsterSpawn.InstanceId, out monsterSpawn);
                    {
                        double distanceToMonster = distance(monsterSpawn.X, monsterSpawn.Y, client.Character.X, client.Character.Y);
                        Logger.Debug($"monster name [{monsterSpawn.Name}] distanceToMonster [{distanceToMonster}] Radius [{monsterSpawn.Radius}] {monsterSpawn.Name}");
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
                            uint newState = client.Character.ClearStateBit(0x8);
                            RecvCharaNotifyStateflag charState = new RecvCharaNotifyStateflag(client.Character.InstanceId, newState);
                            _server.Router.Send(client.Map, charState);
                        }
                        perHp = (((float)monsterSpawn.Hp.current / (float)monsterSpawn.Hp.max) * 100);
                        Logger.Debug($"CurrentHp [{monsterSpawn.Hp.current}] MaxHp[{ monsterSpawn.Hp.max}] perHp[{perHp}]");
                    }
                    break;
                case Character character:
                    NecClient targetClient = client.Map.ClientLookup.GetByCharacterInstanceId(instanceId);
                    double distanceToCharacter = distance(targetClient.Character.X, targetClient.Character.Y, client.Character.X, client.Character.Y);
                    Logger.Debug($"target Character name [{targetClient.Character.Name}] distanceToCharacter [{distanceToCharacter}] Radius {/*[{monsterSpawn.Radius}]*/"125"} {targetClient.Character.Name}");
                    if (distanceToCharacter > /*targetClient.Character.Radius +*/ 125)
                    {
                        //SendBattleReportEndNotify(client, instance);
                        return;
                    }
                    targetClient.Character.Hp.Modify(-damage, character.InstanceId);
                    perHp = ((targetClient.Character.Hp.current / targetClient.Character.Hp.max) * 100);
                    Logger.Debug($"CurrentHp [{targetClient.Character.Hp.current}] MaxHp[{targetClient.Character.Hp.max}] perHp[{perHp}]");
                    RecvCharaUpdateHp cHpUpdate = new RecvCharaUpdateHp(targetClient.Character.Hp.current);
                    _server.Router.Send(targetClient, cHpUpdate.ToPacket());

                    //logic to turn characters to criminals on criminal actions.  possibly should move to character task.
                    client.Character.criminalState += 1;
                    if(client.Character.criminalState == 1 | client.Character.criminalState == 2 | client.Character.criminalState ==3)
                    {
                        IBuffer res40 = BufferProvider.Provide();
                        res40.WriteInt32(client.Character.InstanceId);
                        res40.WriteByte(client.Character.criminalState);

                        Logger.Debug($"Setting crime level for Character {client.Character.Name} to {client.Character.criminalState}");
                        Router.Send(client, (ushort)AreaPacketId.recv_chara_update_notify_crime_lv, res40, ServerType.Area);
                        Router.Send(client.Map, (ushort)AreaPacketId.recv_charabody_notify_crime_lv, res40, ServerType.Area, client);
                    }
                    if (client.Character.criminalState > 255) { client.Character.criminalState = 255; }

                    break;

                default:
                    Logger.Error($"Instance with InstanceId: {instanceId} does not exist");
                    break;
            }

            List<PacketResponse> brTargetList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(client.Character.InstanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionAttackExec brAttack = new RecvBattleReportActionAttackExec((int)instance.InstanceId);
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

        //What do the other _r  recvs do? 
        private void SendBattleAttackExecR(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);  // If not zero next attack is allowed before first complete
            Router.Send(client, (ushort)AreaPacketId.recv_battle_attack_exec_r, res, ServerType.Area);
        }

        static double distance(double targetX, double targetY, double objectX, double objectY)
        {
            // Calculating distance 
            return Math.Sqrt(Math.Pow(objectX - targetX, 2) +
                          Math.Pow(objectY - targetY, 2) * 1.0);
        }
        private void AreaOfEffectCalculation(NecClient client, int damage)
        {
            foreach (NecClient targetClient in client.Map.ClientLookup.GetAll())
            {
                if (targetClient == client) return;
                Logger.Debug($"Hello {targetClient.Character.Name}");

                float perHp = 100.0f;
                double distanceToCharacter = distance(targetClient.Character.X, targetClient.Character.Y, client.Character.X, client.Character.Y);
                Logger.Debug($"target Character name [{targetClient.Character.Name}] distanceToCharacter [{distanceToCharacter}] Radius {/*[{monsterSpawn.Radius}]*/"125"} {targetClient.Character.Name}");
                if (distanceToCharacter > /*targetClient.Character.Radius +*/ 125)
                {
                    //SendBattleReportEndNotify(client, instance);
                    return;
                }
                targetClient.Character.Hp.Modify(-damage, client.Character.InstanceId);
                perHp = ((targetClient.Character.Hp.current / targetClient.Character.Hp.max) * 100);
                Logger.Debug($"CurrentHp [{targetClient.Character.Hp.current}] MaxHp[{targetClient.Character.Hp.max}] perHp[{perHp}]");
                RecvCharaUpdateHp cHpUpdate = new RecvCharaUpdateHp(targetClient.Character.Hp.current);
                _server.Router.Send(targetClient, cHpUpdate.ToPacket());

                //logic to turn characters to criminals on criminal actions.  possibly should move to character task.
                client.Character.criminalState += 1;
                if (client.Character.criminalState == 1 | client.Character.criminalState == 2 | client.Character.criminalState == 3)
                {
                    IBuffer res40 = BufferProvider.Provide();
                    res40.WriteInt32(client.Character.InstanceId);
                    res40.WriteByte(client.Character.criminalState);

                    Logger.Debug($"Setting crime level for Character {client.Character.Name} to {client.Character.criminalState}");
                    Router.Send(client, (ushort)AreaPacketId.recv_chara_update_notify_crime_lv, res40, ServerType.Area);
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_charabody_notify_crime_lv, res40, ServerType.Area, client);
                }
                if (client.Character.criminalState > 255) { client.Character.criminalState = 255; }


                List<PacketResponse> brTargetList = new List<PacketResponse>();
                RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(client.Character.InstanceId);
                RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
                RecvBattleReportActionAttackExec brAttack = new RecvBattleReportActionAttackExec((int)targetClient.Character.InstanceId);
                RecvBattleReportNotifyHitEffect brHit = new RecvBattleReportNotifyHitEffect(targetClient.Character.InstanceId);
                RecvBattleReportPhyDamageHp brPhyHp = new RecvBattleReportPhyDamageHp(targetClient.Character.InstanceId, damage);
                RecvBattleReportDamageHp brHp = new RecvBattleReportDamageHp(targetClient.Character.InstanceId, damage);
                RecvObjectHpPerUpdateNotify oHpUpdate = new RecvObjectHpPerUpdateNotify(targetClient.Character.InstanceId, perHp);
                RecvBattleReportNotifyKnockback brKnockBack = new RecvBattleReportNotifyKnockback(targetClient.Character.InstanceId, 001, 001);


                brTargetList.Add(brStart);
                brTargetList.Add(brAttack);
                brTargetList.Add(brHit);
                //brTargetList.Add(brPhyHp);
                brTargetList.Add(brHp);
                brTargetList.Add(oHpUpdate);
                brTargetList.Add(brKnockBack);
                brTargetList.Add(brEnd);
                Router.Send(client.Map, brTargetList);
            }
        }

        //To be implemented for monsters
        private void SendBattleReportKnockBack(NecClient client, IInstance instance)
        {
            MonsterSpawn monster = (MonsterSpawn)instance;
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(monster.InstanceId);
            res.WriteFloat(0);
            res.WriteFloat(2);   // delay in seconds
            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_noact_notify_knockback, res, ServerType.Area);
        }


    }
}
