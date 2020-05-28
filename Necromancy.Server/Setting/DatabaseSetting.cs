using System;
using System.IO;
using System.Runtime.Serialization;
using Necromancy.Server.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Setting
{
    [DataContract]
    public class DatabaseSettings
    {
        public DatabaseSettings()
        {
            Type = DatabaseType.SQLite;
            SqLiteFolder = Path.Combine(Util.RelativeExecutingDirectory(), "Database");
            ScriptFolder = Path.Combine(Util.RelativeExecutingDirectory(), "Database/Script");
            Host = "localhost";
            Port = 3306;
            Database = "necromancy";
            User = string.Empty;
            Password = string.Empty;
        }

        public DatabaseSettings(DatabaseSettings databaseSettings)
        {
            Type = databaseSettings.Type;
            SqLiteFolder = databaseSettings.SqLiteFolder;
            Host = databaseSettings.Host;
            Port = databaseSettings.Port;
            User = databaseSettings.User;
            Password = databaseSettings.Password;
            Database = databaseSettings.Database;
            ScriptFolder = databaseSettings.ScriptFolder;
        }

        [DataMember(Order = 0)] public DatabaseType Type { get; set; }

        [DataMember(Order = 1)] public string SqLiteFolder { get; set; }

        [DataMember(Order = 2)] public string Host { get; set; }

        [DataMember(Order = 3)] public short Port { get; set; }

        [DataMember(Order = 4)] public string User { get; set; }

        [DataMember(Order = 5)] public string Password { get; set; }

        [DataMember(Order = 6)] public string Database { get; set; }
        
        [DataMember(Order = 7)] public string ScriptFolder { get; set; }
    }
}
