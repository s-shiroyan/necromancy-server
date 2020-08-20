using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSelfDragonPosNotify : PacketResponse
    {
        public RecvSelfDragonPosNotify()
            : base((ushort) AreaPacketId.recv_self_dragon_pos_notify, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            res.WriteFloat(1);
            res.WriteFloat(1);
            res.WriteFloat(1);

            res.WriteByte(1);
            return res;
        }
    }
}