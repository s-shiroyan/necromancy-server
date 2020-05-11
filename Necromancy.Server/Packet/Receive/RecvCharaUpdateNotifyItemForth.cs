using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvCharaUpdateNotifyItemForth : PacketResponse
    {
        private readonly uint _instanceId;
        private readonly int _unknown1;
        private readonly int _unknown2;

        public RecvCharaUpdateNotifyItemForth(uint instanceId, int unknown1, int unknown2)
            : base((ushort) AreaPacketId.recv_chara_update_notify_item_forth, ServerType.Area)
        {
            _instanceId = instanceId;
            _unknown1 = unknown1;
            _unknown2 = unknown2;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_instanceId); // Unique Instance ID
            res.WriteInt32(_unknown1); //unknown
            res.WriteInt32(_unknown2);
            return res;
        }
    }
}
