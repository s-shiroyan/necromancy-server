using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemUpdateHardness : PacketResponse
    {
        public RecvItemUpdateHardness()
            : base((ushort) AreaPacketId.recv_item_update_hardness, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0);
            res.WriteByte(0);
            return res;
        }
    }
}
