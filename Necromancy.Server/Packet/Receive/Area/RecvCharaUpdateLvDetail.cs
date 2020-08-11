using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateLvDetail : PacketResponse
    {
        public RecvCharaUpdateLvDetail()
            : base((ushort) AreaPacketId.recv_chara_update_lv_detail, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt16(0); // new level

            res.WriteInt64(0); // start exp

            res.WriteInt64(0); // exp needed for next level
            return res;
        }
    }
}
