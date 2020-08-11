using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_shop_sell_check : ClientHandler
    {
        public send_shop_sell_check(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_shop_sell_check;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte storageType = packet.Data.ReadByte();
            byte Bag = packet.Data.ReadByte();
            short Slot = packet.Data.ReadInt16();
            //9 bytes left


            InventoryItem inventoryItem = client.Character.Inventory.GetInventoryItem(storageType, Bag, Slot);
            Server.SettingRepository.ItemLibrary.TryGetValue(inventoryItem.Item.Id, out ItemLibrarySetting itemLibrarySetting);

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_shop_sell_check_r, res, ServerType.Area);


            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id); // id?
            res.WriteInt64(10200101); // price?
            res.WriteInt64(10200101); // identify?
            res.WriteInt64(10200101); // curse?
            res.WriteInt64(10200101); // repair?
            Router.Send(client, (ushort)AreaPacketId.recv_shop_notify_item_sell_price, res, ServerType.Area);
        }

    }
}
