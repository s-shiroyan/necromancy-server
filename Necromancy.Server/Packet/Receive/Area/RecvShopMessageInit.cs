using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvShopMessageInit : PacketResponse
    {
        public RecvShopMessageInit()
            : base((ushort) AreaPacketId.recv_shop_message_init, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            
            return res;
        }
    }
}
