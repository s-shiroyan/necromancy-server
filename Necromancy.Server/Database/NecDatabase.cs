using System;
using System.IO;
using Arrowgene.Services.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Setting;

namespace Necromancy.Server.Database
{
    public class NecDatabase
    {
        private readonly ILogger _logger;

        public NecDatabase()
        {
            _logger = LogProvider.Logger(this);
        }

        public void Prepare(DatabaseSettings settings)
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
        }

        private SqLiteDb PrepareSqlLiteDb(string sqlLitePath)
        {
            SqLiteDb db = new SqLiteDb(sqlLitePath);
            ScriptRunner scriptRunner = new ScriptRunner(db);
            scriptRunner.Run(Path.Combine(Util.RelativeCommonDirectory(), "Database/sqlite_schema.sql"));
            return db;
        }
    }
}