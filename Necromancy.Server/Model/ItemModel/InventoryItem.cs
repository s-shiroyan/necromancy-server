namespace Necromancy.Server.Model.ItemModel
{
    public class InventoryItem
    {
        public InventoryItem()
        {
        }

        public int Id { get; set; }
        public int CharacterId { get; set; }
        public int ItemId { get; set; }
        public byte Quantity { get; set; }
        public int CurrentDurability { get; set; }
        public Item Item { get; set; }
        public EquipmentSlotType CurrentEquipmentSlotType { get; set; }
        public byte StorageType { get; set; }
        public byte BagId { get; set; }
        public short BagSlotIndex { get; set; }
        public int State { get; set; }
    }
}
