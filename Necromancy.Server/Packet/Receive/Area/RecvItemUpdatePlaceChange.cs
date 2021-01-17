using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemUpdatePlaceChange : PacketResponse
    {
        private readonly ItemInstance _movedItem;
        private readonly ItemInstance _movedItemSwap;
        public RecvItemUpdatePlaceChange(NecClient client, ItemInstance movedItem, ItemInstance movedItemSwap)
            : base((ushort) AreaPacketId.recv_item_update_place_change, ServerType.Area)
        {
            _movedItem = movedItem;
            _movedItemSwap = movedItemSwap;
            Clients.Add(client);
        }
        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64(_movedItem.InstanceID);
            res.WriteByte((byte)_movedItem.Location.Zone);
            res.WriteByte(_movedItem.Location.Bag);
            res.WriteInt16(_movedItem.Location.Slot);
            res.WriteUInt64(_movedItemSwap.InstanceID);
            res.WriteByte((byte)_movedItemSwap.Location.Zone);
            res.WriteByte(_movedItemSwap.Location.Bag);
            res.WriteInt16(_movedItemSwap.Location.Slot);
            return res;
        }
    }
}
