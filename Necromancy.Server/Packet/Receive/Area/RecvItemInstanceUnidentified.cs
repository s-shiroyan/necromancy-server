using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemInstanceUnidentified : PacketResponse
    {
        private readonly InventoryItem _inventoryItem;
        private readonly NecClient _client;

        public RecvItemInstanceUnidentified(InventoryItem inventoryItem,NecClient client)
            : base((ushort) AreaPacketId.recv_item_instance_unidentified, ServerType.Area)
        {
            _inventoryItem = inventoryItem;
            _client = client;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64((ulong) _inventoryItem.Id); //item instance ID
            res.WriteCString(_inventoryItem.Item.Name); //item name 
            res.WriteInt32((int) _inventoryItem.Item.ItemType); //item type
            res.WriteInt32((int) _inventoryItem.Item.EquipmentSlotType); //equipment slot type
            res.WriteByte(_inventoryItem.Quantity);

            res.WriteInt32((int)Status.Normal); //statuses bitmask /* 10001003 Put The Item Unidentified. 0 put the item Identified 1-2-4-8-16 follow this patterns (8 cursed, 16 blessed)*/
            //BEGIN ITEM  UPDATE EQUMASK SECTION
            res.WriteInt32(_inventoryItem.Item.Id); //Item icon 50100301 = camp
            res.WriteByte((byte)_inventoryItem.Item.Id.ToString()[0]);
            res.WriteByte((byte)_inventoryItem.Item.Id.ToString()[1]);
            res.WriteByte((byte)_inventoryItem.Item.Id.ToString()[2]);

            res.WriteInt32(_inventoryItem.Item.Id);
            res.WriteByte((byte)_inventoryItem.Item.Id.ToString()[0]);
            res.WriteByte((byte)_inventoryItem.Item.Id.ToString()[1]);
            res.WriteByte((byte)_inventoryItem.Item.Id.ToString()[2]);

            res.WriteByte(0); // Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
            res.WriteByte(0); //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
            res.WriteByte(1); // bool
            res.WriteByte((byte)_inventoryItem.Item.Id.ToString()[0]);
            res.WriteByte((byte)_inventoryItem.Item.Id.ToString()[1]);
            res.WriteByte((byte)_inventoryItem.Item.Id.ToString()[2]);
            res.WriteByte(0); //texture related
            res.WriteByte(0);//new
            res.WriteByte(0);//new

            //res.WriteByte((byte)_client.Character.Alignmentid);

            res.WriteByte(_inventoryItem.StorageType); // 0 = adventure bag. 1 = character equipment 2 = Royal bag.
            res.WriteByte(_inventoryItem.BagId); // 0~2
            res.WriteInt16(_inventoryItem.BagSlotIndex);
            res.WriteInt32(_inventoryItem.State); //bit mask. This indicates where to put items.   e.g. 01 head 010 arm 0100 feet etc (0 for not equipped) TODO - change State in database to be this bitmask value
            res.WriteInt64(_client.Character.Alignmentid);
            res.WriteInt16(0);//new
            res.WriteUInt32(_client.Character.Alignmentid);

            res.WriteFixedString("", 16);

            return res;
        }
    }
}
