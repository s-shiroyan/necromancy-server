using System;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using Arrowgene.Services.Logging;

namespace Necromancy.Server.Database
{
    public class SqLiteDb : IDatabase
    {
        public const string MemoryDatabasePath = ":memory:";

        private const string Separator = " ";
        private const long NoAutoIncrement = long.MaxValue;

        private readonly string _databasePath;
        private string _connectionString;
        private readonly ILogger _logger;

        public SqLiteDb(string databasePath)
        {
            _logger = LogProvider.Logger(this);
            _databasePath = databasePath;
            _logger.Info($"Database Path: {_databasePath}");
            CreateDatabase();
        }

        public void Execute(string sql)
        {
            using (SQLiteConnection connection = NewConnection())
            {
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();
            }
        }

        private void ExecuteReader(string sql, Action<DbDataReader> readAction)
        {
            using (SQLiteConnection connection = NewConnection())
            {
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    readAction(reader);
                }
            }
        }

        private int ExecuteNonQuery(string sql)
        {
            using (SQLiteConnection connection = NewConnection())
            {
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected;
            }
        }

        private int ExecuteNonQuery(string sql, out long autoIncrement)
        {
            using (SQLiteConnection connection = NewConnection())
            {
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                int rowsAffected = command.ExecuteNonQuery();
                autoIncrement = GetAutoIncrement(connection);
                return rowsAffected;
            }
        }

        public string ServerVersion()
        {
            return NewConnection().ServerVersion;
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

        private SQLiteConnection NewConnection()
        {
            if (_connectionString == null)
            {
                _connectionString = BuildConnectionString(_databasePath);
            }

            SQLiteConnection connection = new SQLiteConnection(_connectionString);
            return connection.OpenAndReturn();
        }

        private string BuildConnectionString(string source)
        {
            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder();
            builder.DataSource = source;
            builder.Version = 3;
            builder.ForeignKeys = true;

            string connectionString = builder.ToString();
            _logger.Info($"Connection String: {connectionString}");
            return connectionString;
        }
    }
}