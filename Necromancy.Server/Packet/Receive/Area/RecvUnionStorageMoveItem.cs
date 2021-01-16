using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvUnionStorageMoveItem : PacketResponse
    {
        private readonly int _error;
        public RecvUnionStorageMoveItem(NecClient client, int error)
            : base((ushort) AreaPacketId.recv_union_storage_move_item_r, ServerType.Area)
        {
            _error = error;
            Clients.Add(client);
        }
        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_error);
            return res;
        }
    }
}
