using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_battle_attack_long_update_load_num : PacketResponse
    {
        public recv_battle_attack_long_update_load_num()
            : base((ushort) AreaPacketId.recv_battle_attack_long_update_load_num, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            return res;
        }
    }
}
