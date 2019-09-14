using System;
using System.Data.Common;
using Arrowgene.Services.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database
{
    /// <summary>
    /// Implementation of Necromancy database operations.
    /// </summary>
    public abstract class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        protected readonly NecLogger Logger;

        private const string SqlCreateAccount =
            "INSERT INTO `account` (`name`, `normal_name`, `hash`, `mail`, `mail_verified`, `mail_verified_at`, `mail_token`, `password_token`, `state`, `last_login`, `created`) VALUES (@name, @normal_name, @hash, @mail, @mail_verified, @mail_verified_at, @mail_token, @password_token, @state, @last_login, @created);";

        private const string SqlSelectAccountByName =
            "SELECT `id`, `name`, `normal_name`, `hash`, `mail`, `mail_verified`, `mail_verified_at`, `mail_token`, `password_token`, `state`, `last_login`, `created` FROM `account` WHERE `name`=@name;";

        private const string SqlSelectAccountById =
            "SELECT `id`, `name`, `normal_name`, `hash`, `mail`, `mail_verified`, `mail_verified_at`, `mail_token`, `password_token`, `state`, `last_login`, `created` FROM `account` WHERE `id`=@id;";

        public NecSqlDb()
        {
            Logger = LogProvider.Logger<NecLogger>(this);
        }

        protected override void Exception(Exception ex)
        {
            Logger.Exception(ex);
        }

        public Account CreateAccount(string name, string mail, string hash)
        {
            Account account = new Account();
            account.Name = name;
            account.NormalName = name.ToLowerInvariant();
            account.Mail = mail;
            account.Hash = hash;
            account.State = AccountStateType.User;
            account.Created = DateTime.Now;
            int rowsAffected = ExecuteNonQuery(SqlCreateAccount, command =>
            {
                AddParameter(command, "@name", account.Name);
                AddParameter(command, "@normal_name", account.NormalName);
                AddParameter(command, "@hash", account.Hash);
                AddParameter(command, "@mail", account.Mail);
                AddParameter(command, "@mail_verified", account.MailVerified);
                AddParameter(command, "@mail_verified_at", account.MailVerifiedAt);
                AddParameter(command, "@mail_token", account.MailToken);
                AddParameter(command, "@password_token", account.PasswordToken);
                AddParameterEnumInt32(command, "@state", account.State);
                AddParameter(command, "@last_login", account.LastLogin);
                AddParameter(command, "@created", account.Created);
            });
            if (rowsAffected <= 0)
            {
                return null;
            }

            return account;
        }

        public Account SelectAccount(string accountName)
        {
            Account account = null;
            ExecuteReader(SqlSelectAccountByName,
                command => { AddParameter(command, "@name", accountName); }, reader =>
                {
                    if (reader.Read())
                    {
                        account = new Account();
                        account.Id = reader.GetInt32(0);
                        account.Name = reader.GetString(1);
                        account.NormalName = reader.GetString(2);
                        account.Hash = reader.GetString(3);
                        account.Mail = reader.GetString(4);
                        account.MailVerified = reader.GetBoolean(5);
                        account.MailVerifiedAt = ReadNullable(reader, 6, reader.GetDateTime);
                        account.MailToken = ReadNullable(reader, 7, reader.GetString);
                        account.PasswordToken = ReadNullable(reader, 8, reader.GetString);
                        account.State = (AccountStateType) reader.GetInt32(9);
                        account.LastLogin = ReadNullable(reader, 10, reader.GetDateTime);
                        account.Created = reader.GetDateTime(11);
                    }
                });

            return account;
        }

        public Account SelectAccount(int accountId)
        {
            Account account = null;
            ExecuteReader(SqlSelectAccountById, command => { AddParameter(command, "@id", accountId); }, reader =>
            {
                if (reader.Read())
                {
                    account = new Account();
                    account.Id = reader.GetInt32(0);
                    account.Name = reader.GetString(1);
                    account.NormalName = reader.GetString(2);
                    account.Hash = reader.GetString(3);
                    account.Mail = reader.GetString(4);
                    account.MailVerified = reader.GetBoolean(5);
                    account.MailVerifiedAt = ReadNullable(reader, 6, reader.GetDateTime);
                    account.MailToken = ReadNullable(reader, 7, reader.GetString);
                    account.PasswordToken = ReadNullable(reader, 8, reader.GetString);
                    account.State = (AccountStateType) reader.GetInt32(9);
                    account.LastLogin = ReadNullable(reader, 10, reader.GetDateTime);
                    account.Created = reader.GetDateTime(11);
                }
            });
            return account;
        }

        public bool SetSetting(string key, string value)
        {
            int rowsAffected;

            string sql = $"UPDATE `setting` SET `value`='{value}' WHERE `key`='{key}';";
            try

            {
                rowsAffected = ExecuteNonQuery(sql);
            }
            catch (Exception)
            {
                rowsAffected = 0;
            }

            if (rowsAffected <= 0)
            {
                sql = $"INSERT INTO `setting` (`key`, `value`) VALUES ('{key}', '{value}');";
                rowsAffected = ExecuteNonQuery(sql);
            }

            return rowsAffected > 0;
        }

        public string GetSetting(string key)
        {
            string value = null;
            string sql = $"SELECT `value` FROM `setting` WHERE `key` = '{key}';";
            ExecuteReader(sql, reader =>
            {
                if (reader.Read())
                {
                    value = reader.GetString(0);
                }
            });
            return value;
        }

        public bool DeleteSetting(string key)
        {
            string sql = $"DELETE FROM `setting` WHERE `key`='{key}';";
            return ExecuteNonQuery(sql) > 0;
        }
    }
}