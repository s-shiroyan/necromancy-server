using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvMapChangeForce : PacketResponse
    {
        public RecvMapChangeForce()
            : base((ushort) AreaPacketId.recv_map_change_force, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteFixedString("", 65);//IP
            res.WriteInt16(0);//Port

            res.WriteFloat(0);//x coord
            res.WriteFloat(0);//y coord
            res.WriteFloat(0);//z coord
            res.WriteByte(0);//view offset maybe?
            return res;
        }
    }
}
