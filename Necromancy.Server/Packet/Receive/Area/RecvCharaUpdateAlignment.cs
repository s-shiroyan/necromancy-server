using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateAlignment : PacketResponse
    {
        public RecvCharaUpdateAlignment()
            : base((ushort) AreaPacketId.recv_chara_update_alignment, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //Alignment ID
            return res;
        }
    }
}
