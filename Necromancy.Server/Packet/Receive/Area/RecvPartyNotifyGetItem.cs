using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvPartyNotifyGetItem : PacketResponse
    {
        private uint _instanceId; //???

        public RecvPartyNotifyGetItem(uint instanceId)
            : base((ushort) AreaPacketId.recv_party_notify_get_item, ServerType.Area)
        {
            _instanceId = instanceId;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_instanceId);

            res.WriteCString("SecretDesu");

            res.WriteByte(1);
            return res;
        }
    }
}
