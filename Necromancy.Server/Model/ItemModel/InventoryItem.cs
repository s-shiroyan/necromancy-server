namespace Necromancy.Server.Model.ItemModel
{
    public class InventoryItem
    {
        public InventoryItem()
        {
        }

        public int Id { get; set; }
        public int CharacterId { get; set; }
        public uint ItemId { get; set; }
        public int Quantity { get; set; }
        public int CurrentDurability { get; set; }
        public Item Item { get; set; }
    }
}
