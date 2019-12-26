using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvEoUpdateSecondTrapId : PacketResponse
    {
        private readonly uint _instanceId;
        private readonly int _effectId;

        public RecvEoUpdateSecondTrapId(uint instanceId, int effectId)
            : base((ushort) AreaPacketId.recv_eo_update_second_trapid, ServerType.Area)
        {
            _instanceId = instanceId;
            _effectId = effectId;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_instanceId);
            res.WriteInt32(_effectId);
            return res;
        }
    }
}
