using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;

namespace Necromancy.Server.Packet.Area
{
    public class send_item_equip : ClientHandler
    {
        public send_item_equip(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_item_equip;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte storageType = packet.Data.ReadByte();
            byte bagId = packet.Data.ReadByte();//Equip slot maybe?
            short backpackSlot = packet.Data.ReadInt16();//Slot from backpack the item is in
            int equipBit = packet.Data.ReadInt32();
            Logger.Debug($"storageType: [{storageType}] bagId: [{bagId}]  backpackSlot: [{backpackSlot}] equipBit: [{equipBit}]");
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            Router.Send(client, (ushort) AreaPacketId.recv_item_equip_r, res, ServerType.Area);

            //InventoryItem invItem = client.Character.GetInventoryItem(storageType, bagId, backpackSlot);
            //RecvItemUpdateEqMask eqMask = new RecvItemUpdateEqMask(invItem.StorageItem.InstanceId);
            //Router.Send(eqMask, client);
        }
    }
}
