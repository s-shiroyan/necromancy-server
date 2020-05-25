using System.Data.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlCreateItem =
            "INSERT INTO `nec_item` (`id`, `name`, `item_type`, `equipment_slot_type`, `physical`, `magical`, `durability`) VALUES (@id, @name, @item_type, @equipment_slot_type, @physical, @magical, @durability);";

        private const string SqlSelectItemById =
            "SELECT `id`, `name`, `item_type`, `equipment_slot_type`, `physical`, `magical`, `durability` FROM `nec_item` WHERE `id`=@id; ";

        private const string SqlUpdateItem =
            "UPDATE `nec_item` SET `name`=@name, `item_type`=@item_type, `equipment_slot_type`=@equipment_slot_type, `physical`=@physical, `magical`=@magical, `durability`=@durability WHERE `id`=@id;";

        private const string SqlDeleteItem =
            "DELETE FROM `nec_item` WHERE `id`=@id;";

        public bool InsertItem(Item item)
        {
            int rowsAffected = ExecuteNonQuery(SqlCreateItem, command =>
            {
                AddParameter(command, "@id", item.Id);
                AddParameter(command, "@name", item.Name);
                AddParameter(command, "@item_type", (int) item.ItemType);
                AddParameter(command, "@equipment_slot_type", (int) item.EquipmentSlotType);
                AddParameter(command, "@physical", item.Physical);
                AddParameter(command, "@magical", item.Magical);
                AddParameter(command, "@durability", item.Durability);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

            return true;
        }


        public Item SelectItemById(int itemId)
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
            int rowsAffected = ExecuteNonQuery(SqlUpdateItem, command =>
            {
                AddParameter(command, "@id", item.Id);
                AddParameter(command, "@name", item.Name);
                AddParameter(command, "@item_type", (int) item.ItemType);
                AddParameter(command, "@equipment_slot_type", (int) item.EquipmentSlotType);
                AddParameter(command, "@physical", item.Physical);
                AddParameter(command, "@magical", item.Magical);
                AddParameter(command, "@durability", item.Durability);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteItem(int itemId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteItem,
                command => { AddParameter(command, "@id", itemId); });
            return rowsAffected > NoRowsAffected;
        }

        private Item ReadItem(DbDataReader reader)
        {
            Item item = new Item();
            item.Id = GetInt32(reader, "id");
            item.Name = GetString(reader, "name");
            item.ItemType = (ItemType) GetInt32(reader, "item_type");
            item.EquipmentSlotType = (EquipmentSlotType) GetInt32(reader, "equipment_slot_type");
            item.Physical = GetInt32(reader, "physical");
            item.Magical = GetInt32(reader, "magical");
            item.Durability = GetInt32(reader, "durability");
            return item;
        }
    }
}
