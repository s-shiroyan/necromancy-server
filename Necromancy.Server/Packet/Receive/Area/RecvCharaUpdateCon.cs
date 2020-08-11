using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateCon : PacketResponse
    {
        public RecvCharaUpdateCon()
            : base((ushort) AreaPacketId.recv_chara_update_con, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            return res;
        }
    }
}
