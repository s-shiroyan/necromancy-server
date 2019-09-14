using System;
using System.Data.SQLite;
using System.IO;

namespace Necromancy.Server.Database
{
    /// <summary>
    /// SQLite Necromancy database.
    /// </summary>
    public class SqLiteDb : NecSqlDb<SQLiteConnection, SQLiteCommand>, IDatabase
    {
        public const string MemoryDatabasePath = ":memory:";

        private readonly string _databasePath;
        private string _connectionString;

        public SqLiteDb(string databasePath)
        {
            _databasePath = databasePath;
            Logger.Info($"Database Path: {_databasePath}");
            CreateDatabase();
        }


        private long GetAutoIncrement(SQLiteConnection connection)
        {
            return connection.LastInsertRowId;
        }

        private void CreateDatabase()
        {
            if (_databasePath != MemoryDatabasePath && !File.Exists(_databasePath))
            {
                FileStream fs = File.Create(_databasePath);
                fs.Close();
                fs.Dispose();
            }
        }

        private string BuildConnectionString(string source)
        {
            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder();
            builder.DataSource = source;
            builder.Version = 3;
            builder.ForeignKeys = true;
            // Set ADO.NET conformance flag https://system.data.sqlite.org/index.html/info/e36e05e299
            builder.Flags = builder.Flags & SQLiteConnectionFlags.StrictConformance;

            string connectionString = builder.ToString();
            Logger.Info($"Connection String: {connectionString}");
            return connectionString;
        }

        protected override SQLiteConnection Connection()
        {
            if (_connectionString == null)
            {
                _connectionString = BuildConnectionString(_databasePath);
            }

            SQLiteConnection connection = new SQLiteConnection(_connectionString);
            return connection.OpenAndReturn();
        }

        protected override SQLiteCommand Command(string query, SQLiteConnection connection)
        {
            return new SQLiteCommand(query, connection);
        }

        public override int Upsert(string table, string[] columns, object[] values, string whereColumn,
            object whereValue,
            out long autoIncrement)
        {
            throw new NotImplementedException();
        }
    }
}