using Arrowgene.Logging;

namespace Necromancy.Server.Data.Setting
{
    public class ItemLibrarySettingCsvReader : CsvReader<ItemLibrarySetting>
    {
        protected override int NumExpectedItems => 102;
        private static readonly ILogger Logger = LogProvider.Logger(typeof(ItemLibrarySettingCsvReader));


        protected override ItemLibrarySetting CreateInstance(string[] properties)
        {
            if (!int.TryParse(properties[0], out int id)) { Logger.Debug("failed here [  0] ");return null; }

            //Equipment slot Settings
            string itemType = properties[2];
            string equipmentType = properties[3];

            string name = properties[5];

            //Core Attributes
            string rarity = properties[6];
            if (!int.TryParse(properties[8], out int physicalAttack)) { Logger.Debug("failed here [8  ] ");return null; }
            if (!int.TryParse(properties[9], out int magicalAttack)) { Logger.Debug("failed here [  9] ");return null; }
            if (!int.TryParse(properties[10], out int rangeDistance)) { Logger.Debug("failed here [  10] ");return null; }
            if (!int.TryParse(properties[11], out int specialPerformance)) { Logger.Debug("failed here [11  ] ");return null; }
            if (!int.TryParse(properties[12], out int durability)) { Logger.Debug("failed here [ 12 ] ");return null; }
            if (!int.TryParse(properties[13], out int hardness)) { Logger.Debug("failed here [ 13 ] ");return null; }
            if (!float.TryParse(properties[14], out float weight)) { Logger.Debug("failed here [ 14 ] ");return null; }

            //Attack Type
            if (!int.TryParse(properties[15], out int slash)) { Logger.Debug("failed here [ 15 ] ");return null; }
            if (!int.TryParse(properties[16], out int strike)) { Logger.Debug("failed here [  16] ");return null; }
            if (!int.TryParse(properties[17], out int pierce)) { Logger.Debug("failed here [  17] ");return null; }

            //UnKnown
            if (!int.TryParse(properties[19], out int bonusTolerance)) { Logger.Debug("failed here [19  ] ");return null; }

            //Equip restrictions
            if (!int.TryParse(properties[20], out int fig)) { Logger.Debug("failed here [  20] ");return null; }
            if (!int.TryParse(properties[21], out int thi)) { Logger.Debug("failed here [  21] ");return null; }
            if (!int.TryParse(properties[22], out int mag)) { Logger.Debug("failed here [  22] ");return null; }
            if (!int.TryParse(properties[23], out int pri)) { Logger.Debug("failed here [  23] ");return null; }
            if (!int.TryParse(properties[24], out int sam)) { Logger.Debug("failed here [  24] ");return null; }
            if (!int.TryParse(properties[25], out int nin)) { Logger.Debug("failed here [  25] ");return null; }
            if (!int.TryParse(properties[26], out int bis)) { Logger.Debug("failed here [  26] ");return null; }
            if (!int.TryParse(properties[27], out int lor)) { Logger.Debug("failed here [  27] ");return null; }
            if (!int.TryParse(properties[28], out int clo)) { Logger.Debug("failed here [  28] ");return null; }
            if (!int.TryParse(properties[29], out int alc)) { Logger.Debug("failed here [  29] ");return null; }

            //Occupation Restrictions
            if (!int.TryParse(properties[30], out int occupation)) { Logger.Debug("failed here [  30] ");return null; }

            //Bonus Stat gains
            if (!int.TryParse(properties[31], out int hp)) { Logger.Debug("failed here [  31] ");return null; }
            if (!int.TryParse(properties[32], out int mp)) { Logger.Debug("failed here [  32] ");return null; }
            if (!int.TryParse(properties[33], out int str)) { Logger.Debug("failed here [  33] ");return null; }
            if (!int.TryParse(properties[34], out int vit)) { Logger.Debug("failed here [  34] ");return null; }
            if (!int.TryParse(properties[35], out int dex)) { Logger.Debug("failed here [  35] ");return null; }
            if (!int.TryParse(properties[36], out int agi)) { Logger.Debug("failed here [  36] ");return null; }
            if (!int.TryParse(properties[37], out int iNt)) { Logger.Debug("failed here [  37] ");return null; }
            if (!int.TryParse(properties[38], out int pie)) { Logger.Debug("failed here [  38] ");return null; }
            if (!int.TryParse(properties[39], out int luk)) { Logger.Debug("failed here [  39] ");return null; }

            //Bonus Skills on Attack
            if (!int.TryParse(properties[40], out int poison)) { Logger.Debug("failed here [ 40 ] ");return null; }
            if (!int.TryParse(properties[41], out int paralysis)) { Logger.Debug("failed here [41  ] ");return null; }
            if (!int.TryParse(properties[42], out int stone)) { Logger.Debug("failed here [  42] ");return null; }
            if (!int.TryParse(properties[43], out int faint)) { Logger.Debug("failed here [  43] ");return null; }
            if (!int.TryParse(properties[44], out int blind)) { Logger.Debug("failed here [  44] ");return null; }
            if (!int.TryParse(properties[45], out int sleep)) { Logger.Debug("failed here [  45] ");return null; }
            if (!int.TryParse(properties[46], out int charm)) { Logger.Debug("failed here [  46] ");return null; }
            if (!int.TryParse(properties[47], out int confusion)) { Logger.Debug("failed here [  47] ");return null; }
            if (!int.TryParse(properties[48], out int fear)) { Logger.Debug("failed here [  48] ");return null; }

            //Bonus Elemental Defence
            if (!int.TryParse(properties[50], out int fireDef)) { Logger.Debug("failed here [  50] ");return null; }
            if (!int.TryParse(properties[51], out int waterDef)) { Logger.Debug("failed here [  51] ");return null; }
            if (!int.TryParse(properties[52], out int windDef)) { Logger.Debug("failed here [  52] ");return null; }
            if (!int.TryParse(properties[53], out int earthDef)) { Logger.Debug("failed here [  53] ");return null; }
            if (!int.TryParse(properties[54], out int lightDef)) { Logger.Debug("failed here [  54] ");return null; }
            if (!int.TryParse(properties[55], out int darkDef)) { Logger.Debug("failed here [  55] ");return null; }

            //Bonus Elemental Attack
            if (!int.TryParse(properties[56], out int fireAtk)) { Logger.Debug("failed here [  56] ");return null; }
            if (!int.TryParse(properties[57], out int waterAtk)) { Logger.Debug("failed here [  57] ");return null; }
            if (!int.TryParse(properties[58], out int windAtk)) { Logger.Debug("failed here [  58] ");return null; }
            if (!int.TryParse(properties[59], out int earthAtk)) { Logger.Debug("failed here [  59] ");return null; }
            if (!int.TryParse(properties[60], out int lightAtk)) { Logger.Debug("failed here [  60] ");return null; }
            if (!int.TryParse(properties[61], out int darkAtk)) { Logger.Debug("failed here [  61] ");return null; }

            //Transfer Restrictions
            if (!bool.TryParse(properties[62], out bool sellable)) { Logger.Debug("failed here [  62] ");return null; }
            if (!bool.TryParse(properties[63], out bool tradeable)) { Logger.Debug("failed here [  63] ");return null; }
            if (!bool.TryParse(properties[64], out bool newItem)) { Logger.Debug("failed here [  64] ");return null; }
            if (!bool.TryParse(properties[65], out bool lootable)) { Logger.Debug("failed here [  65] ");return null; }
            if (!bool.TryParse(properties[66], out bool blessable)) { Logger.Debug("failed here [  66] ");return null; }
            if (!bool.TryParse(properties[67], out bool curseable)) { Logger.Debug("failed here [  67] ");return null; }

            //Character Level Restrictions
            if (!int.TryParse(properties[68], out int lowerLimit)) { Logger.Debug("failed here [  68] ");return null; }
            if (!int.TryParse(properties[69], out int upperLimit)) { Logger.Debug("failed here [  69] ");return null; }

            //Minimum Stat requirements
            if (!int.TryParse(properties[70], out int requiredStr)) { Logger.Debug("failed here [  70] ");return null; }
            if (!int.TryParse(properties[71], out int requiredVit)) { Logger.Debug("failed here [  71] ");return null; }
            if (!int.TryParse(properties[72], out int requiredDex)) { Logger.Debug("failed here [  72] ");return null; }
            if (!int.TryParse(properties[73], out int requiredAgi)) { Logger.Debug("failed here [  73] ");return null; }
            if (!int.TryParse(properties[74], out int requiredInt)) { Logger.Debug("failed here [  74] ");return null; }
            if (!int.TryParse(properties[75], out int requiredPie)) { Logger.Debug("failed here [  75] ");return null; }
            if (!int.TryParse(properties[76], out int requiredLuk)) { Logger.Debug("failed here [  76] ");return null; }

            //Soul Level Requirement
            if (!int.TryParse(properties[77], out int requiredSoulLevel)) { Logger.Debug("failed here [  77] ");return null; }

            //Allignment Requirement
            string requiredAlignment = properties[78];
            //Race Requirement
            if (!int.TryParse(properties[79], out int requiredHuman)) { Logger.Debug("failed here [  79] ");return null; }
            if (!int.TryParse(properties[80], out int requiredElf)) { Logger.Debug("failed here [  80] ");return null; }
            if (!int.TryParse(properties[81], out int requiredDwarf)) { Logger.Debug("failed here [  81] ");return null; }
            if (!int.TryParse(properties[82], out int requiredGnome)) { Logger.Debug("failed here [  82] ");return null; }
            if (!int.TryParse(properties[83], out int requiredPorkul)) { Logger.Debug("failed here [  83] ");return null; }

            //Gender Requirement
            string requiredGender = properties[84];

            //Special Description Text
            string whenEquippedText = properties[86];


            return new ItemLibrarySetting
            {
                Id = id,
                Name = name,
                ItemType = itemType,
                EquipmentType = equipmentType,
                Rarity = rarity,
                PhysicalAttack = physicalAttack,
                MagicalAttack = magicalAttack,
                RangeDistance = rangeDistance,
                SpecialPerformance = specialPerformance,
                Durability = durability,
                Hardness = hardness,
                Weight = weight,
                Slash = slash,
                Strike = strike,
                Pierce = pierce,
                BonusTolerance = bonusTolerance,
                FIG = fig,
                THI = thi,
                MAG = mag,
                PRI = pri,
                SAM = sam,
                NIN = nin,
                BIS = bis,
                LOR = lor,
                CLO = clo,
                Occupation = occupation,
                HP = hp,
                MP = mp,
                STR = str,
                VIT = vit,
                DEX = dex,
                AGI = agi,
                INT = iNt,
                PIE = pie,
                LUK = luk,
                Poison = poison,
                Paralysis = paralysis,
                Stone = stone,
                Faint = faint,
                Blind = blind,
                Sleep = sleep,
                Charm = charm,
                Confusion = confusion,
                Fear = fear,
                FireDef = fireDef,
                WaterDef = waterDef,
                WindDef = windDef,
                EarthDef = earthDef,
                LightDef = lightDef,
                DarkDef = darkDef,
                FireAtk = fireAtk,
                WaterAtk = waterAtk,
                WindAtk = windAtk,
                EarthAtk = earthAtk,
                LightAtk = lightAtk,
                DarkAtk = darkAtk,
                Sellable = sellable,
                Tradeable = tradeable,
                NewItem = newItem,
                Lootable = lootable,
                Blessable = blessable,
                Curseable = curseable,
                LowerLimit = lowerLimit,
                UpperLimit = upperLimit,
                RequiredStr = requiredStr,
                RequiredVit = requiredVit,
                RequiredDex = requiredDex,
                RequiredAgi = requiredAgi,
                RequiredInt = requiredInt,
                RequiredPie = requiredPie,
                RequiredLuk = requiredLuk,
                RequiredSoulLevel = requiredSoulLevel,
                RequiredAlignment = requiredAlignment,
                RequiredHuman = requiredHuman,
                RequiredElf = requiredElf,
                RequiredDwarf = requiredDwarf,
                RequiredGnome = requiredGnome,
                RequiredPorkul = requiredPorkul,
                RequiredGender = requiredGender,
                WhenEquippedText = whenEquippedText
            };
        }
    }
}
