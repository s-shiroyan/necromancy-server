using Arrowgene.Logging;

namespace Necromancy.Server.Data.Setting
{
    public class EoBaseCsvReader : CsvReader<EoBaseSetting>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(EoBaseCsvReader));

        protected override int NumExpectedItems => 9;

        protected override EoBaseSetting CreateInstance(string[] properties)
        {
            if (!int.TryParse(properties[0], out int id))
            {
                Logger.Debug("First entry empty!!");
                return null;
            }

            int.TryParse(properties[2], out int logId);
            bool.TryParse(properties[4], out bool onlyOwner);
            bool.TryParse(properties[5], out bool showActivationTime);
            bool.TryParse(properties[6], out bool showName);
            int.TryParse(properties[11], out int effectRadius);

            return new EoBaseSetting
            {
                Id = id,
                Name = properties[1],
                LogId = logId,
                Faction = properties[3],
                OnlyOwner = onlyOwner,
                ShowActivationTime = showActivationTime,
                ShowName = showName,
                damageShape = properties[10],
                EffectRadius = effectRadius
            };
        }
    }
}
