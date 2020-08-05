using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_forge_check : ClientHandler
    {
        public send_forge_check(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_forge_check;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte storageType = packet.Data.ReadByte();
            byte Bag = packet.Data.ReadByte();
            short Slot = packet.Data.ReadInt16();
            //14 bytes left


            InventoryItem inventoryItem = client.Character.Inventory.GetInventoryItem(storageType,Bag, Slot);
            Server.SettingRepository.ItemLibrary.TryGetValue(inventoryItem.Item.Id, out ItemLibrarySetting itemLibrarySetting);

                IBuffer res = BufferProvider.Provide();

	        res.WriteInt32(0); //err check
            res.WriteInt64(inventoryItem.Id); 
            res.WriteByte(1/*item count???*/);
            res.WriteFloat(inventoryItem.Item.Physical);
            res.WriteFloat(inventoryItem.Item.Magical);
            res.WriteInt32(itemLibrarySetting.Durability/*Max Durability*/);
            res.WriteByte((byte)itemLibrarySetting.Hardness);
            res.WriteFloat(6/*PhysicalAttack after successful upgrade*/);
            res.WriteFloat(7/*Magical Attack after successful upgrade*/);
            res.WriteInt32(8/*Max Durability after successful upgrade*/);
            res.WriteByte(22/*Hardness after successfull upgrade*/);
            res.WriteInt32(500/*Weight after successful upgrade. times 10*/);
            res.WriteInt16(11 /*GP after upgrade?*/);

            Router.Send(client, (ushort) AreaPacketId.recv_forge_check_r, res, ServerType.Area);
        }
    }
}
