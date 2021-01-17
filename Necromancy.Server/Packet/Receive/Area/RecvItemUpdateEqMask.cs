using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemUpdateEqMask : PacketResponse
    {
        private readonly ItemInstance _itemInstance;

        public RecvItemUpdateEqMask(NecClient client, ItemInstance itemInstance)
            : base((ushort) AreaPacketId.recv_item_update_eqmask, ServerType.Area)
        {
            _itemInstance = itemInstance;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64(_itemInstance.InstanceID);
            res.WriteInt32((int)_itemInstance.CurrentEquipSlot);

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
            res.WriteByte(1); // testing (Theory Torso Tex)
            res.WriteByte(1); // testing (Theory Pants Tex)
            res.WriteByte(1); // testing (Theory Hands Tex)
            res.WriteByte(1); // testing (Theory Feet Tex)
            res.WriteByte(0); //Alternate texture for item model  0 normal : 1 Pink 

            res.WriteByte(0); // separate in assembly
            res.WriteByte(0); // separate in assembly

            return res;
        }
    }
}
