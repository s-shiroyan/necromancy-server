using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvCashGetUrlCommonSteam : PacketResponse
    {
        public RecvCashGetUrlCommonSteam()
            : base((ushort) MsgPacketId.recv_cash_get_url_common_steam_r, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteCString("");//max size is 0x801
            res.WriteCString("");//max size is 0x801
            res.WriteCString("");//max size 0x11
            return res;
        }
    }
}
