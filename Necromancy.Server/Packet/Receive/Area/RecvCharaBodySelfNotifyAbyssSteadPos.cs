using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodySelfNotifyAbyssSteadPos : PacketResponse
    {
        public RecvCharaBodySelfNotifyAbyssSteadPos()
            : base((ushort) AreaPacketId.recv_charabody_self_notify_abyss_stead_pos, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteFloat(0);
            res.WriteFloat(0);
            res.WriteFloat(0);
            return res;
        }
    }
}
