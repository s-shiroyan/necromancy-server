using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemRemove : PacketResponse
    {
        private readonly long _spawnId;
        public RecvItemRemove(NecClient client, long spawnId)
            : base((ushort) AreaPacketId.recv_item_remove, ServerType.Area)
        {
            _spawnId = spawnId;
            Clients.Add(client);
        }
        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(_spawnId); 
            return res;
        }
    }
}
