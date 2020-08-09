using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Auction_House.Logic
{
    public class SearchCriteria
    {
        [Flags]
        public enum Qualities
        {
            None        = 0 << 0,  
            Poor        = 1 << 0,  
            Normal      = 1 << 1,  
            Good        = 1 << 2,  
            Master      = 1 << 3,  
            Artifact    = 1 << 4
        }

        [Flags]
        public enum Classes
        {
            None    = 0 << 0,
            Fighter = 1 << 0,
            Thief   = 1 << 1,
            Priest  = 1 << 2,
            Mage    = 1 << 3
        }

        public int SoulRankMin {get; set; }
        public int SoulRankMax { get; set; }
        public int ForgePriceMin { get; set; }
        public int ForgePriceMax { get; set; }
        public Qualities Quality { get; set; }
        public Classes Class { get;set; }

    }
}
