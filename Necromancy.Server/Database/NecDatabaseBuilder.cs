using System;
using System.IO;
using Arrowgene.Logging;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Database.Sql;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Setting;

namespace Necromancy.Server.Database
{
    public class NecDatabaseBuilder
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(NecDatabaseBuilder));

        private NecSetting _setting;

        public NecDatabaseBuilder(NecSetting setting)
        {
            _setting = setting;
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
                SettingRepository settingRepository = new SettingRepository(_setting.RepositoryFolder).Initialize();

                // create table structure
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "schema_sqlite.sql"));

                // insert items into database
                foreach (int itemId in settingRepository.ItemInfo.Keys)
                {
                    if (!settingRepository.ItemNecromancy.TryGetValue(itemId, out ItemNecromancySetting necItem))
                    {
                        Logger.Error($"ItemId: {itemId} - not found in `SettingRepository.ItemNecromancy`");
                        continue;
                    }

                    Item item = new Item();
                    item.Id = necItem.Id;
                    item.Name = necItem.Name;
                    item.Durability = necItem.Durability;
                    item.Physical = necItem.Physical;
                    item.Magical = necItem.Magical;
                    item.ItemType = Item.ItemTypeByItemId(necItem.Id);
                    item.EquipmentSlotType = Item.EquipmentSlotTypeByItemType(item.ItemType);
                    if (!database.InsertItem(item))
                    {
                        Logger.Error($"ItemId: {itemId} - not found in `SettingRepository.ItemNecromancy`");
                        return;
                    }
                }
                
                // TODO insert npc / monster into DB instead from CSV
                
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_npc.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_monster.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_account.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_skill.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_union.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_auction.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_gimmick.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_maptransition.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_ggate.sql"));
            }

            SqlMigrator migrator = new SqlMigrator(database);
            migrator.Migrate(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "Migrations/"));
        }
    }
}
