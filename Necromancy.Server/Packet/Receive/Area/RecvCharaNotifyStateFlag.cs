using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaNotifyStateflag : PacketResponse
    {
        private readonly uint _instanceId;
        private readonly uint _state;

        public RecvCharaNotifyStateflag(uint instanceId, uint state)
            : base((ushort) AreaPacketId.recv_chara_notify_stateflag, ServerType.Area)
        {
            _instanceId = instanceId;
            _state = state;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_instanceId);
            res.WriteUInt32(_state);
            return res;
        }
    }
}
