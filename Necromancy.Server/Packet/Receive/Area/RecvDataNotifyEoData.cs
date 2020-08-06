using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvDataNotifyEoData : PacketResponse
    {
        public RecvDataNotifyEoData()
            : base((ushort) AreaPacketId.recv_data_notify_eo_data, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteFloat(0);//x
            res.WriteFloat(0);//y
            res.WriteFloat(0);//z

            res.WriteFloat(0);//x
            res.WriteFloat(0);//y
            res.WriteFloat(0);//z

            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);

            res.WriteInt32(0);
            return res;
        }
    }
}
