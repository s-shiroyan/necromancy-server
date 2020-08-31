using Necromancy.Server.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item
{
    public class BaseItem
    {        
        public int BaseId { get; set; }
        public ItemType Type { get; set; }
        public ItemQualities Quality { get; set; }    
        public int MaxSlots { get; set; }
        public ItemEquipSlot EquipSlot { get; set; }        

        public bool RequiresHumanMale { get; set; }
        public bool RequiresHumanFemale { get; set; }
        public bool RequiresElfMale { get; set; }
        public bool RequiresElfFemale { get; set; }
        public bool RequiresDwarfMale { get; set; }
        public bool RequiresDwarfFemale { get; set; }
        public bool RequiresPorkulMale { get; set; }
        public bool RequiresPorkulFemale { get; set; }
        public bool RequiresGnomeMale { get; set; }
        public bool RequiresGnomeFemale { get; set; }

        public ItemRequiredClasses RequiredClasses { get; set; }
        public ItemRequiredAlignments RequiredAlignments { get; set; }

        public int RequiredStrength { get; set; }
        public int RequiredVitality { get; set; }
        public int RequiredDexterity { get; set; }
        public int RequiredAgility { get; set; }
        public int RequiredIntelligence { get; set; }
        public int RequiredPiety { get; set; }
        public int RequiredLuck { get; set; }

        public int RequiredSoulRank { get; set; }
        public int RequiredLevel { get; set; }

        public int PhysicalSlash {get; set;}
        public int PhysicalStrike { get; set; }
        public int PhysicalPierce { get; set; }

        public int PhysicalDefenseFire { get; set; }
        public int PhysicalDefenseWater { get; set; }
        public int PhysicalDefenseWind { get; set; }
        public int PhysicalDefenseEarth { get; set; }
        public int PhysicalDefenseLight { get; set; }
        public int PhysicalDefenseDark { get; set; }

        public int MagicalAttackFire { get; set; }
        public int MagicalAttackWater { get; set; }
        public int MagicalAttackWind { get; set; }
        public int MagicalAttackEarth { get; set; }
        public int MagicalAttackLight { get; set; }
        public int MagicalAttackDark { get; set; }

        public int Hp { get; set; }
        public int Mp { get; set; }
        public int Str { get; set; }
        public int Vit { get; set; }
        public int Dex { get; set; }
        public int Agi { get; set; }
        public int Int { get; set; }
        public int Pie { get; set; }
        public int Luk { get; set; }

        public int ResistPoison { get; set; }
        public int ResistParalyze { get; set; }
        public int ResistPetrified { get; set; }
        public int ResistFaint { get; set; }
        public int ResistBlind { get; set; }
        public int ResistSleep { get; set; }
        public int ResistSilence { get; set; }
        public int ResistCharm { get; set; }
        public int ResistConfusion { get; set; }
        public int ResistFear { get; set; }

        public ItemStatusEffect StatusMalus {get; set; }
        public int StatusMalusPercent { get; set; }

        public string ObjectType = "NONE"; //TODO
        public string EquipSlot2 = "WhoKnows"; //TODO
        public string IconType = "eh"; //TODO

        public bool IsStorable { get; set; }
        public bool IsDiscardable { get; set; }
        public bool IsSellable { get; set; }
        public bool IsTradeable { get; set; }
        public bool IsTradableAfterUse { get; set; }
        public bool IsLootable { get; set; }
        public bool IsGoldBorder { get; set; }
        public string Lore { get; set; }
        public int IconId { get; set; }

    }
}
