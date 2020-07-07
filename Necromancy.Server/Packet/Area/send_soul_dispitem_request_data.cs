using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;
using System.Collections.Generic;

namespace Necromancy.Server.Packet.Area
{
    public class send_soul_dispitem_request_data : ClientHandler
    {
        public send_soul_dispitem_request_data(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_soul_dispitem_request_data;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);            

            Router.Send(client, (ushort) AreaPacketId.recv_soul_dispitem_request_data_r, res, ServerType.Area);

            //notify you of the soul item you got based on something above.
            IBuffer res19 = BufferProvider.Provide();
            res19.WriteInt32(Util.GetRandomNumber(62000001, 62000015)); //soul_dispitem.csv
            Router.Send(client, (ushort)AreaPacketId.recv_soul_dispitem_notify_data, res19, ServerType.Area);

            //populate soul and character inventory from database.
            List<InventoryItem> inventoryItems = Server.Database.SelectInventoryItemsByCharacterId(client.Character.Id);
            foreach (InventoryItem inventoryItem in inventoryItems)
            {
                Item item = Server.Items[inventoryItem.ItemId];
                inventoryItem.Item = item;       

                RecvItemInstanceUnidentified recvItemInstanceUnidentified = new RecvItemInstanceUnidentified(inventoryItem);
                Router.Send(recvItemInstanceUnidentified, client);

                client.Inventory.LoginLoadInventory(inventoryItem);

                //equip items with state flag set to 1 to your paperdoll.
                //if (inventoryItem.State == 1)
                //{
                //    inventoryItem.CurrentEquipmentSlotType = inventoryItem.Item.EquipmentSlotType;

                //    RecvItemUpdateEqMask recvItemUpdateEqMask = new RecvItemUpdateEqMask(inventoryItem);
                //    Router.Send(recvItemUpdateEqMask, client);

                //    res.WriteInt32((int)ItemActionResultType.Ok);
                //    Router.Send(client, (ushort)AreaPacketId.recv_item_equip_r, res, ServerType.Area);
                //}
            }

        }
    }
}
