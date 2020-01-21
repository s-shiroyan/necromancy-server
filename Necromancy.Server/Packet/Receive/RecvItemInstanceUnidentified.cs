using Arrowgene.Services.Buffers;
using Necromancy.Server.Chat;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvItemInstanceUnidentified : PacketResponse
    {
        private readonly InventoryItem _invItem;
        public RecvItemInstanceUnidentified(InventoryItem invItem)
            : base((ushort) AreaPacketId.recv_item_instance_unidentified, ServerType.Area)
        {
            _invItem = invItem;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();

            //res.WriteInt64(dropItem.Item.Id); //Item Object Instance ID 
            res.WriteInt64(_invItem.InstanceId); //Item Object Instance ID 

            res.WriteCString(_invItem.StorageItem.Name); //Name

            //res.WriteInt32(dropItem.Item.IconType); 
            res.WriteInt32(_invItem.StorageItem.IconType); //item type

            res.WriteInt32(0);

            res.WriteByte(_invItem.StorageCount); //Number of items

            res.WriteInt32(0); //Item status 0 = identified  

            res.WriteInt32(_invItem.StorageItem.Id); //Item icon 50100301 = camp
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(1); // bool
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteByte(_invItem.StorageType); // 0 = adventure bag. 1 = character equipment
            res.WriteByte(_invItem.StorageId); // 0~2
            res.WriteInt16(_invItem.StorageSlot); // bag index
            res.WriteInt32(0); //bit mask. This indicates where to put items.   e.g. 01 head 010 arm 0100 feet etc (0 for not equipped)

            res.WriteInt64(0);

            res.WriteInt32(0);

            return res;
        }
    }
}
