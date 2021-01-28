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
            Clients.Add(client);
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64(_itemInstance.InstanceID);
            res.WriteInt32((int)_itemInstance.CurrentEquipSlot);

            res.WriteInt32(220301); //Sets your Item ID per Iteration
            res.WriteByte(00); //? TYPE data/chara/##/ 00 is character model, 01 is npc, 02 is monster
            res.WriteByte(12); //Race and gender tens place is race 1= human, 2= elf 3=dwarf 4=gnome 5=porkul, ones is gender 1 = male 2 = female
            res.WriteByte(00); //??item version

            res.WriteInt32(220101); //testing (Theory, Icon related)
            res.WriteByte(0); //hair
            res.WriteByte(12); //color
            res.WriteByte(0); //face

            res.WriteByte(45); // Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
            res.WriteByte(30); //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
            res.WriteByte(00); // testing (Theory Torso Tex)
            res.WriteByte(0); // testing (Theory Pants Tex)
            res.WriteByte(0); // testing (Theory Hands Tex)
            res.WriteByte(0); // testing (Theory Feet Tex)
            res.WriteByte(1); //Alternate texture for item model  0 normal : 1 Pink 

            res.WriteByte(0); // separate in assembly
            res.WriteByte(0); // separate in assembly

            return res;
        }
    }
}
