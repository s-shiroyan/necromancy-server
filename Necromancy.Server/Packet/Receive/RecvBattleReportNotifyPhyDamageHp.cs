using Arrowgene.Services.Buffers;
using Necromancy.Server.Chat;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvBattleReportPhyDamageHp : PacketResponse
    {
        private readonly int _instanceId;
        private readonly int _damage;
        public RecvBattleReportPhyDamageHp(int instanceId, int damage)
            : base((ushort) AreaPacketId.recv_battle_report_notify_phy_damage_hp, ServerType.Area)
        {
            _instanceId = instanceId;
            _damage = damage;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_instanceId);
            res.WriteInt32(_damage);
            return res;
        }
    }
}
