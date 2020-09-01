using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemUpdateEqMask : PacketResponse
    {
        private readonly InventoryItem _inventoryItem;

        public RecvItemUpdateEqMask(InventoryItem inventoryItem)
            : base((ushort) AreaPacketId.recv_item_update_eqmask, ServerType.Area)
        {
            _inventoryItem = inventoryItem;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64((ulong) _inventoryItem.Id);
            res.WriteInt32((int) _inventoryItem.CurrentEquipmentSlotType);

            res.WriteInt32(_inventoryItem.Item.Id); //Sets your Item ID per Iteration
            res.WriteByte(0); //hair
            res.WriteByte(0); //color
            res.WriteByte(0); //face

            res.WriteInt32(_inventoryItem.Item.Id); //testing (Theory, Icon related)
            res.WriteByte(0); //hair
            res.WriteByte(0); //color
            res.WriteByte(0); //face

            res.WriteByte(0); // Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
            res.WriteByte(10); //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
            res.WriteByte((byte)Util.GetRandomNumber(1, 5)); // testing (Theory Torso Tex)
            res.WriteByte((byte)Util.GetRandomNumber(1, 5)); // testing (Theory Pants Tex)
            res.WriteByte((byte)Util.GetRandomNumber(1, 5)); // testing (Theory Hands Tex)
            res.WriteByte((byte)Util.GetRandomNumber(1, 5)); // testing (Theory Feet Tex)
            res.WriteByte(0); //Alternate texture for item model  0 normal : 1 Pink 

            res.WriteByte((byte)Util.GetRandomNumber(1, 5)); // separate in assembly
            res.WriteByte((byte)Util.GetRandomNumber(1, 5)); // separate in assembly

            return res;
        }
    }
}
