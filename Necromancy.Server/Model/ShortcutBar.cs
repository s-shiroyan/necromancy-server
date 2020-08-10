using System;
using System.Threading;

namespace Necromancy.Server.Model
{
    public class ShortcutBar
    {
        public const int COUNT = 10;
        public ShortcutItem[] Item { get; }           
        public ShortcutBar()
        {
            Item = new ShortcutItem[COUNT];            
        }
    }
}
