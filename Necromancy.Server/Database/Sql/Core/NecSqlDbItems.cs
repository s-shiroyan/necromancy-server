using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlCreateItems =
            "INSERT INTO `Items` (`id`, `ItemName`, `ItemType`, `Physics`, `Magic`, `EnchantID`, `Durab`, `Hardness`, `MaxDur`, `Numbers`, `Level`, `Splevel`, `Weight`, `State`) VALUES (@id, @ItemName, @ItemType, @Physics, @Magic, @EnchantID, @Durab, @Hardness, @MaxDur, @Numbers, @Level, @Splevel, @Weight, @State);";

        private const string SqlSelectItemsById =
            "SELECT `id`, `ItemName`, `ItemType`, `Physics`, `Magic`, `EnchantID`, `Durab`, `Hardness`, `MaxDur`, `Numbers`, `Level`, `Splevel`, `Weight`, `State` FROM `Items` WHERE `id`=@id; ";

        private const string SqlUpdateItems =
            "UPDATE `Items` SET `id`=@id, `ItemName`=@ItemName, `ItemType`=@ItemType,  `Physics`=@Physics, `Magic`=@Magic, `EnchantID`=@EnchantID, `Durab`=@Durab, `Hardness`=@Hardness, `MaxDur`=@MaxDur, `Numbers`=@Numbers, `Level`=@Level, `Splevel`=@Splevel, `Weight`=@Weight, `State`=@State WHERE `id`=@id;";

        private const string SqlDeleteItems =
            "DELETE FROM `Items` WHERE `id`=@id;";

        public bool InsertItems(Items items)
        {
            int rowsAffected = ExecuteNonQuery(SqlCreateItems, command =>
            {
                AddParameter(command, "@id", items.id);
                AddParameter(command, "@ItemName", items.ItemName);
                AddParameter(command, "@ItemType", items.ItemType);
                AddParameter(command, "@Physics", items.Physics);
                AddParameter(command, "@Magic", items.Magic);
                AddParameter(command, "@EnchantID", items.EnchantID);
                AddParameter(command, "@Durab", items.Durab);
                AddParameter(command, "@Hardness", items.Hardness);
                AddParameter(command, "@MaxDur", items.MaxDur);
                AddParameter(command, "@Numbers", items.Numbers);
                AddParameter(command, "@Level", items.Level);
                AddParameter(command, "@Splevel", items.Splevel);
                AddParameter(command, "@Weight", items.Weight);
                AddParameter(command, "@State", items.State);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

            items.id = (int)autoIncrement;
            return true;
        }


        public Items SelectitemsById(int itemsId)
        {
            Items items = null;
            ExecuteReader(SqlSelectItemsById,
                command => { AddParameter(command, "@id", itemsId); }, reader =>
                {
                    if (reader.Read())
                    {
                        items = ReadItems(reader);
                    }
                });
            return items;
        }

        public bool UpdateItems(Items items)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateItems, command =>
            {
                AddParameter(command, "@id", items.id);
                AddParameter(command, "@ItemName", items.ItemName);
                AddParameter(command, "@ItemType", items.ItemType);
                AddParameter(command, "@Physics", items.Physics);
                AddParameter(command, "@Magic", items.Magic);
                AddParameter(command, "@EnchantID", items.EnchantID);
                AddParameter(command, "@Durab", items.Durab);
                AddParameter(command, "@Hardness", items.Hardness);
                AddParameter(command, "@MaxDur", items.MaxDur);
                AddParameter(command, "@Numbers", items.Numbers);
                AddParameter(command, "@Level", items.Level);
                AddParameter(command, "@Splevel", items.Splevel);
                AddParameter(command, "@Weight", items.Weight);
                AddParameter(command, "@State", items.State);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteItems(int itemsId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteItems,
                command => { AddParameter(command, "@id", itemsId); });
            return rowsAffected > NoRowsAffected;
        }

        private Items ReadItems(DbDataReader reader)
        {
            Items items = new Items();
            items.id = GetInt32(reader, "id");
            items.ItemName = GetString(reader, "ItemName");
            items.ItemType = GetInt32(reader, "ItemType");
            items.Physics = (byte)GetInt32(reader, "Physics");
            items.Magic = (byte)GetInt32(reader, "Magic");
            items.EnchantID = GetInt32(reader, "EnchantID");
            items.Durab = (byte)GetInt32(reader, "Durab");
            items.Hardness = (byte)GetInt32(reader, "Hardness");
            items.MaxDur = (byte)GetInt32(reader, "MaxDur");
            items.Numbers = (byte)GetInt32(reader, "Numbers");
            items.Level = (byte)GetInt32(reader, "Level");
            items.Splevel = (byte)GetInt32(reader, "Splevel");
            items.Weight = (byte)GetInt32(reader, "Weight");
            items.State = (byte)GetInt32(reader, "State");
            return items;
        }
    }
}