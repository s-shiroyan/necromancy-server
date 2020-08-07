using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Model
{
    public class ShortcutItem
    {
        public enum ShortcutType
        {
            UNKNOWN0,
            UNKNOWN1,
            UNKNOWN2,
            SKILL,
            SYSTEM,
            EMOTE            
        }
        public long Id { get; }
        public ShortcutType Type { get; }

        public ShortcutItem(long id, ShortcutType shortcutType)
        {
            Id = id;
            Type = shortcutType;
        }

    }

    

}
