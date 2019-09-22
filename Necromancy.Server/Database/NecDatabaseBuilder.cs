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
                    database = PrepareSqlLiteDb(settings.SqLitePath,settings);
                    break;
            }

            if (database == null)
            {
                _logger.Error("Database could not be created, exiting...");
                Environment.Exit(1);
            }

            return database;
        }

        private SqLiteDb PrepareSqlLiteDb(string sqlLitePath,DatabaseSettings settings)
        {
            if (!File.Exists($"{Util.ExecutingDirectory()}/DBVersionFlagFile92119"))
                {
                Console.WriteLine($"DB Flag File not found. Creating : {Util.ExecutingDirectory()}/DBVersionFlagFile92119");
                File.Create($"{Util.ExecutingDirectory()}/DBVersionFlagFile92119");
                Console.WriteLine($"Deleting outdated Database file. Db.sqlite will be re-created with updated schema : {Util.ExecutingDirectory()}/db.sqlite");
                File.Delete($"{Util.ExecutingDirectory()}/db.sqlite");
                }

            SqLiteDb db = new SqLiteDb(sqlLitePath);
            ScriptRunner scriptRunner = new ScriptRunner(db);
            scriptRunner.Run(Path.Combine(Util.RelativeCommonDirectory(), "Database/sqlite_schema.sql"));
            return db;
        }
    }
}