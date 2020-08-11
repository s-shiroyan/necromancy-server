using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodyNotifyLootStartCancel : PacketResponse
    {
        public RecvCharaBodyNotifyLootStartCancel()
            : base((ushort) AreaPacketId.recv_charabody_notify_loot_start_cancel, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteInt16(0);
            res.WriteCString("");//Length 31-2=DEC47
            res.WriteCString("");//Length 5B-1=DEC90
            return res;
        }
    }
}
