using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;
using System;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemInstanceUnidentified : PacketResponse
    {
        private readonly SpawnedItem _spawnedItem;
        public RecvItemInstanceUnidentified(NecClient client, SpawnedItem spawnedItem)
            : base((ushort) AreaPacketId.recv_item_instance_unidentified, ServerType.Area)
        {
            if (spawnedItem.IsIdentified) throw new ArgumentException("Spawned item must be unidentified.");
            _spawnedItem.Statuses |= ItemStatuses.Unidentified;
            _spawnedItem = spawnedItem;
            Clients.Add(client);
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64( _spawnedItem.SpawnId); 
            res.WriteCString( _spawnedItem.UnidentifiedName); 
            res.WriteInt32((int) _spawnedItem.Type); 
            res.WriteInt32((int) _spawnedItem.EquipSlot);
            res.WriteByte(_spawnedItem.Quantity);
            res.WriteInt32((int) _spawnedItem.Statuses);             
            res.WriteInt32( _spawnedItem.IconId); 

            res.WriteByte((byte)0); //unknown
            res.WriteByte((byte)0); //unknown
            res.WriteByte((byte)0); //unknown

            res.WriteInt32(0); //unknown
            res.WriteByte((byte)0); //unknown
            res.WriteByte((byte)0); //unknown
            res.WriteByte((byte)0); //unknown

            res.WriteByte(0); // Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
            res.WriteByte(0); //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
            res.WriteByte(1); // bool
            res.WriteByte((byte)0); //unknown
            res.WriteByte((byte)0); //unknown
            res.WriteByte((byte)0); //unknown
            res.WriteByte(0); //texture related

            res.WriteByte((byte)0); //unknown

            res.WriteByte((byte) _spawnedItem.Location.Zone); 
            res.WriteByte(_spawnedItem.Location.Bag); 
            res.WriteInt16(_spawnedItem.Location.Slot);
            res.WriteInt32((int) _spawnedItem.CurrentEquipSlot); //bit mask. This indicates where to put items.   e.g. 01 head 010 arm 0100 feet etc (0 for not equipped) TODO - change State in database to be this bitmask value
            res.WriteInt64(0); //unknown
            res.WriteUInt32(0); //unknown
            return res;
        }
    }
}
