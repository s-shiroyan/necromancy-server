using System;
using System.IO;
using Arrowgene.Logging;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Database.Sql;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Model.MapModel;
using Necromancy.Server.Setting;

namespace Necromancy.Server.Database
{
    public class NecDatabaseBuilder
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(NecDatabaseBuilder));

        private readonly NecSetting _setting;
        private readonly SettingRepository _settingRepository;

        public NecDatabaseBuilder(NecSetting setting, SettingRepository settingRepository = null)
        {
            _setting = setting;
            _settingRepository = settingRepository;
            if (_settingRepository == null)
            {
                _settingRepository = new SettingRepository(_setting.RepositoryFolder).Initialize();
            }
        }

        public IDatabase Build()
        {
            IDatabase database = null;
            switch (_setting.DatabaseSettings.Type)
            {
                case DatabaseType.SQLite:
                    string sqLitePath = Path.Combine(_setting.DatabaseSettings.SqLiteFolder, "db.sqlite");
                    database = new NecSqLiteDb(sqLitePath);
                    break;
            }

            if (database == null)
            {
                Logger.Error("Database could not be created, exiting...");
                Environment.Exit(1);
            }

            Initialize(database);
            return database;
        }

        private void Initialize(IDatabase database)
        {
            if (database.CreateDatabase())
            {
                ScriptRunner scriptRunner = new ScriptRunner(database);

                // create table structure
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "schema_sqlite.sql"));

                // insert maps
                foreach (MapSetting mapSetting in _settingRepository.Map.Values)
                {
                    MapData mapData = new MapData();
                    mapData.Id = mapSetting.Id;
                    mapData.Country = mapSetting.Country;
                    if (mapData.Country == null)
                    {
                        mapData.Country = "";
                    }
                    mapData.Area = mapSetting.Area;                    
                    if (mapData.Area == null)
                    {
                        mapData.Area = "";
                    }
                    mapData.Place = mapSetting.Place;                    
                    if (mapData.Place == null)
                    {
                        mapData.Place = "";
                    }
                    mapData.X = mapSetting.X;
                    mapData.Y = mapSetting.Y;
                    mapData.Z = mapSetting.Z;
                    mapData.Orientation = mapSetting.Orientation;
                    if (!database.InsertMap(mapData))
                    {
                        Logger.Error($"MapId: {mapData.Id} - failed to insert`");
                        return;
                    }
                }

                //// insert items (un-comment to rebuild table from settings lookup) Currently disabled for inventory development
                //foreach (ItemInfoSetting itemInfoSetting in _settingRepository.ItemInfo.Values)
                //{
                //    if (!_settingRepository.ItemNecromancy.TryGetValue(itemInfoSetting.Id, out ItemNecromancySetting necItem))
                //    {
                //        Logger.Error($"ItemId: {itemInfoSetting.Id} - not found in `SettingRepository.ItemNecromancy`");
                //        continue;
                //    }

                //    Item item = new Item();
                //    item.Id = necItem.Id;
                //    item.Name = itemInfoSetting.Name;
                //    item.Durability = necItem.Durability;
                //    item.Physical = necItem.Physical;
                //    item.Magical = necItem.Magical;
                //    item.ItemType = Item.ItemTypeByItemId(necItem.Id);
                //    item.EquipmentSlotType = Item.EquipmentSlotTypeByItemType(item.ItemType);
                //    if (!database.InsertItem(item))
                //    {
                //        Logger.Error($"ItemId: {item.Id} - could not be inserted into table`");
                //        return;
                //    }
                //}

                // insert items (un-comment to rebuild table from settings lookup) Currently disabled for inventory development
                foreach (ItemInfoSetting itemInfoSetting in _settingRepository.ItemInfo.Values)
                {
                    if (!_settingRepository.ItemLibrary.TryGetValue(itemInfoSetting.Id, out ItemLibrarySetting itemLibrarySetting))
                    {
                        Logger.Debug($"ItemId: {itemInfoSetting.Id} - is in itemInfo.CSV for this client, but not the Item Library`");
                        continue;
                    }

                    Item item = new Item();
                    item.Id = itemLibrarySetting.Id;
                    item.Name = itemLibrarySetting.Name;
                    item.Durability = itemLibrarySetting.Durability;
                    item.Physical = itemLibrarySetting.PhysicalAttack;
                    item.Magical = itemLibrarySetting.MagicalAttack;
                    item.ItemType = (ItemType)Enum.Parse(typeof(ItemType), itemLibrarySetting.ItemType);
                    item.EquipmentSlotType = (EquipmentSlotType)Enum.Parse(typeof(EquipmentSlotType), itemLibrarySetting.EquipmentType);
                    
                    if (!database.InsertItem(item))
                    {
                        Logger.Error($"ItemId: {item.Id} - could not be inserted into table`");
                        return;
                    }
                }

                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_account.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_npc_spawn.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_monster_spawn.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_skill.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_union.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_auction.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_gimmick.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_maptransition.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_ggate.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_item_library0.sql"));
                //scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_item_library.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_item_spawn.sql"));
            }

            SqlMigrator migrator = new SqlMigrator(database);
            migrator.Migrate(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "Migrations/"));
        }
    }
}
