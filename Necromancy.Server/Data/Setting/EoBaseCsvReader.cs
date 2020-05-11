using Arrowgene.Logging;

namespace Necromancy.Server.Data.Setting
{
    public class EoBaseCsvReader : CsvReader<EoBaseSetting>
    {
        private readonly ILogger _logger;

        public EoBaseCsvReader()
        {
            _logger = LogProvider.Logger(this);
        }

        protected override int NumExpectedItems => 9;

        protected override EoBaseSetting CreateInstance(string[] properties)
        {
            if (!int.TryParse(properties[0], out int id))
            {
                _logger.Debug($"First entry empty!!");
                return null;
            }

            /*if (id == 1430211)
            {
                _logger.Debug($"properties[9] [{properties[9]}]");
                _logger.Debug($"properties[10] [{properties[10]}]");
                _logger.Debug($"properties[11] [{properties[11]}]");
            }*/
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
