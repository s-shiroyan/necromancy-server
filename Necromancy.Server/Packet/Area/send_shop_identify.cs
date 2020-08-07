using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_shop_identify : ClientHandler
    {
        public send_shop_identify(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort)AreaPacketId.send_shop_identify;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //err check. 0 for success
            Router.Send(client, (ushort)AreaPacketId.recv_shop_identify_r, res, ServerType.Area);


            byte storageType = packet.Data.ReadByte();
            byte Bag = packet.Data.ReadByte();
            short Slot = packet.Data.ReadInt16();
            //9 bytes left

            InventoryItem inventoryItem = client.Character.Inventory.GetInventoryItem(storageType, Bag, Slot);

            RecvItemInstanceUnidentified recvItemInstanceUnidentified = new RecvItemInstanceUnidentified(inventoryItem);
            Router.Send(recvItemInstanceUnidentified, client);

            itemStats(inventoryItem, client);
        }
        public void itemStats(InventoryItem inventoryItem, NecClient client)
        {
            Server.SettingRepository.ItemLibrary.TryGetValue(inventoryItem.Item.Id, out ItemLibrarySetting itemLibrarySetting);
            if (itemLibrarySetting == null) return;

            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteInt32(itemLibrarySetting.Durability); // MaxDura points
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_maxdur, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteInt32(itemLibrarySetting.Durability - 10); // Durability points
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_durability, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteInt32((int)itemLibrarySetting.Weight+1  * 100 ); // Weight points
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_weight, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteInt16((short)itemLibrarySetting.PhysicalAttack); // Defense and attack points
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_physics, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteInt16((short)itemLibrarySetting.MagicalAttack); // Magic def and attack Points
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_magic, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteInt32(1); // for the moment i don't know what it change
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_enchantid, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteInt16((short)Util.GetRandomNumber(50, 100)); // Shwo GP on certain items
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_ac, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteInt32(1); // for the moment i don't know what it change
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_date_end_protect, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteByte((byte)itemLibrarySetting.Hardness); // Hardness
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_hardness, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteByte(1); //Level requirement
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_level, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteByte(0); //sp Level requirement
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_sp_level, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(inventoryItem.Id);
            res.WriteInt32(0b000000); // State bitmask
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_state, res, ServerType.Area);

        }



    }
}
