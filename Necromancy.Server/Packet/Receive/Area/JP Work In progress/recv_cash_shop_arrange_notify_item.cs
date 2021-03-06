using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_cash_shop_arrange_notify_item : PacketResponse
    {
        public recv_cash_shop_arrange_notify_item()
            : base((ushort) AreaPacketId.recv_cash_shop_arrange_notify_item, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt16(0);
            res.WriteInt32(0);
            res.WriteFixedString("Xeno", 0x10);
            res.WriteInt32(0);
            res.WriteCString("What");
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteInt16(0);
            return res;
        }
    }
}
