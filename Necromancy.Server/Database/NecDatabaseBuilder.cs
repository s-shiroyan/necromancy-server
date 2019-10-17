using System;
using System.IO;
using Arrowgene.Services.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Database.Sql;
using Necromancy.Server.Model;
using Necromancy.Server.Setting;

namespace Necromancy.Server.Database
{
    public class NecDatabaseBuilder
    {
        private readonly ILogger _logger;

        public NecDatabaseBuilder()
        {
            _logger = LogProvider.Logger(this);
        }

        public IDatabase Build(DatabaseSettings settings)
        {
            IDatabase database = null;
            switch (settings.Type)
            {
                case DatabaseType.SQLite:
                    database = PrepareSqlLiteDb(settings.SqLitePath);
                    break;
            }

            if (database == null)
            {
                _logger.Error("Database could not be created, exiting...");
                Environment.Exit(1);
            }

            return database;
        }

        private NecSqLiteDb PrepareSqlLiteDb(string sqLitePath)
        {
            sqLitePath = sqLitePath.Replace(".sqlite", $".v{NecSqLiteDb.Version}.sqlite");
            NecSqLiteDb db = new NecSqLiteDb(sqLitePath);
            ScriptRunner scriptRunner = new ScriptRunner(db);
            scriptRunner.Run(Path.Combine(Util.RelativeCommonDirectory(), "Database/schema_sqlite.sql"));
            scriptRunner.Run(Path.Combine(Util.RelativeCommonDirectory(), "Database/data_item.sql"));
            scriptRunner.Run(Path.Combine(Util.RelativeCommonDirectory(), "Database/data_npc.sql"));
            return db;
        }
    }
}
