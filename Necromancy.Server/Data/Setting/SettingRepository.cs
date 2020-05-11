using System.Collections.Generic;
using System.IO;
using Arrowgene.Logging;

namespace Necromancy.Server.Data.Setting
{
    public class SettingRepository
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(SettingRepository));

        private readonly DirectoryInfo _directory;

        public SettingRepository(string folder)
        {
            _directory = new DirectoryInfo(folder);
            if (!_directory.Exists)
            {
                Logger.Error($"Could not initialize repository, '{folder}' does not exist");
                return;
            }

            Items = new Dictionary<int, ItemSetting>();
            Maps = new Dictionary<int, MapSetting>();
            MapSymbols = new Dictionary<int, MapSymbolSetting>();
            Strings = new StrTableSettingLookup();
            Monster = new Dictionary<int, MonsterSetting>();
            SkillBase = new Dictionary<int, SkillBaseSetting>();
            EoBase = new Dictionary<int, EoBaseSetting>();
            Npc = new Dictionary<int, NpcSetting>();
            ModelAtr = new Dictionary<int, ModelAtrSetting>();
            ModelCommon = new Dictionary<int, ModelCommonSetting>();
        }

        public Dictionary<int, ItemSetting> Items { get; }
        public Dictionary<int, MapSetting> Maps { get; }
        public Dictionary<int, MapSymbolSetting> MapSymbols { get; }
        public Dictionary<int, MonsterSetting> Monster { get; }
        public Dictionary<int, SkillBaseSetting> SkillBase { get; }
        public Dictionary<int, CharacterAttackSetting> CharacterAttack { get; }
        public Dictionary<int, EoBaseSetting> EoBase { get; }
        public StrTableSettingLookup Strings { get; }
        public Dictionary<int, NpcSetting> Npc { get; }
        public Dictionary<int, ModelAtrSetting> ModelAtr { get; }
        public Dictionary<int, ModelCommonSetting> ModelCommon { get; }

        public SettingRepository Initialize()
        {
            Items.Clear();
            Maps.Clear();
            MapSymbols.Clear();
            Strings.Clear();
            Monster.Clear();
            SkillBase.Clear();
            EoBase.Clear();
            Npc.Clear();
            ModelAtr.Clear();
            ModelCommon.Clear();
            Load(Strings, "str_table.csv", new StrTableCsvReader());
            Load(Items, "iteminfo.csv", new ItemInfoCsvReader());
            Load(Monster, "monster.csv", new MonsterCsvReader());
            Load(SkillBase, "skill_base.csv", new SkillBaseCsvReader());
            //Load(CharacterAttack, "chara_attack.csv", new CharacterAttackCsvReader());
            Load(EoBase, "eo_base.csv", new EoBaseCsvReader());
            Load(Npc, "npc.csv", new NpcCsvReader());
            Load(ModelAtr, "model_atr.csv", new ModelAtrCsvReader());
            Load(Maps, "map.csv", new MapCsvReader(Strings));
            //Load(MapSymbols, "map_symbol.csv", new MapSymbolCsvReader());
            Load(ModelCommon, "model_common.csv", new ModelCommonCsvReader(Monster, ModelAtr));
            return this;
        }

        private void Load<T>(List<T> list, string fileName, CsvReader<T> reader)
        {
            string path = Path.Combine(_directory.FullName, fileName);
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                Logger.Error($"Could not load '{fileName}', file does not exist");
            }

            list.AddRange(reader.Read(file.FullName));
        }

        private void Load<T>(Dictionary<int, T> dictionary, string fileName, CsvReader<T> reader)
        {
            List<T> items = new List<T>();
            Load(items, fileName, reader);
            foreach (T item in items)
            {
                if (item is ISettingRepositoryItem repositoryItem)
                {
                    dictionary.Add(repositoryItem.Id, item);
                }
            }
        }

        private void Load(StrTableSettingLookup lookup, string fileName, CsvReader<StrTableSetting> reader)
        {
            List<StrTableSetting> items = new List<StrTableSetting>();
            Load(items, fileName, reader);
            foreach (StrTableSetting item in items)
            {
                lookup.Add(item);
            }
        }
    }
}
