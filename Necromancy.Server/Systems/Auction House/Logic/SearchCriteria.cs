using Necromancy.Server.Model;
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
            None        = 0x0 << 0,  
            Poor        = 0x1 << 0,  
            Normal      = 0x1 << 1,  
            Good        = 0x1 << 2,  
            Master      = 0x1 << 3,  
            Artifact    = 0x1 << 4,
            All         = Poor & Normal & Good & Master & Artifact
        }

        [Flags]
        public enum Classes
        {
            None    = 0x0 << 0,
            Fighter = 0x1 << 0,
            Thief   = 0x1 << 1,
            Priest  = 0x1 << 2,
            Mage    = 0x1 << 3,
            All     = Fighter & Thief & Priest & Mage
        }

        private const int MIN_SOUL_RANK = 0;
        private const int MAX_SOUL_RANK = 99;
        private const int MIN_FORGE_PRICE = 0;
        private const int MAX_FORGE_PRICE = 99;

        public int SoulRankMin {get; set; }
        public int SoulRankMax { get; set; }
        public int ForgePriceMin { get; set; }
        public int ForgePriceMax { get; set; }
        public Qualities Quality { get; set; }
        public Classes Class { get;set; }

        public bool HasdValidClass()
        {
            return (Class & Classes.All) == Class;
        }

        public bool HasValidQuality()
        {
            return (Quality & Qualities.All) == Quality;
        }

        public bool HasValidSoulRankMin()
        {
            return (SoulRankMin >= MIN_SOUL_RANK) && (SoulRankMin <= MAX_SOUL_RANK);
        }

        public bool HasValidSoulRankMax()
        {
            return (SoulRankMax >= MIN_SOUL_RANK) && (SoulRankMax <= MAX_SOUL_RANK);
        }

        public bool HasValidForgePriceMin()
        {
            return (ForgePriceMin >= MIN_FORGE_PRICE) && (ForgePriceMin <= MIN_FORGE_PRICE);
        }

        public bool HasValidForgePriceMax()
        {
            return (ForgePriceMax >= MAX_FORGE_PRICE) && (ForgePriceMax <= MAX_FORGE_PRICE);
        }
    }
}
