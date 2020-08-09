using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Auction_House.Logic
{
    public class SearchCriteria
    {

        public enum QualityFlags
        {
            None        = 0b_0000_0000,  
            Poor        = 0b_0000_0001,  
            Normal      = 0b_0000_0010,  
            Good        = 0b_0000_0100,  
            Master      = 0b_0000_1000,  
            Artifact    = 0b_0001_0000 
        }

        public enum ClassFlags
        {
            None    = 0b_0000_0000,
            Fighter = 0b_0000_0001,
            Thief   = 0b_0000_0010,
            Priest  = 0b_0000_0100,
            Mage    = 0b_0000_1000
        }

        public int SoulRankMin {get; set; }
        public int SoulRankMax { get; set; }
        public int ForgePriceMin { get; set; }
        public int ForgePriceMax { get; set; }
        public QualityFlags Quality { get; set; }
        public ClassFlags Class { get;set; }

    }
}
