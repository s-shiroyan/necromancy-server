using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_chara_skill_combo_free : PacketResponse
    {
        public recv_chara_skill_combo_free()
            : base((ushort) AreaPacketId.recv_chara_skill_combo_free, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);

            return res;
        }
    }
}
