namespace Necromancy.Server.Data.Setting
{
    /// <summary>
    /// Additional Item information, that was not provided or could not be extracted from the client.
    /// </summary>
    public class ItemLibrarySetting : ISettingRepositoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //Equipment Slot Settings
        public string ItemType { get; set; }
        public string EquipmentType { get; set; }

        //Core Attributes
        public string Rarity { get; set; }
        public int PhysicalAttack { get; set; }
        public int MagicalAttack { get; set; }
        public int RangeDistance { get; set; }
        public int SpecialPerformance { get; set; }
        public int Hardness { get; set; }
        public float Weight { get; set; }

        //Attack type
        public int Slash { get; set; }
        public int Strike { get; set; }
        public int Pierce { get; set; }

        //UnKnown
        public int BonusTolerance { get; set; }

        //Equip Restrictions
        public bool FIG { get; set; }
        public bool THI { get; set; }
        public bool MAG { get; set; }
        public bool PRI { get; set; }
        public bool SAM { get; set; }
        public bool NIN { get; set; }
        public bool BIS { get; set; }
        public bool LOR { get; set; }
        public bool CLO { get; set; }
        public bool ALC { get; set; }

        //Bitmask of occupation restrictions
        public int Occupation { get; set; }

        //Bonus Stat gains
        public int HP { get; set; }
        public int MP { get; set; }
        public int STR { get; set; }
        public int VIT { get; set; }
        public int DEX { get; set; }
        public int AGI { get; set; }
        public int INT { get; set; }
        public int PIE { get; set; }
        public int LUK { get; set; }

        //Bonus Skills on Attack
        public int Poison { get; set; }
        public int Paralysis { get; set; }
        public int Stone { get; set; }
        public int Faint { get; set; }
        public int Blind { get; set; }
        public int Sleep { get; set; }
        public int Charm { get; set; }
        public int Confusion { get; set; }
        public int Fear { get; set; }

        //Bonus Elemental Defence
        public int FireDef { get; set; }
        public int WaterDef { get; set; }
        public int WindDef { get; set; }
        public int EarthDef { get; set; }
        public int LightDef { get; set; }
        public int DarkDef { get; set; }

        //Bonus Elemental Attack
        public int FireAtk { get; set; }
        public int WaterAtk { get; set; }
        public int WindAtk { get; set; }
        public int EarthAtk { get; set; }
        public int LightAtk { get; set; }
        public int DarkAtk { get; set; }

        //Transfer Restrictions
        public bool Sellable { get; set; }
        public bool Tradeable { get; set; }
        public bool NewItem { get; set; }
        public bool Lootable { get; set; }
        public bool Blessable { get; set; }
        public bool Curseable { get; set; }

        //Character Level Restrictions
        public int LowerLimit { get; set; }
        public int UpperLimit { get; set; }

        //Minimum Stat Requirements
        public int RequiredStr { get; set; }
        public int RequiredVit { get; set; }
        public int RequiredDex { get; set; }
        public int RequiredAgi { get; set; }
        public int RequiredInt { get; set; }
        public int RequiredPie { get; set; }
        public int RequiredLuk { get; set; }

        //Soul Level Requirement
        public int RequiredSoulLevel { get; set; }
        
        //Allignment Requirement
        public string RequiredAlignment { get; set; }

        //Race Requirement
        public bool RequiredHuman { get; set; }
        public bool RequiredElf { get; set; }
        public bool RequiredDwarf { get; set; }
        public bool RequiredPorkul { get; set; }

        //Gender Requirement
        public string RequiredGender { get; set; }
        
        //Special Description Text
        public string WhenEquippedText { get; set; }


















    }
}
