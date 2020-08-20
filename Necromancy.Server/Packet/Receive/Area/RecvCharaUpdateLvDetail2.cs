using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateLvDetail2 : PacketResponse
    {
        public RecvCharaUpdateLvDetail2()
            : base((ushort) AreaPacketId.recv_chara_update_lv_detail2, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt16(0); // level
            res.WriteInt64(0); // exp start
            res.WriteInt64(0); // exp next
            res.WriteInt64(0); // exp next 2
            return res;
        }
    }
}
