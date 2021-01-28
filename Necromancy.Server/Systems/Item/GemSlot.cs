using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item
{
    public class GemSlot
    {
        //TODO make gems their own type maybe?
        private ItemInstance _gem;
        public GemType Type{ get; set; }
        public ItemInstance Gem { 
            get {
                return _gem;
            }
            set {
                _gem = value;
                IsFilled = true; 
            } 
        }

        /// <summary>
        /// Helper Property to determine if the slot is filled or not.
        /// </summary>
        public bool IsFilled { get; private set; }
    }
}
