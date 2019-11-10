using System.Text.RegularExpressions;

namespace Necromancy.Server.Data.Setting
{
    public class MapSymbolCsvReader : CsvReader<MapSymbolSetting>
    {

        public MapSymbolCsvReader()
        {
        }

        protected override int NumExpectedItems => 10;



        protected override MapSymbolSetting CreateInstance(string[] properties)
        {
            if (!int.TryParse(properties[0], out int id))
            {
                return null;
            }

            if (!int.TryParse(properties[1], out int map))
            {
                return null;
            }

            if (!int.TryParse(properties[2], out int displayConditionflag))
            {
                return null;
            }

            if (!int.TryParse(properties[3], out int splitMapNumber))
            {
                return null;
            }

            if (!int.TryParse(properties[4], out int dettingTypeFlag))
            {
                return null;
            }

            if (!int.TryParse(properties[5], out int settingIdOrText))
            {
                //Catch the strings, and convert them to a unique int for assigment of behavoir later.
                switch (properties[4])
                {
                    case "Recovery Spring":
                        return null;
                    //return 99009901;
                    case "Quopaty Temple":
                        return null;
                    //return 99009902;
                    case "Adventurer's Inn":
                        return null;
                    //return 99009903;
                    case "Gilgamesh's Tavern":
                        return null;
                    //return 99009904;
                    case "Adventurer's Guild":
                        return null;
                    //return 99009905;
                    case "Change Channels":
                        return null;
                    //return 99009906;
                    case "Departure Gate":
                        return null;
                    //return 99009907;
                    case "Modamus Arms":
                        return null;
                    //return 99009908;
                    case "Vortak's Items":
                        return null;
                    //return 99009909;
                    case "Sagil Armor":
                        return null;
                    //return 99009910;
                    case "Magic by Junon":
                        return null;
                    //return 99009911;
                    case "Gidol's Forge & Alchemy":
                        return null;
                    //return 99009912;


                    default:
                        return null;
                }
            }

            if (!int.TryParse(properties[6], out int iconType))
            {
                return null;
            }

            if (!int.TryParse(properties[7], out int displayPositionX))
            {
                return null;
            }

            if (!int.TryParse(properties[8], out int displayPositionY))
            {
                return null;
            }

            if (!int.TryParse(properties[9], out int displayPositionZ))
            {
                return null;
            }

            return new MapSymbolSetting
            {
                 Id = id,
                 Map = map,
                 DisplayConditionflag = displayConditionflag,
                 SplitMapNumber = splitMapNumber,
                 SettingTypeFlag = dettingTypeFlag,
                 SettingIdOrText = settingIdOrText,
                 IconType = iconType,
                 DisplayPositionX = displayPositionX,
                 DisplayPositionY = displayPositionY,
                 DisplayPositionZ = displayPositionZ,
            };
        }
    }
}
