using System;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model
{
    public class InventoryItem : IInstance64
    {
        public ulong InstanceId { get; set; }
        public byte StorageType { get; set; }
        public byte StorageId { get; set; }
        public short StorageSlot { get; set; }
        public Item StorageItem { get; set; }
        public InventoryItem()
        {
            StorageItem = new Item();
        }
    }
}
