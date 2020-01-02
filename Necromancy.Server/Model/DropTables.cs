using Necromancy.Server.Common;
using System.Collections.Generic;
namespace Necromancy.Server.Model
{
    public class DropTables
    {
        private List<DropTable> dropTables;

        public DropTables()
        {
            dropTables = new List<DropTable>();
            DropTable beetle = new DropTable(40101);
            DropTableItem item1 = new DropTableItem();
            item1.ItemId = 1;
            item1.Rarity = 1;
            item1.MinItems = 1;
            item1.Maxitems = 5;
            DropTableItem item2 = new DropTableItem();
            item2.ItemId = 50430001;
            item2.Rarity = 2;
            item2.MinItems = 1;
            item2.Maxitems = 2;
            DropTableItem item3 = new DropTableItem();
            item3.ItemId = 80000101;
            item3.Rarity = 3;
            item3.MinItems = 1;
            item3.Maxitems = 2;
            beetle.AddItem(item1);
            beetle.AddItem(item2);
            beetle.AddItem(item3);
            dropTables.Add(beetle);
        }

        public DropItem GetLoot(int monsterId)
        {
            int roll = LootRoll();
            DropTable monsterDrop = dropTables.Find(x => x.MonsterId == monsterId);
            DropItem item = null;

            if (monsterDrop != null)
            {
                List<DropTableItem> ItemDrop = monsterDrop.FindAll(roll);
                if (ItemDrop.Count == 1)
                {

                    item = new DropItem(ItemDrop[0].ItemId, GetNumberItems(ItemDrop[0].MinItems, ItemDrop[0].Maxitems));
                }
            }
            else
            {
                item = new DropItem(50100301, 1);//this is a default Camp Item to prevent un-handled exceptions
            }
            return item;
        }
        private int LootRoll()
        {
            int lootRoll = Util.GetRandomNumber(1, 1001);

            if (lootRoll < 631)
            {
                return 1;
            }
            else if (lootRoll < 911)
            {
                return 2;
            }
            else if (lootRoll < 971)
            {
                return 3;
            }
            else if (lootRoll < 999)
            {
                return 4;
            }
            return 5;
        }
        public int GetNumberItems(int min, int max)
        {
            return Util.GetRandomNumber(min, max); ;
        }
    }
    public class DropTable
    {
        public int MonsterId { get; }
        public List<DropTableItem> DropTableItems { get; }
        public DropTable(int monsterId)
        {
            MonsterId = monsterId;
            DropTableItems = new List<DropTableItem>();
        }

        public void AddItem(DropTableItem item)
        {
            DropTableItems.Add(item);
        }
        public List<DropTableItem> FindAll(int rarity)
        {
             return DropTableItems.FindAll(x => x.Rarity == rarity);
        }
    }
    public class DropTableItem
    {
        public int Rarity;
        public int ItemId;
        public int MinItems;
        public int Maxitems;
    }
    public class DropItem
    {
        public int ItemId;
        public int NumItems;
        public DropItem (int itemId, int numItems)
        {
            ItemId = itemId;
            NumItems = numItems;
        }
    }
}
