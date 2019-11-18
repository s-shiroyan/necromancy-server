using System;

namespace Necromancy.Server.Model
{
    public class ShortcutBar
    {
        public int Id { get; set; }
        public int Slot0 { get; set; }
        public int Slot1 { get; set; }
        public int Slot2 { get; set; }
        public int Slot3 { get; set; }
        public int Slot4 { get; set; }
        public int Slot5 { get; set; }
        public int Slot6 { get; set; }
        public int Slot7 { get; set; }
        public int Slot8 { get; set; }
        public int Slot9 { get; set; }

        public ShortcutBar()
        {
            Id = -1;
            Slot0 = -1;
            Slot1 = -1;
            Slot2 = -1;
            Slot3 = -1;
            Slot4 = -1;
            Slot5 = -1;
            Slot6 = -1;
            Slot7 = -1;
            Slot8 = -1;
            Slot9 = -1;
        }

        public int[] getArray()
        {
            int[] shortcutBar = new int[10];
            shortcutBar[0] = Slot0;
            shortcutBar[1] = Slot1;
            shortcutBar[2] = Slot2;
            shortcutBar[3] = Slot3;
            shortcutBar[4] = Slot4;
            shortcutBar[5] = Slot5;
            shortcutBar[6] = Slot6;
            shortcutBar[7] = Slot7;
            shortcutBar[8] = Slot8;
            shortcutBar[9] = Slot9;
            return shortcutBar;
        }
        public void setArray(int [] shortcutBar)
        {
            Slot0 = shortcutBar[0];
            Slot1 = shortcutBar[1];
            Slot2 = shortcutBar[2];
            Slot3 = shortcutBar[3];
            Slot4 = shortcutBar[4];
            Slot5 = shortcutBar[5];
            Slot6 = shortcutBar[6];
            Slot7 = shortcutBar[7];
            Slot8 = shortcutBar[8];
            Slot9 = shortcutBar[9];
        }
    }
}
