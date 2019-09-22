using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlCreateCharacter =
            "INSERT INTO `nec_character` (`account_id`, `soul_id`, `name`, `race_id`, `sex_id`, `hair_id`, `hair_color_id`, `face_id`, `level`, `created`) VALUES (@account_id, @soul_id, @name, @race_id, @sex_id, @hair_id, @hair_color_id, @face_id, @level, @created);";

        private const string SqlSelectCharacterById =
            "SELECT `id`, `account_id`, `soul_id`, `name`, `race_id`, `sex_id`, `hair_id`, `hair_color_id`, `face_id`, `level`, `created` FROM `nec_character` WHERE `id`=@id; ";

        private const string SqlSelectCharactersByAccountId =
            "SELECT `id`, `account_id`, `soul_id`, `name`, `race_id`, `sex_id`, `hair_id`, `hair_color_id`, `face_id`, `level`, `created` FROM `nec_character` WHERE `account_id`=@account_id; ";

        private const string SqlSelectCharactersBySoulId =
            "SELECT `id`, `account_id`, `soul_id`, `name`, `race_id`, `sex_id`, `hair_id`, `hair_color_id`, `face_id`, `level`, `created` FROM `nec_character` WHERE `soul_id`=@soul_id; ";

        private const string SqlUpdateCharacter =
            "UPDATE `nec_character` SET `account_id`=@account_id, `soul_id`=@soul_id, `name`=@name, `race_id`=@race_id, `sex_id`=@sex_id, `hair_id`=@hair_id, `hair_color_id`=@hair_color_id, `face_id`=@face_id, `level`=@level, `created`=@created WHERE `id`=@id;";

        private const string SqlDeleteCharacter =
            "DELETE FROM `nec_character` WHERE `id`=@id;";

        public bool InsertCharacter(Character character)
        {
            int rowsAffected = ExecuteNonQuery(SqlCreateCharacter, command =>
            {
                AddParameter(command, "@account_id", character.AccountId);
                AddParameter(command, "@soul_id", character.AccountId);
                AddParameter(command, "@name", character.Name);
                AddParameter(command, "@race_id", character.Raceid);
                AddParameter(command, "@sex_id", character.Sexid);
                AddParameter(command, "@hair_id", character.HairId);
                AddParameter(command, "@hair_color_id", character.HairColorId);
                AddParameter(command, "@face_id", character.FaceId);
                AddParameter(command, "@level", character.Level);
                AddParameter(command, "@created", character.Created);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

            character.Id = (int)autoIncrement;
            return true;
        }


        public Character SelectCharacterById(int characterId)
        {
            Character character = null;
            ExecuteReader(SqlSelectCharacterById,
                command => { AddParameter(command, "@id", characterId); }, reader =>
                {
                    if (reader.Read())
                    {
                        character = ReadCharacter(reader);
                    }
                });
            return character;
        }

        public List<Character> SelectCharacterByAccountId(int accountId)
        {
            List<Character> characters = new List<Character>();
            ExecuteReader(SqlSelectCharactersByAccountId,
                command => { AddParameter(command, "@account_id", accountId); }, reader =>
                {
                    while (reader.Read())
                    {
                        Character character = ReadCharacter(reader);
                        characters.Add(character);
                    }
                });
            return characters;
        }

        public List<Character> SelectCharacterBySoulId(int soulId)
        {
            List<Character> characters = new List<Character>();
            ExecuteReader(SqlSelectCharactersBySoulId,
                command => { AddParameter(command, "@soul_id", soulId); }, reader =>
                {
                    while (reader.Read())
                    {
                        Character character = ReadCharacter(reader);
                        characters.Add(character);
                    }
                });
            return characters;
        }

        public bool UpdateCharacter(Character character)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateCharacter, command =>
            {
                AddParameter(command, "@account_id", character.AccountId);
                AddParameter(command, "@soul_id", character.AccountId);
                AddParameter(command, "@name", character.Name);
                AddParameter(command, "@race_id", character.Raceid);
                AddParameter(command, "@sex_id", character.Sexid);
                AddParameter(command, "@hair_id", character.HairId);
                AddParameter(command, "@hair_color_id", character.HairColorId);
                AddParameter(command, "@face_id", character.FaceId);
                AddParameter(command, "@level", character.Level);
                AddParameter(command, "@created", character.Created);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteCharacter(int characterId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteCharacter,
                command => { AddParameter(command, "@id", characterId); });
            return rowsAffected > NoRowsAffected;
        }

        private Character ReadCharacter(DbDataReader reader)
        {
            Character character = new Character();
            character.Id = GetInt32(reader, "id");
            character.AccountId = GetInt32(reader, "account_id");
            character.SoulId = GetInt32(reader, "soul_id");
            character.Created = GetDateTime(reader, "created");
            character.Name = GetString(reader, "name");
            character.Raceid = (byte)GetInt32(reader, "race_id");
            character.Sexid = (byte)GetInt32(reader, "sex_id");
            character.HairId = (byte)GetInt32(reader, "hair_id");
            character.HairColorId = (byte)GetInt32(reader, "hair_color_id");
            character.FaceId = (byte)GetInt32(reader, "face_id");
            character.Level = (byte)GetInt32(reader, "level");
            return character;
        }
    }
}