using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemInstanceUnidentified : PacketResponse
    {
        public RecvItemInstanceUnidentified()
            : base((ushort) AreaPacketId.recv_item_instance_unidentified, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0);

            res.WriteCString("ToBeFound");

            res.WriteInt32(0);

            res.WriteInt32(0);

            res.WriteByte(0);

            res.WriteInt32(0);

            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(1); // bool
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteByte(0);

            res.WriteByte(0);

            res.WriteInt16(0);

            res.WriteInt32(0);

            res.WriteInt64(0);

            res.WriteInt32(0);
            return res;
        }
    }
}
