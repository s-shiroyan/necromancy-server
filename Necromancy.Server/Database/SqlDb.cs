using System;
using System.Data;
using System.Data.Common;

namespace Necromancy.Server.Database
{
    /// <summary>
    /// Operations for SQL type databases.
    /// </summary>
    public abstract class SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        public const long NoAutoIncrement = long.MaxValue;
        public const int NoRowsAffected = 0;

        public SqlDb()
        {
        }

        protected abstract TCon Connection();
        protected abstract TCom Command(string query, TCon connection);

        public abstract int Upsert(string table, string[] columns, object[] values, string whereColumn,
            object whereValue, out long autoIncrement);

        public int ExecuteNonQuery(string query, Action<TCom> nonQueryAction)
        {
            try
            {
                using (TCon connection = Connection())
                {
                    using (TCom command = Command(query, connection))
                    {
                        nonQueryAction(command);
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected;
                    }
                }
            }
            catch (Exception ex)
            {
                Exception(ex);
                return NoRowsAffected;
            }
        }

        public void ExecuteReader(string query, Action<TCom> nonQueryAction, Action<DbDataReader> readAction)
        {
            try
            {
                using (TCon connection = Connection())
                {
                    using (TCom command = Command(query, connection))
                    {
                        nonQueryAction(command);
                        using (DbDataReader reader = command.ExecuteReader())
                        {
                            readAction(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exception(ex);
            }
        }

        public void ExecuteReader(string query, Action<DbDataReader> readAction)
        {
            try
            {
                using (TCon connection = Connection())
                {
                    using (TCom command = Command(query, connection))
                    {
                        using (DbDataReader reader = command.ExecuteReader())
                        {
                            readAction(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exception(ex);
            }
        }

        public void Execute(string query)
        {
            try
            {
                using (TCon connection = Connection())
                {
                    using (TCom command = Command(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Exception(ex);
            }
        }

        public int ExecuteNonQuery(string query)
        {
            try
            {
                using (TCon connection = Connection())
                {
                    using (TCom command = Command(query, connection))
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected;
                    }
                }
            }
            catch (Exception ex)
            {
                Exception(ex);
                return NoRowsAffected;
            }
        }

        public string ServerVersion()
        {
            try
            {
                using (TCon connection = Connection())
                {
                    return connection.ServerVersion;
                }
            }
            catch (Exception ex)
            {
                Exception(ex);
                return string.Empty;
            }
        }

        protected virtual void Exception(Exception ex)
        {
            throw ex;
        }

        protected DbParameter Parameter(TCom command, string name, object value, DbType type)
        {
            DbParameter parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            parameter.DbType = type;
            return parameter;
        }

        protected DbParameter Parameter(TCom command, string name, string value)
        {
            return Parameter(command, name, value, DbType.String);
        }

        protected void AddParameter(TCom command, string name, object value, DbType type)
        {
            DbParameter parameter = Parameter(command, name, value, type);
            command.Parameters.Add(parameter);
        }

        protected void AddParameter(TCom command, string name, string value)
        {
            AddParameter(command, name, value, DbType.String);
        }

        protected void AddParameter(TCom command, string name, Int32 value)
        {
            AddParameter(command, name, value, DbType.Int32);
        }

        protected void AddParameterEnumInt32<T>(TCom command, string name, T value) where T : Enum
        {
            AddParameter(command, name, (Int32) (object) value, DbType.Int32);
        }

        protected void AddParameter(TCom command, string name, DateTime? value)
        {
            AddParameter(command, name, value, DbType.DateTime);
        }

        protected void AddParameter(TCom command, string name, DateTime value)
        {
            AddParameter(command, name, value, DbType.DateTime);
        }

        protected void AddParameter(TCom command, string name, bool value)
        {
            AddParameter(command, name, value, DbType.Boolean);
        }

        protected DateTime? ReadNullable(DbDataReader reader, int ordinal, Func<int, DateTime> getValue)
        {
            if (reader.IsDBNull(ordinal))
            {
                return null;
            }

            return getValue(ordinal);
        }

        protected string ReadNullable(DbDataReader reader, int ordinal, Func<int, string> getValue)
        {
            if (reader.IsDBNull(ordinal))
            {
                return null;
            }

            return getValue(ordinal);
        }
    }
}