using Necromancy.Server.Model;
using Necromancy.Server.Systems.Auction_House;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Inventory.Logic
{
    public class Inventory
    {
        private readonly Character character;

        public int Gold { 
            get{ return 0; }
            set { Gold = value; }
        }

        public Inventory(Character character)
        {
            this.character = character;
        }

    }
}
