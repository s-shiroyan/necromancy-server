using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSelfBuffNotify : PacketResponse
    {
        public RecvSelfBuffNotify()
            : base((ushort) AreaPacketId.recv_self_buff_notify, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0x80;
            res.WriteInt32(numEntries);//less than or equal to 0x80
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
            }
            return res;
        }
    }
}
