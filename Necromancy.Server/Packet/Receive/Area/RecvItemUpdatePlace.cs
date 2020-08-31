using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemUpdatePlace : PacketResponse
    {
        private readonly SpawnedItem _movedItem;
        public RecvItemUpdatePlace(NecClient client, SpawnedItem movedItem)
            : base((ushort) AreaPacketId.recv_item_update_place, ServerType.Area)
        {
            _movedItem = movedItem;
            Clients.Add(client);
        }
        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(_movedItem.SpawnId);
            res.WriteByte((byte)_movedItem.Location.Zone);
            res.WriteByte(_movedItem.Location.Bag);
            res.WriteInt16(_movedItem.Location.Slot);
            return res;
        }
    }
}
