using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Items
{
    public class GemSlot
    {
        private SpawnedItem _gem;
        public GemType Type{ get; set; }
        public SpawnedItem Gem { 
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
