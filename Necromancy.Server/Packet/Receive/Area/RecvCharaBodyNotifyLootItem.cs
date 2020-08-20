using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodyNotifyLootItem : PacketResponse
    {
        public RecvCharaBodyNotifyLootItem()
            : base((ushort) AreaPacketId.recv_charabody_notify_loot_item, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteInt16(0);

            res.WriteInt16(0); //Number here is "pieces" 
            res.WriteCString("item name"); // Length 0x31 
            res.WriteCString("chara name"); // Length 0x5B
            return res;
        }
    }
}
