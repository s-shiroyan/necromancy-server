using System;
using System.IO;
using Arrowgene.Logging;
using Necromancy.Server.Database.Sql;
using Necromancy.Server.Model;
using Necromancy.Server.Setting;

namespace Necromancy.Server.Database
{
    public class NecDatabaseBuilder
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(NecDatabaseBuilder));
        
        public IDatabase Build(DatabaseSettings settings)
        {
            IDatabase database = null;
            switch (settings.Type)
            {
                case DatabaseType.SQLite:
                    database = PrepareSqlLiteDb(settings.SqLiteFolder);
                    break;
            }

            if (database == null)
            {
                Logger.Error("Database could not be created, exiting...");
                Environment.Exit(1);
            }

            return database;
        }

        private NecSqLiteDb PrepareSqlLiteDb(string sqLiteFolder)
        {
            string sqLitePath = Path.Combine(sqLiteFolder, $"db.sqlite");
            NecSqLiteDb db = new NecSqLiteDb(sqLitePath);
            if (db.CreateDatabase())
            {
                ScriptRunner scriptRunner = new ScriptRunner(db);
                scriptRunner.Run(Path.Combine(sqLiteFolder, "Script/schema_sqlite.sql"));
                scriptRunner.Run(Path.Combine(sqLiteFolder, "Script/data_item.sql"));
                scriptRunner.Run(Path.Combine(sqLiteFolder, "Script/data_npc.sql"));
                scriptRunner.Run(Path.Combine(sqLiteFolder, "Script/data_monster.sql"));
                scriptRunner.Run(Path.Combine(sqLiteFolder, "Script/data_account.sql"));
                scriptRunner.Run(Path.Combine(sqLiteFolder, "Script/data_skill.sql"));
                scriptRunner.Run(Path.Combine(sqLiteFolder, "Script/data_union.sql"));
                scriptRunner.Run(Path.Combine(sqLiteFolder, "Script/data_auction.sql"));
                scriptRunner.Run(Path.Combine(sqLiteFolder, "Script/data_gimmick.sql"));
                scriptRunner.Run(Path.Combine(sqLiteFolder, "Script/data_maptransition.sql"));
            }

            new SqlMigrator(db).Migrate(Path.Combine(sqLiteFolder, "Script/Migrations/"));
            return db;
        }
    }
}
