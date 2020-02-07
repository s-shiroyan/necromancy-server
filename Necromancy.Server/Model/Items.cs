using System;

namespace Necromancy.Server.Model
{
    public class Items
    {
        public int id { get; set; }
        public string ItemName { get; set; }
        public int ItemType { get; set; }
        public Int16 Physics { get; set; }
        public Int16 Magic { get; set; }

        public Int32 EnchantID { get; set; }

        public Int32 Durab { get; set; }

        public byte Hardness { get; set; }

        public Int32 MaxDur { get; set; }

        public byte Numbers { get; set; }

        public byte Level { get; set; }

        public byte Splevel { get; set; }

        public Int32 Weight { get; set; }

        public Int32 State { get; set; }

        public Items()
        {

        }
    }

}