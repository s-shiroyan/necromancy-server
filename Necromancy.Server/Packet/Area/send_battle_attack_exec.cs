using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_battle_attack_exec : ClientHandler
    {
        public send_battle_attack_exec(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_battle_attack_exec;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int Unknown1 = packet.Data.ReadInt32();
            int TargetID = packet.Data.ReadInt32();
            int Unknown2 = packet.Data.ReadInt32();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(TargetID);

            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_attack_exec_r, res, ServerType.Area, client);
            //Router.Send(client, (ushort) AreaPacketId.recv_battle_attack_exec_direct_r, res, ServerType.Area);


            //SendBattleAteckExecDirect(client);
           /* SendReportAcctionAtackExec(client);
            SendReportDamageHP(client);
            SendBattleReportstartNotify(client); */
        }

        /*private void SendBattleReportstartNotify(NecClient client)
        {
            IBuffer res4 = BufferProvider.Provide();
            res4.WriteInt32(client.Character.Id);
            Router.Send(client, (ushort)AreaPacketId.recv_battle_report_start_notify, res4);


        }


        private void SendReportAcctionAtackExec(NecClient client)
        {
            IBuffer res4 = BufferProvider.Provide();
            res4.WriteInt32(client.Character.Id);
            Router.Send(client, (ushort)AreaPacketId.recv_battle_report_action_attack_exec, res4);


        }

        private void SendReportDamageHP(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(client.Character.Id);
            res.WriteInt32(0);
            Router.Send(client, (ushort)AreaPacketId.recv_battle_report_notify_damage_hp, res, ServerType.Area);


        }

        /*   private void SendBattleAteckExecDirect(NecClient client)
            {
                IBuffer res4 = BufferProvider.Provide();
                res4.WriteInt32(client.Character.Id);
                Router.Send(client, (ushort)AreaPacketId.recv_battle_attack_exec_direct_r, res4); */


    }



    }

