using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemInstanceUnidentified : PacketResponse
    {
        private readonly ItemInstance _itemInstance;
        private readonly NecClient _client;

        public RecvItemInstanceUnidentified(NecClient client, ItemInstance itemInstance)
            : base((ushort) AreaPacketId.recv_item_instance_unidentified, ServerType.Area)
        {
            _itemInstance = itemInstance;
            _client = client;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64((ulong) _itemInstance.InstanceID);              //item instance ID
            res.WriteCString(_itemInstance.UnidentifiedName);               //unidentified item name 
            res.WriteInt32((int) _itemInstance.Type);                       //item type
            res.WriteInt32((int) _itemInstance.EquipAllowedSlots);          //allowed equipment slots
            res.WriteByte(_itemInstance.Quantity);

            res.WriteInt32((int)_itemInstance.Statuses);                    //statuses
            //BEGIN ITEM  UPDATE EQUMASK SECTION
            res.WriteInt32(_itemInstance.BaseID); //Sets your Item ID per Iteration
            res.WriteByte(0); //hair
            res.WriteByte(0); //color
            res.WriteByte(0); //face

            res.WriteInt32(_itemInstance.BaseID); //testing (Theory, Icon related)
            res.WriteByte(0); //hair
            res.WriteByte(0); //color
            res.WriteByte(0); //face

            res.WriteByte(0); // Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
            
            res.WriteByte(10); //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
            res.WriteByte((byte)Util.GetRandomNumber(0, 0)); // alternate model?  equip_ref.csv
            res.WriteByte((byte)Util.GetRandomNumber(0, 0)); // id-n alternate tex?
            res.WriteByte((byte)Util.GetRandomNumber(0, 0)); // id-c alternate tex?
            res.WriteByte((byte)Util.GetRandomNumber(0, 0)); // id-s alternate tex?
            res.WriteByte(0); //Alternate texture for item model  0 normal : 1 Pink 

            res.WriteByte((byte)Util.GetRandomNumber(0, 0)); // separate in assembly
            res.WriteByte((byte)Util.GetRandomNumber(0, 0)); // separate in assembly


            res.WriteByte((byte)_itemInstance.Location.ZoneType); 
            res.WriteByte(_itemInstance.Location.Container); 
            res.WriteInt16(_itemInstance.Location.Slot);
            res.WriteInt32((int)_itemInstance.CurrentEquipSlot); //CURRENT EQUIP SLOT
            res.WriteInt64(0); //unknown
            res.WriteUInt32(1);//unknown
            res.WriteInt16(5);//unknown
            res.WriteUInt32(1); //unknown

            res.WriteFixedString($"", 16); //unknown

            return res;
        }
    }
}
