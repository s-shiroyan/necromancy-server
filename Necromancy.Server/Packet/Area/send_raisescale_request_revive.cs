using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class send_raisescale_request_revive : ClientHandler
    {
        public send_raisescale_request_revive(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_raisescale_request_revive;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_raisescale_request_revive_r, res, ServerType.Area);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            RecvCharaUpdateHp cHpUpdate = new RecvCharaUpdateHp(client.Character.Hp.current);
            Router.Send(client, cHpUpdate.ToPacket());

            IBuffer res4 = BufferProvider.Provide();
            res4.WriteUInt32(client.Character.InstanceId);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_start_notify, res4, ServerType.Area);

            IBuffer res5 = BufferProvider.Provide();
            res5.WriteUInt32(client.Character.InstanceId);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_notify_raise, res5, ServerType.Area);

            IBuffer res6 = BufferProvider.Provide();
            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_end_notify, res6, ServerType.Area);

            IBuffer res3 = BufferProvider.Provide();
            res3.WriteUInt32(client.Character.DeadBodyInstanceId);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_object_disappear_notify, res3, ServerType.Area);

            client.Character.HasDied = false;
            client.Character.Hp.depleted = false;
            RecvDataNotifyCharaData cData = new RecvDataNotifyCharaData(client.Character, client.Soul.Name);
            Router.Send(client, cData.ToPacket());

            IBuffer res7 = BufferProvider.Provide();
            res7.WriteByte(0);
            Router.Send(client, (ushort)AreaPacketId.recv_event_end, res7, ServerType.Area);
        }
    }
}
