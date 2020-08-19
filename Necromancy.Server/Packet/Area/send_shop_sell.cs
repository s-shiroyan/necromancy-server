using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_shop_sell : ClientHandler
    {
        public send_shop_sell(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_shop_sell;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte fromStoreType = packet.Data.ReadByte();
            byte fromBagId = packet.Data.ReadByte();
            short fromSlot = packet.Data.ReadInt16();
            long saleGold = packet.Data.ReadInt64();
            byte unknown = packet.Data.ReadByte();

            InventoryItem inventoryItem = client.Character.Inventory.GetInventoryItem(fromStoreType, fromBagId, fromSlot);
            if (inventoryItem == null)
            {
                IBuffer res = BufferProvider.Provide();
                res.WriteInt32((int)ItemActionResultType.ErrorGeneric);
                Router.Send(client, (ushort)AreaPacketId.recv_shop_sell_r, res, ServerType.Area);
                return;
            }
            else
            {
                IBuffer res = BufferProvider.Provide();
                res.WriteInt32((int)ItemActionResultType.Ok);
                Router.Send(client, (ushort)AreaPacketId.recv_shop_sell_r, res, ServerType.Area);

                client.Character.AdventureBagGold += saleGold; //Updates your Character.AdventureBagGold

                IBuffer res2 = BufferProvider.Provide();
                res2.WriteInt64(client.Character.AdventureBagGold); // Sets your Adventure Bag Gold
                Router.Send(client, (ushort)AreaPacketId.recv_self_money_notify, res2, ServerType.Area);
                client.Character.Inventory.RemoveInventoryItem(inventoryItem);

                res = BufferProvider.Provide();
                res.WriteInt64(inventoryItem.Id);
                Router.Send(client, (ushort)AreaPacketId.recv_item_remove, res, ServerType.Area);

                Server.Database.DeleteItem(inventoryItem.Id);
            }
        }

    }
}
