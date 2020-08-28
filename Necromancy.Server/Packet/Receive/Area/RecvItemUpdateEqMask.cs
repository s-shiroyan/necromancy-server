using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Items;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemUpdateEqMask : PacketResponse
    {
        private readonly SpawnedItem _spawnedItem;
        public RecvItemUpdateEqMask(NecClient client, SpawnedItem spawnedItem)
            : base((ushort) AreaPacketId.recv_item_update_eqmask, ServerType.Area)
        {
            _spawnedItem = spawnedItem;
            Clients.Add(client);
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64( _spawnedItem.SpawnId);
            res.WriteInt32((int) _spawnedItem.CurrentEquipSlot);

            res.WriteInt32( _spawnedItem.BaseId); //TODO unknown test
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteInt32( _spawnedItem.BaseId); //todo unknown test
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0); //bool
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            return res;
        }
    }
}
