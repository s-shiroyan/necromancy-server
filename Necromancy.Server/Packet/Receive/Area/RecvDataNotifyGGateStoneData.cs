using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvDataNotifyGGateStoneData : PacketResponse
    {
        public RecvDataNotifyGGateStoneData()
            : base((ushort) AreaPacketId.recv_data_notify_ggate_stone_data, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);//
            res.WriteInt32(0);//
            res.WriteByte(0);//
            res.WriteCString("");//"0x5B"
            res.WriteCString("");//"0x5B"
            res.WriteFloat(0);//X
            res.WriteFloat(0);//Y
            res.WriteFloat(0);//Z
            res.WriteByte(0);//
            res.WriteInt32(0);//

            res.WriteInt16(0);//  BP Set


            res.WriteInt32(0);//

            res.WriteInt32(0);//
            return res;
        }
    }
}
