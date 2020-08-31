using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;
using System;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvItemInstance : PacketResponse
    {        
        private readonly SpawnedItem _spawnedItem;

        public RecvItemInstance(NecClient client, SpawnedItem spawnedItem)
            : base((ushort) AreaPacketId.recv_item_instance, ServerType.Area)
        {
            if (!spawnedItem.IsIdentified) throw new ArgumentException("Spawned item must be identified.");
            _spawnedItem.Statuses |= ItemStatuses.Identified;
            _spawnedItem = spawnedItem;
            Clients.Add(client);
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64( _spawnedItem.SpawnId);          
            res.WriteInt32( _spawnedItem.BaseId);                               
            res.WriteByte(  _spawnedItem.Quantity);                        
            res.WriteInt32((int) _spawnedItem.Statuses);    
            res.WriteFixedString("", 16);                    //unknown
            res.WriteByte((byte) _spawnedItem.Location.Zone);    
            res.WriteByte(  _spawnedItem.Location.Bag);                          
            res.WriteInt16( _spawnedItem.Location.Slot);                      
            res.WriteInt32((int) _spawnedItem.CurrentEquipSlot);         
            res.WriteInt32( _spawnedItem.CurrentDurability);                         
            res.WriteByte(  _spawnedItem.EnhancementLevel);                         
            res.WriteByte(  _spawnedItem.SpecialForgeLevel);                                
            res.WriteCString("");                            //unknown
            res.WriteInt16( _spawnedItem.Physical);                       
            res.WriteInt16( _spawnedItem.Magical);                       
            res.WriteInt32( _spawnedItem.MaximumDurability);                        
            res.WriteByte(  _spawnedItem.Hardness);                         
            res.WriteInt32((int) 0);                         //unknown

            const int MAX_WHATEVER_SLOTS = 2;
            const int numEntries = 2;
            res.WriteInt32(numEntries);                  //less than or equal to 2?
            for (int j = 0; j < numEntries; j++)
            {
                res.WriteInt32((byte)    0);                 //unknown
            }

            res.WriteInt32(_spawnedItem.GemSlots.Length);                
            for (int j = 0; j < _spawnedItem.GemSlots.Length; j++)
            {
                res.WriteByte(Convert.ToByte(_spawnedItem.GemSlots[j].IsFilled));                       
                res.WriteInt32((int)_spawnedItem.GemSlots[j].Type);                       
                res.WriteInt32(_spawnedItem.GemSlots[j].Gem.BaseId);                       
                res.WriteInt32(0);                       //maybe gem item 2 id for diamon 2 gem combine
            }

            res.WriteInt32(0);                           //unknown
            res.WriteInt32(0);                           //unknown
            res.WriteInt16((short)0);                    //unknown
            res.WriteInt32(_spawnedItem.EnchantId);                           //enchant id 
            res.WriteInt16(_spawnedItem.GP);                    //GP
            return res;
        }
    }
}
