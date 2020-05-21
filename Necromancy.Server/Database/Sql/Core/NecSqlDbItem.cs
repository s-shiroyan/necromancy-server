using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlCreateItem =
            "INSERT INTO `nec_item` (`name`) VALUES (@name);";

        private const string SqlSelectItemById =
            "SELECT `id`, `name` FROM `nec_item` WHERE `id`=@id; ";

        private const string SqlUpdateItem =
            "UPDATE `nec_item` SET `name`=@name WHERE `id`=@id;";

        private const string SqlDeleteItem =
            "DELETE FROM `nec_item` WHERE `id`=@id;";

        public bool InsertItem(Item item)
        {
            int rowsAffected = ExecuteNonQuery(SqlCreateItem, command => { AddParameter(command, "@name", item.Name); },
                out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

            item.Id = (uint) autoIncrement;
            return true;
        }

        public Item SelectItemById(uint itemId)
        {
            Item item = null;
            ExecuteReader(SqlSelectItemById,
                command => { AddParameter(command, "@id", itemId); }, reader =>
                {
                    if (reader.Read())
                    {
                        item = ReadItem(reader);
                    }
                });
            return item;
        }

        public bool UpdateItem(Item item)
        {
            int rowsAffected =
                ExecuteNonQuery(SqlUpdateItem, command => { AddParameter(command, "@name", item.Name); });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteItemById(uint itemId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteItem,
                command => { AddParameter(command, "@id", itemId); });
            return rowsAffected > NoRowsAffected;
        }

        private Item ReadItem(DbDataReader reader)
        {
            Item item = new Item();
            item.Id = GetUInt32(reader, "id");
            item.Name = GetString(reader, "name");
            return item;
        }
    }
}
