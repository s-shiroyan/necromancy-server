using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Items;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemUpdateDurability : PacketResponse
    {
        private readonly SpawnedItem _durabilityChangedItem;
        public RecvItemUpdateDurability(NecClient client, SpawnedItem durabilityChangedItem)
            : base((ushort) AreaPacketId.recv_item_update_durability, ServerType.Area)
        {
            _durabilityChangedItem = durabilityChangedItem;
            Clients.Add(client);
        }
        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(_durabilityChangedItem.SpawnId);
            res.WriteInt32(_durabilityChangedItem.CurrentDurability);
            return res;
        }
    }
}
