namespace Necromancy.Server.Data.Setting
{
    public class ItemLibrarySettingCsvReader : CsvReader<ItemLibrarySetting>
    {
        protected override int NumExpectedItems => 102;

        protected override ItemLibrarySetting CreateInstance(string[] properties)
        {
            if (!int.TryParse(properties[0], out int id)) { return null; }

            //Equipment slot Settings
            string itemType = properties[2];
            string equipmentType = properties[3];

            string name = properties[5];

            //Core Attributes
            string rarity = properties[6];
            if (!int.TryParse(properties[8], out int physicalAttack)) { return null; }
            if (!int.TryParse(properties[9], out int magicalAttack)) { return null; }
            if (!int.TryParse(properties[10], out int rangeDistance)) { return null; }
            if (!int.TryParse(properties[11], out int specialPerformance)) { return null; }
            if (!int.TryParse(properties[12], out int durability)) { return null; }
            if (!int.TryParse(properties[13], out int hardness)) { return null; }
            if (!float.TryParse(properties[14], out float weight)) { return null; }

            //Attack Type
            if (!int.TryParse(properties[15], out int slash)) { return null; }
            if (!int.TryParse(properties[16], out int strike)) { return null; }
            if (!int.TryParse(properties[17], out int pierce)) { return null; }

            //UnKnown
            if (!int.TryParse(properties[19], out int bonusTolerance)) { return null; }

            //Equip restrictions
            if (!bool.TryParse(properties[20], out bool fig)) { return null; }
            if (!bool.TryParse(properties[21], out bool thi)) { return null; }
            if (!bool.TryParse(properties[22], out bool mag)) { return null; }
            if (!bool.TryParse(properties[23], out bool pri)) { return null; }
            if (!bool.TryParse(properties[24], out bool sam)) { return null; }
            if (!bool.TryParse(properties[25], out bool nin)) { return null; }
            if (!bool.TryParse(properties[26], out bool bis)) { return null; }
            if (!bool.TryParse(properties[27], out bool lor)) { return null; }
            if (!bool.TryParse(properties[28], out bool clo)) { return null; }
            if (!bool.TryParse(properties[29], out bool alc)) { return null; }

            //Occupation Restrictions
            if (!int.TryParse(properties[30], out int occupation)) { return null; }

            //Bonus Stat gains
            if (!int.TryParse(properties[31], out int hp)) { return null; }
            if (!int.TryParse(properties[32], out int mp)) { return null; }
            if (!int.TryParse(properties[33], out int str)) { return null; }
            if (!int.TryParse(properties[34], out int vit)) { return null; }
            if (!int.TryParse(properties[35], out int dex)) { return null; }
            if (!int.TryParse(properties[36], out int agi)) { return null; }
            if (!int.TryParse(properties[37], out int iNt)) { return null; }
            if (!int.TryParse(properties[38], out int pie)) { return null; }
            if (!int.TryParse(properties[39], out int luk)) { return null; }

            //Bonus Skills on Attack
            if (!int.TryParse(properties[40], out int poison)) { return null; }
            if (!int.TryParse(properties[41], out int paralysis)) { return null; }
            if (!int.TryParse(properties[42], out int stone)) { return null; }
            if (!int.TryParse(properties[43], out int faint)) { return null; }
            if (!int.TryParse(properties[44], out int blind)) { return null; }
            if (!int.TryParse(properties[45], out int sleep)) { return null; }
            if (!int.TryParse(properties[46], out int charm)) { return null; }
            if (!int.TryParse(properties[47], out int confusion)) { return null; }
            if (!int.TryParse(properties[48], out int fear)) { return null; }

            //Bonus Elemental Defence
            if (!int.TryParse(properties[50], out int fireDef)) { return null; }
            if (!int.TryParse(properties[51], out int waterDef)) { return null; }
            if (!int.TryParse(properties[52], out int windDef)) { return null; }
            if (!int.TryParse(properties[53], out int earthDef)) { return null; }
            if (!int.TryParse(properties[54], out int lightDef)) { return null; }
            if (!int.TryParse(properties[55], out int darkDef)) { return null; }

            //Bonus Elemental Attack
            if (!int.TryParse(properties[56], out int fireAtk)) { return null; }
            if (!int.TryParse(properties[57], out int waterAtk)) { return null; }
            if (!int.TryParse(properties[58], out int windAtk)) { return null; }
            if (!int.TryParse(properties[59], out int earthAtk)) { return null; }
            if (!int.TryParse(properties[60], out int lightAtk)) { return null; }
            if (!int.TryParse(properties[61], out int darkAtk)) { return null; }

            //Transfer Restrictions
            if (!bool.TryParse(properties[62], out bool sellable)) { return null; }
            if (!bool.TryParse(properties[63], out bool tradeable)) { return null; }
            if (!bool.TryParse(properties[64], out bool newItem)) { return null; }
            if (!bool.TryParse(properties[65], out bool lootable)) { return null; }
            if (!bool.TryParse(properties[66], out bool blessable)) { return null; }
            if (!bool.TryParse(properties[67], out bool curseable)) { return null; }

            //Character Level Restrictions
            if (!int.TryParse(properties[68], out int lowerLimit)) { return null; }
            if (!int.TryParse(properties[69], out int upperLimit)) { return null; }

            //Minimum Stat requirements
            if (!int.TryParse(properties[70], out int requiredStr)) { return null; }
            if (!int.TryParse(properties[71], out int requiredVit)) { return null; }
            if (!int.TryParse(properties[72], out int requiredDex)) { return null; }
            if (!int.TryParse(properties[73], out int requiredAgi)) { return null; }
            if (!int.TryParse(properties[74], out int requiredInt)) { return null; }
            if (!int.TryParse(properties[75], out int requiredPie)) { return null; }
            if (!int.TryParse(properties[76], out int requiredLuk)) { return null; }

            //Soul Level Requirement
            if (!int.TryParse(properties[77], out int requiredSoulLevel)) { return null; }

            //Allignment Requirement
            string requiredAlignment = properties[78];
            //Race Requirement
            if (!bool.TryParse(properties[79], out bool requiredHuman)) { return null; }
            if (!bool.TryParse(properties[80], out bool requiredElf)) { return null; }
            if (!bool.TryParse(properties[81], out bool requiredDwarf)) { return null; }
            if (!bool.TryParse(properties[82], out bool requiredGnome)) { return null; }
            if (!bool.TryParse(properties[83], out bool requiredPorkul)) { return null; }

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
                RequiredPorkul = requiredPorkul,
                RequiredGender = requiredGender,
                WhenEquippedText = whenEquippedText
            };
        }
    }
}
