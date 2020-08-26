using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Items;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvItemInstance : PacketResponse
    {
        private readonly InventoryItem _inventoryItem;
        private readonly NecClient _client;

        public RecvItemInstance(InventoryItem inventoryItem, NecClient client)
            : base((ushort) AreaPacketId.recv_item_instance, ServerType.Area)
        {
            _inventoryItem = inventoryItem;
            _client = client;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0);                               //spawned item iD
            res.WriteInt32(0);                               //item id
            res.WriteByte((byte) 1);                         //quantity
            res.WriteInt32((int)ItemStatuses.Identified);             //Item status
            res.WriteFixedString("", 16);                    //unknown
            res.WriteByte((byte)(ItemZone.AdventureBag));    //item zone
            res.WriteByte((byte)0);                          //bag number                
            res.WriteInt16((short)0);                         //bag slot or slot for bags to go in
            res.WriteInt32((int)ItemEquipSlot.None);         //slot equipped to
            res.WriteInt32((int) 0);                         //current durability
            res.WriteByte((byte) 0);                         //enhancement level +1, +2 etc
            res.WriteByte(0);                                //special forge level, must be less than or equal the the req special forge level in table
            res.WriteCString("");                            //unknown
            res.WriteInt16((short) 0);                       //phys attr (attack, def)
            res.WriteInt16((short) 0);                       //mag attr (mattack, mdef)
            res.WriteInt32((int) 0);                         //maximum durability
            res.WriteByte((byte) 0);                         //hardness
            res.WriteInt32((int) 0);                         //unknown

            const int MAX_WHATEVER_SLOTS = 2;
            const int numEntries = 2;
            res.WriteInt32(numEntries);                  //less than or equal to 2?
            for (int j = 0; j < numEntries; j++)
            {
                res.WriteInt32((byte)    0);                 //unknown
            }

            const int MAX_GEM_SLOTS = 3;
            const int numGemSlots = 1;
            res.WriteInt32(numGemSlots);                 //number of gem slots
            for (int j = 0; j < numGemSlots; j++)
            {
                res.WriteByte(0);                        //is gem slot filled
                res.WriteInt32(0);                       //slot type 1 round, 2 triangle, 3 diamond
                res.WriteInt32(0);                       //gem item id
                res.WriteInt32(0);                       //maybe gem item 2 id for diamon 2 gem combine
            }

            res.WriteInt32(0);                           //unknown
            res.WriteInt32(0);                           //unknown
            res.WriteInt16((short)0);                    //unknown
            res.WriteInt32(0);                           //enchant id 
            res.WriteInt16((short)0);                    //GP
            return res;
        }
    }
}
