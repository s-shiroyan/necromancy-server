using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemUpdatePhysics : PacketResponse
    {
        public RecvItemUpdatePhysics()
            : base((ushort) AreaPacketId.recv_item_update_physics, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0); //item instance id

            res.WriteInt16(0); //item's attack stat
            return res;
        }
    }
}
