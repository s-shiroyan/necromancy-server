using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class Send_shop_repair : ClientHandler
    {
        public Send_shop_repair(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort)AreaPacketId.send_shop_repair;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int repairCount = packet.Data.ReadInt32();
            for (int i = 0; i < repairCount; i++)
            {
                byte storageType = packet.Data.ReadByte();
                byte Bag = packet.Data.ReadByte();
                short Slot = packet.Data.ReadInt16();

                InventoryItem inventoryItem = client.Character.Inventory.GetInventoryItem(storageType, Bag, Slot);

                IBuffer res3 = BufferProvider.Provide();
                res3.WriteInt64(inventoryItem.Id);
                res3.WriteInt32(inventoryItem.Item.Durability); // Current durability points
                Router.Send(client, (ushort)AreaPacketId.recv_item_update_durability, res3, ServerType.Area);
                //When repairing this should be item's max durability.
                inventoryItem.Item.Durability += 10;
            }
            long repairFee = packet.Data.ReadInt64();

            client.Character.AdventureBagGold -= repairFee;

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt64(client.Character.AdventureBagGold); // Sets your Adventure Bag Gold
            Router.Send(client, (ushort)AreaPacketId.recv_self_money_notify, res2, ServerType.Area);


            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort)AreaPacketId.recv_shop_repair_r, res, ServerType.Area);
 

        }

    }
}
