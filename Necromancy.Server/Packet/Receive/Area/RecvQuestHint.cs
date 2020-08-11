using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvQuestHint : PacketResponse
    {
        public RecvQuestHint()
            : base((ushort) AreaPacketId.recv_quest_hint, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            res.WriteInt32(0);

            res.WriteFloat(0);
            res.WriteFloat(0);
            res.WriteFloat(0);

            res.WriteInt32(0);
            return res;
        }
    }
}
