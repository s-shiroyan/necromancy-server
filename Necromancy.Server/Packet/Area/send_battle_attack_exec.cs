using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Common.Instance;
using System.Threading.Tasks;
using System;
using Necromancy.Server.Packet.Response;


namespace Necromancy.Server.Packet.Area
{
    public class send_battle_attack_exec : ClientHandler
    {
        public send_battle_attack_exec(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_battle_attack_exec;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int Unknown1 = packet.Data.ReadInt32();
            int instanceId = packet.Data.ReadInt32();
            int Unknown2 = packet.Data.ReadInt32();

            client.Character.eventSelectReadyCode = (uint)instanceId;
            Logger.Debug($"Just attacked Target {client.Character.eventSelectReadyCode}");



            SendBattleAttackExecR(client);
            if (instanceId == 0)
            {
                return;
            }
            IInstance instance = Server.Instances.GetInstance((uint)instanceId);
            //Router.Send(client, (ushort) AreaPacketId.recv_battle_attack_exec_direct_r, res, ServerType.Area);
            SendBattleReportStartNotify(client, instance);
            SendReportNotifyHitEffect(client, instance);
            SendReportDamageHP(client, instance);
            //SendDataNotifyEoData(client, instance);
            SendBattleReportEndNotify(client, instance);
        }

        private void SendBattleReportStartNotify(NecClient client, IInstance instance)
        {
            IBuffer res4 = BufferProvider.Provide();
            res4.WriteInt32(instance.InstanceId);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_start_notify, res4, ServerType.Area);
        }
        private void SendBattleReportEndNotify(NecClient client, IInstance instance)
        {
            IBuffer res4 = BufferProvider.Provide();
            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_end_notify, res4, ServerType.Area);
        }


        private void SendReportAcctionAtackExec(NecClient client, IInstance instance)
        {
            IBuffer res4 = BufferProvider.Provide();
            res4.WriteInt32(instance.InstanceId);
            Router.Send(client, (ushort)AreaPacketId.recv_battle_report_action_attack_exec, res4, ServerType.Area);


        }
        private void SendReportNotifyHitEffect(NecClient client, IInstance instance)
        {
            IBuffer res4 = BufferProvider.Provide();
            res4.WriteInt32(100020);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_notify_hit_effect, res4, ServerType.Area);


        }

        private void SendReportDamageHP(NecClient client, IInstance instance)
        {
            int damage = Util.GetRandomNumber(4000000, 6000000);
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(instance.InstanceId);
            res.WriteInt32(damage);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_notify_phy_damage_hp, res, ServerType.Area);

            if (client.Map.MonsterSpawns.TryGetValue((int)instance.InstanceId, out MonsterSpawn _monsterSpawn))
            {
                _monsterSpawn.CurrentHp -= damage;

                IBuffer res4 = BufferProvider.Provide();
                res4.WriteInt32(instance.InstanceId);
                if (_monsterSpawn.CurrentHp <= 0) { _monsterSpawn.CurrentHp = 0; }
                Logger.Debug($"hp: {_monsterSpawn.CurrentHp} max hp : {_monsterSpawn.MaxHp}  hp calc {((double)_monsterSpawn.CurrentHp / (double)_monsterSpawn.MaxHp) * 100}");
                res4.WriteByte((byte)(((double)_monsterSpawn.CurrentHp / (double)_monsterSpawn.MaxHp) * 100)); // % hp remaining of target.  need to store current NPC HP and OD as variables to "attack" them
                Router.Send(client.Map, (ushort)AreaPacketId.recv_object_hp_per_update_notify, res4, ServerType.Area);

                //Check if the monster has 0 hp.  if so,  animate it's death,  then decompose the body,  then remove it from the map, then re-spawn it with full HP.
                if (_monsterSpawn.CurrentHp <= 0) {

                    //Death Animation
                    IBuffer res5 = BufferProvider.Provide();
                    res5.WriteInt32(instance.InstanceId);
                    res5.WriteInt32(1); //Death int
                    res5.WriteInt32(0);
                    res5.WriteInt32(0);
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_noact_notify_dead, res5, ServerType.Area);

                    //Make the monster a lootable state
                    IBuffer res10 = BufferProvider.Provide();
                    res10.WriteInt32(_monsterSpawn.InstanceId);
                    res10.WriteInt32(2);//Toggles state between Alive(attackable),  Dead(lootable), or Inactive(nothing). 
                    Router.Send(client, (ushort)AreaPacketId.recv_monster_state_update_notify, res10, ServerType.Area);

                    //Give the player time to loot the corpse, then decompose the body
                    Task.Delay(TimeSpan.FromSeconds(15)).ContinueWith
                    (t1 =>
                        {
                            _monsterSpawn.CurrentHp = _monsterSpawn.MaxHp; //re-set HP to max

                        //decompose the body
                        IBuffer res7 = BufferProvider.Provide();
                            res7.WriteInt32(instance.InstanceId);
                            res7.WriteInt32(5);//4 here causes a cloud and the model to disappear, 5 causes a mist to happen and disappear
                        res7.WriteInt32(1);
                            Router.Send(client.Map, (ushort)AreaPacketId.recv_charabody_notify_deadstate, res7, ServerType.Area);

                        //leave the monster gone for a while,  then re-spawn it.
                        Task.Delay(TimeSpan.FromSeconds(15)).ContinueWith
                            (t1 =>
                                {

                                //Remove the dead monster object from the map (reset the instance for re-use)
                                RecvObjectDisappearNotify objectDisappearData = new RecvObjectDisappearNotify(_monsterSpawn.InstanceId);
                                    Router.Send(client.Map, objectDisappearData);

                                //Re-spawn the monster
                                RecvDataNotifyMonsterData monsterData = new RecvDataNotifyMonsterData(_monsterSpawn);
                                    Router.Send(client.Map, monsterData);

                                }
                            );

                        }
                    );

                }
            }
        }

        private void SendBattleAteckExecDirect(NecClient client, IInstance instance)
        {
            IBuffer res4 = BufferProvider.Provide();
            res4.WriteInt32(instance.InstanceId);
            Router.Send(client, (ushort)AreaPacketId.recv_battle_attack_exec_direct_r, res4, ServerType.Area);


        }
        private void SendBattleAttackExecR(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);  // If not zero next attack is allowed before first complete

            Router.Send(client, (ushort)AreaPacketId.recv_battle_attack_exec_r, res, ServerType.Area);
        }


    }
}
