using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Items;

namespace Necromancy.Server.Packet.Area
{
    public class send_forge_check : ClientHandler
    {
        public send_forge_check(NecServer server) : base(server) { }
        public override ushort Id => (ushort) AreaPacketId.send_forge_check;
        public override void Handle(NecClient client, NecPacket packet)
        {
            byte storageType = packet.Data.ReadByte();
            byte Bag = packet.Data.ReadByte();
            short Slot = packet.Data.ReadInt16();
            //14 bytes left
            //TODO

            ItemService itemService = new ItemService(client.Character);
         //   InventoryItem inventoryItem = client.Character.Inventory.GetInventoryItem(storageType,Bag, Slot);
         //   Server.SettingRepository.ItemLibrary.TryGetValue(inventoryItem.Item.Id, out ItemLibrarySetting itemLibrarySetting);

         //       IBuffer res = BufferProvider.Provide();

	        //res.WriteInt32(0); //err check
         //   res.WriteInt64(inventoryItem.Id); 
         //   res.WriteByte(1/*item count???*/);
         //   res.WriteFloat(inventoryItem.Item.Physical);
         //   res.WriteFloat(inventoryItem.Item.Magical);
         //   res.WriteInt32(itemLibrarySetting.Durability/*Max Durability*/);
         //   res.WriteByte((byte)itemLibrarySetting.Hardness);
         //   res.WriteFloat(100/*PhysicalAttack after successful upgrade*/);
         //   res.WriteFloat(101/*Magical Attack after successful upgrade*/);
         //   res.WriteInt32(102/*Max Durability after successful upgrade*/);
         //   res.WriteByte(103/*Hardness after successfull upgrade*/);
         //   res.WriteInt32(1000/*Weight after successful upgrade. times 10*/);
         //   res.WriteInt16(104 /*GP after upgrade?*/);

            //Router.Send(client, (ushort) AreaPacketId.recv_forge_check_r, res, ServerType.Area);
        }
    }
}
